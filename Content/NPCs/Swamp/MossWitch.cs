using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ReLogic.Content;
using Synergia.Content.Items.ActiveAccessories;
using Synergia.Helpers;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;

namespace Synergia.Content.NPCs.Swamp
{
    [AutoloadBossHead]
    public class MossWitch : ModNPC
    {
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TheMossWitchCrystal>(), chanceDenominator: 6)); 
        }
        public override void OnKill() => NPC.SetEventFlagCleared(ref Common.ModSystems.SynergiaWorld.mossWitchDead, -1);

        public override string LocalizationCategory => Category(CategoryName.NPC);

        //attack
        public const int ATTACK_CAST_DURATION = 120;
        public const int ATTACK_MAX_RANGE_TILES = 45;
        public const int HANDS_AMOUNT = 5;
        public const int SWAMPLINGS_SPAWN_PER_ATTACK = 1;
        public const int HAND_SPAWN_SPREAD_TILES = 10;
        //visual
        public const int POSTATTACK_VFX_TICKS = 30;
        public const int ATTACK_DUST_CAST = 10;
        public ref float AttackTimer => ref NPC.ai[0];
        public ref float PlayerCounter => ref NPC.ai[1];
        public ref float PostAttackVFXTimer => ref NPC.ai[2];

        readonly int attackMaxRangeSqr = ATTACK_MAX_RANGE_TILES * ATTACK_MAX_RANGE_TILES * 256;
        public override void SetDefaults()
        {
            NPC.width = 26;
            NPC.height = 38;
            NPC.friendly = false;
            NPC.defense = 15;
            NPC.lifeMax = 3000;
            NPC.damage = 20;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.value = 250f;
            NPC.boss = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            AnimationType = -1;
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 15;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if(NPC.frameCounter >= 5)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type])
                    NPC.frame.Y = 0;
            }
        }

        float distanceSqr, minDistance;
        public override void AI()
        {
            PlayerCounter = 0;
            minDistance = float.MaxValue;
            foreach (Player player in Main.ActivePlayers)
            {
                if (player.dead || player.ghost) continue;
                distanceSqr = Vector2.DistanceSquared(player.Center, NPC.Center);
                if (distanceSqr > attackMaxRangeSqr) continue;
                if (distanceSqr < minDistance)
                {
                    minDistance = distanceSqr;
                    NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;
                }
                PlayerCounter++;
            }

            if (PlayerCounter == 0)
            {
                AttackTimer = 0;
                return;
            }
            AttackTimer++;
            if (PostAttackVFXTimer > 0)
                PostAttackVFXTimer--;
            if(AttackTimer >= ATTACK_CAST_DURATION)
            {
                AttackTimer = 0;
                PostAttackVFXTimer = POSTATTACK_VFX_TICKS;
                for (int i = 0; i < ATTACK_DUST_CAST; i++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height / 2, DustID.GreenTorch, 0, -20f, 120, default, 1.75f);
                SpawnHands();
                SoundEngine.PlaySound(SoundID.Item103, NPC.Center);
            }
        }
        void SpawnHands()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;

            List<Player> playersToAttack = new();
            foreach (Player player in Main.ActivePlayers)
            {
                if (player.dead || player.ghost) continue; 
                distanceSqr = Vector2.DistanceSquared(player.Center, NPC.Center);
                if (distanceSqr <= attackMaxRangeSqr) playersToAttack.Add(player);
            }

            foreach (Player player in playersToAttack)
            {
                Vector2 playerPosition = player.Center;
                Vector2 handSpawnPosition;

                for (int i = 0; i < HANDS_AMOUNT; i++)
                {
                    handSpawnPosition = playerPosition;
                    if (i > 0)
                        handSpawnPosition.X += Main.rand.NextFloat(-HAND_SPAWN_SPREAD_TILES * 16, HAND_SPAWN_SPREAD_TILES * 16);
                    if (!TryGetSuitablePosition(handSpawnPosition, playerPosition, out handSpawnPosition))
                        continue;

                    Projectile.NewProjectile(
                        NPC.GetSource_None(),
                        handSpawnPosition,
                        Vector2.Zero,
                        ModContent.ProjectileType<MossArm>(),
                        30,
                        0,
                        Main.myPlayer,
                        ai1: i < SWAMPLINGS_SPAWN_PER_ATTACK ? 1 : 0,
                        ai2: Main.rand.Next(0, 45)
                    );
                }
            }
        }
        bool TryGetSuitablePosition(Vector2 position, Vector2 playerPosition, out Vector2 handSpawnPosition)
        {
            handSpawnPosition = Vector2.Zero;

            Point tilePos = new Point((int)(position.X / 16), (int)(position.Y / 16));
            tilePos.Y -= 21;
            int yOffset = 41;
            List<Vector2> possibleSpawns = new();
            while (yOffset >= 0)
            {
                tilePos.Y++;
                yOffset--;

                if (!IsSolidTile(tilePos) && IsSolidTile(tilePos + new Point(0, 1)))
                {
                    int slopeOffset = Main.tile[tilePos + new Point(0, 1)].TopSlope || Main.tile[tilePos + new Point(0, 1)].IsHalfBlock ? 8 : 0;
                    possibleSpawns.Add(
                        new Vector2(tilePos.X * 16 + 8, tilePos.Y * 16 + 16 + slopeOffset)
                    );
                }
            }
            if (possibleSpawns.Count == 0)
                return false;

            float maxDistanceSqr = float.MaxValue;
            foreach (Vector2 place in possibleSpawns)
            {
                distanceSqr = Vector2.DistanceSquared(place, playerPosition);
                if (distanceSqr < maxDistanceSqr)
                {
                    maxDistanceSqr = distanceSqr;
                    handSpawnPosition = place;
                }
            }
            return true;
        }
        bool IsSolidTile(Point pos) => 
            Main.tile[pos].HasTile && !Main.tile[pos].IsActuated && (Main.tileSolid[Main.tile[pos].TileType] || Main.tileSolidTop[Main.tile[pos].TileType]);
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Vector2 position = NPC.Center - screenPos;
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / Main.npcFrameCount[NPC.type] - NPC.height / 2);
            Rectangle frame = NPC.frame;

            if(PostAttackVFXTimer > 0)
            {
                float progress = PostAttackVFXTimer / POSTATTACK_VFX_TICKS;
                spriteBatch.Draw(
                    texture,
                    position,
                    frame,
                    Color.Lerp(Color.Transparent, drawColor * 0.5f, progress),
                    NPC.rotation,
                    origin,
                    NPC.scale * Vector2.Lerp(Vector2.One, new Vector2(1.25f), progress),
                    NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                    0
                );
            }
            spriteBatch.Draw(
                texture,
                position,
                frame,
                drawColor,
                NPC.rotation,
                origin,
                NPC.scale,
                NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0
            );
            return false;
        }
    }

    public class MossArm : ModProjectile
    {
        public const int TELEGRAPH_DURATION_TICK = 60;
        public const int ATTACK_DURATION_TICK = 60;
        public const int FRAMES_AMOUNT = 10;
        public const int DUST_CAST_AMOUNT = 5;
        public ref float FrameCounter => ref Projectile.ai[0];
        public ref float ShouldSpawnSwampling => ref Projectile.ai[1];
        public ref float SpawnDelay => ref Projectile.ai[2];

        private readonly List<int> CanSpawnAfterDiggingUp = new List<int>() {
            ModContent.NPCType<Swamling>(), ModContent.NPCType<SwamplingWarrior>(), ModContent.NPCType<MischievousDuo>()
        };
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 36;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = TELEGRAPH_DURATION_TICK + ATTACK_DURATION_TICK;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.aiStyle = -1;
            Projectile.hide = true;
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DontAttachHideToAlpha[Projectile.type] = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.position.Y -= Projectile.height / 2;
        }
        public override void AI()
        {
            if(SpawnDelay > 0)
            {
                SpawnDelay--;
                return;
            }
            FrameCounter++;
            if(FrameCounter == TELEGRAPH_DURATION_TICK)
            {
                SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, Projectile.position);
                for(int i = 0; i < DUST_CAST_AMOUNT; i++)
                    Collision.HitTiles(Projectile.position + new Vector2(0, Projectile.height), Vector2.UnitY, Projectile.width, 6);
                if(ShouldSpawnSwampling == 1)
                    SpawnSwampling();
            }
        }

        void SpawnSwampling()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;

            // 20% chance added
            if (Main.rand.NextFloat() < 0.2f)
            {
                int npcToSpawn = CanSpawnAfterDiggingUp[Main.rand.Next(0, CanSpawnAfterDiggingUp.Count)];
                NPC npc = NPC.NewNPCDirect(
                    Projectile.GetSource_None(),
                    (int)Projectile.Center.X,
                    (int)Projectile.Center.Y,
                    npcToSpawn
                );
                npc.velocity.Y = -12;
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < DUST_CAST_AMOUNT / 2; i++)
                Collision.HitTiles(Projectile.position + new Vector2(0, Projectile.height), Vector2.UnitY, Projectile.width, 6);
        }
        public override bool CanHitPlayer(Player target)
        {
            return FrameCounter > TELEGRAPH_DURATION_TICK;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if(FrameCounter < TELEGRAPH_DURATION_TICK + 15)
                target.GetModPlayer<MossArmThrowEffect>().IsDamaged = true;
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (FrameCounter < TELEGRAPH_DURATION_TICK + 15)
                modifiers.HitDirectionOverride = 0;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 position = Projectile.Center - Main.screenPosition;
            if (FrameCounter <= TELEGRAPH_DURATION_TICK)
                DrawTelegraph(position);
            else
                DrawHand(position, lightColor);
            return false;
        }
        void DrawTelegraph(Vector2 position)
        {
            Texture2D rayTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ray").Value;
            Rectangle frame = new Rectangle(rayTexture.Width / 2, 0, 1, rayTexture.Height);
            float percentage = AttackTelegraphProgress(FrameCounter / TELEGRAPH_DURATION_TICK);
            Color color = Color.Lerp(Color.Transparent, Color.Red, percentage);
            position.Y += Projectile.height / 2;
            Vector2 scale = new Vector2(24f * percentage, 0.5f);
            Vector2 origin = new Vector2(0.5f, rayTexture.Height);
            Main.EntitySpriteDraw(
                rayTexture,
                position + Vector2.UnitY * 2,
                frame,
                color,
                0,
                origin,
                scale,
                SpriteEffects.None,
                0
            );
        }
        void DrawHand(Vector2 position, Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            int frameCount = GetHandFrame();
            int frameHeight = texture.Height / FRAMES_AMOUNT;
            Rectangle frame = new Rectangle(0, frameCount * frameHeight, texture.Width, frameHeight);
            Vector2 origin = new Vector2(texture.Width / 2f, frameHeight - Projectile.height / 2);
            Main.EntitySpriteDraw(
                texture,
                position + Vector2.UnitY * 2,
                frame,
                lightColor,
                0,
                origin,
                1f,
                SpriteEffects.None,
                0
            );
        }
        float AttackTelegraphProgress(float progress)
        {
            if (progress < 0.3)
                return EaseFunctions.EaseOutQuad(progress / 0.3f);
            else if (progress < 0.9)
                return 1f - EaseFunctions.EaseInCubic((progress - 0.3f) / 0.6f);
            else
                return 0;
        }
        int GetHandFrame()
        {
            int frameCounter = (int)FrameCounter - TELEGRAPH_DURATION_TICK;
            if (frameCounter < ATTACK_DURATION_TICK - 9)
                return Math.Clamp(frameCounter / 5, 0, 9);
            else
                return ATTACK_DURATION_TICK - frameCounter;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }
    }

    public class MossArmThrowEffect : ModPlayer
    {
        public bool IsDamaged;
        public const float ThrowingForce = 14;
        public override void PostHurt(Player.HurtInfo info)
        {
            if (!IsDamaged) return;

            Player.velocity.Y = -ThrowingForce;
            Player.AddBuff(BuffID.Slow, 120, false);
            IsDamaged = false;
        }
        public override void UpdateDead()
        {
            IsDamaged = false;
        }
    }
}
