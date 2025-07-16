using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static Terraria.Audio.SoundEngine;

namespace Vanilla.Content.Projectiles
{
	public class NaquadahProjectile : ModProjectile
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
			proj.timeLeft = 60 * 5; // live 5 seconds
			proj.ignoreWater = true;
			proj.tileCollide = true;

			AIType = ProjectileID.BoneJavelin;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			// On hit dusts
			for (int i = 0; i < 4; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
				DustID.GemAmethyst, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 
				100, default, 1.2f);
			}

			PlaySound(SoundID.Dig, Projectile.position);
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 6; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
				DustID.GemAmethyst, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			}
			PlaySound(SoundID.Dig, Projectile.position);
		}

		public override void AI()
		{
			Projectile.velocity *= 1.01f;

			if (Projectile.velocity.Length() > 20f)
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 20f;
		}
	}
}
