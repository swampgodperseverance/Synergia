using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Synergia.Content.Projectiles.Friendly
{
	public class AirflowFeather : ModProjectile
	{
		private const int MaxLifetime = 60 * 8; 
		private float fadeOutPercent => 1f - (float)Projectile.timeLeft / MaxLifetime; 

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Throwing;
			Projectile.penetrate = 1;
			Projectile.timeLeft = MaxLifetime;
			Projectile.alpha = 0;
			Projectile.scale = 1f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			Projectile.velocity *= 0.98f;
			
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			
			if (Projectile.timeLeft < 60 * 2)
			{
					Projectile.alpha = (int)(255 * (1f - (float)Projectile.timeLeft / (60 * 2)));
			}
			
			if (Main.rand.NextBool(3))
			{
				Dust.NewDustPerfect(
					Projectile.Center + Main.rand.NextVector2Circular(10, 10), 
					DustID.Cloud,
					Projectile.velocity * 0.2f,
					100, default, 0.7f
				);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Color drawColor = Color.Lerp(lightColor, Color.White, 0.5f) * (1f - fadeOutPercent);
			
			Main.EntitySpriteDraw(
				Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value,
				Projectile.Center - Main.screenPosition,
				null,
				drawColor,
				Projectile.rotation,
				Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Size() / 2,
				Projectile.scale * (1f - fadeOutPercent * 0.3f),
				SpriteEffects.None,
				0
			);
			
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			for (int i = 0; i < 8; i++)
			{
				Dust.NewDust(
					Projectile.position, 
					Projectile.width, 
					Projectile.height,
					DustID.Cloud,
					Projectile.velocity.X * 0.1f, 
					Projectile.velocity.Y * 0.1f, 
					100, default, 1f
				);
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust.NewDust(
					Projectile.position, 
					Projectile.width, 
					Projectile.height,
					DustID.Cloud,
					Main.rand.NextFloat(-2f, 2f),
					Main.rand.NextFloat(-2f, 2f),
					100, default, 1.2f
				);
			}
		}
	}
}