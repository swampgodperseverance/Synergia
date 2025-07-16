using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

public class AcceleratingFireball : GlobalProjectile
{
    public override bool InstancePerEntity => true;

    public override void AI(Projectile projectile)
    {
        if (projectile.type == ProjectileID.Fireball)
        {
            // Увеличиваем таймер
            projectile.localAI[0]++;

            // Плавное ускорение в течение первых 60 тиков (1 секунда)
            if (projectile.localAI[0] <= 60f)
            {
                projectile.velocity *= 1.03f; // плавное увеличение скорости
            }
        }
    }
}