using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace Vanilla.Content.Projectiles.Friendly
{
	public class OculithSkull : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;

			Projectile.hostile = false;
			Projectile.friendly = true;

			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;

			Projectile.penetrate = 1; 

			Projectile.light = 0.1f;

			Projectile.timeLeft = 60 * 5; 
			
			Projectile.DamageType = DamageClass.Throwing;
			Projectile.damage = 20;
		}

		public override void AI() {
			if (++Projectile.frameCounter >= 8)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Projectile.type])
					Projectile.frame = 0;
			}

			NPC target = null;
			float minDistance = float.MaxValue;

			foreach (NPC npc in Main.npc)
			{
					if (npc.CanBeChasedBy(this) && !npc.friendly && npc.lifeMax > 5)
					{
						float distance = Vector2.Distance(Projectile.Center, npc.Center);
						if (distance < minDistance)
						{
							minDistance = distance;
							target = npc;
						}
					}
			}

			if (target != null)
			{
					Vector2 direction = target.Center - Projectile.Center;
					direction.Normalize();
					direction *= 6f;

					Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction, 0.025f);
					Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			}

			if (Main.rand.NextBool(3))
			{
				Dust.NewDustPerfect(
					Projectile.Center + Main.rand.NextVector2Circular(10, 10),
					DustID.Shadowflame,
					Projectile.velocity * 0.2f,
					100, default, 1.5f
				).noGravity = true;
			}

			Lighting.AddLight(Projectile.Center, 0.5f, 0.4f, 0.9f);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.ShadowFlame, 180);
			
			for (int i = 0; i < 15; i++)
			{
				Dust.NewDustPerfect(
					Projectile.Center,
					DustID.Shadowflame,
					Main.rand.NextVector2Circular(5f, 5f),
					100, default, 2f
				).noGravity = true;
			}
			
			SoundEngine.PlaySound(SoundID.NPCHit2, Projectile.position);
		}

		public override void OnKill(int timeLeft) {
			for (int i = 0; i < 30; i++)
			{
				Dust.NewDustPerfect(
					Projectile.Center,
					DustID.Shadowflame,
					Main.rand.NextVector2Circular(7f, 7f),
					100, default, 2.5f
				).noGravity = true;
			}
			SoundEngine.PlaySound(SoundID.NPCHit2, Projectile.position);
		}

		public override Color? GetAlpha(Color lightColor)
			=> new Color(255, 255, 255, 200);
	}
}