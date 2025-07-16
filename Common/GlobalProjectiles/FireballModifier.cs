using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

public class FireballMadifier : GlobalProjectile
{
    public override bool InstancePerEntity => true;

    public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
    {
        if (projectile.type == ProjectileID.Fireball)
        {
            // ѕросто уничтожаем снар€д при столкновении с блоками
            projectile.Kill();
            return false; // Ќе обрабатывать стандартный отскок
        }

        return base.OnTileCollide(projectile, oldVelocity);
    }
}