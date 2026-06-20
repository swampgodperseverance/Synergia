using ParticleLibrary.Utilities;
using ReLogic.Content;
using Synergia.Helpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Utilities.Terraria.Utilities;

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
        private static Asset<Texture2D> backgroundTexture;
        private static Asset<Texture2D> crystalTexture;
        private static Asset<Texture2D> rayTexture;
        private static Asset<Texture2D> cinderTexture;
        private static Asset<Texture2D> vignetteTexture;
        private static Asset<Texture2D> logoTexture;
        private static Asset<Texture2D> logoGlowTexture;
        private static Asset<Texture2D> logoCrossTexture;
        private static float CurrentMusicVolume = 1f;

        private float raysTimer = 30f;
        private float vignettePulse = 0f;
        private float vignettePower = 1f;
        private float logoBobTimer = 0f;
        private float logoHoverProgress = 0f;
        private bool isLogoHovered = false;
        private bool isLogoClicked = false;
        private float clickPulseTimer = 0f;

        private const float CRYSTAL_RAYS_SPEED = 0.001f;
        private const int LOGO_WIDTH = 594;
        private const int LOGO_HEIGHT = 280;
        private float logoScale = 0.8f;
        private const float LOGO_BOB_AMOUNT = 4f;
        private const float LOGO_BOB_SPEED = 0.025f;
        private const float VIGNETTE_POWER_SPEED = 0.025f;

        public override string DisplayName => "Dante's Inferno";
        public override int Music => MusicLoader.GetMusicSlot(GetSongByName2("Netherworld"));
        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("Terraria/Images/UI/CharCreation/PanelGrayscale");

        public override void Load()
        {
            backgroundTexture = ModContent.Request<Texture2D>("Synergia/Menu/DantesInferno");
            crystalTexture = ModContent.Request<Texture2D>("Synergia/Menu/DantesInferno_Crystal");
            rayTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ray");
            cinderTexture = ModContent.Request<Texture2D>("Synergia/Menu/InfernoDust");
            vignetteTexture = ModContent.Request<Texture2D>("Synergia/Menu/Vignette");
            logoTexture = ModContent.Request<Texture2D>("Synergia/Menu/Logo");
            logoGlowTexture = ModContent.Request<Texture2D>("Synergia/Menu/Logo_Glow");
            logoCrossTexture = ModContent.Request<Texture2D>("Synergia/Menu/Logo_Cross");
            On_Main.UpdateAudio += Main_UpdateAudio;
        }

        public override void Unload()
        {
            cinders.Clear();
            meltDusts.Clear();
            On_Main.UpdateAudio -= Main_UpdateAudio;
        }

        private static float SmoothApproach(float current, float target, float speed)
        {
            return current + (target - current) * speed;
        }

        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            Texture2D background = backgroundTexture.Value;
            Texture2D tex = cinderTexture.Value;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);

            spriteBatch.Draw(background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero,
                new Vector2(Main.screenWidth / (float)background.Width, Main.screenHeight / (float)background.Height),
                SpriteEffects.None, 0f);
            DrawCrystal(spriteBatch);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);

            DrawCinders(spriteBatch, tex);
            DrawMeltDusts(spriteBatch, tex);
            DrawAnimatedLogo(spriteBatch, logoDrawCenter);
            DrawVignette(spriteBatch);

            drawColor = Color.White;
            Main.time = 5000;
            Main.dayTime = true;

            return false;
        }
        public override void Update(bool isOnTitleScreen)
        {
            if (Main.curMusic == Music)
                CurrentMusicVolume = 1f - 0.6f * vignettePower;
            else
                CurrentMusicVolume = 1f;
        }
        private void Main_UpdateAudio(On_Main.orig_UpdateAudio orig, Main self)
        {
            float origVolume = Main.musicVolume;
            if (Main.gameMenu)
                Main.musicVolume = origVolume * CurrentMusicVolume;
            orig(self);
            Main.musicVolume = origVolume;
        }

        private void DrawCrystal(SpriteBatch spriteBatch)
        {
            Texture2D crystal = crystalTexture.Value;
            Texture2D ray = rayTexture.Value;

            raysTimer += CRYSTAL_RAYS_SPEED;

            Vector2 rayCenter = new Vector2(Main.screenWidth * 0.25078125f, Main.screenHeight * 0.4388889f);
            Vector2 rayOrigin = new Vector2(ray.Width / 2, ray.Height);

            for(int i = 0; i < 13; i++)
            {
                Vector2 rayScale = new Vector2(4.5f, 2.5f + 0.5f * MathF.Cos(i + raysTimer));
                float rayRotate = MathF.Sin((i + 1) * 13.97f + raysTimer) * 3f;
                spriteBatch.Draw(
                    ray,
                    rayCenter,
                    null,
                    new Color(255, 130, 70),
                    rayRotate,
                    rayOrigin,
                    rayScale,
                    SpriteEffects.None,
                    0f
                );
            }

            spriteBatch.Draw(crystal, Vector2.Zero, null, Color.White, 0f, Vector2.Zero,
                new Vector2(Main.screenWidth / (float)crystal.Width, Main.screenHeight / (float)crystal.Height),
                SpriteEffects.None, 0f);
        }

        private void DrawAnimatedLogo(SpriteBatch spriteBatch, Vector2 logoCenter)
        {
            if (logoTexture == null || logoTexture.Value == null)
                return;

            logoBobTimer += LOGO_BOB_SPEED;
            vignettePulse += 0.015f;

            Texture2D logo = logoTexture.Value;
            float logoWidth = LOGO_WIDTH * logoScale;
            float logoHeight = LOGO_HEIGHT * logoScale;
            

            float bobOffset = (float)Math.Sin(logoBobTimer) * LOGO_BOB_AMOUNT;
            Vector2 logoPosition = new Vector2(logoCenter.X, logoCenter.Y + bobOffset);

            Rectangle logoRect = new Rectangle(
                (int)(logoPosition.X - logoWidth / 2),
                (int)(logoPosition.Y - logoHeight / 2),
                (int)logoWidth,
                (int)logoHeight
            );
            Vector2 logoOrigin = new Vector2(LOGO_WIDTH, LOGO_HEIGHT) / 2f;
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
            vignettePower = Math.Clamp(vignettePower + VIGNETTE_POWER_SPEED * (target * 2f - 1f), 0f, 1f);
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
                    logoOrigin,
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
                    logoOrigin,
                    currentScale * pulseFactor,
                    SpriteEffects.None,
                    0f
                );
            }

            float glowIntensity = (0.3f + clickPulse * 0.5f) * logoHoverProgress;
            if (glowIntensity > 0.1f)
            {
                Texture2D logoGlow = logoGlowTexture.Value;
                for (int i = 0; i < 6; i++)
                {
                    float angle = vignettePulse + i * 1.0472f + clickPulseTimer * 2f;
                    float glowDistance = 3f + clickPulse * 8f;
                    Vector2 glowOffset = new Vector2(
                        (float)Math.Cos(angle) * glowDistance * logoHoverProgress,
                        (float)Math.Sin(angle) * glowDistance * logoHoverProgress
                    );

                    Color glowColor = new Color(
                        200,
                        40 + (int)(30 * logoHoverProgress) + (int)(20 * clickPulse),
                        10 + (int)(10 * clickPulse),
                        (int)((60 + 80 * clickPulse) * Math.Min(glowIntensity, 1f))
                    );

                    spriteBatch.Draw(
                        logoGlow,
                        logoPosition + glowOffset,
                        null,
                        glowColor,
                        0f,
                        logoOrigin,
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
                logoOrigin,
                currentScale * (1f + clickPulse * 0.02f),
                SpriteEffects.None,
                0f
            );

            if(clickPulseTimer > 0f)
            {
                Texture2D logoCross = logoCrossTexture.Value;
                Color lightColor = Color.Lerp(Color.White, Color.Transparent, clickPulseTimer / 2f);
                spriteBatch.Draw(
                    logoCross,
                    logoPosition,
                    null,
                    lightColor.WithAlpha(0f),
                    0f,
                    logoOrigin,
                    currentScale * (1f + clickPulse * 0.05f),
                    SpriteEffects.None,
                    0f
                );
            }
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
            float pulse = 0.5f + 0.25f * EaseFunctions.EaseInOutCubic(vignettePower) + MathF.Sin(vignettePulse) * 0.08f;
            Color vignetteColor = new Color(0, 0, 0, (byte)(pulse * 255));
            spriteBatch.Draw(vignetteTexture.Value,
                new Rectangle(0, 0, Main.screenWidth, Main.screenHeight),
                null, vignetteColor, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}