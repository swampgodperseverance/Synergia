using System;
using Microsoft.Xna.Framework;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using System.IO;
using Synergia.Content.Projectiles.Hostile;
using Terraria.ModLoader;
using static Synergia.Common.SUtils.LocUtil;    

namespace Synergia.Content.NPCs
{
    public class ElderSnowman : ModNPC
    {
        public override string LocalizationCategory => Category(CategoryName.NPC);

        public int cloudProjectileIndex = -1;
        public int attackCooldown = 0;
        public int attackTimer = 0;
        public bool isAttacking = false;
        public int attackFrameCounter = 0;
        public int despawnTimer = 0;
        public bool isDespawning = false;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 8;
            NPCID.Sets.BelongsToInvasionFrostLegion[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 30;
            NPC.height = 64;
            NPC.damage = 30;
            NPC.defense = 20;
            NPC.lifeMax = 1200;
            NPC.HitSound = SoundID.NPCHit11;
            NPC.DeathSound = SoundID.NPCDeath15;
            NPC.value = 1000f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(isAttacking);
            writer.Write(attackTimer);
            writer.Write(attackFrameCounter);
            writer.Write(cloudProjectileIndex);
            writer.Write(despawnTimer);
            writer.Write(isDespawning);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            isAttacking = reader.ReadBoolean();
            attackTimer = reader.ReadInt32();
            attackFrameCounter = reader.ReadInt32();
            cloudProjectileIndex = reader.ReadInt32();
            despawnTimer = reader.ReadInt32();
            isDespawning = reader.ReadBoolean();
        }

        public override void OnSpawn(IEntitySource source)
        {
            CreateCloud();
        }

        public override void AI()
        {
            Player target = Main.player[NPC.target];

            if (!target.active || target.dead)
            {
                NPC.TargetClosest(faceTarget: false);
                target = Main.player[NPC.target];

                if (!target.active || target.dead)
                {
                    if (!isDespawning)
                    {
                        isDespawning = true;
                        despawnTimer = 0;
                        NPC.netUpdate = true;
                    }

                    if (isDespawning)
                    {
                        DespawnAnimation();
                    }
                    return;
                }
            }
            else if (isDespawning)
            {
                isDespawning = false;
                despawnTimer = 0;
                NPC.alpha = 0;
            }

            despawnTimer = 0; 

            UpdateCloud();

            if (!isAttacking && !isDespawning)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter >= 6)
                {
                    NPC.frameCounter = 0;
                    int currentFrame = NPC.frame.Y / NPC.height;
                    int nextFrame = (currentFrame + 1) % 3;
                    NPC.frame.Y = nextFrame * NPC.height;
                }

                FlyAbovePlayerSmooth(target);

                if (Math.Abs(NPC.velocity.X) > 0.1f)
                {
                    NPC.spriteDirection = NPC.velocity.X > 0 ? 1 : -1;
                }

