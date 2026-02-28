using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Audio;
using System.IO;

namespace Synergia.Content.Projectiles.Hostile.Bosses
{
    public class EyeBlinkGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        // Спин-атака
        private int spinTimer = 0;       // отсчет времени до спина
        private int spinInterval = 0;    // случайный интервал между спинами

        public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == NPCID.EyeofCthulhu;

        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
            if(Main.netMode == 0) return;
            binaryWriter.Write(spinTimer);
            binaryWriter.Write(spinInterval);
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
            if(Main.netMode == 0) return;
            spinTimer = binaryReader.ReadInt32();
            spinInterval = binaryReader.ReadInt32();
        }

        public override void AI(NPC npc)
        {
            base.AI(npc);

            // Проверка цели
            if (npc.target < 0 || npc.target >= Main.maxPlayers || !Main.player[npc.target].active || Main.player[npc.target].dead)
            {
                npc.TargetClosest();
                if (npc.target < 0 || npc.target >= Main.maxPlayers || !Main.player[npc.target].active || Main.player[npc.target].dead)
                    return;
            }

            Player player = Main.player[npc.target];
            if (player == null || !player.active || player.dead) return;

            const float hpThresholdFraction = 0.5f; 
            const float spinHpMax = 0.65f;
            const float spinHpMin = 0.5f;
            const int shrinkDuration = 40;
            const int teleportHold = 8;
            const float dashSpeed = 15f;
            const float dashAccel = 1.06f;
            const int dashDuration = 45;
            const int fadeAlphaTarget = 255;
            const int dustCount = 12;
            const int attackCooldownTicks = 600; 
            const int spinDuration = 120;
            const float spinProjectileSpeed = 8f; 
            const int spinSpawnInterval = 10;
            const int firstSpinDelay = 300;       
            const int spinCooldownMin = 480;     
            const int spinCooldownMax = 720;      // 


            if (npc.localAI[3] > 0f) // телепорт/рывок
                npc.localAI[3] = Math.Max(0f, npc.localAI[3] - 1f);


            if (npc.life < npc.lifeMax * spinHpMax && npc.life > npc.lifeMax * spinHpMin)
            {
                // Таймер спина
                spinTimer++;

                if (npc.localAI[0] == 0f && spinTimer >= (spinInterval == 0 ? firstSpinDelay : spinInterval))
                {
                    npc.localAI[0] = 4f; 
                    npc.localAI[1] = 0f; 
                    spinInterval = Main.rand.Next(spinCooldownMin, spinCooldownMax);
                    spinTimer = 0; 
                    npc.netUpdate = true;
                }
            }

            if (npc.localAI[0] == 4f) 
            {
                npc.localAI[1]++;

                float t = MathHelper.Clamp(npc.localAI[1] / (float)spinDuration, 0f, 1f);
                float rotationSpeed = EaseInOutCubic(t) * MathHelper.TwoPi; 
                npc.rotation += rotationSpeed * 0.016f;

                if (npc.localAI[1] % spinSpawnInterval == 0)
                {
                    float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                    Vector2 velocity = angle.ToRotationVector2() * spinProjectileSpeed;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, velocity, ModContent.ProjectileType<EyeP>(), 20, 1f, Main.myPlayer);
                    }
                }

                if (npc.localAI[1] >= spinDuration)
                {
                    npc.localAI[0] = 0f;
                    npc.localAI[1] = 0f;
                    npc.netUpdate = true;
                }
                return;
            }


            if (npc.life < npc.lifeMax * hpThresholdFraction && npc.localAI[0] == 0f && npc.localAI[3] <= 0f)
            {
                npc.localAI[0] = 1f;
                npc.localAI[1] = 0f;
                npc.netUpdate = true;
            }

            if (npc.localAI[0] == 1f)
            {
                npc.localAI[1]++;
                float t = MathHelper.Clamp(npc.localAI[1] / (float)shrinkDuration, 0f, 1f);
                npc.scale = MathHelper.Lerp(1f, 0.35f, EaseOutCubic(t));
                npc.alpha = (int)MathHelper.Lerp(0f, fadeAlphaTarget, EaseInCubic(t));

                if (Main.rand.NextBool(3))
                {
                    int d = Dust.NewDust(npc.position, npc.width, npc.height, DustID.GoldCoin, 0f, 0f, 150);
                    Main.dust[d].velocity *= 0.3f;
                    Main.dust[d].noGravity = true;
                }

                npc.velocity *= 0.9f;

                if (npc.localAI[1] >= shrinkDuration)
                {
                    npc.localAI[0] = 2f;
                    npc.localAI[1] = 0f;
                    npc.netUpdate = true;
                }
                return;
            }


            if (npc.localAI[0] == 2f)
            {
                npc.localAI[1]++;
                if (npc.localAI[1] == 1f)
                {
                    SoundEngine.PlaySound(SoundID.Item8, npc.Center);
                    int side = Main.rand.NextBool() ? 1 : -1;
                    float horizontalOffset = 450f;
                    float verticalOffset = Main.rand.NextFloat(-80f, 80f);
                    npc.Center = player.Center + new Vector2(side * horizontalOffset, verticalOffset);
                    npc.localAI[2] = side;

                    for (int i = 0; i < dustCount; i++)
                    {
                        Vector2 vel = new Vector2(-side * Main.rand.NextFloat(2f, 6f), Main.rand.NextFloat(-2f, 2f));
                        int d = Dust.NewDust(npc.position, npc.width, npc.height, DustID.PurpleCrystalShard, vel.X, vel.Y, 150);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].velocity *= 1.1f;
                    }

                    npc.alpha = fadeAlphaTarget;
                    npc.scale = 0.35f;
                    npc.velocity = Vector2.Zero;
                    npc.netUpdate = true;
                }

                if (npc.localAI[1] >= teleportHold)
                {
                    npc.localAI[0] = 3f;
                    npc.localAI[1] = 0f;
                    npc.netUpdate = true;
                }
                return;
            }


            if (npc.localAI[0] == 3f)
            {
                npc.localAI[1]++;
                if (npc.localAI[1] == 1f)
                {
                    npc.alpha = 0;
                    npc.scale = 1f;
                    Vector2 dir = player.Center - npc.Center;
                    if (dir == Vector2.Zero) dir = new Vector2(0f, 1f);
                    dir.Normalize();
                    float side = npc.localAI[2] >= 0f ? 1f : -1f;
                    npc.velocity = dir * dashSpeed;
                    npc.velocity.X += side * 1.2f;
                    SoundEngine.PlaySound(SoundID.Roar, npc.Center);
                    npc.netUpdate = true;
                }
                else
                {
                    npc.velocity *= dashAccel;
                    if (npc.velocity.Length() > 28f) npc.velocity = Vector2.Normalize(npc.velocity) * 28f;
                }

                if (npc.localAI[1] > dashDuration)
                {
                    npc.localAI[0] = 0f;
                    npc.localAI[1] = 0f;
                    npc.localAI[2] = 0f;
                    npc.localAI[3] = attackCooldownTicks;
                    npc.velocity *= 0.25f;
                    npc.netUpdate = true;
                }
                return;
            }
        }

        private float EaseOutCubic(float t) => 1f - (float)Math.Pow(1f - MathHelper.Clamp(t, 0f, 1f), 3);
        private float EaseInCubic(float t) => (float)Math.Pow(MathHelper.Clamp(t, 0f, 1f), 3);
        private float EaseInOutCubic(float t)
        {
            t = MathHelper.Clamp(t, 0f, 1f);
            return t < 0.5f ? 4f * t * t * t : 1f - (float)Math.Pow(-2f * t + 2f, 3) / 2f;
        }
    }
}

