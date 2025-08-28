using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Synergia.Content.Projectiles.Friendly
{
    public abstract class CogwormBaseProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
        }
    }
}
