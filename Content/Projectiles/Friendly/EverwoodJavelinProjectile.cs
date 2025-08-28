using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Synergia.Content.Buffs;
using static Terraria.Audio.SoundEngine;

namespace Synergia.Content.Projectiles.Friendly
{
	public class EverwoodJavelinProjectile : ModProjectile
	{
		public override void SetDefaults()
		{
			var proj = Projectile;

			proj.width = 14;
			proj.height = 14;
			proj.aiStyle = 1;
			proj.friendly = true;
			proj.DamageType = DamageClass.Throwing;
			proj.penetrate = 1; 
			proj.timeLeft = 60 * 5;
			proj.ignoreWater = true;
			proj.tileCollide = true;

			AIType = ProjectileID.BoneJavelin;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			for (int i = 0; i < 8; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
				DustID.HallowedPlants, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 
				100, default, 1.2f);
			}

			if (Main.rand.NextFloat() < 0.2f)
			{
				if (target.boss)
				{
					target.AddBuff(ModContent.BuffType<EverwoodJavelinDebuff>(), 270);
				}
				target.AddBuff(ModContent.BuffType<EverwoodJavelinDebuff>(), 90);
			}

			PlaySound(SoundID.Dig, Projectile.position);
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
				DustID.HallowedPlants, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			}
			PlaySound(SoundID.Dig, Projectile.position);
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center + new Vector2(0, -10f), 0.05f, 0.1f, 0.9f);
		}
	}
}