                if (attackCooldown <= 0 && Main.rand.NextBool(300))
                {
                    StartAttack();
                }
                else if (attackCooldown > 0)
                {
                    attackCooldown--;
                }
            }
            else if (!isDespawning)
            {
                PerformAttack(target);
            }
        }

        private void DespawnAnimation()
        {
            despawnTimer++;
            
            float progress = despawnTimer / 60f;
            NPC.alpha = (int)(255 * progress);
            
            if (despawnTimer <= 60)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient && despawnTimer % 5 == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 vel = Main.rand.NextVector2Circular(7f, 7f);
                        Dust d = Dust.NewDustPerfect(NPC.Center + new Vector2(Main.rand.Next(-20, 21), Main.rand.Next(-40, 41)), 
                            DustID.IceTorch, vel, Scale: Main.rand.NextFloat(1.2f, 2f));
                        d.noGravity = true;
                        d.velocity.Y -= 0.5f;
                    }
                    
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 vel = Main.rand.NextVector2Circular(5f, 5f);
                        Dust d = Dust.NewDustPerfect(NPC.Center + new Vector2(Main.rand.Next(-20, 21), Main.rand.Next(-40, 41)), 
                            DustID.Snow, vel, Scale: Main.rand.NextFloat(1f, 1.8f));
                        d.noGravity = false;
                    }
                }
                
                NPC.velocity.Y -= 0.2f;
                NPC.velocity *= 0.9f;
                
                if (cloudProjectileIndex >= 0 && cloudProjectileIndex < Main.maxProjectiles)
                {
                    Projectile cloud = Main.projectile[cloudProjectileIndex];
                    if (cloud.active)
                    {
                        cloud.alpha = NPC.alpha;
                    }
                }
            }
            else
            {
                if (cloudProjectileIndex >= 0 && cloudProjectileIndex < Main.maxProjectiles)
                {
                    Projectile cloud = Main.projectile[cloudProjectileIndex];
                    if (cloud.active)
                    {
                        cloud.Kill();
                    }
                }
                NPC.active = false;
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI);
            }
        }

        public void StartDespawnEffect()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                SoundEngine.PlaySound(SoundID.Item27 with { Volume = 1.2f, Pitch = 0.1f }, NPC.Center);

                for (int i = 0; i < 40; i++)
                {
                    Vector2 vel = Main.rand.NextVector2Circular(7f, 7f);
                    Dust d = Dust.NewDustPerfect(NPC.Center, DustID.IceTorch, vel, Scale: Main.rand.NextFloat(1.6f, 2.4f));
                    d.noGravity = true;
                }

                for (int i = 0; i < 25; i++)
                {
                    Vector2 vel = Main.rand.NextVector2Circular(5f, 5f);
                    Dust d = Dust.NewDustPerfect(NPC.Center, DustID.Snow, vel, Scale: Main.rand.NextFloat(1.2f, 2f));
                    d.noGravity = false;
                }
            }

            NPC.netUpdate = true;
        }

        public void FlyAbovePlayerSmooth(Player target)
        {
            float offsetX = Main.rand.NextBool(3) ? -200f : 200f;
            offsetX += Main.rand.Next(-50, 51);
            float offsetY = -150f - Main.rand.Next(0, 101);
            Vector2 desiredPos = target.Center + new Vector2(offsetX, offsetY);
            Vector2 toDesired = desiredPos - NPC.Center;
            float maxSpeed = 4.5f;
            float inertia = 18f;

            if (toDesired.Length() > 20f)
            {
                toDesired.Normalize();
                NPC.velocity += toDesired * (maxSpeed / inertia);
                if (NPC.velocity.Length() > maxSpeed)
                    NPC.velocity = Vector2.Normalize(NPC.velocity) * maxSpeed;
            }
            else
            {
                NPC.velocity *= 0.96f;
                float bob = (float)Math.Sin(Main.GameUpdateCount * 0.06f) * 0.08f;
                NPC.velocity.X += bob;
            }
        }

        public void StartAttack()
        {
            isAttacking = true;
            attackTimer = 0;
            attackFrameCounter = 0;
            NPC.velocity = Vector2.Zero;
            NPC.frame.Y = 3 * NPC.height;
            NPC.netUpdate = true;
        }

        public void PerformAttack(Player target)
        {
            attackTimer++;

            if (attackFrameCounter < 5 && attackTimer % 5 == 0)
            {
                attackFrameCounter++;
                NPC.frame.Y = (3 + attackFrameCounter) * NPC.height;
                NPC.netUpdate = true;
                if (attackFrameCounter == 5)
                {
                    NPC.velocity = Vector2.Zero;
                }
            }

            if (attackFrameCounter == 5)
            {
                NPC.frame.Y = 7 * NPC.height;

                if (attackTimer < 300)
                {
                    if (attackTimer % 120 == 0)
                        LaunchShockwave(target);

                    if (Main.expertMode && attackTimer % 60 == 0)
                        BuffNearbyNPCs();

                    if (Main.rand.NextBool(4))
                    {
                        float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                        Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Main.rand.NextFloat(30f, 50f);
                        Vector2 dustVel = new Vector2(-offset.Y, offset.X) * 0.07f;

                        Dust dust = Dust.NewDustPerfect(NPC.Center + offset, DustID.IceTorch, dustVel, Scale: Main.rand.NextFloat(1.2f, 1.8f));
                        dust.noGravity = true;
                    }
                }
                else
                {
                    EndAttack();
                }
            }
        }

        public void EndAttack()
        {
            isAttacking = false;
            attackCooldown = 300;
            attackTimer = 0;
            attackFrameCounter = 0;
            NPC.frame.Y = 0;
            NPC.netUpdate = true;
        }

        public void LaunchShockwave(Player target)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;

            Vector2 direction = target.Center - NPC.Center;
            if (direction == Vector2.Zero) direction = Vector2.UnitY;
            direction.Normalize();

            Projectile.NewProjectile(
                NPC.GetSource_FromAI(),
                NPC.Center,
                direction * 8f,
                ModContent.ProjectileType<Shockwave>(),
                NPC.damage / 2,
                2f,
                Main.myPlayer
            );

            SoundEngine.PlaySound(SoundID.Item67 with { Volume = 0.8f }, NPC.Center);
        }

        public void BuffNearbyNPCs()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.type != Type && npc.Distance(NPC.Center) < 500f)
                {
                    npc.defense += 10;
                    npc.life = Math.Min(npc.life + 20, npc.lifeMax);

                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, i);
                }
            }
        }

        public void CreateCloud()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;

            cloudProjectileIndex = Projectile.NewProjectile(
                NPC.GetSource_FromAI(),
                NPC.Center + new Vector2(0, NPC.height / 2 + 7),
                Vector2.Zero,
                ModContent.ProjectileType<EldestSnowmanCloud>(),
                0,
                0f,
                Main.myPlayer,
                NPC.whoAmI,
                0
            );
        }

        public void UpdateCloud()
        {
            if (cloudProjectileIndex < 0 || cloudProjectileIndex >= Main.maxProjectiles)
            {
                CreateCloud();
                return;
            }

            Projectile cloud = Main.projectile[cloudProjectileIndex];
            if (cloud.active && cloud.type == ModContent.ProjectileType<EldestSnowmanCloud>())
            {
                cloud.Center = NPC.Center + new Vector2(0, NPC.height / 2 + 7);
                cloud.timeLeft = 60;
            }
            else
            {
                cloudProjectileIndex = -1;
                CreateCloud();
            }
        }

        public override void OnKill()
        {
            if (cloudProjectileIndex >= 0 && cloudProjectileIndex < Main.maxProjectiles && Main.projectile[cloudProjectileIndex].active)
            {
                Main.projectile[cloudProjectileIndex].Kill();
            }
        }

      
        
    }

    public class EldestSnowmanCloud : ModProjectile
    {
        public override string Texture => "Synergia/Content/NPCs/EldestSnowmanCloud";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 24;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 60;
            Projectile.alpha = 50;
            Projectile.netImportant = true;
        }

        public override void AI()
        {
            int ownerIndex = (int)Projectile.ai[0];
            if (ownerIndex < 0 || ownerIndex >= Main.maxNPCs)
            {
                Projectile.Kill();
                return;
            }

            NPC owner = Main.npc[ownerIndex];
            if (!owner.active || owner.type != ModContent.NPCType<ElderSnowman>())
            {
                Projectile.Kill();
                return;
            }

            ElderSnowman modNPC = owner.ModNPC as ElderSnowman;
            if (modNPC != null && modNPC.isDespawning)
            {
                Projectile.alpha = owner.alpha;
            }

            Projectile.Center = owner.Center + new Vector2(0, owner.height / 2 + 7);
            Projectile.timeLeft = 60;

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 8)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 3;
            }

            Projectile.position.X += (float)Math.Sin(Main.GameUpdateCount * 0.05f) * 0.5f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle sourceRect = texture.Frame(1, Main.projFrames[Type], 0, Projectile.frame);
            Vector2 origin = sourceRect.Size() / 2f;
            Color drawColor = Color.White * (1f - Projectile.alpha / 255f);

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                sourceRect,
                drawColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );
            return false;
        }
    }
}