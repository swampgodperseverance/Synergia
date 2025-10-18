using Microsoft.Xna.Framework;
using Synergia.Common;
using Synergia.Common.GlobalPlayer;
using Synergia.Content.Items;
using Synergia.Content.Items.Misc;
using Synergia.Content.Items.Weapons.Cogworm;
using Synergia.Content.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Placeable.Blocks;

namespace Synergia.Content.NPCs
{
    [AutoloadBossHead]
    public class Cogworm : ModNPC
    {
        private bool spawned = false;
        private int attackTimer = 0;
        internal int attackPhase = 0;
        private int segmentCount = 15;
        private float chargeSpeed = 20f;
        private Vector2 lastDirection = -Vector2.UnitY;
        private bool phaseTwoStarted = false;
        private int lastAttackPhase = -1;
        private int stalactiteCooldown = 0;
        private bool stalactitePhaseActive = false;
        private int stalactiteSpawnIndex = 0;
        private Vector2[] stalactitePositions = new Vector2[20];
        private bool isDashing = false;
        private int postDashingTime;
        private int dashSpriteTimer = 0;
        private const int DashSpriteDuration = 30;
        private float dashScale = 1f;
        
        // Новые поля для атаки LavaBone
        private int lavaBoneAttackTimer = 0;
        private bool lavaBoneAttackActive = false;
        private const int LavaBoneAttackCooldown = 420;

