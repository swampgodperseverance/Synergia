using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Vanilla.Content.Projectiles.Aura
{
	[ExtendsFromMod("ValhallaMod")]
	public class SuperAuraScytheCut : ModProjectile
	{
		public override void SetDefaults()
		{
			var proj = Projectile;

			proj.width = 48;
			proj.height = 48;
			proj.aiStyle = 0;
			proj.friendly = true;
			proj.penetrate = 5;
			proj.tileCollide = false;
			proj.timeLeft = 30;
			proj.DamageType = DamageClass.Summon;
			proj.alpha = 255;
			proj.extraUpdates = 4;
		}

		public override void AI()
		{
			Projectile.localAI[0]++;
			if (Projectile.localAI[0] > 0f)
			{
				for (int i = 0; i < 360; i += 15)
				{
					float angle = MathHelper.ToRadians(i);
					Vector2 circlePos = Projectile.Center + angle.ToRotationVector2() * 2f;

					Dust cursedFire = Dust.NewDustPerfect(circlePos, DustID.CursedTorch);
					cursedFire.velocity = Vector2.Zero;
					cursedFire.scale = Main.rand.NextFloat(1.0f, 1.4f);
					cursedFire.noGravity = true;
					cursedFire.fadeIn = 1.2f;
				}
			}
		}
		
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.CursedInferno, 300);
		}
	}
}