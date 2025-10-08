using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Synergia.Common.ModConfigs;
using Terraria.Audio;

namespace Synergia.Common.GlobalNPCs.AI
{
    public class PapuanWizardUpgrades : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public static bool HardAIEnabled = true;

        private int teleportTimer = 0;
        private int pulseTimer = 0;
        private int dustPulse = 0;
        private bool initialized = false;

        private const string TARGET_NAME = "PapuanWizard";

        private bool IsTarget(NPC npc)
        {
            if (npc.ModNPC != null)
            {
                if (npc.ModNPC.Name == TARGET_NAME) return true;
                if ((npc.ModNPC.GetType().FullName ?? "").EndsWith(TARGET_NAME, StringComparison.Ordinal))
                    return true;
            }
            return false;
        }

        public override void SetDefaults(NPC npc)
        {
            if (!initialized && IsTarget(npc))
            {
                initialized = true;
                npc.lifeMax = (int)(npc.lifeMax * 1.4f);
                npc.damage = (int)(npc.damage * 1.4f);
                npc.defense += 20;
            }
        }

        public override void AI(NPC npc)
        {
            if (!PapuanWizardUpgrades.HardAIEnabled)
    return; 

            if (!IsTarget(npc))
                return;

            teleportTimer++;
            pulseTimer++;
            dustPulse++;

            Player player = Main.player[npc.target];
            if (player == null || !player.active)
                player = Main.LocalPlayer;

            var source = npc.GetSource_FromThis();

            if (pulseTimer >= 60)
            {
                pulseTimer = 0;
                if (npc.ai[0] > 0) npc.ai[0] += 4;
                if (npc.ai[1] > 0) npc.ai[1] += 4;
                if (npc.ai[2] > 0) npc.ai[2] += 4;
                SpawnPulseDust(npc.Center, 20, 169);
            }

            if (teleportTimer >= 720) 
            {
                teleportTimer = 0;

                for (int i = 0; i < 5; i++)
                {
                    Vector2 dir = player.Center - npc.Center;
                    dir.Normalize();
                    dir = dir.RotatedByRandom(MathHelper.ToRadians(5)); 
                    dir *= 14f; 

                    Projectile.NewProjectile(
                        source,
                        npc.Center,
                        dir,
                        ModContent.ProjectileType<Bismuth.Content.Projectiles.SandWaveEnemyP>(),
                        36,   
                        6f,   
                        Main.myPlayer
                    );

                    SoundEngine.PlaySound(SoundID.Item20, npc.Center);
                    SpawnPulseDust(npc.Center, 24, 169);


                    for (int d = 0; d < 8; d++)
                    {
                        int dust = Dust.NewDust(npc.Center, 10, 10, 169);
                        Main.dust[dust].velocity = dir.RotatedByRandom(0.3f) * Main.rand.NextFloat(0.4f, 1.2f);
                        Main.dust[dust].noGravity = true;
                    }
                }


                TryTeleportNearPlayer(npc, player, 200, 420);
            }

            if (dustPulse >= 10)
            {
                dustPulse = 0;
                int d = Dust.NewDust(npc.position, npc.width, npc.height, 169);
                Main.dust[d].noGravity = true;
            }
        }

        private void SpawnPulseDust(Vector2 center, int count, int dustType)
        {
            for (int i = 0; i < count; i++)
            {
                double angle = i * (Math.PI * 2) / count;
                Vector2 pos = center + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Main.rand.NextFloat(24f, 60f);
                int d = Dust.NewDust(pos, 6, 6, dustType);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = (pos - center) * 0.02f;
            }
        }

        private void TryTeleportNearPlayer(NPC npc, Player player, int minDist, int maxDist)
        {
            Vector2 oldCenter = npc.Center;
            var source = npc.GetSource_FromThis();

            for (int attempt = 0; attempt < 40; attempt++)
            {
                double ang = Main.rand.NextDouble() * Math.PI * 2;
                float dist = Main.rand.NextFloat(minDist, maxDist);
                Vector2 candidate = player.Center + new Vector2((float)Math.Cos(ang), (float)Math.Sin(ang)) * dist;
                candidate -= new Vector2(npc.width / 2, npc.height / 2);

                if (!Collision.SolidCollision(candidate, npc.width, npc.height))
                {
                    PerformTeleportEffect(npc, oldCenter, candidate, source);
                    return;
                }
            }

            PerformTeleportEffect(npc, oldCenter, player.Center + new Vector2(0, -200f), source);
        }

        private void PerformTeleportEffect(NPC npc, Vector2 oldCenter, Vector2 newPos, IEntitySource source)
        {
            SpawnPulseDust(oldCenter, 28, 169);
            SoundEngine.PlaySound(SoundID.Item8, oldCenter);

            npc.position = newPos;
            npc.velocity = Vector2.Zero;

            SpawnPulseDust(npc.Center, 36, 169);
            SoundEngine.PlaySound(SoundID.Item4, npc.Center);
        }
    }
}