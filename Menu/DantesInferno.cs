using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Menu
{
    public class DantesInfernoMenu : ModMenu
    {
        public class InfernoCinder
        {
            public Vector2 Position;
            public Vector2 Velocity;
            public float Scale;
            public float Rotation;
            public float Alpha;
            public int Time;
            public int Lifetime;
            public Color Color;

            public InfernoCinder(int lifetime, float depth, Color color, Vector2 pos, Vector2 vel)
            {
                Lifetime = lifetime;
                Color = color;
                Position = pos;
                Velocity = vel;
                Scale = Main.rand.NextFloat(0.6f, 1.4f);
                Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                Alpha = 1f;
            }
        }

        private static readonly List<InfernoCinder> cinders = new();
        private static readonly List<InfernoCinder> meltDusts = new();
        private static Asset<Texture2D> bg;
        private static Asset<Texture2D> cinderTex;
        private static Asset<Texture2D> vignetteTex;
        private static Asset<Texture2D> logoTexture;

        private float vignettePulse = 0f;
        private float logoBobTimer = 0f;
        private float logoHoverProgress = 0f;
        private bool isLogoHovered = false;
        private bool isLogoClicked = false;
        private float clickPulseTimer = 0f;

        private const int LOGO_WIDTH = 594;
        private const int LOGO_HEIGHT = 280;
        private float logoScale = 0.8f;
        private const float LOGO_BOB_AMOUNT = 4f;
        private const float LOGO_BOB_SPEED = 0.025f;

        public override string DisplayName => "Dante's Inferno";
        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("Terraria/Images/UI/CharCreation/PanelGrayscale");

        public override void Load()
        {
            bg = ModContent.Request<Texture2D>("Synergia/Menu/DantesInferno");
            cinderTex = ModContent.Request<Texture2D>("Synergia/Menu/InfernoDust");
            vignetteTex = ModContent.Request<Texture2D>("Synergia/Menu/Vignette");
            logoTexture = ModContent.Request<Texture2D>("Synergia/Menu/Logo");
        }

        public override void Unload()
        {
            cinders.Clear();
            meltDusts.Clear();
        }

        private static float SmoothApproach(float current, float target, float speed)
        {
            return current + (target - current) * speed;
        }

        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            Texture2D background = bg.Value;
            Texture2D tex = cinderTex.Value;

            spriteBatch.Draw(background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero,
                new Vector2(Main.screenWidth / (float)background.Width, Main.screenHeight / (float)background.Height),
                SpriteEffects.None, 0f);

            DrawCinders(spriteBatch, tex);
            DrawMeltDusts(spriteBatch, tex);
            DrawAnimatedLogo(spriteBatch);
            DrawVignette(spriteBatch);

            drawColor = Color.White;
            Main.time = 27000;
            Main.dayTime = true;
            return false;
        }

        private void DrawAnimatedLogo(SpriteBatch spriteBatch)
        {
            if (logoTexture == null || logoTexture.Value == null)
                return;

            logoBobTimer += LOGO_BOB_SPEED;
            vignettePulse += 0.015f;

            Texture2D logo = logoTexture.Value;
            float logoWidth = LOGO_WIDTH * logoScale;
            float logoHeight = LOGO_HEIGHT * logoScale;

            float xPos = (Main.screenWidth - logoWidth) / 2f;
            float baseY = Main.screenHeight * 0.00f;
            float bobOffset = (float)Math.Sin(logoBobTimer) * LOGO_BOB_AMOUNT;
            Vector2 logoPosition = new Vector2(xPos, baseY + bobOffset);

            Rectangle logoRect = new Rectangle(
                (int)logoPosition.X,
                (int)logoPosition.Y,
                (int)logoWidth,
                (int)logoHeight
            );
            isLogoHovered = logoRect.Contains(Main.MouseScreen.ToPoint());

            if (isLogoHovered && Main.mouseLeft && Main.mouseLeftRelease)
            {
                isLogoClicked = true;
                clickPulseTimer = 0f;
                Main.mouseLeftRelease = false;
            }

            if (isLogoClicked)
            {
                clickPulseTimer += 0.03f;
                if (clickPulseTimer > 2f)
                {
                    isLogoClicked = false;
                    clickPulseTimer = 0f;
                }
            }

            float target = isLogoHovered ? 1f : 0f;
            logoHoverProgress = SmoothApproach(logoHoverProgress, target, 0.08f);

            float currentScale = logoScale + logoHoverProgress * 0.04f;

            float clickPulse = 0f;
            if (isLogoClicked)
            {
                clickPulse = (float)Math.Sin(clickPulseTimer * 8f) * 0.08f * (1f - clickPulseTimer / 2f);
                clickPulse = Math.Max(clickPulse, 0f);
            }

            Color outlineColor = Color.Lerp(Color.Black, new Color(200, 50, 30), logoHoverProgress + clickPulse);

            float pulseFactor = 1f + clickPulse * 0.15f;
            float outlinePulse = 1f + clickPulse * 0.1f;

            Vector2[] outlineDirections = new Vector2[]
            {
                new Vector2(-1, -1), new Vector2(0, -1), new Vector2(1, -1),
                new Vector2(-1, 0), new Vector2(1, 0),
                new Vector2(-1, 1), new Vector2(0, 1), new Vector2(1, 1)
            };

            float outlineOffset = currentScale * 2.5f * outlinePulse;

            foreach (Vector2 dir in outlineDirections)
            {
                Vector2 offset = dir * outlineOffset;
                spriteBatch.Draw(
                    logo,
                    logoPosition + offset,
                    null,
                    outlineColor,
                    0f,
                    Vector2.Zero,
                    currentScale * pulseFactor,
                    SpriteEffects.None,
                    0f
                );
            }

            Vector2[] extraOutlineDirs = new Vector2[]
            {
                new Vector2(-2, -1), new Vector2(-1, -2), new Vector2(1, -2), new Vector2(2, -1),
                new Vector2(-2, 1), new Vector2(-1, 2), new Vector2(1, 2), new Vector2(2, 1),
                new Vector2(-2, 0), new Vector2(2, 0), new Vector2(0, -2), new Vector2(0, 2)
            };

            Color extraOutlineColor = Color.Lerp(
                new Color(0, 0, 0, 200),
                new Color(180 + (int)(40 * clickPulse), 40 + (int)(20 * clickPulse), 20, 200),
                logoHoverProgress + clickPulse * 0.5f
            );
            float extraOffset = currentScale * 1.5f * outlinePulse;

            foreach (Vector2 dir in extraOutlineDirs)
            {
                Vector2 offset = dir * extraOffset;
                spriteBatch.Draw(
                    logo,
                    logoPosition + offset,
                    null,
                    extraOutlineColor,
                    0f,
                    Vector2.Zero,
                    currentScale * pulseFactor,
                    SpriteEffects.None,
                    0f
                );
            }

            float glowIntensity = (0.3f + clickPulse * 0.5f) * logoHoverProgress;
            if (glowIntensity > 0.01f)
            {
                for (int i = 0; i < 6; i++)
                {
                    float angle = vignettePulse + i * 1.0472f + clickPulseTimer * 2f;
                    float glowDistance = 3f + clickPulse * 8f;
                    Vector2 glowOffset = new Vector2(
                        (float)Math.Cos(angle) * glowDistance * logoHoverProgress,
                        (float)Math.Sin(angle) * glowDistance * logoHoverProgress
                    );

                    Color glowColor = new Color(
                        255,
                        100 + (int)(80 * logoHoverProgress) + (int)(50 * clickPulse),
                        50 + (int)(30 * clickPulse),
                        (int)((60 + 80 * clickPulse) * Math.Min(glowIntensity, 1f))
                    );

                    spriteBatch.Draw(
                        logo,
                        logoPosition + glowOffset,
                        null,
                        glowColor,
                        0f,
                        Vector2.Zero,
                        currentScale * (1f + clickPulse * 0.05f),
                        SpriteEffects.None,
                        0f
                    );
                }
            }

            spriteBatch.Draw(
                logo,
                logoPosition,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                currentScale * (1f + clickPulse * 0.02f),
                SpriteEffects.None,
                0f
            );
        }

        private void DrawCinders(SpriteBatch spriteBatch, Texture2D tex)
        {
            for (int i = 0; i < 2; i++)
            {
                if (cinders.Count < 140 && Main.rand.NextBool(3))
                {
                    Vector2 pos = new(Main.rand.NextFloat(0, Main.screenWidth), Main.screenHeight + 40f);
                    Vector2 vel = new(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-4f, -1.5f));
                    Color color = Color.Lerp(Color.DarkRed, Color.OrangeRed, Main.rand.NextFloat());
                    cinders.Add(new InfernoCinder(Main.rand.Next(180, 320), 0, color, pos, vel));
                }
            }

            for (int i = cinders.Count - 1; i >= 0; i--)
            {
                var c = cinders[i];
                c.Velocity.X += (float)Math.Sin((c.Time + i) * 0.05f) * 0.01f;
                c.Position += c.Velocity;
                c.Rotation += 0.01f;
                float progress = c.Time / (float)c.Lifetime;
                c.Alpha = 1f - progress;

                spriteBatch.Draw(tex, c.Position, null, c.Color * c.Alpha, c.Rotation, tex.Size() / 2f, c.Scale, SpriteEffects.None, 0f);

                c.Time++;
                if (c.Time >= c.Lifetime || c.Position.Y < -50f)
                    cinders.RemoveAt(i);
            }
        }

        private void DrawMeltDusts(SpriteBatch spriteBatch, Texture2D tex)
        {
            if (meltDusts.Count < 90 && Main.rand.NextBool(2))
            {
                Vector2 pos = new(Main.rand.NextFloat(0, Main.screenWidth), Main.screenHeight + 60f);
                Vector2 vel = new(Main.rand.NextFloat(-1.2f, 1.2f), Main.rand.NextFloat(-3.5f, -1.8f));
                Color color = Color.Lerp(Color.Yellow, Color.OrangeRed, Main.rand.NextFloat(0.3f, 0.8f));

                var dust = new InfernoCinder(Main.rand.Next(60, 130), 0, color, pos, vel);
                dust.Scale = Main.rand.NextFloat(0.25f, 0.65f);
                meltDusts.Add(dust);
            }

            for (int i = meltDusts.Count - 1; i >= 0; i--)
            {
                var d = meltDusts[i];
                d.Velocity.X += (float)Math.Sin(d.Time * 0.12f) * 0.025f;
                d.Position += d.Velocity;
                d.Rotation += 0.03f;

                float progress = d.Time / (float)d.Lifetime;
                d.Alpha = (1f - progress) * (1f - progress);

                spriteBatch.Draw(tex, d.Position, null, d.Color * d.Alpha, d.Rotation, tex.Size() / 2f, d.Scale, SpriteEffects.None, 0f);

                d.Time++;
                if (d.Time >= d.Lifetime || d.Position.Y < -40f)
                    meltDusts.RemoveAt(i);
            }
        }

        private void DrawVignette(SpriteBatch spriteBatch)
        {
            float pulse = 0.75f + MathF.Sin(vignettePulse) * 0.08f;
            Color vignetteColor = new Color(0, 0, 0, (byte)(pulse * 255));
            spriteBatch.Draw(vignetteTex.Value,
                new Rectangle(0, 0, Main.screenWidth, Main.screenHeight),
                null, vignetteColor, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}