        // Новые поля для спец-атаки при 50%
        private bool hasDoneHalfHealthDash = false;
        private int halfHealthDashTimer = 0;
        private const int HalfHealthDashDuration = 120;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 2;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.ShouldBeCountedAsBoss[Type] = true;

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Position = new Vector2(0f, 8f),
                PortraitPositionXOverride = 0f,
                PortraitPositionYOverride = 12f
            });
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;

            NPC.frameCounter++;

            if (NPC.frameCounter >= 8 && (isDashing || postDashingTime > 0))
            {
                if(postDashingTime < 0) NPC.frameCounter = 0;

                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type])
                {
                    NPC.frame.Y = 0;
                }
            }
        }
        public override void SetDefaults()
        {
            NPC.width = 42;
            NPC.height = 42;
            NPC.damage = 50;
            NPC.defense = 18;
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

            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Sounds/HellExecution");
            }
        }

        public override bool CheckActive()
        {
            return false;
        }

        private bool IsPlayerInHell(Player player)
        {
            bool isDeepEnough = player.position.Y > Main.rockLayer * 16f;
            bool hasHellBackground = player.ZoneUnderworldHeight;
            
            int lavaTilesNearby = 0;
            int ashTilesNearby = 0;
            
            int checkRadius = 50;
            Point playerTile = player.Center.ToTileCoordinates();
            
            for (int x = playerTile.X - checkRadius; x <= playerTile.X + checkRadius; x++)
            {
                for (int y = playerTile.Y - checkRadius; y <= playerTile.Y + checkRadius; y++)
                {
                    if (WorldGen.InWorld(x, y))
                    {
                        Tile tile = Framing.GetTileSafely(x, y);
                        if (tile.LiquidType == LiquidID.Lava && tile.LiquidAmount > 0)
                            lavaTilesNearby++;
                        if (tile.TileType == TileID.Ash)
                            ashTilesNearby++;
                    }
                }
            }
            
            bool hasLavaAndAsh = lavaTilesNearby > 10 && ashTilesNearby > 20;
            bool isInHellByCoordinates = player.position.Y > (Main.maxTilesY - 250) * 16f;

            return (isDeepEnough && hasHellBackground) || hasLavaAndAsh || isInHellByCoordinates;
        }

        private bool IsPlayerInHellSimple(Player player)
        {
            return player.position.Y > (Main.maxTilesY * 0.80f) * 16f;
        }

        public override void AI()
        {
            Player target = Main.player[NPC.target];
            
            if (!IsPlayerInHellSimple(target))
            {
                bool foundPlayerInHell = false;
                
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player player = Main.player[i];
                    if (player.active && !player.dead && IsPlayerInHellSimple(player))
                    {
                        NPC.target = i;
                        target = player;
                        foundPlayerInHell = true;
                        break;
                    }
                }
                
                if (!foundPlayerInHell)
                {
                    NPC.active = false;
                    NPC.netUpdate = true;
                    
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC otherNPC = Main.npc[i];
                            if (otherNPC.active && (otherNPC.type == ModContent.NPCType<CogwormBody>() || 
                                otherNPC.type == ModContent.NPCType<CogwormTail>()) && 
                                otherNPC.ai[3] == NPC.whoAmI)
                            {
                                otherNPC.active = false;
                                if (Main.netMode == NetmodeID.Server)
                                {
                                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, i);
                                }
                            }
                        }
                    }
                    return;
                }
            }

            if (NPC.life <= 0 || NPC.timeLeft < 10)
            {
                lastDirection = -Vector2.UnitY;
                NPC.rotation = lastDirection.ToRotation() + MathHelper.PiOver2;
                NPC.spriteDirection = 1;
                return;
            }
            if (halfHealthDashTimer <= 0) // обычное поведение
            {
                if (NPC.velocity.Length() > 0.1f)
                {
                    lastDirection = NPC.velocity.SafeNormalize(Vector2.UnitX);
                    NPC.spriteDirection = lastDirection.X < 0 ? -1 : 1;
                }
            }
            else // во время спец-атаки
            {
                lastDirection = NPC.velocity.SafeNormalize(Vector2.UnitX);
                NPC.spriteDirection = lastDirection.X < 0 ? -1 : 1;
            }
            // Проверка на 50% здоровья для спец-атаки
            if (!hasDoneHalfHealthDash && NPC.life < NPC.lifeMax * 0.5f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                hasDoneHalfHealthDash = true;
                halfHealthDashTimer = HalfHealthDashDuration;
                NPC.netUpdate = true;
                
                // Эффектный рывок вправо
                NPC.velocity = new Vector2(30f, 0f);
                lastDirection = Vector2.UnitX;
                
                // Звуковой эффект
                SoundEngine.PlaySound(SoundID.Roar with { Volume = 1.5f, Pitch = -0.3f }, NPC.Center);
                
                // Визуальные эффекты
                for (int i = 0; i < 50; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 
                        DustID.Lava, 0f, 0f, 100, default, 3f);
                    dust.noGravity = true;
                    dust.velocity = new Vector2(-10f, Main.rand.NextFloat(-5f, 5f));
                }
            }

            if (halfHealthDashTimer > 0)
            {
                halfHealthDashTimer--;

                if (halfHealthDashTimer == HalfHealthDashDuration / 2)
                {
                    NPC.Center = new Vector2(target.Center.X - 1480f, target.Center.Y);
                    NPC.velocity = new Vector2(25f, 0f);
                    lastDirection = Vector2.UnitX;

                    for (int i = 0; i < 30; i++)
                    {
                        Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height,
                            DustID.Torch, 0f, 0f, 150, new Color(255, 100, 0), 2.5f);
                        dust.noGravity = true;
                        dust.velocity = Main.rand.NextVector2Circular(8f, 8f);
                    }

                    SoundEngine.PlaySound(SoundID.Item74 with { Volume = 1.2f }, NPC.Center);
                }

                if (halfHealthDashTimer <= 0)
                {
                    phaseTwoStarted = true;
                    attackPhase = 0;
                    attackTimer = 0;
                }

                NPC.rotation = lastDirection.ToRotation() + MathHelper.PiOver2;
                NPC.spriteDirection = lastDirection.X < 0 ? -1 : 1;

                return;
            }


            if (stalactiteCooldown > 0)
                stalactiteCooldown--;

            // Обновление таймера атаки LavaBone
            if (lavaBoneAttackTimer > 0)
                lavaBoneAttackTimer--;

            if (!phaseTwoStarted && NPC.life < NPC.lifeMax * 0.5f)
            {
                phaseTwoStarted = true;
                attackPhase = 0;
                attackTimer = 0;
                NPC.netUpdate = true;
            }

            if (!target.active || target.dead)
            {
                NPC.TargetClosest(false);
                target = Main.player[NPC.target];
                if (target.dead || !target.active)
                {
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player player = Main.player[i];
                        if (player.active && !player.dead && IsPlayerInHellSimple(player))
                        {
                            NPC.target = i;
                            target = player;
                            break;
                        }
                    }
                    
                    if (target.dead || !target.active || !IsPlayerInHellSimple(target))
                    {
                        NPC.velocity = new Vector2(0f, -10f);
                        NPC.timeLeft = 10;
                        lastDirection = -Vector2.UnitY;
                        NPC.rotation = lastDirection.ToRotation() + MathHelper.PiOver2;
                        NPC.spriteDirection = 1;
                        return;
                    }
                }
            }

            NPC.TargetClosest(true);

            if (!spawned && Main.netMode != NetmodeID.MultiplayerClient)
            {
                spawned = true;
                SpawnSegments();
                NPC.netUpdate = true;
            }

            isDashing = (attackPhase == 1) || (phaseTwoStarted && attackTimer % (260 + 40) >= 260 && attackTimer % (260 + 40) < 260 + 40);
            
            if (isDashing)
            {
                dashScale = MathHelper.Lerp(dashScale, 1.3f, 0.1f);
                postDashingTime = 45;
            }
            else
            {
                dashScale = MathHelper.Lerp(dashScale, 1f, 0.05f);
                postDashingTime--;
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

            if (!lavaBoneAttackActive && lavaBoneAttackTimer <= 0 && attackTimer % Main.rand.Next(120, 240) == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                lavaBoneAttackActive = true;
                lavaBoneAttackTimer = LavaBoneAttackCooldown;
                NPC.netUpdate = true;
    
                // Воспроизведение звука LavaBone
                SoundEngine.PlaySound(new SoundStyle("Synergia/Assets/Sounds/LavaBone"), NPC.Center);
            }

            // Выполнение атаки LavaBone
            if (lavaBoneAttackActive && Main.netMode != NetmodeID.MultiplayerClient)
            {
                ExecuteLavaBoneAttack(target);
            }

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
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < Main.maxPlayers; i++)
                        {
                            Player player = Main.player[i];
                            if (player.active && player.GetModPlayer<ScreenShakePlayer>() is ScreenShakePlayer shake)
                            {
                                shake.TriggerShake(30);
                            }
                        }

                        SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                    }
                }
            }

            if (localCycle == chargeDelay)
            {
                Vector2 dashVelocity = NPC.DirectionTo(target.Center) * 22f;
                NPC.velocity = dashVelocity;
                lastDirection = dashVelocity;
    
                // Активируем спрайт атаки
                dashSpriteTimer = DashSpriteDuration;
                NPC.netUpdate = true;
    
                if (NPC.life <= NPC.lifeMax * 0.2f)
                {
                    for (int i = -2; i <= 2; i++)
                    {
                        float angleOffset = MathHelper.ToRadians(12f * i);
                        Vector2 baseDirection = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitY); // Исправлено направление
                        Vector2 velocity = baseDirection.RotatedBy(angleOffset) * Main.rand.NextFloat(6f, 10f);
                        int type = Utils.SelectRandom(Main.rand, ModContent.ProjectileType<HellMeteor1>(), ModContent.ProjectileType<HellMeteor2>(), ModContent.ProjectileType<HellMeteor3>());
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(
                                NPC.GetSource_FromAI(),
                                NPC.Center,
                                velocity,
                                type,
                                36,
                                1f,
                                Main.myPlayer);
                        }
                    }
                }
            }

            if (localCycle == chargeDelay + chargeDuration - 1 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = -5; i <= 5; i++)
                {
                    float angleOffset = MathHelper.ToRadians(20f * i);
                    Vector2 baseDirection = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitY); // Исправлено направление
                    Vector2 velocity = baseDirection.RotatedBy(angleOffset) * Main.rand.NextFloat(8f, 12f);

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
                        1f,
                        Main.myPlayer);
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
                
                NPC.netUpdate = true;
            }

            if (stalactitePhaseActive && stalactiteSpawnIndex < stalactitePositions.Length && attackTimer % 2 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                int type = Utils.SelectRandom(Main.rand,
                    ModContent.ProjectileType<HellStalactite1>(),
                    ModContent.ProjectileType<HellStalactite2>(),
                    ModContent.ProjectileType<HellStalactite3>());

                float speedY = NPC.life <= NPC.lifeMax * 0.2f ? Main.rand.NextFloat(2f, 3f) : Main.rand.NextFloat(0.8f, 1.2f);

                Projectile.NewProjectile(
                    NPC.GetSource_FromAI(),
                    stalactitePositions[stalactiteSpawnIndex],
                    new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), speedY),
                    type,
                    40,
                    1f,
                    Main.myPlayer);

                stalactiteSpawnIndex++;

                if (stalactiteSpawnIndex >= stalactitePositions.Length)
                {
                    stalactitePhaseActive = false;
                    NPC.netUpdate = true;
                }
            }
            
            if (dashSpriteTimer > 0)
                dashSpriteTimer--;
        }

        private void ExecuteLavaBoneAttack(Player target)
        {
            // Определяем направление к игроку
            Vector2 directionToPlayer = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitY);
        
            // Количество проектов (3-5 случайное)
            int numberOfProjectiles = Main.rand.Next(3, 6);
        
            // Угол разброса (веер)
            float spreadAngle = MathHelper.ToRadians(45f);
        
            for (int i = 0; i < numberOfProjectiles; i++)
            {
                float angleVariation = Main.rand.NextFloat(-spreadAngle, spreadAngle);
                Vector2 velocity = directionToPlayer.RotatedBy(angleVariation) * 12f;
                velocity *= Main.rand.NextFloat(0.9f, 1.1f);
            
                Projectile.NewProjectile(
                    NPC.GetSource_FromAI(),
                    NPC.Center,
                    velocity,
                    ModContent.ProjectileType<LavaBone>(),
                    35,
                    2f,
                    Main.myPlayer);
            }
        
            for (int i = 0; i < 25; i++)
            {
                Vector2 dustVelocity = directionToPlayer.RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f)) * Main.rand.Next(5, 15);
                
                Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 
                    DustID.Lava, 0f, 0f, 100, default, 2f);
                dust.noGravity = true;
                dust.velocity = dustVelocity;
                
                if (i % 3 == 0)
                {
                    Dust smoke = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 
                        DustID.Smoke, 0f, 0f, 150, new Color(100, 100, 100), 1.5f);
                    smoke.noGravity = true;
                    smoke.velocity = dustVelocity * 0.7f;
                }
            }
        
            SoundEngine.PlaySound(SoundID.Item20 with { Volume = 1.2f, Pitch = -0.2f }, NPC.Center);
        
            lavaBoneAttackActive = false;
            NPC.netUpdate = true;
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
                
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, bodyID);
                }
            }

            int tailID = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y,
                ModContent.NPCType<CogwormTail>(), NPC.whoAmI, latest);
            Main.npc[tailID].realLife = NPC.whoAmI;
            Main.npc[tailID].ai[1] = latest;
            Main.npc[tailID].ai[3] = NPC.whoAmI;
            
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, tailID);
            }
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
                        NPC.netUpdate = true;

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
                            NPC.netUpdate = true;
                        }
                    }
                    break;

                case 1:
                    if (attackTimer == 1)
                    {
                        dashSpriteTimer = DashSpriteDuration;
                        NPC.netUpdate = true;
                    }
    
                    if (attackTimer % 4 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, lastDirection * Main.rand.NextFloat(8f, 12f), ProjectileID.GreekFire1, 40, 2f, Main.myPlayer);
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

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = -1; i <= 1; i++)
                            {
                                Vector2 projDir = zigzag.RotatedBy(MathHelper.ToRadians(10f * i));
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, projDir * Main.rand.NextFloat(5f, 9f),
                                    ProjectileID.GreekFire2, 25, 1f, Main.myPlayer);
                            }
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

                    if (attackTimer == 15 && Main.netMode != NetmodeID.MultiplayerClient)
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
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CogwormTrophy>(), 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Sinstone>(), 1, 38, 82));
            npcLoot.Add(ItemDropRule.Common(ItemID.GreaterHealingPotion, 1, 10, 18));

            LeadingConditionRule notExpertRule = new(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Cleavage>(), ModContent.ItemType<Menace>(), ModContent.ItemType<Pyroclast>(), ModContent.ItemType<HellgateAuraScythe>(), ModContent.ItemType<Impact>()));                                             
            npcLoot.Add(notExpertRule);

            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<CogwormBag>()));

            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<CogwormRelicItem>()));
        }
        private void FireGroupedStalactites(Player player)
        {
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
    
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player p = Main.player[i];
                if (p.active && p.GetModPlayer<ScreenShakePlayer>() is ScreenShakePlayer shake)
                {
                    shake.TriggerShake(15, 0.8f);
                }
            }
        }

        private void CreateDustEffects()
        {
            Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Lava, 0f, 0f, 100, default, 1f);
            dust.noGravity = true;
            dust.velocity *= 0.5f;
        }

        private void PlayRandomHitSound()
        {
            if (NPC.life <= 0) return;
        
            if (Main.rand.NextBool())
            {
                SoundEngine.PlaySound(Sounds.CragwormHit with { Volume = 0.8f, Pitch = -0.1f }, NPC.Center);
            }
            else
            {
                SoundEngine.PlaySound(Sounds.CragwormHit2 with { Volume = 0.8f, Pitch = -0.1f }, NPC.Center);
            }
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

        public override void HitEffect(NPC.HitInfo hit)
        {
            PlayRandomHitSound();
            
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 60; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Lava, 0f, 0f, 100, default, 3.5f);
                    dust.noGravity = true;
                    dust.velocity = Main.rand.NextVector2Circular(15f, 15f);
                    
                    if (i % 3 == 0)
                    {
                        Dust fireDust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Torch, 
                            0f, 0f, 150, new Color(255, 100, 0), 4.5f);
                        fireDust.noGravity = true;
                        fireDust.velocity = Main.rand.NextVector2Circular(12f, 12f);
                    }

                    if (i % 5 == 0)
                    {
                        Dust smokeDust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Smoke, 
                            0f, 0f, 100, new Color(100, 100, 100), 2.5f);
                        smokeDust.noGravity = true;
                        smokeDust.velocity = Main.rand.NextVector2Circular(8f, 8f) + new Vector2(0, -2f);
                    }
                }

                SoundEngine.PlaySound(new SoundStyle("Synergia/Assets/Sounds/WormBoom"), NPC.Center);

                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player player = Main.player[i];
                    if (player.active && player.GetModPlayer<ScreenShakePlayer>() is ScreenShakePlayer shake)
                    {
                        shake.TriggerShake(80, 2.5f);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Lava, 0f, 0f, 100, default, 1.5f);
                    dust.noGravity = true;
                    dust.velocity *= 2f;
                }
            }
            
            base.HitEffect(hit);
        }

        public override void BossHeadRotation(ref float rotation)
        {
            rotation = NPC.rotation - MathHelper.PiOver2;
        }

        public override void OnKill()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC otherNPC = Main.npc[i];
                    if (otherNPC.active && (otherNPC.type == ModContent.NPCType<CogwormBody>() || 
                        otherNPC.type == ModContent.NPCType<CogwormTail>()) && 
                        otherNPC.ai[3] == NPC.whoAmI)
                    {
                        if (Main.netMode != NetmodeID.Server)
                        {
                            for (int j = 0; j < 15; j++)
                            {
                                Dust dust = Dust.NewDustDirect(otherNPC.position, otherNPC.width, otherNPC.height, 
                                    DustID.Lava, 0f, 0f, 100, default, 2.5f);
                                dust.noGravity = true;
                                dust.velocity = Main.rand.NextVector2Circular(10f, 10f);
                            }
                        }
                        
                        otherNPC.active = false;
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, i);
                        }
                    }
                }
            }
        }
    }
}