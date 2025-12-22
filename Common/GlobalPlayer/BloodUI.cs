using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Synergia.Helpers;

namespace Synergia.Common.GlobalPlayer {
    public class BloodUI : ModPlayer {
        public void DrawBloodUI(SpriteBatch spriteBatch) {
            BloodPlayer bPlayer = Player.GetModPlayer<BloodPlayer>();
            if (bPlayer.activeBloodUI) {
                Vector2 drawPos = UIHelper.PlayerPos(-30, 30);
                Texture2D barTextura = Request<Texture2D>(Synergia.GetUIElementName("BloodUI")).Value;
                Texture2D fullBarTextura = Request<Texture2D>(Synergia.GetUIElementName("BloodUIBar")).Value;
                spriteBatch.Draw(barTextura, drawPos, Color.White);
                float progress = (float)BloodPlayer.hitForActiveBloodBuff <= 0 ? 1f : (float)bPlayer.currentHit / (float)BloodPlayer.hitForActiveBloodBuff;
                progress = MathHelper.Clamp(progress, 0f, 1f);
                int barWidth = (int)(fullBarTextura.Width * progress);
                spriteBatch.Draw(fullBarTextura, new Vector2(drawPos.X + 7f, drawPos.Y + 7f), new Rectangle(0, 0, barWidth, fullBarTextura.Height), Color.White);
            }
        }
    }
}