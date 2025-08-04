using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Avalon.Projectiles.Hostile.TuhrtlOutpost;
using Avalon.Projectiles.Hostile.BacteriumPrime;
using Vanilla.Common.GlobalPlayer;

namespace Vanilla.Common.GlobalNPCs.AI
{
    public class BacteriumPrimeAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int phase1Timer;
        private int phase2Timer;

        private bool isGasAttackActive = false;
        private int gasAttackTimer = 0;
        private int gasAttackPhase = 0; 
        private int gasShotsLeft = 0;

        public override void SetDefaults(NPC npc)
        {
            if (npc.type == ModContent.NPCType<Avalon.NPCs.Bosses.PreHardmode.BacteriumPrime>())
            {
                npc.buffImmune[BuffID.Venom] = true;
            }
        }

        public override void AI(NPC npc)
        {
            if (npc.type != ModContent.NPCType<Avalon.NPCs.Bosses.PreHardmode.BacteriumPrime>())
                return;

            Player target = Main.player[npc.target];

            bool inPhaseTwo = npc.localAI[0] == 1f;

            if (!isGasAttackActive && npc.life <= npc.lifeMax * 0.45f && npc.localAI[1] == 0f)
            {
                npc.localAI[1] = 1f;
                StartGasAttack();
            }
            else if (!isGasAttackActive && npc.life <= npc.lifeMax * 0.28f && npc.localAI[2] == 0f)
            {
                npc.localAI[2] = 1f;
                StartGasAttack();
            }
            else if (!isGasAttackActive && npc.life <= npc.lifeMax * 0.10f && npc.localAI[3] == 0f)
            {
                npc.localAI[3] = 1f;
                StartGasAttack();
            }

            if (isGasAttackActive)
            {
                npc.velocity = Vector2.Zero;

                gasAttackTimer++;

                if (gasAttackPhase == 0 && gasAttackTimer == 30)
                {
                    PlayEffects(npc, target); 
                    gasAttackPhase = 1;
                    gasAttackTimer = 0;
                    gasShotsLeft = 12; 
                }
                else if (gasAttackPhase == 1 && gasAttackTimer % 5 == 0 && gasShotsLeft > 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 direction = (target.Center - npc.Center).SafeNormalize(Vector2.One);
                        direction = direction.RotatedByRandom(MathHelper.ToRadians(15)) * Main.rand.NextFloat(5f, 6f);

                        Projectile.NewProjectile(
                            npc.GetSource_FromAI(),
                            npc.Center,
                            direction,
                            ModContent.ProjectileType<BacteriumGas>(),
                            30, 0f, Main.myPlayer);
                    }
                    gasShotsLeft--;

                    if (gasShotsLeft <= 0)
                    {
                        gasAttackPhase = 2;
                        gasAttackTimer = 0;
                    }
                }
                else if (gasAttackPhase == 2 && gasAttackTimer >= 30)
                {
                    isGasAttackActive = false;
                    gasAttackPhase = 0;
                    gasAttackTimer = 0;
                }

                return; 
            }

            if (!inPhaseTwo && npc.life <= npc.lifeMax / 2)
            {
                npc.localAI[0] = 1f;
                phase1Timer = 0;
                npc.netUpdate = true;
            }

            if (!inPhaseTwo)
            {
                phase1Timer++;
                if (phase1Timer >= 480)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        DoPhaseOneAttack(npc, target);

                    PlayEffects(npc, target);
                    phase1Timer = 0;
                }
            }
            else
            {
                phase2Timer++;
                if (phase2Timer >= 1060)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        DoPhaseTwoAttack(npc, target);

                    PlayEffects(npc, target);
                    phase2Timer = 0;
                }
            }
        }

        private void StartGasAttack()
        {
            isGasAttackActive = true;
            gasAttackPhase = 0;
            gasAttackTimer = 0;
            gasShotsLeft = 0;
        }

        private void DoPhaseOneAttack(NPC npc, Player target)
        {
            Vector2 baseDirection = (target.Center - npc.Center).SafeNormalize(Vector2.UnitX);

            for (int i = -1; i <= 1; i++)
            {
                Vector2 perturbed = baseDirection.RotatedBy(MathHelper.ToRadians(10 * i)) * 10f;

                Projectile.NewProjectile(
                    npc.GetSource_FromAI(),
                    npc.Center,
                    perturbed,
                    ModContent.ProjectileType<PoisonGasTrap>(),
                    30, 0f, Main.myPlayer);
            }
        }

        private void DoPhaseTwoAttack(NPC npc, Player target)
        {
            for (int i = 0; i < 12; i++)
            {
                Vector2 spawnPos = target.Center + new Vector2(Main.rand.Next(-600, 600), -800);
                Vector2 velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(6f, 10f));

                Projectile.NewProjectile(
                    npc.GetSource_FromAI(),
                    spawnPos,
                    velocity,
                    ModContent.ProjectileType<BouncyBoogerBall>(),
                    25, 0f, Main.myPlayer);
            }
        }

        private void PlayEffects(NPC npc, Player target)
        {
            SoundEngine.PlaySound(SoundID.Roar with { Pitch = -0.5f }, npc.Center);

            if (target.whoAmI == Main.myPlayer)
            {
                target.GetModPlayer<ScreenShakePlayer>().TriggerShake(15);
            }

            npc.velocity = Vector2.Zero;
        }
    }
}