using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Hostile;
using ValhallaMod.Projectiles.Spawn;
using Terraria.Audio;

namespace Synergia.Content.GlobalProjectiles
{
    public class WidowsWhipProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        private bool IsWidowsWhip(Projectile projectile)
        {
            return projectile.ModProjectile != null &&
                   projectile.ModProjectile.Mod?.Name == "ValhallaMod" &&
                   projectile.ModProjectile.Name == "WidowsWhip";
        }

        public override void AI(Projectile projectile)
        {
            if (!IsWidowsWhip(projectile))
                return;
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!IsWidowsWhip(projectile))
                return;

            Player owner = Main.player[projectile.owner];

            int spiderCount = Main.rand.Next(2, 5);

            for (int i = 0; i < spiderCount; i++)
            {
                Vector2 spawnPos = target.Center + Main.rand.NextVector2Circular(30f, 30f);
                Vector2 velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 5f);

                int spiderType = ModContent.ProjectileType<Spiders>();

                Projectile.NewProjectile(
                    projectile.GetSource_FromThis(),
                    spawnPos,
                    velocity,
                    spiderType,
                    damageDone / 3, 
                    0f,
                    owner.whoAmI
                );
            }

            
            for (int i = 0; i < 8; i++)
            {
                int dust = Dust.NewDust(target.position, target.width, target.height, DustID.Web, 0f, 0f, 100, default, 1.3f);
                Main.dust[dust].velocity *= 1.2f;
                Main.dust[dust].noGravity = true;
            }

            SoundEngine.PlaySound(SoundID.NPCDeath1 with { Pitch = 0.4f }, target.Center);
        }
    }
}
