using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Avalon.Projectiles.Hostile.BacteriumPrime;
using Avalon.NPCs.Bosses.PreHardmode;

namespace Synergia.Common.GlobalNPCs.AI
{
    public class BacteriumPrimeBuff : ModSystem
    {
        public static bool Enabled = false;

        private const float SpeedMultiplier = 1.3f;  
        private const float DamageMultiplier = 1.5f; 
        private const float AttackRateMultiplier = 1.9f; 
        private const float MaxSpeed = 15f; 
        private const float MaxDistance = 700f; 
        private const int SporeAttackCooldownMin = 240; 
        private const int SporeAttackCooldownMax = 420; 
        private int sporeAttackTimer = 0;

        public override void Load()
        {
            On_NPC.AI += BuffBacteriumPrimeAI;
        }

        public override void Unload()
        {
            On_NPC.AI -= BuffBacteriumPrimeAI;
        }

        private void BuffBacteriumPrimeAI(On_NPC.orig_AI orig, NPC npc)
        {
            orig(npc);

            if (!Enabled)
                return;

            if (npc.type != ModContent.NPCType<BacteriumPrime>() || !npc.active)
                return;

            if (npc.velocity.Length() > 0.1f)
            {
                npc.velocity = Vector2.Normalize(npc.velocity) *
                               MathHelper.Clamp(npc.velocity.Length() * SpeedMultiplier, 0, MaxSpeed);
            }
            KeepBossNearTarget(npc);
            BuffPhase1Attacks(npc);
            BuffPhase2Attacks(npc);
            BuffSporeSeedAttack(npc);
        }

        private void BuffSporeSeedAttack(NPC npc)
        {
            // below 50% hp
            if (npc.life >= npc.lifeMax * 0.5f)
            {
                sporeAttackTimer = 0;
                return;
            }

            if (sporeAttackTimer > 0)
            {
                sporeAttackTimer--;
                return;
            }

            Player target = Main.player[npc.target];
            if (!target.active || target.dead) return;

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 baseDirection = npc.DirectionTo(target.Center);
                float spreadAngle = MathHelper.ToRadians(70);
                for (int i = 0; i < 7; i++)
                {
                    float angle = MathHelper.Lerp(-spreadAngle / 2, spreadAngle / 2, i / 6f);
                    Vector2 velocity = baseDirection.RotatedBy(angle) * 8f;
            
                    Projectile.NewProjectile(
                        npc.GetSource_FromAI(),
                        npc.Center,
                        velocity,
                        ModContent.ProjectileType<SporeSeed>(),
                        (int)(npc.damage * 0.7f * DamageMultiplier),
                        0,
                        -1
                    );
                }
            }

            sporeAttackTimer = Main.rand.Next(SporeAttackCooldownMin, SporeAttackCooldownMax);
        }

        private void KeepBossNearTarget(NPC npc)
        {
            Player target = Main.player[npc.target];
            if (!target.active || target.dead) return;

            float distance = Vector2.Distance(npc.Center, target.Center);

            if (distance > MaxDistance)
            {
                Vector2 direction = npc.DirectionTo(target.Center);
                npc.velocity = Vector2.Lerp(npc.velocity, direction * 12f, 0.1f);

                npc.Center = Vector2.Lerp(npc.Center, target.Center, 0.02f);
            }
        }

        private void BuffPhase1Attacks(NPC npc)
        {
            if (npc.ai[3] == 0)
            {
                if (npc.ai[0] % (15 / AttackRateMultiplier) == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Projectile.NewProjectile(
                            npc.GetSource_FromAI(),
                            npc.Center + Main.rand.NextVector2Circular(npc.width * 0.7f, npc.height * 0.7f),
                            Main.rand.NextVector2Circular(3f, 3f),
                            ModContent.ProjectileType<BacteriumGas>(),
                            (int)(12 * DamageMultiplier),
                            0,
                            -1,
                            1
                        );
                    }
                }

                if (npc.ai[0] > 600 / AttackRateMultiplier)
                {
                    npc.velocity = npc.Center.DirectionTo(Main.player[npc.target].Center).RotatedBy(npc.localAI[0]) *
                                   MathHelper.Clamp(npc.Center.Distance(Main.player[npc.target].position) * 0.04f, 4, 10);
                }
            }
        }

        private void BuffPhase2Attacks(NPC npc)
        {
            if (npc.ai[3] == 60)
            {
                Player target = Main.player[npc.target];
                if (!target.active || target.dead) return;

                if (npc.ai[1] > 150 / AttackRateMultiplier && npc.ai[1] % (15 / AttackRateMultiplier) == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 10; i++) 
                        {
                            Vector2 velocity = npc.Center.DirectionTo(target.Center)
                                .RotatedByRandom(0.4f) * Main.rand.NextFloat(6f, 11f);

                            Projectile.NewProjectile(
                                npc.GetSource_FromAI(),
                                npc.Center,
                                velocity,
                                ModContent.ProjectileType<CorrosiveMucus>(),
                                (int)(npc.damage * 0.6f * DamageMultiplier), 
                                0,
                                -1
                            );
                        }
                    }
                }

                if (npc.localAI[0] <= 0)
                {
                    for (int i = 0; i < 10; i++) 
                    {
                        Projectile.NewProjectile(
                            npc.GetSource_FromAI(),
                            npc.Center,
                            new Vector2(Main.rand.NextFloat(6f, 10f), 0)
                                .RotatedBy(npc.Center.DirectionTo(target.Center).ToRotation() +
                                           MathHelper.Pi / (10 * 2) - (i * MathHelper.Pi / (10 * 2))),
                            ModContent.ProjectileType<BouncyBoogerBall>(),
                            (int)(20 * DamageMultiplier), 
                            0,
                            -1
                        );
                    }

                    npc.localAI[0] = 360; 
                }
                else
                {
                    npc.localAI[0]--;
                }
                if (npc.localAI[1] <= 0)
                {
                    Vector2 dashVelocity = npc.DirectionTo(target.Center) * 18f; 
                    npc.velocity = dashVelocity;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 5; i++) 
                        {
                            Projectile.NewProjectile(
                                npc.GetSource_FromAI(),
                                npc.Center + Main.rand.NextVector2Circular(20f, 20f),
                                Main.rand.NextVector2Circular(3f, 3f),
                                ModContent.ProjectileType<BouncyBoogerBall>(),
                                (int)(15 * DamageMultiplier),
                                0,
                                -1
                            );
                        }
                    }

                    npc.localAI[1] = 420; 
                }
                else
                {
                    npc.localAI[1]--;
                }
            }
        }
    }
}