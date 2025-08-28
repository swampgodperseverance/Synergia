using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Content.Projectiles.Hostile;

namespace Synergia.Content.Projectiles.Hostile
{
	public class MicroOrb : ModProjectile
	{
		private const int MaxLifetime = 60 * 8; 
		private float fadeOutPercent => 1f - (float)Projectile.timeLeft / MaxLifetime;

		private bool spawnedShard = false; 

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = false;
			Projectile.hostile = true; 
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

            if (!spawnedShard && Projectile.timeLeft <= MaxLifetime - 70 && Projectile.timeLeft > MaxLifetime - 90)
            {
                float shakeAmount = 2f; 
                Projectile.position += Main.rand.NextVector2Circular(shakeAmount, shakeAmount);
            }

            if (Projectile.timeLeft < 60 * 2)
            {
                Projectile.alpha = (int)(255 * (1f - (float)Projectile.timeLeft / (60 * 2)));
            }

            if (Main.rand.NextBool(3))
            {
                Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(10, 10),
                    ModContent.DustType<JadeDust>(),
                    Projectile.velocity * 0.2f,
                    100, default, 0.7f
                );
            }

            if (!spawnedShard && Projectile.timeLeft <= MaxLifetime - 90)
            {
                spawnedShard = true;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Player closestPlayer = Main.player[Player.FindClosest(Projectile.Center, Projectile.width, Projectile.height)];

                    if (closestPlayer != null && closestPlayer.active && !closestPlayer.dead)
                    {
                        Vector2 direction = closestPlayer.Center - Projectile.Center;
                        direction.Normalize();
                        direction *= 6f;

                        for (int i = 0; i < 20; i++)
                        {
                            Vector2 dustVelocity = Main.rand.NextVector2Circular(3f, 3f);
                            Dust.NewDust(
                                Projectile.Center,
                                0, 0,
                                ModContent.DustType<JadeDust>(),
                                dustVelocity.X,
                                dustVelocity.Y,
                                100,
                                default,
                                1.2f
                            );
                        }

                        Projectile.NewProjectile(
                            Projectile.GetSource_FromAI(),
                            Projectile.Center,
                            direction,
                            ModContent.ProjectileType<JadeShard2>(),
                            Projectile.damage / 2,
                            Projectile.knockBack,
                            Projectile.owner
                        );

                        Projectile.Kill();
                    }
                }
            }
        }

	}
}
