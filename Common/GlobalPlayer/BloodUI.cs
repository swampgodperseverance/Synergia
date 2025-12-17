using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalPlayer {
    public class BloodUI : ModPlayer {
        public void DrawBloodUI(SpriteBatch spriteBatch) {
            BloodPlayer bPlayer = Player.GetModPlayer<BloodPlayer>();
            if (bPlayer.activeBloodUI) {
                int drawX = (int)(Player.position.X + Player.width / 2f - Main.screenPosition.X - 40);
                int drawY = (int)(Player.position.Y + Player.height / 2f + 89 - Main.screenPosition.Y - 45);
                Vector2 drawPos = new(drawX, drawY);
                Texture2D barTextura = ModContent.Request<Texture2D>(Synergia.GetUIElementName("BloodUI")).Value;
                Texture2D fullBarTextura = ModContent.Request<Texture2D>(Synergia.GetUIElementName("BloodUIBar")).Value;
                spriteBatch.Draw(barTextura, drawPos, Color.White);
                float progress = (float)BloodPlayer.hitForActiveBloodBuff == 0 ? 1f : (float)bPlayer.currentHit / (float)BloodPlayer.hitForActiveBloodBuff;
                spriteBatch.Draw(fullBarTextura, new Vector2(drawPos.X + 7f, drawPos.Y + 7f), new Rectangle(0, 0, (int)(fullBarTextura.Width * progress), fullBarTextura.Height), Color.White);
            }
        }
    }
}