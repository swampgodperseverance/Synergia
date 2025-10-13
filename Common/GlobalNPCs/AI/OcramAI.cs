    using Terraria;
    using Terraria.ModLoader;
    using Terraria.ID;
    using Terraria.Audio;
    using Microsoft.Xna.Framework;
    using System;
    using Consolaria.Content.NPCs.Bosses.Ocram;
    using Synergia.Content.Projectiles.Hostile;
    using Consolaria.Content.Projectiles.Enemies;
    using Synergia.Common.ModConfigs;

    namespace Synergia.Common.GlobalNPCs.AI
    {
        public class OcramUpgrades : ModSystem
        {
            // Configurable Parameters
            public static bool HardModeEnabled = true;
        
            private const float Phase2Trigger = 0.6f;
            private const float RageTrigger = 0.3f;
            private const int LaserDamage = 35;
            private const int ScytheDamage = 40;
            private const int SpiralScytheDamage = 45;
            private const int RageScytheDamage = 50;
            private const float MaxVelocityBoost = 1.05f;
            private const float MaxSpeed = 15f;

            public override void Load()
            {
                On_NPC.AI += EnhancedOcramAI;
            }

            public override void Unload()
            {
                On_NPC.AI -= EnhancedOcramAI;
            }

            private void EnhancedOcramAI(On_NPC.orig_AI orig, NPC npc)
            {
                orig(npc);

                if (!IsOcram(npc) || !HardModeEnabled)
                    return;

                HandlePhaseOneAttacks(npc);
                HandlePhaseTwoAttacks(npc);
                HandleSpecialSpiralAttack(npc);
                HandleRagePhase(npc);
                HandleServantSummon(npc);
            }

            private static bool IsOcram(NPC npc)
            {
                return npc.active && npc.type == ModContent.NPCType<Ocram>();
            }

            private void HandlePhaseOneAttacks(NPC npc)
            {
                if (npc.ai[0] == 0f && npc.ai[2] < 360f && npc.ai[3] % 30 == 0)
                {
                    ShootSpreadProjectiles(npc, ModContent.ProjectileType<OcramLaser1>(), 3, LaserDamage, 2f, 10f, 15f);
                }
            }

            private void HandlePhaseTwoAttacks(NPC npc)
            {
                if (npc.ai[0] == 1f && npc.ai[2] > 200f && npc.ai[2] < 600f && npc.localAI[3] % 40 == 0)
                {
                    ShootDirectProjectiles(npc, ModContent.ProjectileType<OcramScythe>(), 2, ScytheDamage, 4f, 12f);
                }
            }

        private void HandleSpecialSpiralAttack(NPC npc)
        {
            if (npc.life >= npc.lifeMax * RageTrigger && npc.life < npc.lifeMax * Phase2Trigger && npc.ai[1] == 0f && Main.rand.NextBool(1000))
            {
                npc.ai[1] = 4f;
                npc.ai[2] = npc.ai[3] = 0f;
                npc.netUpdate = true;
                SoundEngine.PlaySound(SoundID.Item84, npc.Center);
            }

            if (npc.ai[1] == 4f && npc.life >= npc.lifeMax * RageTrigger) 
            {
                float currentSpeed;
        
                if (npc.ai[2] < 180f) 
                {
                    currentSpeed = 0.5f; 
                }
                else if (npc.ai[2] < 240f) 
                {
                    float slowDownProgress = (npc.ai[2] - 180f) / 60f;
                    currentSpeed = MathHelper.Lerp(0.5f, 0.1f, slowDownProgress);
                }
                else 
                {
                    currentSpeed = 0.1f; 
                }

                npc.rotation += currentSpeed;


                if (npc.ai[2] < 180f && npc.ai[2] % 15 == 0)
                {
                    ShootRotatingScythes(npc, 4, SpiralScytheDamage, 5f, npc.rotation);
                    SoundEngine.PlaySound(SoundID.Item71, npc.Center);
                }

                CreateShadowflameDust(npc);

                npc.ai[2]++;
                if (npc.ai[2] >= 300f)
                {
                    npc.ai[1] = 0f;
                    npc.ai[2] = 0f;
                    SoundEngine.PlaySound(SoundID.Item25, npc.Center);
                }
            }
        }
            private void HandleRagePhase(NPC npc)
            {
                if (npc.life < npc.lifeMax * RageTrigger)
                {
                    npc.defense = 20;
                    npc.damage = 70;

                    if (Main.rand.NextBool(190))
                    {
                        Vector2 direction = (Main.player[npc.target].Center - npc.Center).SafeNormalize(Vector2.Zero);
                        for (int i = 0; i < 4; i++)
                        {
                            Vector2 velocity = direction.RotatedByRandom(MathHelper.ToRadians(15)) * 8f;
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, velocity, 
                                ModContent.ProjectileType<ServantScythe>(), RageScytheDamage, 5f);
                        }
                        SoundEngine.PlaySound(SoundID.Item62, npc.Center);
                    }

                    if (npc.velocity.Length() < MaxSpeed)
                    {
                        npc.velocity *= MaxVelocityBoost;
                    }
                }
            }

            private void HandleServantSummon(NPC npc)
            {
                if (npc.ai[0] == 0f && npc.ai[2] >= 540f && npc.ai[2] <= 740f && npc.ai[2] % 20 == 0)
                {
                    int servant = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, 
                        ModContent.NPCType<ServantofOcram>());
                    Main.npc[servant].damage = (int)(Main.npc[servant].damage * 1.5f);
                }
            }

            private void ShootSpreadProjectiles(NPC npc, int type, int count, int damage, float knockback, float speed, float spreadAngle)
            {
                for (int i = 0; i < count; i++)
                {
                    Vector2 velocity = (Main.player[npc.target].Center - npc.Center).SafeNormalize(Vector2.Zero) * speed;
                    velocity = velocity.RotatedByRandom(MathHelper.ToRadians(spreadAngle));
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, velocity, type, damage, knockback);
                }
            }

            private void ShootDirectProjectiles(NPC npc, int type, int count, int damage, float knockback, float speed)
            {
                for (int i = 0; i < count; i++)
                {
                    Vector2 velocity = (Main.player[npc.target].Center - npc.Center).SafeNormalize(Vector2.Zero) * speed;
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, velocity, 
                        type, damage, knockback, Main.myPlayer, 0f, npc.whoAmI);
                }
            }

            private void ShootRotatingScythes(NPC npc, int count, int damage, float knockback, float baseRotation)
            {
                for (int i = 0; i < count; i++)
                {
                    Vector2 velocity = new Vector2(0, 10f).RotatedBy(baseRotation + MathHelper.TwoPi / count * i);
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, velocity, 
                        ModContent.ProjectileType<OcramScythe>(), damage, knockback);
                }
            }

            private void CreateShadowflameDust(NPC npc)
            {
                if (Main.rand.NextBool(3))
                {
                    Dust.NewDust(npc.Center, 0, 0, DustID.Shadowflame, 0f, 0f, 100, default, 2f);
                }
            }
        }
    }