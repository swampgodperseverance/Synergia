using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Vanilla.Content.Projectiles.Friendly;

namespace Vanilla.Content.Projectiles.Friendly
{
	public class AirflowProjectile : ModProjectile
	{
		private const float FeatherSpawnRate = 40 * 2;
		private int featherTimer = 0;

		public override void SetDefaults()
		{
			var proj = Projectile;

			proj.width = 20;
			proj.height = 20;
			proj.aiStyle = 3;
			proj.friendly = true;
			proj.DamageType = DamageClass.Throwing;
			proj.penetrate = -1;
			proj.timeLeft = 60 * 5;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
				DustID.BlueCrystalShard, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1.2f);
			}
		}

		public override void AI()
		{
			featherTimer++;
			
			if (featherTimer >= FeatherSpawnRate)
			{
				featherTimer = 0;
				SpawnHarpyFeathers();
			}
		}

		private void SpawnHarpyFeathers()
		{
			for (int i = 0; i < 4; i++)
			{
				Projectile.NewProjectile(
					Projectile.GetSource_FromThis(),
					Projectile.Center,
					new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-5f, 0f)),
					ModContent.ProjectileType<AirflowFeather>(),
					20, 
					Projectile.knockBack * 0.5f,
					Projectile.owner
				);
			}
		}
	}
}
