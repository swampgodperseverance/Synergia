using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile
{
    public class PlanteraFlower : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 18000;
        }

        private int timer;
        private bool planteraAlive = true;

        public override void AI()
        {
            if (!NPC.AnyNPCs(NPCID.Plantera) && !NPC.AnyNPCs(NPCID.PlanterasTentacle))
            {
                planteraAlive = false;
                Projectile.Kill();
                return;
            }

            Projectile.rotation += 0.05f;
            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
            }

            Player target = Main.player[Player.FindClosest(Projectile.Center, Projectile.width, Projectile.height)];
            float speed = 1.0f; 

            if (target != null && target.active && !target.dead)
            {
                Vector2 direction = target.Center - Projectile.Center;
                float distance = direction.Length();

                if (distance > 10f) 
                {
                    direction.Normalize();
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * speed, 0.05f); 
                }
                else
                {
                    Projectile.velocity *= 0.9f; 
                }
            }

            Projectile.position += Projectile.velocity;

            timer++;
            if (timer % 50 == 0 && Main.netMode != NetmodeID.MultiplayerClient && planteraAlive)
            {
                Vector2 spawnPos = Projectile.Center;

                Projectile.NewProjectile(
                    Projectile.GetSource_FromAI(),
                    spawnPos,
                    Vector2.Zero,
                    ModContent.ProjectileType<PoisonDnaSeed>(),
                    20,
                    0f,
                    Main.myPlayer,
                    1f); 

                Projectile.NewProjectile(
                    Projectile.GetSource_FromAI(),
                    spawnPos,
                    Vector2.Zero,
                    ModContent.ProjectileType<PoisonDnaSeed>(),
                    20,
                    0f,
                    Main.myPlayer,
                    -1f); 
            }
        }
    }
}