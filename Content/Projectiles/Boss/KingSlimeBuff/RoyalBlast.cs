using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Synergia.Content.Projectiles.Boss.KingSlimeBuff
{
	public class RoyalBlast : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_126";
		public override void SetDefaults() {
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.aiStyle = -1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 180;
		}
		public override void AI() {
			for(int i = 0; i < 4; i++) {
				int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 86 + (int)Projectile.ai[1], Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 50, default(Color), Projectile.scale * 1.2f);
				Main.dust[d].noGravity = true;
				Dust dust = Main.dust[d];
				dust.scale *= 1.25f;
				dust = Main.dust[d];
				dust.velocity *= 0.5f;
			}
			if(Projectile.ai[0] > 0) {
				double rotOff = (Main.player[(int)Projectile.ai[0] - 1].Center - Projectile.Center).ToRotation() - Projectile.velocity.ToRotation();
				if(rotOff > MathHelper.Pi) rotOff -= MathHelper.Pi * 2.0;
				if(rotOff < -MathHelper.Pi) rotOff += MathHelper.Pi * 2.0;
				double turnSpeed = MathHelper.ToRadians(Projectile.velocity.Length() * 0.3f);
				if(System.Math.Abs(rotOff) < turnSpeed) Projectile.ai[0] = 0f;
				Projectile.velocity = Projectile.velocity.RotatedBy(rotOff * turnSpeed);
			}
			Projectile.tileCollide = Projectile.ai[0] >= 0f;
		}
		public override void Kill(int timeLeft) {
			for(int a = 0; a < 36; a++) {
				Dust dust = Dust.NewDustPerfect(Projectile.Center, 86 + (int)Projectile.ai[1], MathHelper.ToRadians(a * 10f).ToRotationVector2() * Projectile.velocity.Length(), 100, default(Color), 1.2f);
				dust.noGravity = true;
				dust.scale *= 1.25f;
				dust.velocity *= 0.5f;
			}
		}
		public override Color? GetAlpha(Color newColor) => Color.Transparent;
	}
}