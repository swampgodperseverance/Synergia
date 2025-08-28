using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile
{
    public class Hedgehog : ModProjectile
    {
        private bool hasBounced = false;

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = 0;
            Projectile.hostile = true; 
            Projectile.friendly = false;
            Projectile.damage = 60;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.rotation += 0.3f * Projectile.direction;

            Projectile.velocity.Y += 0.3f;

            if (Main.rand.NextBool(6))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Dirt, 0, 0, 150, default, 0.8f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!hasBounced)
            {
                hasBounced = true;

                if (Projectile.velocity.Y > 0)
                {
                    Projectile.velocity.Y = -Projectile.velocity.Y * 0.8f; 
                }

                Projectile.velocity.X *= 0.8f;
                return false; 
            }
            else
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 spawnVelocity = new Vector2(Projectile.velocity.X, -5f); 

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        spawnVelocity,
                        ModContent.ProjectileType<Waternado>(),
                        Projectile.damage,
                        0f,
                        Projectile.owner
                    );
                }

                Projectile.Kill();
                return false;
            }
        }

    }
}
