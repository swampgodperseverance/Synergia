using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.BrainOfCthulhuBuff
{
	public class BrainWaves : ModProjectile
	{
		public override string Texture => "Terraria/Images/Extra_98";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Type] = 7;
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 10;
			Projectile.aiStyle = -1;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 300;
		}
		public override void AI() {
			Vector2 sineMotion = Projectile.velocity.RotatedBy(MathHelper.PiOver2) * (float)System.Math.Sin(Projectile.timeLeft / 30f * MathHelper.Pi) * (Projectile.timeLeft > 285 ? 0.25f : 0.5f);
			Projectile.position += sineMotion;
			Projectile.rotation = (Projectile.velocity + sineMotion).ToRotation() + MathHelper.PiOver2;
			if(Projectile.timeLeft < 15) Projectile.alpha += 17;
			else if(Projectile.alpha > 0) Projectile.alpha -= 17;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			for(int i = 0; i < Projectile.oldPos.Length; i++) Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + Projectile.Size * 0.5f + Vector2.UnitY * 16f - Main.screenPosition, null, new Color(255, 255, 0, 0) * MathHelper.Lerp(Projectile.Opacity, 0f, (float)i / (float)Projectile.oldPos.Length), Projectile.oldRot[i], texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}