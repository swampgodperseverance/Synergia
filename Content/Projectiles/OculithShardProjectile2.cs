using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static Terraria.Audio.SoundEngine;

namespace Vanilla.Content.Projectiles
{
	public class OculithShardProjectile2 : ModProjectile
	{
		public override void SetDefaults()
		{
			var proj = Projectile;

			proj.width = 14;
			proj.height = 14;
			proj.aiStyle = 1;
			proj.friendly = true;
			proj.DamageType = DamageClass.Throwing;
			proj.penetrate = 5; // across 5 enemy
			proj.timeLeft = 60 * 5; // live 5 seconds
			proj.ignoreWater = true;
			proj.tileCollide = true;

			AIType = ProjectileID.BoneJavelin;

			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10; // количество кадров трейла
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;      // тип трейла (0 = обычный)
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			// On hit dusts
			for (int i = 0; i < 8; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
				DustID.Shadowflame, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 
				100, default, 1.2f);
			}

			PlaySound(SoundID.Dig, Projectile.position);
		}

		public override void OnKill(int timeLeft)
		{
			Dust.NewDustPerfect(
					Projectile.Center,
					DustID.Shadowflame,
					Main.rand.NextVector2Circular(7f, 7f),
					100, default, 2.5f
				);

			PlaySound(SoundID.Dig, Projectile.position);
		}

		public override void AI()
		{
			Projectile.velocity *= 1.01f;

			if (Projectile.velocity.Length() > 20f)
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 20f;

			Lighting.AddLight(Projectile.Center, 0.5f, 0.4f, 0.9f);
		}
	}
}
