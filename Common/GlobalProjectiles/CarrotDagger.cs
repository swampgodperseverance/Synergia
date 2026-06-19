using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Trails;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.Ranged.Javelins;

namespace Synergia.Common.GlobalProjectiles
{
    public class CarrotDaggerGP : GlobalProjectile
    {
        private const int MaxBounces = 1;
        private const float BounceFactor = 0.55f; 

        public override bool AppliesToEntity(Projectile projectile, bool lateInstantiation) =>
            projectile.type == ModContent.ProjectileType<CarrotDagger>();

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults(Projectile projectile)
        {
            if (!AppliesToEntity(projectile, false)) return;

            projectile.localAI[0] = 0f; 
        }

        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (!AppliesToEntity(projectile, false)) return true;

            if (projectile.localAI[0] < MaxBounces)
            {
                projectile.localAI[0] += 1f;

                if (Math.Abs(projectile.velocity.Y) > 1f)
                {
                    projectile.velocity.Y = -oldVelocity.Y * BounceFactor;
                }
                else
                {
                    projectile.velocity.Y = -4f; 
                }

                projectile.velocity.X *= 0.75f; 

                return false;
            }

            return true;
        }

    }
}