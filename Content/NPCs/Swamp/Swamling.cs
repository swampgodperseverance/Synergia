using System;
using Bismuth.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Synergia.Common.SUtils.LocUtil;

namespace Synergia.Content.NPCs.Swamp
{
    public class Swamling : ModNPC
    {
        public override string LocalizationCategory => Category(CategoryName.NPC);
        private bool wasAggro;
        private int wanderTimer;
        private int idleTimer;
        private int jumpCooldown;
        private int actionTimer;
        private int stepUpCooldown;
        private bool idleState;
        private bool isJumping;
        private float currentSpeedX;

        private const float WALK_SPEED = 1.2f;
        private const float CHASE_SPEED = 2.8f;
        private const float ACCEL = 0.15f;
        private const float FRICTION = 0.88f;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 14;
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 48;
            NPC.damage = 20;
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
            NPC.TargetClosest(faceTarget: true);

            Player player = Main.player[NPC.target];
            float distanceToTarget = Vector2.Distance(NPC.Center, player.Center);

            bool canSeePlayer = Collision.CanHitLine(
                NPC.Center, 1, 1,
                player.Center, 1, 1
            );  

            bool aggro = canSeePlayer && distanceToTarget < 400f && player.active && !player.dead;
            if (aggro && !wasAggro)
            {
                OnEnterAggro(player);
                wasAggro = true;
            }
            else if (!aggro)
            {
                wasAggro = false;
            }

            TryStepUp();

            if (!aggro)
            {
                IdleBehavior();
            }
            else
            {
                AggressiveBehavior(player, distanceToTarget);
            }

            ApplyPhysics();
            UpdateDirection();
            SpawnWalkEffects();
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

        private void IdleBehavior()
        {
            actionTimer++;
            wanderTimer++;

            // side change
            if (actionTimer > 180 + Main.rand.Next(120))
            {
                actionTimer = 0;

                int action = Main.rand.Next(6);

                switch (action)
                {
                    case 0: // case 0 is about stupid idlin      
                        idleState = true;
                        idleTimer = 0;
                        break;

                    case 1: // direction change for more living behaviour 
                        NPC.direction *= -1;
                        break;

                    case 2: // jumps
                        if (NPC.collideY)
                        {
                            NPC.velocity.Y = -5.5f;
                            NPC.velocity.X += NPC.direction * 0.5f;
                        }
                        break;

                    case 3: // digging
                        if (Main.rand.NextBool(3))
                        {
                            // couldnt choose dusts
                            for (int i = 0; i < 3; i++)
                            {
                                Dust.NewDust(
                                    NPC.Bottom + new Vector2(-10, -10),
                                    20, 10,
                                    ModContent.DustType<SwampDust>(),
                                    Main.rand.NextFloat(-1f, 1f),
                                    -Main.rand.NextFloat(1f, 2f)
                                );
                            }
                        }
                        break;

                    case 4: // 1 sec acceleration
                        currentSpeedX = NPC.direction * WALK_SPEED * 1.5f;
                        break;

                    case 5: // pause
                        idleState = true;
                        idleTimer = -30; 
                        break;
                }
            }

            if (idleState)
            {
                currentSpeedX *= 0.9f;
                idleTimer++;
                if (idleTimer % 30 == 0 && Main.rand.NextBool(3))
                {
                    NPC.direction = Main.rand.NextBool() ? 1 : -1;
                }
                if (idleTimer > 45 + Main.rand.Next(60))
                {
                    idleState = false;
                    idleTimer = 0;
                }
                return;
            }

            float targetSpeed = NPC.direction * (WALK_SPEED + Main.rand.NextFloat(-0.2f, 0.2f));
            currentSpeedX = MathHelper.Lerp(currentSpeedX, targetSpeed, ACCEL * 1.2f);

                                                        if (wanderTimer > 200 && Main.rand.NextBool(10))
                                                        {
                                                            wanderTimer = 0;
                                                            idleState = true;
                                                        }
        }

        private void AggressiveBehavior(Player player, float distance)
        {
            jumpCooldown++;
//chase depends on distance 
            float speedMultiplier = 1f;

            if (distance < 80f)
                speedMultiplier = 0.5f; // nearby
            else if (distance > 300f)
                speedMultiplier = 1.3f; // farby

            float targetSpeed = (player.Center.X > NPC.Center.X ? 1 : -1) * CHASE_SPEED * speedMultiplier;
            currentSpeedX = MathHelper.Lerp(currentSpeedX, targetSpeed, ACCEL * 1.3f);

            // aggresive jumps
            if (jumpCooldown > 40 + Main.rand.Next(30) && NPC.collideY)
            {
                jumpCooldown = 0;

                float jumpPower = distance > 200f ? -8.2f : -6.8f;
                NPC.velocity.Y = jumpPower;

                // overwhelming jump
                float horizontalBoost = (player.Center.X - NPC.Center.X) * 0.025f;
                NPC.velocity.X += MathHelper.Clamp(horizontalBoost, -2.5f, 2.5f);

                // again digging
                for (int i = 0; i < 6; i++)
                {
                    Dust dust = Dust.NewDustDirect(
                        NPC.Bottom + new Vector2(-15, -8),
                        30, 8,
                        ModContent.DustType<SwampDust>(),
                        NPC.velocity.X * 0.3f,
                        -Main.rand.NextFloat(2f, 4f),
                        Scale: 1.2f
                    );
                    dust.noGravity = true;
                }
            }
        }

