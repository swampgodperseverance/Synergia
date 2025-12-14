using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Trails;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
	// Token: 0x020000DF RID: 223
	public class SpaceCowboyRework : ModProjectile
	{
		// Token: 0x06000483 RID: 1155 RVA: 0x0002AD01 File Offset: 0x00028F01
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 35;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 3;
		}
		private Vector2 startVelocity;
		private bool velocityInitialized;
		private int accelTimer;
		private const int AccelDuration = 20;
		// Token: 0x06000484 RID: 1156 RVA: 0x00031A18 File Offset: 0x0002FC18
		public override bool PreDraw(ref Color lightColor)
		{
			default(SpaceCowboyShot).Draw(base.Projectile);
			return true;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00031A3C File Offset: 0x0002FC3C
		public override void SetDefaults()
		{
			base.Projectile.width = 14;
			base.Projectile.height = 14;
			base.Projectile.aiStyle = 0;
			base.Projectile.friendly = true;
			base.Projectile.penetrate = 7;
			base.Projectile.tileCollide = true;
			base.Projectile.timeLeft = 700;
			base.Projectile.DamageType = DamageClass.Magic;
			base.Projectile.extraUpdates = 1;
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0002ADCF File Offset: 0x00028FCF
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
		
		}

			public override void AI()
		{
			if (!velocityInitialized)
			{
				startVelocity = Projectile.velocity;
				velocityInitialized = true;
			}

			if (accelTimer < AccelDuration)
			{
				accelTimer++;

				float t = accelTimer / (float)AccelDuration; 
				float ease = Helpers.EaseFunctions.EaseInQuint(t);

				float speedMultiplier = MathHelper.Lerp(0.4f, 2.2f, ease);
				Projectile.velocity = startVelocity * speedMultiplier;
			}

			Projectile.rotation =
				(float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X)
				+ MathHelper.ToRadians(90f);

			int dustIndex = Dust.NewDust(
				Projectile.position,
				Projectile.width,
				Projectile.height,
				174,
				0f,
				0f,
				Projectile.alpha,
				default,
				0.5f
			);

			Dust dust = Main.dust[dustIndex];
			dust.noGravity = true;
			dust.color = new Color(0, 220, 255);
		}



		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(Color.White);
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 2; i++)
			{
				Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 174, 0f, 0f, base.Projectile.alpha, default(Color), 1f);
			}
		}
	}
}
