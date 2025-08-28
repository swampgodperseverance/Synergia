using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile
{
    public class BloodAttackMarker : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.timeLeft = 60; 
            Projectile.hide = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(10, 10),
                    DustID.LifeDrain,
                    Vector2.Zero,
                    0,
                    Color.Red,
                    1.5f
                );
                dust.noGravity = true;
            }
        }
    }
}