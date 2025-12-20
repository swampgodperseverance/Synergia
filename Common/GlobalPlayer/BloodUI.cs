using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalPlayer {
    public class BloodUI : ModPlayer {
        public void DrawBloodUI(SpriteBatch spriteBatch) {
            BloodPlayer bPlayer = Player.GetModPlayer<BloodPlayer>();
            if (bPlayer.activeBloodUI) {
                int drawX = (int)(Player.position.X - Main.screenPosition.X - 110);
                int drawY = (int)(Player.position.Y - Main.screenPosition.Y + 5);
                Vector2 drawPos = new(drawX, drawY);
                Texture2D barTextura = ModContent.Request<Texture2D>(Synergia.GetUIElementName("BloodUI")).Value;
                Texture2D fullBarTextura = ModContent.Request<Texture2D>(Synergia.GetUIElementName("BloodUIBar")).Value;
                spriteBatch.Draw(barTextura, drawPos, Color.White);
                float progress = (float)BloodPlayer.hitForActiveBloodBuff <= 0 ? 1f : (float)bPlayer.currentHit / (float)BloodPlayer.hitForActiveBloodBuff;
                progress = MathHelper.Clamp(progress, 0f, 1f);
                int barWidth = (int)(fullBarTextura.Width * progress);
                spriteBatch.Draw(fullBarTextura, new Vector2(drawPos.X + 7f, drawPos.Y + 7f), new Rectangle(0, 0, barWidth, fullBarTextura.Height), Color.White);
            }
        }
    }
}