using Microsoft.Xna.Framework.Graphics;
using Synergia.Dataset;
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalPlayer {
    public class ThrowingUI : ModPlayer {
        Step_By_StepAnimationData throwAnimation;
        Vector2 shake = Vector2.Zero;
        Vector2 velocity = Vector2.Zero;
        public void DrawThrowingUI() {
            ThrowingPlayer throwing = Player.GetModPlayer<ThrowingPlayer>();
            if (throwing.ActiveUI) {
                if (throwing.comboCount == 5) {
                    shake = Main.rand.NextVector2Circular(1f, 1f);
                }
                if (throwing.comboCount >= 0 && throwing.comboCount <= 4) {
                    float velocityY = (float)Math.Sin(Main.GameUpdateCount * 0.08f) * 3f;
                    velocity = new Vector2(0, velocityY);
                }
                Texture2D texture = ModContent.Request<Texture2D>("Synergia/Content/Projectiles/Thrower/ThrowerInterface1").Value;
                throwAnimation.Init(1, 6); // <- 6 сколько фреймов, 1 колонок
                throwAnimation.Update(throwing.comboCount); // <- тут меняем фрейм от количества чего то там я не помну. И только Byte 
                throwAnimation.GetSource(texture); // <- сама текстура
                int drawX = (int)(Player.position.X + Player.width / 2f - Main.screenPosition.X - 40);
                int drawY = (int)(Player.position.Y + Player.height / 2f + 89 - Main.screenPosition.Y - 45);
                Vector2 drawPos = new(drawX, drawY);
                drawPos += shake;
                drawPos += velocity;
                Color color = Lighting.GetColor((int)(Player.Center.X / 16), (int)(Player.Center.Y / 16)) * 1;
                Main.spriteBatch.Draw(texture, drawPos, throwAnimation.GetSource(texture), color, 0f, Vector2.Zero, 0.95f, SpriteEffects.None, 0);
            }
        }
    }
}