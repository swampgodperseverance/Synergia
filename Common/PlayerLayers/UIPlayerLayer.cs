// Code by 𝒜𝑒𝓇𝒾𝓈
using Synergia.Common.GlobalPlayer;
using System;
using Terraria;
using Terraria.DataStructures;

namespace Synergia.Common.PlayerLayers {
    public class UIPlayerLayer : PlayerDrawLayer {
        public override Position GetDefaultPosition() => PlayerDrawLayers.BeforeFirstVanillaLayer;
        protected override void Draw(ref PlayerDrawSet drawInfo) {
            Player player = drawInfo.drawPlayer;
            if (drawInfo.shadow != 0f || player.dead || player.whoAmI != Main.myPlayer) { return; }
            Vector2 Position = drawInfo.Position;
            Vector2 pos = new((int)(Position.X - Main.screenPosition.X + player.width / 2), (int)(Position.Y - Main.screenPosition.Y + (player.height / 2) - 2f * player.gravDir));
            ThrowingPlayer throwing = player.GetModPlayer<ThrowingPlayer>();
            BloodPlayer bPlayer = player.GetModPlayer<BloodPlayer>();
            if (throwing.ActiveUI) { drawInfo.DrawDataCache.Add(ThrowingUI(pos, throwing)); }
            if (bPlayer.activeBloodUI) { drawInfo.DrawDataCache.Add(BloodUI(pos, bPlayer)); }
        }
        static DrawData ThrowingUI(Vector2 pos, ThrowingPlayer throwing) {
            Texture2D texture = Request<Texture2D>(GetUIElementName("ThrowerInterface")).Value;
            Vector2 throwingPos = new(pos.X, pos.Y + 175);
            Vector2 shake = Vector2.Zero;
            Vector2 velocity = Vector2.Zero;
            if (throwing.comboCount == 5) {
                shake = Main.rand.NextVector2Circular(1f, 1f);
            }
            if (throwing.comboCount >= 0 && throwing.comboCount <= 4) {
                float velocityY = (float)Math.Sin(Main.GameUpdateCount * 0.08f) * 3f;
                velocity = new Vector2(0, velocityY);
            }
            throwingPos += shake;
            throwingPos += velocity;
            throwingPos = new Vector2((int)throwingPos.X, (int)throwingPos.Y);
            return new(texture, throwingPos, texture.Frame(1, 6, 0, throwing.comboCount, 0, 0), Color.White, 0f, texture.Size() * 0.5f, Main.UIScale, SpriteEffects.None, 0); ;
        }
        static DrawData BloodUI(Vector2 pos, BloodPlayer bPlayer) {
            Texture2D barTextura = Request<Texture2D>(GetUIElementName("BloodUI")).Value;
            Texture2D fullBarTextura = Request<Texture2D>(GetUIElementName("BloodUIBar")).Value;
            Vector2 bloodPos = new(pos.X, pos.Y + 50);
            Main.spriteBatch.Draw(barTextura, bloodPos, null, Color.White, 0f, barTextura.Size() * 0.5f, Main.UIScale, SpriteEffects.None, 0);
            float progress = (float)BloodPlayer.hitForActiveBloodBuff <= 0 ? 1f : (float)bPlayer.currentHit / (float)BloodPlayer.hitForActiveBloodBuff;
            progress = MathHelper.Clamp(progress, 0f, 1f);
            int barWidth = (int)(fullBarTextura.Width * progress);
            return new(fullBarTextura, bloodPos, new Rectangle(0, 0, barWidth, fullBarTextura.Height), Color.White, 0f, fullBarTextura.Size() * 0.5f, Main.UIScale, SpriteEffects.None, 0);
        }
    }
}