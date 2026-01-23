using Synergia.Helpers;
using System;
using Terraria;

namespace Synergia.Common.GlobalPlayer {
    public class ThrowingUI : ModPlayer {
        //Step_By_StepAnimationData throwAnimation;
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
                Texture2D texture = Request<Texture2D>(Reassures.Reassures.GetUIElementName("ThrowerInterface")).Value;
                Vector2 drawPos = UIHelper.PlayerPos(0, +100);
                drawPos += shake;
                drawPos += velocity;
                Main.spriteBatch.Draw(texture, drawPos, texture.Frame(1, 6, 0, throwing.comboCount), Color.White, 0f, texture.Size() * 0.5f, Main.UIScale, SpriteEffects.None, 0);
            }
        }
    }
}