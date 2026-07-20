using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Common.GlobalPlayer;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Synergia.Content.NPCs.Underworld{
    public class Nibbler : ModNPC{
        private enum NibbleState
        {
            Flying,
            Dashing
        }

        private NibbleState currentState = NibbleState.Flying;
        private int dashTimer = 0;
        private int dashCooldown = 0;
        private int flightTimer = 0;
        private Vector2 dashVelocity = Vector2.Zero;
        private float targetXOffset = 0f;
        private float currentXOffset = 0f;
        private int wingSoundTimer = 0;

        private bool isTeleporting = false;
        private Vector2 teleportStartPos;
        private Vector2 teleportTargetPos;
        private int teleportProgress = 0;
        private int teleportDuration = 10;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;
            Lists.NPCs.NewHellNPCs.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers
            {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(base.Type, value);
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("Flies around the bridge and trying to escape its own nightmares.")
            });
        }

        public override void SetDefaults(){
            NPC.width = 34;
            NPC.height = 34;
            NPC.damage = 50;
            NPC.defense = 12;
            NPC.lifeMax = 480;
            NPC.value = 500f;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.knockBackResist = 0.3f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true; 
        }

        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Player target = Main.player[NPC.target];

                if (!target.active || target.dead)
                {
                    NPC.TargetClosest(false);
                    target = Main.player[NPC.target];
                    if (!target.active || target.dead)
                    {
                        NPC.velocity.Y -= 0.1f;
                        if (NPC.timeLeft > 60) NPC.timeLeft = 60;
                        return;
                    }
                }

                switch (currentState)
                {
                    case NibbleState.Flying:
                        FlyingBehavior(target);
                        break;
                    case NibbleState.Dashing:
                        DashingBehavior(target);
                        break;
                }

                NPC.rotation = NPC.velocity.X * 0.1f;

                if (currentState != NibbleState.Dashing)
                {
                    if (NPC.velocity.X > 0)
                        NPC.spriteDirection = 1;
                    else if (NPC.velocity.X < 0)
                        NPC.spriteDirection = -1;
                }

                if (Main.netMode == NetmodeID.Server)
                {
                    NPC.netUpdate = true;
                }
            }
            else
            {
                if (isTeleporting)
                {
                    teleportProgress++;
                    float t = (float)teleportProgress / teleportDuration;
                    float smoothT = MathHelper.SmoothStep(0, 1, t);
                    NPC.position = Vector2.Lerp(teleportStartPos, teleportTargetPos, smoothT);

                    if (Main.rand.NextBool(3))
                    {
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Shadowflame,
                            (teleportTargetPos.X - teleportStartPos.X) * 0.1f,
                            (teleportTargetPos.Y - teleportStartPos.Y) * 0.1f, 80, default, 1f);
                    }

                    if (teleportProgress >= teleportDuration)
                    {
                        isTeleporting = false;
                        NPC.netUpdate = true;
                    }
                }
            }
        }

        private void FlyingBehavior(Player target)
        {
            flightTimer++;
            if (dashCooldown > 0) dashCooldown--;

            float desiredY = target.Center.Y - 120f - (float)System.Math.Sin(flightTimer * 0.03f) * 20f;

            if (flightTimer % 180 == 0 || targetXOffset == 0)
            {
                targetXOffset = Main.rand.Next(-180, 181);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    NPC.netUpdate = true;
            }

            currentXOffset = MathHelper.Lerp(currentXOffset, targetXOffset, 0.05f);
            float desiredX = target.Center.X + currentXOffset;

            Vector2 desiredPosition = new Vector2(desiredX, desiredY);
            Vector2 direction = desiredPosition - NPC.Center;
            float distance = direction.Length();

            if (distance > 5f)
            {
                direction.Normalize();
                NPC.velocity = (NPC.velocity * 0.95f) + (direction * 0.15f);
                NPC.velocity = Vector2.Clamp(NPC.velocity, new Vector2(-6f, -6f), new Vector2(6f, 6f));
            }
            else
            {
                NPC.velocity *= 0.95f;
            }

            if (dashCooldown == 0 && Main.rand.NextBool(180) && distance < 150f)
            {
                StartDash(target);
            }
        }

        private void StartDash(Player target){
            currentState = NibbleState.Dashing;
            dashTimer = 25;

            Vector2 dashDirection = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
            dashVelocity = dashDirection * 20f;
            if (target.Center.X > NPC.Center.X)
                NPC.spriteDirection = 1;
            else
                NPC.spriteDirection = -1;
            for (int i = 0; i < 15; i++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood,
                    -dashVelocity.X * 0.5f, -dashVelocity.Y * 0.5f, 100, default, 1.5f);
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood,
                    -dashVelocity.X * 0.3f, -dashVelocity.Y * 0.3f, 100, default, 1.2f);
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
                NPC.netUpdate = true;
        }

        private void DashingBehavior(Player target){
            NPC.velocity = dashVelocity;

            if (dashTimer > 0 && Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Blood,
                    -NPC.velocity.X * 0.3f, -NPC.velocity.Y * 0.3f, 80, default, 1.3f);
                dust.noGravity = true;
            }

            dashTimer--;

            if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
            {
                TeleportToSafeSpot(target);
                return;
            }

            if (dashTimer <= 0)
            {
                EndDash(target);
            }
        }

        private void TeleportToSafeSpot(Player target){
            Vector2 bestPosition = NPC.position;
            bool foundSpot = false;
            for (int radius = 60; radius <= 250; radius += 30)
            {
                for (int angle = 0; angle < 360; angle += 15)
                {
                    Vector2 offset = new Vector2(radius, 0).RotatedBy(MathHelper.ToRadians(angle));
                    Vector2 teleportPos = target.Center + offset - new Vector2(NPC.width / 2, NPC.height / 2);

                    Rectangle npcRect = new Rectangle((int)teleportPos.X, (int)teleportPos.Y, NPC.width, NPC.height);

                    if (!Collision.SolidCollision(teleportPos, NPC.width, NPC.height) &&
                        !Collision.CheckAABBvAABBCollision(teleportPos, new Vector2(NPC.width, NPC.height), target.position, target.Hitbox.Size()))
                    {
                        bestPosition = teleportPos;
                        foundSpot = true;
                        break;
                    }
                }
                if (foundSpot) break;
            }

            if (foundSpot)
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    PerformTeleport(bestPosition);
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    NPC.position = bestPosition;
                    NPC.netUpdate = true;

                    for (int i = 0; i < 20; i++)
                    {
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Shadowflame, 0, 0, 100, default, 1.2f);
                    }
                }
            }

            EndDash(target);
        }

        private void PerformTeleport(Vector2 targetPosition)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                isTeleporting = true;
                teleportStartPos = NPC.position;
                teleportTargetPos = targetPosition;
                teleportProgress = 0;

                for (int i = 0; i < 15; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Shadowflame, 0, 0, 100, default, 1f);
                }
                SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
            }
            else
            {
                NPC.position = targetPosition;
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Shadowflame, 0, 0, 100, default, 1.2f);
                }
                SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
            }
        }

        private void EndDash(Player target){
            currentState = NibbleState.Flying;
            dashCooldown = 420;
            NPC.velocity *= 0.5f;

            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 0, 0, 80, default, 1f);
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
                NPC.netUpdate = true;
        }

        public override void FindFrame(int frameHeight){
            if (currentState == NibbleState.Dashing)
            {
                NPC.frame.Y = 5 * frameHeight;
            }
            else
            {
                NPC.frameCounter++;
                if (NPC.frameCounter >= 6)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y = (NPC.frame.Y / frameHeight + 1) % 5 * frameHeight;
                }
                if ((NPC.frame.Y / frameHeight) == 3 && wingSoundTimer == 0)
                {
                    SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, NPC.Center);
                    wingSoundTimer = 10;
                }
            }

            if (wingSoundTimer > 0) wingSoundTimer--;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor){
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / Main.npcFrameCount[Type] / 2);
            SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            float drawRotation = NPC.rotation;

            if (currentState == NibbleState.Dashing)
            {
                Player target = Main.player[NPC.target];
                if (target.active && !target.dead)
                {
                    Vector2 directionToPlayer = target.Center - NPC.Center;
                    float angleToPlayer = directionToPlayer.ToRotation();

                    drawRotation = angleToPlayer + MathHelper.Pi;
                }
            }

            if (currentState != NibbleState.Dashing)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 offset = new Vector2(1, 0).RotatedBy(MathHelper.PiOver2 * i);
                    spriteBatch.Draw(texture, NPC.Center + offset - screenPos, NPC.frame, Color.Black * 0.5f,
                        drawRotation, origin, NPC.scale, effects, 0f);
                }
            }

            if (currentState == NibbleState.Dashing && dashTimer > 0)
            {
                for (int i = 1; i <= 4; i++)
                {
                    float opacity = 0.4f - (i * 0.1f);
                    Vector2 trailPos = NPC.Center - NPC.velocity * i * 0.8f - screenPos;
                    spriteBatch.Draw(texture, trailPos, NPC.frame, Color.Red * opacity,
                        drawRotation, origin, NPC.scale, effects, 0f);
                }
            }

            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, drawColor,
                drawRotation, origin, NPC.scale, effects, 0f);

            return false;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return true;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (currentState == NibbleState.Dashing)
            {
                modifiers.SourceDamage *= 2f;
            }
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (currentState == NibbleState.Dashing)
                return false;
            return null; 
        }

        public override bool CanBeHitByNPC(NPC attacker)
        {
            if (currentState == NibbleState.Dashing)
                return false;
            return true;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 8; i++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood,
                    hit.HitDirection * 2, -2f, 80, default, 1f);
            }

            if (NPC.life <= 0)
            {
                for (int i = 0; i < 30; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood,
                        hit.HitDirection * 3, -3f, 100, default, 1.5f);
                }
                SoundEngine.PlaySound(SoundID.NPCDeath1, NPC.Center);
            }
        }

        public override bool CheckDead()
        {
            for (int i = 0; i < 40; i++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood,
                    Main.rand.Next(-3, 4), Main.rand.Next(-3, 4), 100, default, 1.8f);
            }
            return true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((int)currentState);
            writer.Write(dashTimer);
            writer.Write(dashCooldown);
            writer.Write(currentXOffset);
            writer.Write(targetXOffset);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo) => NPC.downedPlantBoss && spawnInfo.Player.GetModPlayer<BiomePlayer>().lakeBiome && spawnInfo.Player.ZoneUnderworldHeight ? 0.6f : 0f;
        public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.Add(ItemDropRule.Common(ItemType<ValhallaMod.Items.Placeable.Blocks.SinstoneMagma>(), 1, 2, 5));
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            currentState = (NibbleState)reader.ReadInt32();
            dashTimer = reader.ReadInt32();
            dashCooldown = reader.ReadInt32();
            currentXOffset = reader.ReadSingle();
            targetXOffset = reader.ReadSingle();
        }
    }
}