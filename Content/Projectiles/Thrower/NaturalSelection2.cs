using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Avalon.Dusts;

namespace Synergia.Content.Projectiles.Thrower
{
    public class NaturalSelection2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.aiStyle = 3; 
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {

            if (Main.rand.NextBool(5))
            {
                Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    ModContent.DustType<ContagionDust>(),
                    Projectile.velocity.X * 0.2f,
                    Projectile.velocity.Y * 0.2f,
                    100,
                    default,
                    1.2f
                );
            }

            if (Main.netMode != NetmodeID.MultiplayerClient && Main.rand.NextBool(45))
            {
                Vector2 velocity = Main.rand.NextVector2CircularEdge(1f, 1f) * 6f;

                Projectile.NewProjectile(
                    Projectile.GetSource_FromAI(),
                    Projectile.Center,
                    velocity,
                    ModContent.ProjectileType<RangedProjectiles.Sludge>(),
                    Projectile.damage / 2,
                    1f,
                    Projectile.owner
                );
            }
        }

    }
}