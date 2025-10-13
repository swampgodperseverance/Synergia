using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Synergia.Common
{
    public class GlobalProjectiles : GlobalProjectile
    {
        public override void PostAI(Projectile projectile)
        {
            if (projectile.type == ModContent.ProjectileType<Avalon.Projectiles.Melee.MasterSwordBeam>())
            {
                float homingRange = 400f; 
                float lerpStrength = 0.1f;

                NPC target = null;
                float distanceToTarget = homingRange;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(projectile))
                    {
                        float distance = Vector2.Distance(projectile.Center, npc.Center);
                        if (distance < distanceToTarget)
                        {
                            distanceToTarget = distance;
                            target = npc;
                        }
                    }
                }

                if (target != null)
                {
                    Vector2 desiredVelocity = (target.Center - projectile.Center).SafeNormalize(Vector2.Zero) * projectile.velocity.Length();
                    projectile.velocity = Vector2.Lerp(projectile.velocity, desiredVelocity, lerpStrength);
                }
            }
        }
    }
}