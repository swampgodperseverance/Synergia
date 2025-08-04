using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;
using Vanilla.Content.Projectiles.Hostile;
using Vanilla.Content.NPCs;
using Vanilla;
using Vanilla.Common.GlobalPlayer;
using Vanilla.Common;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Terraria.UI;

namespace Vanilla.Content.NPCs
{
    public class Cogworm : ModNPC
    {
        private bool spawned = false;
        private int attackTimer = 0;
        internal int attackPhase = 0;
        private int segmentCount = 15;
        private float chargeSpeed = 20f;
        private Vector2[] oldPositions = new Vector2[10];
        private Vector2 lastDirection = -Vector2.UnitY;
        private bool phaseTwoStarted = false;
        private int lastAttackPhase = -1;
        private int stalactiteCooldown = 0;

        private bool stalactitePhaseActive = false;
        private int stalactiteSpawnIndex = 0;
        private Vector2[] stalactitePositions = new Vector2[20];

        private Texture2D defaultTexture;
        private Texture2D dashTexture;
        private bool isDashing = false;
        private float dashScale = 1f;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.WyvernHead];

            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.MPAllowedEnemies[Type] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                CustomTexturePath = "Vanilla/Content/NPCs/CogwormHeadBoss",
                PortraitScale = 0.6f,
                PortraitPositionYOverride = 0f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.width = 42;
            NPC.height = 42;
            NPC.damage = 50;
            NPC.defense = 15;
            NPC.lifeMax = 41000;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.boss = true;
            NPC.netAlways = true;
            NPC.aiStyle = -1;
            NPC.HitSound = null;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.value = Item.buyPrice(0, 10, 0, 0);
            NPC.BossBar = ModContent.GetInstance<CogwormBossBar>();
            NPC.npcSlots = 10f;
        }

        public override void Load()
        {
            if (!Main.dedServ)
            {
                defaultTexture = ModContent.Request<Texture2D>(Texture).Value;
                dashTexture = ModContent.Request<Texture2D>("Vanilla/Content/NPCs/CogwormDash").Value;
            }
        }

        public override void Unload()
        {
            defaultTexture = null;
            dashTexture = null;
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            PlayRandomHitSound();
            base.OnHitByItem(player, item, hit, damageDone);
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            PlayRandomHitSound();
            base.OnHitByProjectile(projectile, hit, damageDone);
        }

        private void PlayRandomHitSound()
        {
            if (Main.rand.NextBool())
            {
                SoundEngine.PlaySound(Sounds.CragwormHit with { Volume = 0.6f, Pitch = -0.1f }, NPC.Center);
            }
            else
            {
                SoundEngine.PlaySound(Sounds.CragwormHit2 with { Volume = 0.6f, Pitch = -0.1f }, NPC.Center);
            }
        }

        public override void AI()
        {
            if (stalactiteCooldown > 0)
                stalactiteCooldown--;

            if (!phaseTwoStarted && NPC.life < NPC.lifeMax * 0.5f)
            {
                phaseTwoStarted = true;
                attackPhase = 0;
                attackTimer = 0;
            }

            if (oldPositions[0] == Vector2.Zero)
            {
                for (int i = 0; i < oldPositions.Length; i++)
                    oldPositions[i] = NPC.Center;
            }

            for (int i = oldPositions.Length - 1; i > 0; i--)
                oldPositions[i] = oldPositions[i - 1];

            oldPositions[0] = NPC.Center;

            Player target = Main.player[NPC.target];
            if (!target.active || target.dead)
            {
                NPC.TargetClosest(false);
                target = Main.player[NPC.target];
                if (target.dead || !target.active)
                {
                    NPC.velocity = new Vector2(0f, -10f);
                    NPC.timeLeft = 10;
                    return;
                }
            }

            NPC.TargetClosest(true);

            if (!spawned && Main.netMode != NetmodeID.MultiplayerClient)
            {
                spawned = true;
                SpawnSegments();
            }

            isDashing = (attackPhase == 1) || 
                  (phaseTwoStarted && attackTimer % (260 + 40) >= 260 && attackTimer % (260 + 40) < 260 + 40);

            if (isDashing)
            {
                dashScale = MathHelper.Lerp(dashScale, 1.3f, 0.1f);
            }
            else
            {
                dashScale = MathHelper.Lerp(dashScale, 1f, 0.05f);
            }

            if (phaseTwoStarted)
                PhaseTwoAI(target);
            else
                HandleAttackPhases(target);

            if (NPC.velocity.Length() > 0.1f)
            {
                lastDirection = NPC.velocity.SafeNormalize(Vector2.UnitX);
                NPC.spriteDirection = lastDirection.X < 0 ? -1 : 1;
            }

            NPC.rotation = lastDirection.ToRotation() + MathHelper.PiOver2;

            if (Main.rand.NextBool(5))
                CreateDustEffects();
        }

        private void PhaseTwoAI(Player target)
        {
            attackTimer++;

            Vector2 desiredPosition = target.Center + new Vector2(0, -100);
            Vector2 toTarget = desiredPosition - NPC.Center;
            float followSpeed = 8f;

            const int chargeDelay = 260;
            const int chargeDuration = 40;
            const int fullCycle = chargeDelay + chargeDuration;

            int localCycle = attackTimer % fullCycle;

            if (localCycle < chargeDelay)
            {
                NPC.velocity = Vector2.Lerp(NPC.velocity, toTarget.SafeNormalize(Vector2.UnitY) * followSpeed, 0.05f);

                if (localCycle == chargeDelay - 1)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient &&
                        Main.player[NPC.target].GetModPlayer<ScreenShakePlayer>() is ScreenShakePlayer shake)
                    {
                        shake.TriggerShake(30);
                    }

                    SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                }
            }

            if (localCycle == chargeDelay)
            {
                Vector2 dashVelocity = NPC.DirectionTo(target.Center) * 22f;
                NPC.velocity = dashVelocity;
                lastDirection = dashVelocity;
                NPC.netUpdate = true;

                if (NPC.life <= NPC.lifeMax * 0.2f)
                {
                    for (int i = -2; i <= 2; i++)
                    {
                        float angleOffset = MathHelper.ToRadians(12f * i);
                        Vector2 baseDirection = Vector2.UnitY * 1f;
                        Vector2 velocity = baseDirection.RotatedBy(angleOffset) * -8f;

                        int type = Utils.SelectRandom(Main.rand,
                            ModContent.ProjectileType<HellMeteor1>(),
                            ModContent.ProjectileType<HellMeteor2>(),
                            ModContent.ProjectileType<HellMeteor3>());

                        Projectile.NewProjectile(
                            NPC.GetSource_FromAI(),
                            NPC.Center,
                            velocity,
                            type,
                            36,
                            1f);
                    }
                }
            }

            if (localCycle == chargeDelay + chargeDuration - 1)
            {
                for (int i = -5; i <= 5; i++)
                {
                    float angleOffset = MathHelper.ToRadians(20f * i);
                    Vector2 baseDirection = Vector2.UnitY * 1f;
                    Vector2 velocity = baseDirection.RotatedBy(angleOffset) * -10f;

                    int type = Utils.SelectRandom(Main.rand,
                        ModContent.ProjectileType<HellMeteor1>(),
                        ModContent.ProjectileType<HellMeteor2>(),
                        ModContent.ProjectileType<HellMeteor3>());

                    Projectile.NewProjectile(
                        NPC.GetSource_FromAI(),
                        NPC.Center,
                        velocity,
                        type,
                        40,
                        1f);
                }
            }

            if (attackTimer % 420 == 0 && !stalactitePhaseActive)
            {
                stalactitePhaseActive = true;
                stalactiteSpawnIndex = 0;

                int stalactiteCount = stalactitePositions.Length;
                if (NPC.life <= NPC.lifeMax * 0.2f)
                    stalactiteCount = stalactitePositions.Length * 2;

                stalactitePositions = new Vector2[stalactiteCount];

                for (int i = 0; i < stalactiteCount; i++)
                {
                    float xRange = NPC.life <= NPC.lifeMax * 0.2f ? 1800f : 1200f;
                    float xOffset = Main.rand.NextFloat(-xRange, xRange);
                    stalactitePositions[i] = new Vector2(target.Center.X + xOffset, target.Center.Y - 600f);
                }
            }

            if (stalactitePhaseActive && stalactiteSpawnIndex < stalactitePositions.Length && attackTimer % 2 == 0)
            {
                int type = Utils.SelectRandom(Main.rand,
                    ModContent.ProjectileType<HellStalactite1>(),
                    ModContent.ProjectileType<HellStalactite2>(),
                    ModContent.ProjectileType<HellStalactite3>());

                float speedY = NPC.life <= NPC.lifeMax * 0.2f ? 2.5f : 1f;

                Projectile.NewProjectile(
                    NPC.GetSource_FromAI(),
                    stalactitePositions[stalactiteSpawnIndex],
                    new Vector2(0f, speedY),
                    type,
                    40,
                    1f);

                stalactiteSpawnIndex++;

                if (stalactiteSpawnIndex >= stalactitePositions.Length)
                {
                    stalactitePhaseActive = false;
                }
            }
        }

        private void SpawnSegments()
        {
            int latest = NPC.whoAmI;

            for (int i = 0; i < segmentCount; i++)
            {
                int bodyID = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y,
                    ModContent.NPCType<CogwormBody>(), NPC.whoAmI, latest, i);
                Main.npc[bodyID].realLife = NPC.whoAmI;
                Main.npc[bodyID].ai[1] = latest;
                Main.npc[bodyID].ai[3] = NPC.whoAmI;
                Main.npc[bodyID].dontTakeDamage = true;
                latest = bodyID;
            }

            int tailID = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y,
                ModContent.NPCType<CogwormTail>(), NPC.whoAmI, latest);
            Main.npc[tailID].realLife = NPC.whoAmI;
            Main.npc[tailID].ai[1] = latest;
            Main.npc[tailID].ai[3] = NPC.whoAmI;
        }

        private void HandleAttackPhases(Player target)
        {
            Vector2 toPlayer = target.Center - NPC.Center;
            Vector2 dir = toPlayer.SafeNormalize(Vector2.UnitY);

            attackTimer++;

            switch (attackPhase)
            {
                case 0:
                    float orbitRadius = 200f;
                    float orbitSpeed = 0.02f;
                    float angle = attackTimer * orbitSpeed;
                    Vector2 orbitPosition = target.Center + orbitRadius * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

                    Vector2 moveTo = orbitPosition - NPC.Center;
                    NPC.velocity = Vector2.Lerp(NPC.velocity, moveTo.SafeNormalize(Vector2.UnitY) * 6f, 0.1f);

                    if (attackTimer >= 120)
                    {
                        int newPhase;
                        int attempts = 0;
                        do 
                        {
                            newPhase = Main.rand.Next(0, 4);
                            attempts++;
                            if (attempts > 5) break;
                        } 
                        while (newPhase == 3 && (lastAttackPhase == 3 || stalactiteCooldown > 0));

                        attackPhase = newPhase;
                        lastAttackPhase = newPhase;
                        attackTimer = 0;

                        if (attackPhase == 1)
                        {
                            Vector2 chargeTarget = target.Center - new Vector2(0, 50);
                            Vector2 chargeDir = (chargeTarget - NPC.Center).SafeNormalize(Vector2.UnitY);
                            NPC.velocity = chargeDir * chargeSpeed;
                            lastDirection = chargeDir;
                            SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                            NPC.netUpdate = true;
                        }
                        else if (attackPhase == 3)
                        {
                            NPC.velocity = Vector2.Zero;
                            stalactiteCooldown = 600;
                        }
                    }
                    break;

                case 1:
                    if (attackTimer % 4 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, lastDirection * 10f,
                            ProjectileID.GreekFire1, 40, 2f, Main.myPlayer);
                    }

                    if (attackTimer >= 30)
                    {
                        attackPhase = 0;
                        attackTimer = 0;
                        NPC.velocity *= 0.4f;
                        NPC.netUpdate = true;
                    }
                    break;

                case 2:
                    if (attackTimer == 1 || attackTimer == 15 || attackTimer == 30)
                    {
                        int zigzagDir = (attackTimer == 15) ? -1 : 1;
                        Vector2 baseDir = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitY);
                        Vector2 zigzag = baseDir.RotatedBy(MathHelper.ToRadians(15f * zigzagDir));
                        NPC.velocity = zigzag * chargeSpeed;
                        lastDirection = zigzag;

                        for (int i = -1; i <= 1; i++)
                        {
                            Vector2 projDir = zigzag.RotatedBy(MathHelper.ToRadians(10f * i));
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, projDir * 7f,
                                ProjectileID.GreekFire2, 25, 1f, Main.myPlayer);
                        }

                        SoundEngine.PlaySound(SoundID.Item74, NPC.position);
                    }

                    if (attackTimer >= 45)
                    {
                        attackPhase = 0;
                        attackTimer = 0;
                        NPC.velocity *= 0.5f;
                        NPC.netUpdate = true;
                    }
                    break;

                case 3:
                    if (attackTimer == 1)
                    {
                        NPC.velocity = new Vector2(0f, chargeSpeed * 1.5f);
                        lastDirection = Vector2.UnitY;
                        SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                        NPC.netUpdate = true;
                    }

                    if (attackTimer == 15)
                    {
                        FireGroupedStalactites(target);
                    }

                    if (attackTimer >= 35)
                    {
                        attackPhase = 0;
                        attackTimer = 0;
                        NPC.velocity *= 0.3f;
                        NPC.netUpdate = true;
                    }
                    break;
            }
        }

        private void FireGroupedStalactites(Player player)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            Vector2 spawnBase = player.Center + new Vector2(0f, 300f);

            for (int group = 0; group < 3; group++)
            {
                float groupX = Main.rand.NextFloat(-300f, 300f);
                float spacing = 60f;

                for (int i = 0; i < 3; i++)
                {
                    Vector2 spawnPos = spawnBase + new Vector2(
                        groupX + (i - 1) * spacing,
                        Main.rand.Next(-15, 15));

                    Vector2 velocity = new Vector2(
                        Main.rand.NextFloat(-0.3f, 0.3f),
                        -Main.rand.NextFloat(14f, 18f));

                    Projectile.NewProjectile(
                        NPC.GetSource_FromAI(),
                        spawnPos,
                        velocity,
                        ModContent.ProjectileType<LavaStalactite>(),
                        35,
                        0f,
                        Main.myPlayer);
                }
            }

            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode with { Volume = 1.2f }, player.Center);
    
            if (Main.player[NPC.target].GetModPlayer<ScreenShakePlayer>() is ScreenShakePlayer shake)
            {
                shake.TriggerShake(15, 0.8f);
            }
        }

        private void CreateDustEffects()
        {
            Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Lava, 0f, 0f, 100, default, 1f);
            dust.noGravity = true;
            dust.velocity *= 0.5f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = isDashing && dashTexture != null ? dashTexture : defaultTexture;
            
            if (texture == null || texture.IsDisposed)
                texture = ModContent.Request<Texture2D>(Texture).Value;

            if (texture == null || texture.IsDisposed)
                return false;

            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            for (int i = oldPositions.Length - 1; i > 0; i--)
            {
                float alpha = 0.5f * (1f - (float)i / oldPositions.Length);
                spriteBatch.Draw(texture, oldPositions[i] - screenPos, null, drawColor * alpha,
                    NPC.rotation, origin, NPC.scale * dashScale, effects, 0f);
            }

            spriteBatch.Draw(texture, NPC.Center - screenPos, null, drawColor,
                NPC.rotation, origin, NPC.scale * dashScale, effects, 0f);

            return false;
        }

        public override void BossHeadRotation(ref float rotation)
        {
            rotation = NPC.rotation - MathHelper.PiOver2;
        }
        public override void BossHeadSlot(ref int index)
        {
            index = ModContent.GetModBossHeadSlot("Vanilla/Content/NPCs/CogwormHeadBoss");
        }
    }
}