using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Reworks.Reworks2;

namespace Synergia.Common.GlobalProjectiles
{
    public class DerpYoyoGP : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        private int shootSeriesTimer = 0; 
        private int burstCounter = 0;    
        private int burstInterval = 5;    // Интервал между выстрелами
        private int burstTotal = 0;       
        private bool shootingBurst = false;

        public override void AI(Projectile projectile)
        {
            if (projectile.ModProjectile != null &&
                projectile.ModProjectile.Mod.Name == "ValhallaMod" &&
                projectile.ModProjectile.Name == "Derpfling")
            {
                shootSeriesTimer++;


                if (!shootingBurst && shootSeriesTimer >= 120)
                {
                    shootingBurst = true;
                    burstCounter = 0;
                    burstTotal = Main.rand.Next(3, 5); 
                    shootSeriesTimer = 0;
                }

                if (shootingBurst)
                {

                    if (shootSeriesTimer % burstInterval == 0 && burstCounter < burstTotal)
                    {
                        Vector2 velocity = projectile.velocity.RotatedByRandom(MathHelper.ToRadians(15));
                        Projectile.NewProjectile(
                            projectile.GetSource_FromThis(),
                            projectile.Center,
                            velocity,
                            ModContent.ProjectileType<DerpYoyoP>(),
                            projectile.damage,
                            projectile.knockBack,
                            projectile.owner
                        );

                        burstCounter++;
                    }

             
                    if (burstCounter >= burstTotal)
                    {
                        shootingBurst = false;
                        shootSeriesTimer = 0;
                    }
                }
            }
        }
    }
}
