using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Synergia.Content.Projectiles.Friendly
{
    public class AirflowProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            var proj = Projectile;

            proj.width = 20;
            proj.height = 20;
            proj.aiStyle = 3;
            proj.friendly = true;
            proj.DamageType = DamageClass.Throwing;
            proj.penetrate = -1;
            proj.timeLeft = 60 * 5;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.BlueCrystalShard,
                    Projectile.velocity.X * 0.2f,
                    Projectile.velocity.Y * 0.2f,
                    100,
                    default,
                    1.2f
                );
            }
        }
    }
}
