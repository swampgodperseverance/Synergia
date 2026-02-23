using System;
using Bismuth.Content.Dusts;
using Bismuth.Content.Items.Weapons.Throwing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Material;
using static Synergia.Common.SUtils.LocUtil;

namespace Synergia.Content.NPCs.Swamp
{
    public class SwamplingWarrior : ModNPC
    {
        public override string LocalizationCategory => Category(CategoryName.NPC);
        private bool wasAggro;
        private int wanderTimer;
        private int idleTimer;
        private int jumpCooldown;
        private int actionTimer;
        private int stepUpCooldown;

        private bool idleState;

        private float currentSpeedX;

        private bool isPreparingDash;
        private bool isJumpingForDash;
        private bool isDashing;

        private int dashPrepareTimer;
        private int dashTimer;
        private int dashCooldown;

        private const float WALK_SPEED = 1.2f;
        private const float CHASE_SPEED = 2.8f;
        private const float ACCEL = 0.15f;
        private const float FRICTION = 0.88f;

        private const int DASH_PREPARE_TIME = 20;
        private const int DASH_DURATION = 20;
        private const float DASH_SPEED = 8.5f;

        private int baseDamage;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 14;
            NPCID.Sets.TrailCacheLength[NPC.type] = 3;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
        }

        public override void SetDefaults()
        {
            NPC.width = 50;
            NPC.height = 42 ;
            NPC.damage = 20;
            baseDamage = NPC.damage;
            NPC.defense = 8;
            NPC.lifeMax = 145;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 160f * 0.9f;
            NPC.knockBackResist = 0.65f;
            NPC.aiStyle = -1;
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];

            float distance = Vector2.Distance(NPC.Center, player.Center);
            bool canSee = Collision.CanHitLine(NPC.Center, 1, 1, player.Center, 1, 1);
            bool aggro = canSee && distance < 400f && player.active && !player.dead;
            if (aggro && !wasAggro)
            {
                OnEnterAggro(player);
                wasAggro = true;
            }
            else if (!aggro)
            {
                wasAggro = false;
            }

            dashCooldown--;

            TryStepUp();

            if (aggro)
                AggressiveBehavior(player, distance);
            else
                IdleBehavior();

