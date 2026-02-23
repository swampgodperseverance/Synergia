using System;
using Bismuth.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Synergia.Common.SUtils.LocUtil;

namespace Synergia.Content.NPCs.Swamp
{
    public class MischievousDuo : ModNPC
    {
        public override string LocalizationCategory => Category(CategoryName.NPC);

        private int wanderTimer;
        private int idleTimer;
        private int actionTimer;
        private int stepUpCooldown;
        private bool idleState;
        private float currentSpeedX;

        private const float WALK_SPEED = 1.3f;
        private const float CHASE_SPEED = 3.1f;
        private const float ACCEL = 0.18f;
        private const float FRICTION = 0.9f;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 14;
        }

        public override void SetDefaults()
        {
            NPC.width = 50;
            NPC.height = 60;
            NPC.damage = 24;
            NPC.defense = 9;
            NPC.lifeMax = 165;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 200f;
            NPC.knockBackResist = 0.55f;
            NPC.aiStyle = -1;
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];

            float distance = Vector2.Distance(NPC.Center, player.Center);
            bool canSee = Collision.CanHitLine(NPC.Center, 1, 1, player.Center, 1, 1);
            bool aggro = canSee && distance < 450f && player.active && !player.dead;

            TryStepUp();

            if (!aggro)
                IdleBehavior();
            else
                AggressiveBehavior(player, distance);

            ApplyPhysics();
            UpdateDirection();
            SpawnWalkEffects();
        }

        private void IdleBehavior()
        {
            actionTimer++;
            wanderTimer++;

            if (actionTimer > 170 + Main.rand.Next(120))
            {
                actionTimer = 0;
                int action = Main.rand.Next(4);

                if (action == 0)
                {
                    idleState = true;
                    idleTimer = 0;
                }
                else if (action == 1)
                {
                    NPC.direction *= -1;
                }
                else if (action == 2)
                {
                    currentSpeedX = NPC.direction * WALK_SPEED * 1.6f;
                }
                else
                {
                    idleState = true;
                    idleTimer = -25;
                }
            }

            if (idleState)
            {
                currentSpeedX *= 0.88f;
                idleTimer++;

                if (idleTimer > 50 + Main.rand.Next(60))
                {
                    idleState = false;
                    idleTimer = 0;
                }

                return;
            }

            float targetSpeed = NPC.direction * WALK_SPEED;
            currentSpeedX = MathHelper.Lerp(currentSpeedX, targetSpeed, ACCEL);

            if (wanderTimer > 200 && Main.rand.NextBool(8))
            {
                wanderTimer = 0;
                idleState = true;
            }
        }

        private void AggressiveBehavior(Player player, float distance)
        {
            float speedMultiplier = 1f;

            if (distance < 80f)
                speedMultiplier = 0.7f;
            else if (distance > 300f)
                speedMultiplier = 1.3f;

            float direction = player.Center.X > NPC.Center.X ? 1 : -1;
            float targetSpeed = direction * CHASE_SPEED * speedMultiplier;

            currentSpeedX = MathHelper.Lerp(currentSpeedX, targetSpeed, ACCEL * 1.3f);
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

                    for (int i = 0; i < 4; i++)
                    {
                        Dust.NewDust(
                            NPC.Bottom + new Vector2(-10, -8),
                            20,
                            8,
                            ModContent.DustType<SwampDust>(),
                            NPC.direction * 1.3f,
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
                NPC.velocity.Y += 0.35f;
                if (NPC.velocity.Y > 10f)
                    NPC.velocity.Y = 10f;
            }

            if (NPC.collideY && Math.Abs(NPC.velocity.Y) < 0.1f)
            {
                currentSpeedX *= FRICTION;
                if (Math.Abs(currentSpeedX) < 0.05f)
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

        private void SpawnWalkEffects()
        {
            if (!NPC.collideY || Math.Abs(NPC.velocity.X) < 0.8f)
                return;

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(
                    NPC.Bottom + new Vector2(-12, -6),
                    24,
                    6,
                    ModContent.DustType<SwampDust>(),
                    NPC.velocity.X * 0.2f,
                    -Main.rand.NextFloat(0.5f, 1.5f)
                );

                if (Main.rand.NextBool(2))
                {
                    dust.noGravity = true;
                    dust.velocity.Y -= 0.2f;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (!NPC.collideY)
            {
                NPC.frame.Y = frameHeight;
                return;
            }

            if (idleState && Math.Abs(NPC.velocity.X) < 0.2f)
            {
                NPC.frame.Y = 0;
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

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    Dust.NewDust(
                        NPC.position,
                        NPC.width,
                        NPC.height,
                        ModContent.DustType<SwampDust>(),
                        Main.rand.NextFloat(-3f, 3f),
                        Main.rand.NextFloat(-3f, 1f)
                    );
                }

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.NewNPC(
                        NPC.GetSource_Death(),
                        (int)NPC.Center.X,
                        (int)NPC.Center.Y,
                        ModContent.NPCType<SwamplingWarrior>()
                    );

                    NPC.NewNPC(
                        NPC.GetSource_Death(),
                        (int)NPC.Center.X,
                        (int)NPC.Center.Y,
                        ModContent.NPCType<Swamling>()
                    );
                }
            }
        }
    }
}