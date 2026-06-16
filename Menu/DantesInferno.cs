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
        private float vignettePulse = 0f;

        public override string DisplayName => "Dante's Inferno";
        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("Terraria/Images/UI/CharCreation/PanelGrayscale");

        public override void Load()
        {
            bg = ModContent.Request<Texture2D>("Synergia/Menu/DantesInferno");
            cinderTex = ModContent.Request<Texture2D>("Synergia/Menu/InfernoDust");
            vignetteTex = ModContent.Request<Texture2D>("Synergia/Menu/Vignette");
        }

        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            Texture2D background = bg.Value;
            Texture2D tex = cinderTex.Value;

            spriteBatch.Draw(background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero,
                new Vector2(Main.screenWidth / (float)background.Width, Main.screenHeight / (float)background.Height),
                SpriteEffects.None, 0f);

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

            vignettePulse += 0.015f;
            float pulse = 0.75f + MathF.Sin(vignettePulse) * 0.08f;
            Color vignetteColor = new Color(0, 0, 0, (byte)(pulse * 255));
            spriteBatch.Draw(vignetteTex.Value,
                new Rectangle(0, 0, Main.screenWidth, Main.screenHeight),
                null, vignetteColor, 0f, Vector2.Zero, SpriteEffects.None, 0f);

            drawColor = Color.White;
            Main.time = 27000;
            Main.dayTime = true;
            return false;
        }
    }
}