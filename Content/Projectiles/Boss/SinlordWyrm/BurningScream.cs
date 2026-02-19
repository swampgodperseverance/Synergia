using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Synergia.Common.GlobalPlayer;

namespace Synergia.Content.Projectiles.Boss.SinlordWyrm
{
    public class BurningScream : ModProjectile
    {
        public override void SetStaticDefaults() => ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2048;

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 30;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;
            Projectile.hide = true;
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 29 && Main.myPlayer == Projectile.owner)
                Main.player[Projectile.owner].GetModPlayer<ScreenShakePlayer>().TriggerShake(6, 0.8f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.OrangeRed * ((float)Projectile.timeLeft / 90f);
            lightColor.A = 0;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Main.rand.NextFloat(MathHelper.TwoPi), texture.Size() / 2, Projectile.ai[0] / 2f * ((float)(30 - Projectile.timeLeft) / 30f), SpriteEffects.None, 0);
            return false;
        }

        public override bool? CanDamage() => false;
        public override bool ShouldUpdatePosition() => false;

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
            => overWiresUI.Add(index);
    }
}