            ApplyPhysics();
            UpdateDirection();
            SpawnWalkEffects();
        }

        private void AggressiveBehavior(Player player, float distance)
        {
            if (isPreparingDash)
            {
                dashPrepareTimer--;
                currentSpeedX *= 0.8f;

                if (dashPrepareTimer <= 0)
                {
                    isPreparingDash = false;
                    isJumpingForDash = true;

                    NPC.velocity.Y = -10.5f;
                    NPC.velocity.X = 0f;
                }

                return;
            }

            if (isJumpingForDash)
            {
                if (NPC.velocity.Y >= 0f)
                {
                    isJumpingForDash = false;
                    isDashing = true;
                    dashTimer = DASH_DURATION;

                    float dir = Math.Sign(player.Center.X - NPC.Center.X);
                    NPC.velocity.X = dir * DASH_SPEED;
                    currentSpeedX = NPC.velocity.X;
                }

                return;
            }

            if (isDashing)
            {
                dashTimer--;

                float dir = Math.Sign(player.Center.X - NPC.Center.X);
                NPC.velocity.X = dir * DASH_SPEED;
                currentSpeedX = NPC.velocity.X;

                NPC.damage = (int)(baseDamage * 2f);
                NPC.knockBackResist = 0f;

                if (dashTimer <= 0 || NPC.collideX)
                {
                    isDashing = false;
                    dashCooldown = 240;
                    NPC.damage = baseDamage;
                    NPC.knockBackResist = 0.65f;
                }

                return;
            }

            jumpCooldown++;

            float speedMultiplier = 1f;

            if (distance < 80f)
                speedMultiplier = 0.5f;
            else if (distance > 300f)
                speedMultiplier = 1.3f;

            float targetSpeed = (player.Center.X > NPC.Center.X ? 1 : -1) * CHASE_SPEED * speedMultiplier;
            currentSpeedX = MathHelper.Lerp(currentSpeedX, targetSpeed, ACCEL * 1.3f);

            if (jumpCooldown > 40 + Main.rand.Next(30) && NPC.collideY)
            {
                jumpCooldown = 0;
                float jumpPower = distance > 200f ? -8.2f : -6.8f;
                NPC.velocity.Y = jumpPower;
            }

            if (!isDashing && !isPreparingDash && dashCooldown <= 0 && NPC.collideY)
            {
                if (Main.rand.NextBool(220))
                {
                    isPreparingDash = true;
                    dashPrepareTimer = DASH_PREPARE_TIME;
                    SoundEngine.PlaySound(SoundID.Zombie35 with { Pitch = -0.2f }, NPC.Center);
                }
            }
        }
        private void OnEnterAggro(Player player)
        {
            SoundStyle SwaR = Reassures.Reassures.RSounds.SwamplingRoar;
            SwaR.Volume = 0.8f;
            SwaR.PitchVariance = 0.12f;

            SoundEngine.PlaySound(
SwaR,
NPC.Center
);
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath27, NPC.position);
                for (int k = 1; k < 4; k++)
                {
                    if (Mod.Find<ModGore>($"SwampGore{k}") != null)
                    {
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity * 0.4f, Mod.Find<ModGore>($"SwampGore{k}").Type);
                    }
                }
                for (int i = 0; i < 24; i++)
                {
                    Dust d = Dust.NewDustPerfect(NPC.Center, DustID.Blood, Main.rand.NextVector2Circular(5f, 5f), 50);
                    d.noGravity = true;
                    d.scale = Main.rand.NextFloat(0.9f, 1.6f);
                }
            }
        }
        private void IdleBehavior()
        {
            actionTimer++;
            wanderTimer++;

            if (actionTimer > 200)
            {
                actionTimer = 0;
                NPC.direction *= -1;
            }

            float targetSpeed = NPC.direction * WALK_SPEED;
            currentSpeedX = MathHelper.Lerp(currentSpeedX, targetSpeed, ACCEL);
        }

        private void TryStepUp()
        {
            if (!NPC.collideY || stepUpCooldown > 0)
            {
                stepUpCooldown--;
                return;
            }

            Vector2 frontCheck = NPC.position + new Vector2(NPC.direction * 22, 0);
            bool blocked = Collision.SolidCollision(frontCheck, NPC.width, NPC.height - 8);

            if (blocked && NPC.collideX)
            {
                Vector2 stepUpPos = NPC.position + new Vector2(NPC.direction * 20, -20);

                if (!Collision.SolidCollision(stepUpPos, NPC.width, NPC.height))
                {
                    NPC.position.Y -= 20;
                    NPC.velocity.Y = -1f;
                    stepUpCooldown = 10;
                }
            }
        }

        private void ApplyPhysics()
        {
            NPC.velocity.X = currentSpeedX;

            if (!NPC.collideY)
            {
                NPC.velocity.Y += 0.35f;
                if (NPC.velocity.Y > 12f)
                    NPC.velocity.Y = 12f;
            }

            if (NPC.collideY && Math.Abs(NPC.velocity.Y) < 0.1f)
            {
                currentSpeedX *= FRICTION;
                if (Math.Abs(currentSpeedX) < 0.1f)
                    currentSpeedX = 0f;
            }
        }

        private void UpdateDirection()
        {
            if (Math.Abs(currentSpeedX) > 0.1f)
            {
                NPC.direction = Math.Sign(currentSpeedX);
                NPC.spriteDirection = NPC.direction;
            }
        }
                
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Lancea>(), 7, 1, 1));
        }
        private void SpawnWalkEffects()
        {
            if (!NPC.collideY || Math.Abs(NPC.velocity.X) < 0.8f)
                return;

            if (Main.rand.NextBool(4))
                Dust.NewDust(NPC.Bottom, 10, 6, ModContent.DustType<SwampDust>(), NPC.velocity.X * 0.2f, -1f);
        }

        public override void FindFrame(int frameHeight)
        {
            if (isPreparingDash || isJumpingForDash || isDashing)
            {
                NPC.frame.Y = frameHeight * 6;
                return;
            }

            if (!NPC.collideY)
            {
                NPC.frame.Y = frameHeight;
                return;
            }

            float speed = Math.Abs(currentSpeedX);
            float animSpeed = speed > 2f ? 3f : 6f;

            NPC.frameCounter += speed * 1.5f;

            if (NPC.frameCounter >= animSpeed)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y >= frameHeight * 13)
                    NPC.frame.Y = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (isDashing)
            {
                Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;

                for (int i = 0; i < NPC.oldPos.Length; i++)
                {
                    Vector2 drawPos = NPC.oldPos[i] + NPC.Size / 2f - screenPos;
                    float alpha = (NPC.oldPos.Length - i) / (float)NPC.oldPos.Length;

                    spriteBatch.Draw(
                        texture,
                        drawPos,
                        NPC.frame,
                        Color.DarkOliveGreen * alpha * 0.5f,
                        NPC.rotation,
                        NPC.frame.Size() / 2f,
                        NPC.scale,
                        NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                        0f
                    );
                }
            }

            return true;
        }
    }
}