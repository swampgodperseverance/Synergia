using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Common.GlobalPlayer.ThrowingWeapons;
using Synergia.Dataset;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.PlayerLayers {
    //public class ThrowingLayers : PlayerDrawLayer {
    //    Step_By_StepAnimationData throwAnimation;
    //    Vector2 shake = Vector2.Zero;
    //    Vector2 velocity = Vector2.Zero;
    //    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.FrontAccFront);
    //    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.active && !drawInfo.drawPlayer.dead;
    //    protected override void Draw(ref PlayerDrawSet drawInfo) {
    //        Player player = drawInfo.drawPlayer;
    //        ThrowingPlayer throwing = player.GetModPlayer<ThrowingPlayer>();
    //        if (throwing.ActiveUI) {
    //            if (throwing.comboCount == 5) {
    //                shake = Main.rand.NextVector2Circular(1f, 1f); 
    //            }
    //            if (throwing.comboCount >= 0 && throwing.comboCount <= 4) {
    //                float velocityY = (float)Math.Sin(Main.GameUpdateCount * 0.08f) * 3f;
    //                velocity = new Vector2(0, velocityY);
    //            }
    //            Texture2D texture = ModContent.Request<Texture2D>("Synergia/Content/Projectiles/Thrower/ThrowerInterface1").Value;
    //            throwAnimation.Init(1, 6); // <- 6 сколько фреймов, 1 колонок
    //            throwAnimation.Update(throwing.comboCount); // <- тут меняем фрейм от количества чего то там я не помну. И только Byte 
    //            throwAnimation.GetSource(texture); // <- сама текстура
    //            int drawX = (int)(drawInfo.Position.X + player.width / 2f - Main.screenPosition.X - 40);
    //            int drawY = (int)(drawInfo.Position.Y + player.height / 2f + 89 - Main.screenPosition.Y - 45);
    //            Vector2 drawPos = new(drawX, drawY);
    //            drawPos += shake;
    //            drawPos += velocity;
    //            Color color = Lighting.GetColor((int)(player.Center.X / 16), (int)(player.Center.Y / 16)) * 1;
    //            DrawData data = new(texture, drawPos, throwAnimation.GetSource(texture), color, 0f, Vector2.Zero, 0.95f, SpriteEffects.None, 0) // <- вот тут рисуем
    //            drawInfo.DrawDataCache.Add(data); // <- тут регистрируем
    //        }
    //    }
    //}
}