        private void TryStepUp()
        {
            if (!NPC.collideY || stepUpCooldown > 0)
            {
                stepUpCooldown--;
                return;
            }
            Vector2 frontCheck = NPC.position + new Vector2(NPC.direction * 22, 0);
            Rectangle frontRect = new Rectangle((int)frontCheck.X, (int)frontCheck.Y, NPC.width, NPC.height);

            bool blocked = Collision.SolidCollision(frontCheck, NPC.width, NPC.height - 8);

            if (blocked && NPC.collideX)
            {
                Vector2 stepUpPos = NPC.position + new Vector2(NPC.direction * 20, -20);

                if (!Collision.SolidCollision(stepUpPos, NPC.width, NPC.height))
                {
                    NPC.position.Y -= 20;
                    NPC.velocity.Y = -1f;
                    stepUpCooldown = 10;

                    for (int i = 0; i < 3; i++)
                    {
                        Dust.NewDust(
                            NPC.Bottom + new Vector2(-10, -8),
                            20, 8,
                            ModContent.DustType<SwampDust>(),
                            NPC.direction * 1.5f,
                            -1f
                        );
                    }
                }
            }
        }

        private void ApplyPhysics()
        {
            NPC.velocity.X = currentSpeedX;

            if (!NPC.collideY)
            {
                NPC.velocity.Y += 0.3f;
                if (NPC.velocity.Y > 10f)
                    NPC.velocity.Y = 10f;
            }

            if (NPC.collideY && Math.Abs(NPC.velocity.Y) < 0.1f)
            {
                currentSpeedX *= FRICTION;
                if (Math.Abs(currentSpeedX) < 0.1f)
                {
                    currentSpeedX = 0f;
                }
            }
        }

        private void UpdateDirection()
        {
            if (Math.Abs(currentSpeedX) > 0.1f)
            {
                NPC.direction = Math.Sign(currentSpeedX);
                NPC.spriteDirection = NPC.direction;
            }
            else if (Main.player[NPC.target].active && !Main.player[NPC.target].dead)
            {
                float toPlayer = Main.player[NPC.target].Center.X - NPC.Center.X;
                if (Math.Abs(toPlayer) < 200f && Main.rand.NextBool(10))
                {
                    NPC.direction = Math.Sign(toPlayer);
                    NPC.spriteDirection = NPC.direction;
                }
            }
        }

        private void SpawnWalkEffects()
        {
            if (!NPC.collideY || Math.Abs(NPC.velocity.X) < 0.8f) return;

            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(
                    NPC.Bottom + new Vector2(-12, -6),
                    24, 6,
                    ModContent.DustType<SwampDust>(),
                    NPC.velocity.X * 0.2f,
                    -Main.rand.NextFloat(0.5f, 1.5f),
                    Scale: Main.rand.NextFloat(0.7f, 1.1f)
                );

                if (Main.rand.NextBool(3))
                {
                    dust.noGravity = true;
                    dust.velocity.Y -= 0.3f;
                }
            }

            if (Main.rand.NextBool(15))
            {
                Gore.NewGore(
                    NPC.GetSource_FromAI(),
                    NPC.Bottom + new Vector2(Main.rand.Next(-15, 15), -8),
                    new Vector2(NPC.velocity.X * 0.3f, -Main.rand.NextFloat(0.5f, 1f)),
                    GoreID.TreeLeaf_Normal
                );
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (!NPC.collideY)
            {
                NPC.frame.Y = frameHeight * 1;

                if (Math.Abs(NPC.velocity.Y) > 4f && Main.rand.NextBool(3))
                {
                    NPC.frame.Y = frameHeight * 2;
                }
                return;
            }

            if (idleState && Math.Abs(NPC.velocity.X) < 0.2f)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter > 30)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y = frameHeight * 13;
                }
                else
                {
                    NPC.frame.Y = 0; 
                }
                return;
            }

            float speed = Math.Abs(currentSpeedX);
            float animSpeed = speed > 2f ? 3f : (speed > 1f ? 5f : 8f);

            NPC.frameCounter += speed * 1.5f;

            if (NPC.frameCounter >= animSpeed)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y >= frameHeight * 13)
                {
                    NPC.frame.Y = 0;
                }
            }
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
    }
}