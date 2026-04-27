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
            public float Depth;
            public float Rotation;
            public float Alpha;
            public int Time;
            public int Lifetime;
            public Color Color;

            public InfernoCinder(int lifetime, float depth, Color color, Vector2 pos, Vector2 vel)
            {
                Lifetime = lifetime;
                Depth = depth;
                Color = color;
                Position = pos;
                Velocity = vel;
                Scale = Main.rand.NextFloat(0.6f, 1.4f);
                Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                Alpha = 1f;
            }
        }

        private static readonly List<InfernoCinder> cinders = new();

        private static Asset<Texture2D> bg;
        private static Asset<Texture2D> cinderTex;

        public override string DisplayName => "Dante's Inferno";

        public override Asset<Texture2D> Logo =>
            ModContent.Request<Texture2D>("Terraria/Images/UI/CharCreation/PanelGrayscale");

        public override void Load()
        {
            bg = ModContent.Request<Texture2D>("Synergia/Menu/DantesInferno");
            cinderTex = ModContent.Request<Texture2D>("Synergia/Menu/InfernoDust");
        }

        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter,
            ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            Texture2D background = bg.Value;
            Texture2D tex = cinderTex.Value;

            spriteBatch.Draw(background, Vector2.Zero, null, Color.White, 0f,
                Vector2.Zero,
                new Vector2(Main.screenWidth / (float)background.Width,
                            Main.screenHeight / (float)background.Height),
                SpriteEffects.None, 0f);

            for (int i = 0; i < 2; i++)
            {
                if (cinders.Count < 140 && Main.rand.NextBool(3))
                {
                    Vector2 pos = new(
                        Main.rand.NextFloat(0, Main.screenWidth),
                        Main.screenHeight + 40f
                    );

                    Vector2 vel = new(
                        Main.rand.NextFloat(-0.5f, 0.5f),
                        Main.rand.NextFloat(-4f, -1.5f)
                    );

                    float depth = Main.rand.NextFloat(1f, 4f);

                    Color color = Color.Lerp(
                        Color.DarkRed,
                        Color.OrangeRed,
                        Main.rand.NextFloat()
                    );

                    cinders.Add(new InfernoCinder(
                        Main.rand.Next(180, 320),
                        depth,
                        color,
                        pos,
                        vel
                    ));
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

                spriteBatch.Draw(
                    tex,
                    c.Position,
                    null,
                    c.Color * c.Alpha,
                    c.Rotation,
                    tex.Size() / 2f,
                    c.Scale,
                    SpriteEffects.None,
                    0f
                );

                c.Time++;

                if (c.Time >= c.Lifetime || c.Position.Y < -50f)
                    cinders.RemoveAt(i);
            }

            drawColor = Color.White;
            Main.time = 27000;
            Main.dayTime = true;

            return false;
        }
    }
}