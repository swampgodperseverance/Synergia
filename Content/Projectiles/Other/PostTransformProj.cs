using Terraria;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Other
{
    public class PostTransformProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.damage = 0;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.alpha = 0;
            Projectile.timeLeft = 30;
        }
        public override void AI()
        {
            if (Projectile.timeLeft == 0)
            {
                Projectile.Kill();
            }
        }
    }
}