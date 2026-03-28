using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Avalon.Projectiles.Hostile.BacteriumPrime;
using Synergia.Content.Projectiles.Hostile;
using Synergia.Common.GlobalPlayer;
using Synergia.Common.ModConfigs; 
using System.IO;

namespace Synergia.Common.GlobalNPCs.AI
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
        private int sporeAttackTimer = 0;

        public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == ModContent.NPCType<Avalon.NPCs.Bosses.PreHardmode.BacteriumPrime>();

        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
            if(Main.netMode == 0 || Disabled) return;
            bitWriter.WriteBit(isGasAttackActive);
            binaryWriter.Write(phase1Timer);
            binaryWriter.Write(phase2Timer);
            binaryWriter.Write(gasAttackTimer);
            binaryWriter.Write(gasAttackPhase);
            binaryWriter.Write(gasShotsLeft);
            binaryWriter.Write(sporeAttackTimer);
            for(int i = 0; i < npc.localAI.Length; i++) binaryWriter.Write(npc.localAI[i]);
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
            if(Main.netMode == 0 || Disabled) return;
            isGasAttackActive = bitReader.ReadBit();
            phase1Timer = binaryReader.ReadInt32();
            phase2Timer = binaryReader.ReadInt32();
            gasAttackTimer = binaryReader.ReadInt32();
            gasAttackPhase = binaryReader.ReadInt32();
            gasShotsLeft = binaryReader.ReadInt32();
            sporeAttackTimer = binaryReader.ReadInt32();
            for(int i = 0; i < npc.localAI.Length; i++) npc.localAI[i] = binaryReader.ReadSingle();
        }

        //hard
        private const float DamageMultiplier = 1.7f;
        private const float AttackRateMultiplier = 1.5f;
        private const float ProjectileSpeedMultiplier = 1.3f;
        private const int ExtraProjectiles = 2;

        private static bool BuffEnabled => ModContent.GetInstance<BossConfig>().BacteriumPrimeBuffEnabled;
        internal static bool Disabled = false;

        public override void SetDefaults(NPC npc)
        {
            npc.buffImmune[BuffID.Venom] = true;
        }

        public override void AI(NPC npc)
        {
            if(Disabled) return;
            if(npc.ai[3] > 0f && npc.ai[3] < 60f) return;
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

                int shotInterval = BuffEnabled ? 3 : 5;
                int shotsCount   = BuffEnabled ? 20 : 12;

                if (gasAttackPhase == 0 && gasAttackTimer == 30)
                {
                    PlayEffects(npc, target);
                    gasAttackPhase = 1;
                    gasAttackTimer = 0;
                    gasShotsLeft = shotsCount;
                }
                else if (gasAttackPhase == 1 && gasAttackTimer % shotInterval == 0 && gasShotsLeft > 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 direction = (target.Center - npc.Center).SafeNormalize(Vector2.One);
                        float spread = BuffEnabled ? 25f : 15f;

                        direction = direction.RotatedByRandom(MathHelper.ToRadians(spread)) *
                                    Main.rand.NextFloat(5f, 6f) *
                                    (BuffEnabled ? ProjectileSpeedMultiplier : 1f);

                        Projectile.NewProjectile(
                            npc.GetSource_FromAI(),
                            npc.Center,
                            direction,
                            ModContent.ProjectileType<BacteriumGas>(),
                            BuffEnabled ? (int)(30 * DamageMultiplier) : 30,
                            0f,
                            Main.myPlayer);
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
                int attackTime = BuffEnabled ? (int)(480 / AttackRateMultiplier) : 480;

                if (phase1Timer >= attackTime)
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
                int attackTime = BuffEnabled ? (int)(1060 / AttackRateMultiplier) : 1060;

                if (phase2Timer >= attackTime)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        DoPhaseTwoAttack(npc, target);

                    PlayEffects(npc, target);
                    phase2Timer = 0;
                }
            }

            if (!BacteriumPrimeBuff.Enabled)
                return;


            if (npc.velocity.Length() > 0.1f)
            {
                npc.velocity = Vector2.Normalize(npc.velocity) *
                               MathHelper.Clamp(npc.velocity.Length() * BacteriumPrimeBuff.SpeedMultiplier, 0, BacteriumPrimeBuff.MaxSpeed);
            }
            KeepBossNearTarget(npc);
            BuffPhase1Attacks(npc);
            BuffPhase2Attacks(npc);
            BuffSporeSeedAttack(npc);
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
            int count = BuffEnabled ? 5 : 3;

            for (int i = -count / 2; i <= count / 2; i++)
            {
                Vector2 perturbed = baseDirection.RotatedBy(MathHelper.ToRadians(10 * i)) * 10f *
                                    (BuffEnabled ? ProjectileSpeedMultiplier : 1f);

                Projectile.NewProjectile(
                    npc.GetSource_FromAI(),
                    npc.Center,
                    perturbed,
                    ModContent.ProjectileType<PoisonGasTrap>(),
                    BuffEnabled ? (int)(30 * DamageMultiplier) : 30,
                    0f,
                    Main.myPlayer);
            }
        }

	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
	{
		if(projectile.ModProjectile is PoisonGasTrap) {
			modifiers.FinalDamage *= 0f;
			modifiers.FinalDamage.Flat--;
		}
	}

        private void DoPhaseTwoAttack(NPC npc, Player target)
        {
            int count = BuffEnabled ? 20 : 12;

            for (int i = 0; i < count; i++)
            {
                Vector2 spawnPos = target.Center + new Vector2(Main.rand.Next(-600, 600), -800);
                Vector2 velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(6f, 10f)) *
                                   (BuffEnabled ? ProjectileSpeedMultiplier : 1f);

                Projectile.NewProjectile(
                    npc.GetSource_FromAI(),
                    spawnPos,
                    velocity,
                    ModContent.ProjectileType<BouncyBoogerBall>(),
                    BuffEnabled ? (int)(25 * DamageMultiplier) : 25,
                    0f,
                    Main.myPlayer);
            }
        }

        private void PlayEffects(NPC npc, Player target)
        {
            SoundEngine.PlaySound(SoundID.Roar with { Pitch = -0.5f }, npc.Center);

            if (target.whoAmI == Main.myPlayer)
            {
                target.GetModPlayer<ScreenShakePlayer>().TriggerShake(BuffEnabled ? 25 : 15);
            }

            npc.velocity = Vector2.Zero;
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
                        (int)(npc.damage * 0.7f * BacteriumPrimeBuff.DamageMultiplier),
                        0,
                        -1
                    );
                }
            }

            sporeAttackTimer = Main.rand.Next(BacteriumPrimeBuff.SporeAttackCooldownMin, BacteriumPrimeBuff.SporeAttackCooldownMax);
        }

        private void KeepBossNearTarget(NPC npc)
        {
            Player target = Main.player[npc.target];
            if (!target.active || target.dead || npc.ai[0] > 600 / BacteriumPrimeBuff.AttackRateMultiplier) return;

            float distance = Vector2.Distance(npc.Center, target.Center);

            if (distance > BacteriumPrimeBuff.MaxDistance)
            {
                Vector2 direction = npc.DirectionTo(target.Center);
                npc.velocity = Vector2.Lerp(npc.velocity, direction * MathHelper.Min(12f, distance), 0.1f);

                npc.Center = Vector2.Lerp(npc.Center, target.Center, 0.02f);
            }
        }

        private void BuffPhase1Attacks(NPC npc)
        {
            if (npc.ai[3] == 0)
            {
                if (npc.ai[0] % (15 / BacteriumPrimeBuff.AttackRateMultiplier) == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Projectile.NewProjectile(
                            npc.GetSource_FromAI(),
                            npc.Center + Main.rand.NextVector2Circular(npc.width * 0.7f, npc.height * 0.7f),
                            Main.rand.NextVector2Circular(3f, 3f),
                            ModContent.ProjectileType<BacteriumGas>(),
                            (int)(12 * BacteriumPrimeBuff.DamageMultiplier),
                            0,
                            -1,
                            1
                        );
                    }
                }

                if (npc.ai[0] > 600 / BacteriumPrimeBuff.AttackRateMultiplier)
                {
                    npc.velocity = npc.Center.DirectionTo(Main.player[npc.target].Center).RotatedBy(npc.localAI[0]) *
                                   MathHelper.Min(npc.Center.Distance(Main.player[npc.target].Center) * 0.04f, 10);
                }
            }
        }

        private void BuffPhase2Attacks(NPC npc)
        {
            if (npc.ai[3] == 60)
            {
                Player target = Main.player[npc.target];
                if (!target.active || target.dead) return;

                if (npc.ai[1] > 150 / BacteriumPrimeBuff.AttackRateMultiplier && npc.ai[1] % (15 / BacteriumPrimeBuff.AttackRateMultiplier) == 0)
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
                                (int)(npc.damage * 0.6f * BacteriumPrimeBuff.DamageMultiplier), 
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
                            (int)(20 * BacteriumPrimeBuff.DamageMultiplier), 
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
                                (int)(15 * BacteriumPrimeBuff.DamageMultiplier),
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
