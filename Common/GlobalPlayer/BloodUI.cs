using Synergia.Helpers;
using Terraria;

namespace Synergia.Common.GlobalPlayer {
    public class BloodUI : ModPlayer {
        public void DrawBloodUI(SpriteBatch spriteBatch) {
            BloodPlayer bPlayer = Player.GetModPlayer<BloodPlayer>();
            if (bPlayer.activeBloodUI) {

                Vector2 screenPos = UIHelper.PlayerPos(0, -25);
                float uiScale = Main.UIScale;
                Texture2D barTextura = Request<Texture2D>(Reassures.Reassures.GetUIElementName("BloodUI")).Value;
                Texture2D fullBarTextura = Request<Texture2D>(Reassures.Reassures.GetUIElementName("BloodUIBar")).Value;

                spriteBatch.Draw(barTextura, screenPos, null, Color.White, 0f, barTextura.Size() * 0.5f, uiScale, SpriteEffects.None, 0);

                float progress = (float)BloodPlayer.hitForActiveBloodBuff <= 0 ? 1f : (float)bPlayer.currentHit / (float)BloodPlayer.hitForActiveBloodBuff;
                progress = MathHelper.Clamp(progress, 0f, 1f);
                int barWidth = (int)(fullBarTextura.Width * progress);

                spriteBatch.Draw(fullBarTextura, new Vector2(screenPos.X, screenPos.Y), new Rectangle(0, 0, barWidth, fullBarTextura.Height), Color.White, 0f, fullBarTextura.Size() * 0.5f, uiScale, SpriteEffects.None, 0);
            }
        }
    }
}