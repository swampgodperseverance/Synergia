using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.AI;
using static Terraria.ModLoader.ModContent;

namespace Vanilla.Content.Projectiles.Aura
{
	[ExtendsFromMod("ValhallaMod")]
	public class OcramAuraSpawn : AuraDamageAI
	{
		public override void SetDefaults()
		{
			AIType = 0;
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = 90;
			Projectile.penetrate = -1;
		}

		//Use CustomAI instead of AI() to not override killing this projectile outside of aura's border
		public override void CustomAI()
		{
			//Projectile.rotation += 0.2f;
			int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CorruptTorch, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default, 1.2f);
			Main.dust[dustIndex].velocity += Projectile.velocity * 0.1f;
			Main.dust[dustIndex].velocity *= 0.1f;
			Main.dust[dustIndex].noLight = false;
			Main.dust[dustIndex].noGravity = true;

			for (int i = 0; i < 8; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
				DustID.CorruptTorch, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f,
				100, default, 1.2f);
			}

			Projectile.velocity = Projectile.velocity.RotatedBy(0.02f);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			for (int i = 0; i < 4; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
				DustID.CorruptTorch, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 
				100, default, 1.2f);
			}
			target.AddBuff(BuffID.ShadowFlame, 90);
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void OnKill(int timeLeft)
		{
			for (int num543 = 0; num543 < 10; ++num543)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CorruptTorch, Projectile.velocity.X * 0.01f, Projectile.velocity.Y * 0.01f, 150, default, 1.2f);
			}
		}
	}
}