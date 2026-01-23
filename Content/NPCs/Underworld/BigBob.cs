using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Synergia.Common.SUtils.LocUtil;

namespace Synergia.Content.NPCs.Underworld
{
    public class BigBob : ModNPC
    {
        public override string LocalizationCategory => "NPC";

        private enum AIState { Walking, PreCast, Casting, PostCast }
        private AIState CurrentState = AIState.Walking;

        private int stateTimer;
        private int attackCooldown;
        private bool hasFiredThisCast;

        private const int PRECAST_DURATION = 96;
        private const int CAST_DURATION = 84;
        private const int POSTCAST_DURATION = 54;
        private const int MIN_ATTACK_GAP = 270;
        private const int MAX_ATTACK_GAP = 390;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 20;
        }

        public override void SetDefaults()
        {
            NPC.width = 50;
            NPC.height = 68;
            NPC.scale = 1.2f;
            NPC.damage = 50;
            NPC.defense = 18;
            NPC.lifeMax = 1700;
            NPC.knockBackResist = 0.3f;
            NPC.value = 600f;
            NPC.aiStyle = -1;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.lavaImmune = true;
            NPC.stepSpeed = 2f;
        }

        public override void AI()
        {
            NPC.TargetClosest();
            Player target = Main.player[NPC.target];

            bool hasAggro = target.active && !target.dead &&
                            Vector2.Distance(NPC.Center, target.Center) < 2000f;

            if (hasAggro)
            {
                NPC.direction = target.Center.X > NPC.Center.X ? 1 : -1;
                NPC.spriteDirection = NPC.direction;
            }

            if (attackCooldown > 0)
                attackCooldown--;

            stateTimer++;

            switch (CurrentState)
            {
                case AIState.Walking:
                    if (hasAggro)
                    {
                        NPC.velocity.X = 1.55f * NPC.direction;
                        if (NPC.collideX)
                            NPC.velocity.Y = -4.7f;
                    }
                    else
                    {
                        NPC.velocity.X *= 0.91f;
                    }

                    if (hasAggro && attackCooldown <= 0 && stateTimer >= Main.rand.Next(180, 301))
                    {
                        CurrentState = AIState.PreCast;
                        stateTimer = 0;
                        NPC.velocity *= 0.3f;
                        attackCooldown = Main.rand.Next(MIN_ATTACK_GAP, MAX_ATTACK_GAP + 1);
                        NPC.netUpdate = true;
                    }
                    break;

                case AIState.PreCast:
                    NPC.velocity *= 0.84f;

                    if (stateTimer >= PRECAST_DURATION)
                    {
                        CurrentState = AIState.Casting;
                        stateTimer = 0;
                        hasFiredThisCast = false;
                        NPC.netUpdate = true;
                    }
                    break;

                case AIState.Casting:
                    NPC.velocity = Vector2.Zero;
                    NPC.rotation = 0f;

                    float progress = (float)stateTimer / CAST_DURATION;

                    if (Main.rand.NextFloat() < 0.12f + progress * 0.18f)
                    {
                        float rot = Main.rand.NextFloat(MathHelper.TwoPi);
                        float speed = 0.9f + progress * 2.1f;
                        Vector2 vel = rot.ToRotationVector2() * speed;
                        int d = Dust.NewDust(NPC.Center, 0, 0, DustID.LifeDrain, vel.X, vel.Y, 140, default, 1.1f);
                        Main.dust[d].noGravity = true;
                    }

                    if (stateTimer >= 48 && stateTimer <= 68)
                    {
                        if (stateTimer == 50 || stateTimer == 54 || stateTimer == 59 || stateTimer == 64)
                        {
                            bool shouldFire = !hasFiredThisCast || Main.rand.NextBool(3);

                            if (shouldFire && hasAggro && target.active &&
                                Collision.CanHitLine(NPC.Center, 1, 1, target.Center, 1, 1))
                            {
                                Vector2 baseDir = target.Center - NPC.Center;
                                baseDir.Normalize();

                                int shotCount = hasFiredThisCast ? Main.rand.Next(1, 3) : 1;

                                for (int i = 0; i < shotCount; i++)
                                {
                                    float spread = Main.rand.NextFloat(-0.12f, 0.12f);
                                    Vector2 dir = baseDir.RotatedBy(spread) * 8.6f;

                                    Projectile.NewProjectile(
                                        NPC.GetSource_FromAI(),
                                        NPC.Center,
                                        dir,
                                        ModContent.ProjectileType<Projectiles.Hostile.BigBobLaser>(),
                                        22,
                                        1f,
                                        Main.myPlayer
                                    );
                                }

                                hasFiredThisCast = true;

                                for (int k = 0; k < 8; k++)
                                {
                                    Vector2 burst = Main.rand.NextVector2Circular(3.5f, 3.5f);
                                    var d = Dust.NewDustPerfect(NPC.Center, DustID.LifeDrain, burst * 1.6f, 100, default, 1.6f);
                                    d.noGravity = true;
                                }
                            }
                        }
                    }

                    if (stateTimer >= CAST_DURATION)
                    {
                        CurrentState = AIState.PostCast;
                        stateTimer = 0;
                    }
                    break;

                case AIState.PostCast:
                    NPC.velocity *= 0.87f;

                    if (stateTimer >= POSTCAST_DURATION)
                    {
                        CurrentState = AIState.Walking;
                        stateTimer = 0;
                        NPC.rotation = 0f;
                    }
                    break;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;

            int walkFrames = 11;
            int precastStart = 11;
            int castStart = 14;

            switch (CurrentState)
            {
                case AIState.Walking:
                    if (NPC.frameCounter >= 7)
                    {
                        NPC.frameCounter = 0;
                        NPC.frame.Y += frameHeight;
                        if (NPC.frame.Y >= frameHeight * walkFrames)
                            NPC.frame.Y = 0;
                    }
                    break;

                case AIState.PreCast:
                    if (NPC.frameCounter >= 11)
                    {
                        NPC.frameCounter = 0;
                        if (NPC.frame.Y / frameHeight < precastStart)
                            NPC.frame.Y = frameHeight * precastStart;
                        else
                        {
                            NPC.frame.Y += frameHeight;
                            if (NPC.frame.Y / frameHeight >= precastStart + 3)
                                NPC.frame.Y = frameHeight * precastStart;
                        }
                    }
                    break;

                case AIState.Casting:
                    if (NPC.frameCounter >= 10)
                    {
                        NPC.frameCounter = 0;
                        if (NPC.frame.Y / frameHeight < castStart)
                            NPC.frame.Y = frameHeight * castStart;
                        else
                        {
                            NPC.frame.Y += frameHeight;
                            if (NPC.frame.Y / frameHeight >= Main.npcFrameCount[NPC.type])
                                NPC.frame.Y = frameHeight * castStart;
                        }
                    }
                    break;

                case AIState.PostCast:
                    NPC.frame.Y = 0;
                    NPC.frameCounter = 0;
                    break;
            }
        }
    }
}