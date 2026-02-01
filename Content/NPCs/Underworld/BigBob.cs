using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Synergia.Common.GlobalPlayer;
using Terraria.ModLoader.IO;
using System.IO;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.NPCs.Underworld
{
    public class BigBob : ModNPC
    {
        public override string LocalizationCategory => "NPC";

        private enum AIState { Walking, PreCast, Casting, PostCast }
        private AIState CurrentState = AIState.Walking;
        private int stateTimer;
        private int attackCooldown;
        private float flashTimer;
        private bool hasFiredThisCast;
        private Vector2 retreatVelocity;
        private Player closestPlayer;

        private const int PRECAST_DURATION = 96;
        private const int CAST_DURATION = 84;
        private const int POSTCAST_DURATION = 54;
        private const int MIN_ATTACK_GAP = 270;
        private const int MAX_ATTACK_GAP = 390;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 20;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.CantTakeLunchMoney[Type] = false;
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
            closestPlayer = Main.player[NPC.target];

            float closestDistance = float.MaxValue;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead)
                {
                    float distance = Vector2.Distance(NPC.Center, player.Center);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPlayer = player;
                        NPC.target = i;
                    }
                }
            }

            bool hasAggro = closestPlayer.active && !closestPlayer.dead && closestDistance < 2000f;

            if (hasAggro)
            {
                NPC.direction = closestPlayer.Center.X > NPC.Center.X ? 1 : -1;
                NPC.spriteDirection = NPC.direction;
            }

            if (attackCooldown > 0) attackCooldown--;
            stateTimer++;

            switch (CurrentState)
            {
                case AIState.Walking:
                    if (hasAggro)
                    {
                        NPC.velocity.X = 1.55f * NPC.direction;
                        if (NPC.collideX) NPC.velocity.Y = -4.7f;
                    }
                    else
                    {
                        NPC.velocity.X *= 0.91f;
                    }

                    if (hasAggro && attackCooldown <= 0 && stateTimer >= Main.rand.Next(180, 301))
                    {
                        CurrentState = AIState.PreCast;
                        stateTimer = 0;
                        hasFiredThisCast = false;
                        retreatVelocity = Vector2.Zero;
                        NPC.velocity *= 0.4f;
                        attackCooldown = Main.rand.Next(MIN_ATTACK_GAP, MAX_ATTACK_GAP + 1);
                        NPC.netUpdate = true;
                    }
                    break;

                case AIState.PreCast:
                    NPC.velocity *= 0.87f;
                    if (stateTimer >= PRECAST_DURATION)
                    {
                        CurrentState = AIState.Casting;
                        stateTimer = 0;
                        NPC.ai[0] = Main.rand.Next(45, 56);
                        NPC.netUpdate = true;
                    }
                    break;

                case AIState.Casting:
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

                    int volleyStart = (int)NPC.ai[0];
                    if (stateTimer >= volleyStart && stateTimer < volleyStart + 30)
                    {
                        if ((stateTimer - volleyStart) % 10 == 0)
                        {
                            int shotIndex = (stateTimer - volleyStart) / 10;
                            if (shotIndex >= 0 && shotIndex < 3 && hasAggro && !closestPlayer.dead)
                            {
                                Vector2 toTarget = closestPlayer.Center - NPC.Center;
                                toTarget.Normalize();
                                float[] spreads = { -0.18f, 0f, 0.18f };
                                float spread = spreads[shotIndex];
                                Vector2 dir = toTarget.RotatedBy(spread) * 8.6f;

                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
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

                                flashTimer = 1f;
                                hasFiredThisCast = true;

                                for (int k = 0; k < 10; k++)
                                {
                                    Vector2 burst = Main.rand.NextVector2Circular(3.5f, 3.5f);
                                    var dd = Dust.NewDustPerfect(NPC.Center, DustID.LifeDrain, burst * 1.6f, 100, default, 1.6f);
                                    dd.noGravity = true;
                                }

                                bool lowHP = NPC.life < NPC.lifeMax * 0.25f;
                                if (lowHP && hasAggro && hasFiredThisCast)
                                {
                                    Vector2 away = NPC.Center - closestPlayer.Center;
                                    away.Normalize();
                                    retreatVelocity = away * 3.5f;
                                }
                            }
                        }
                    }

                    if (retreatVelocity != Vector2.Zero)
                    {
                        NPC.velocity = retreatVelocity * 0.85f;
                        retreatVelocity *= 0.85f;
                        if (retreatVelocity.Length() < 0.1f) retreatVelocity = Vector2.Zero;
                    }
                    else
                    {
                        NPC.velocity *= 0.94f;
                    }

                    if (stateTimer >= CAST_DURATION)
                    {
                        CurrentState = AIState.PostCast;
                        stateTimer = 0;
                        retreatVelocity = Vector2.Zero;
                        NPC.netUpdate = true;
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

            if (flashTimer > 0f)
            {
                flashTimer -= 0.1f;
                if (flashTimer < 0f) flashTimer = 0f;
            }
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte)CurrentState);
            writer.Write(stateTimer);
            writer.Write(attackCooldown);
            writer.Write(flashTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            CurrentState = (AIState)reader.ReadByte();
            stateTimer = reader.ReadInt32();
            attackCooldown = reader.ReadInt32();
            flashTimer = reader.ReadSingle();
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
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            var biomePlayer = spawnInfo.Player.GetModPlayer<BiomePlayer>();

            if (biomePlayer.villageBiome && spawnInfo.Player.ZoneUnderworldHeight)
                return 0.6f;

            return 0f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (flashTimer > 0f)
            {
                float intensity = flashTimer * 0.6f;
                Color flashColor = new Color(255, 60, 40) * intensity;

                BlendState old = Main.graphics.GraphicsDevice.BlendState;
                Main.graphics.GraphicsDevice.BlendState = BlendState.Additive;

                Texture2D glow = ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_1").Value;
                Vector2 origin = glow.Size() / 2f;

                Main.EntitySpriteDraw(
                    glow,
                    NPC.Center - Main.screenPosition,
                    null,
                    flashColor * 0.8f,
                    0f,
                    origin,
                    NPC.scale * 1.2f * intensity,
                    SpriteEffects.None,
                    0
                );

                Main.EntitySpriteDraw(
                    glow,
                    NPC.Center - Main.screenPosition,
                    null,
                    flashColor * 0.3f,
                    0f,
                    origin,
                    NPC.scale * 1.8f * intensity,
                    SpriteEffects.None,
                    0
                );

                Main.graphics.GraphicsDevice.BlendState = old;
            }

            return true;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Mod.Find<ModGore>("BigBobGore1") != null)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity * 0.4f, Mod.Find<ModGore>("BigBobGore1").Type);
                if (Mod.Find<ModGore>("BigBobGore2") != null)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity * 0.4f, Mod.Find<ModGore>("BigBobGore2").Type);
                if (Mod.Find<ModGore>("BigBobGore3") != null)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity * 0.4f, Mod.Find<ModGore>("BigBobGore3").Type);
                if (Mod.Find<ModGore>("BigBobGore4") != null)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity * 0.4f, Mod.Find<ModGore>("BigBobGore4").Type);

                for (int i = 0; i < 24; i++)
                {
                    Dust d = Dust.NewDustPerfect(NPC.Center, DustID.Blood, Main.rand.NextVector2Circular(5f, 5f), 50);
                    d.noGravity = true;
                    d.scale = Main.rand.NextFloat(0.9f, 1.6f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(
                ModContent.ItemType<ValhallaMod.Items.Placeable.Blocks.SinstoneMagma>(),
                1, 2, 5
            ));
        }

        public override void OnKill()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI);
            }
        }
    }
}