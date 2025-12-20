using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using NewHorizons.Content.Projectiles.Throwing;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Thrower
{
	public class HeavyStone : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;

			Projectile.friendly = true;
			Projectile.hostile = false;

			Projectile.penetrate = 1;
			Projectile.timeLeft = 240;

			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;

			Projectile.DamageType = DamageClass.Throwing;
		}

		public override void AI()
		{
			Projectile.velocity.Y += 0.6f;
			if (Projectile.velocity.Y > 16f)
				Projectile.velocity.Y = 16f;

			Projectile.rotation += Projectile.velocity.X * 0.08f;

			if (Projectile.velocity.Y == 0f)
			{
				Projectile.velocity.X *= 0.97f; 
				if (Math.Abs(Projectile.velocity.X) < 0.1f)
					Projectile.velocity.X = 0f;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			
			if (oldVelocity.Y > 2f)
				Projectile.velocity.Y = -oldVelocity.Y * 0.25f;

			if (Math.Abs(oldVelocity.X) > 1f)
				Projectile.velocity.X = oldVelocity.X * 0.6f;

			return false; 
		}

		public override void OnKill(int timeLeft)
		{
	
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

		
			for (int i = 0; i < 25; i++)
			{
				Dust.NewDust(
					Projectile.position,
					Projectile.width,
					Projectile.height,
					DustID.Stone,
					Main.rand.NextFloat(-4f, 4f),
					Main.rand.NextFloat(-4f, 4f)
				);
			}


			int count = Main.rand.Next(6, 10);
			for (int i = 0; i < count; i++)
			{
				Vector2 velocity = Main.rand.NextVector2Circular(6f, 6f);

				Projectile.NewProjectile(
					Projectile.GetSource_Death(),
					Projectile.Center,
					velocity,
					ModContent.ProjectileType<RockProj>(),
					Projectile.damage / 2,
					1f,
					Projectile.owner
				);
			}
		}
	}
}
