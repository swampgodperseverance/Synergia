using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Friendly;
using Synergia.Content.Projectiles.Reworks.Reworks2;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class JadeSpearRework : ModProjectile
    {
        private float glowRotation;
        private const float maxReach = 110f;
        private const float minReach = 40f;
        private int soulTimer = 0;
        private bool shotShard = false;
        
        private Color jadeColor = new Color(80, 255, 120, 255);
        private Color jadeGlow = new Color(120, 255, 160, 180);
        private Color jadeLight = new Color(180, 255, 200, 150);

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            int size = 22;
            Projectile.Size = new Vector2(size, size);
            Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hide = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.ignoreWater = true;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
            Projectile.alpha = 255;
            Projectile.netImportant = true;
            Projectile.scale = 1.05f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 origin = player.RotatedRelativePoint(player.MountedCenter);
            Projectile.direction = player.direction;
            player.heldProj = Projectile.whoAmI;
            Projectile.Center = origin;

            if (Projectile.ai[0] == 1)
            {
                SoundStyle spearSound = new SoundStyle("Synergia/Assets/Sounds/SpearSound")
                {
                    Volume = 0.5f,
                    PitchVariance = 0.2f
                };
                SoundEngine.PlaySound(spearSound, Projectile.Center);
            }

            if (player.dead)
            {
                Projectile.Kill();
                return;
            }

            Projectile.ai[0]++;

            if (!player.frozen)
            {
                Projectile.spriteDirection = Projectile.direction = player.direction;
                Projectile.alpha = Math.Max(0, Projectile.alpha - 127);
                if (Projectile.localAI[0] > 0f)
                    Projectile.localAI[0] -= 1f;

                float animProgress = player.itemAnimation / (float)player.itemAnimationMax;
                float reach = MathHelper.Lerp(minReach, maxReach, (float)Math.Sin(animProgress * Math.PI));
                float velRot = Projectile.velocity.ToRotation();
                float velLen = Projectile.velocity.Length();

                Vector2 spinningpoint = new Vector2(1.5f, 0f)
                    .RotatedBy(Math.PI + (1f - animProgress) * (Math.PI * 3f)) 
                    * new Vector2(velLen, Projectile.ai[0]);

                Projectile.position += spinningpoint.RotatedBy(velRot)
                    + new Vector2(velLen + reach, 0f).RotatedBy(velRot);

                Vector2 target = origin + spinningpoint.RotatedBy(velRot)
                    + new Vector2(velLen + reach, 0f).RotatedBy(velRot);

                Projectile.rotation = origin.AngleTo(target) + (float)Math.PI / 4f * player.direction;

                if (Projectile.spriteDirection == -1)
                    Projectile.rotation += (float)Math.PI;

     
                if (!shotShard && animProgress > 0.55f)
                {
                    shotShard = true;

                    Vector2 spearDirection = (target - origin).SafeNormalize(Vector2.UnitX);
                    float spread = Main.rand.NextFloat(-0.08f, 0.08f);
                    Vector2 shootDirection = (spearDirection * 14f).RotatedBy(spread);
                    Vector2 spawnPosition = target + spearDirection * 12f;

                    int shard = Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        spawnPosition,
                        shootDirection,
                        ModContent.ProjectileType<JadeShardBig>(),
                        Projectile.damage,
                        1f,
                        Projectile.owner
                    );

                    SoundEngine.PlaySound(SoundID.Item20.WithPitchOffset(-0.2f), target);
                    
                    for (int i = 0; i < 6; i++)
                    {
                        Dust dust = Dust.NewDustDirect(
                            spawnPosition, 8, 8,
                            DustID.GemEmerald,
                            0f, 0f, 150
                        );
                        dust.velocity = shootDirection.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.3f, 1f);
                        dust.scale = Main.rand.NextFloat(1f, 1.6f);
                        dust.noGravity = true;
                        dust.color = jadeColor;
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        Dust flash = Dust.NewDustPerfect(
                            spawnPosition,
                            DustID.GemEmerald,
                            Vector2.Zero,
                            200,
                            jadeLight,
                            Main.rand.NextFloat(1.5f, 2f)
                        );
                        flash.noGravity = true;
                        flash.fadeIn = 1.5f;
                    }
                }


                if (Main.netMode != NetmodeID.Server)
                {
                    Vector2 normVel = Projectile.velocity.SafeNormalize(Vector2.UnitY);
  
                    for (int i = 0; i < 2; i++)
                    {
                        Dust jadeDust = Dust.NewDustDirect(
                            Projectile.Center, 16, 16,
                            DustID.GemEmerald,
                            0f, 0f, 130
                        );

                        jadeDust.velocity = origin.DirectionTo(jadeDust.position) * 1.8f;
                        jadeDust.position = Projectile.Center + normVel.RotatedBy(
                            (1f - animProgress) * ((float)Math.PI * 2f) * 2f
                            + i * ((float)Math.PI)
                        ) * 10f;

                        jadeDust.scale = Main.rand.NextFloat(0.9f, 1.5f);
                        jadeDust.velocity += normVel * 2.5f;
                        jadeDust.noGravity = true;
                        jadeDust.color = Color.Lerp(jadeColor, Color.White, 0.3f);
                        
            
                        if (Main.rand.NextBool(4) && animProgress > 0.3f)
                        {
                            Dust spark = Dust.NewDustPerfect(
                                jadeDust.position + Main.rand.NextVector2Circular(5, 5),
                                DustID.GreenFairy,
                                jadeDust.velocity * 1.3f,
                                100,
                                jadeGlow,
                                Main.rand.NextFloat(0.7f, 1.2f)
                            );
                            spark.noGravity = true;
                        }
                    }
                    
           
                    if (animProgress > 0.4f && animProgress < 0.9f)
                    {
                        if (Main.rand.NextBool(3))
                        {
                            Vector2 tipPosition = target + Main.rand.NextVector2Circular(8, 8);
                            Dust tipDust = Dust.NewDustPerfect(
                                tipPosition,
                                DustID.GemEmerald,
                                Main.rand.NextVector2Circular(1, 1),
                                180,
                                jadeLight,
                                Main.rand.NextFloat(0.8f, 1.3f)
                            );
                            tipDust.noGravity = true;
                            tipDust.fadeIn = 1.2f;
                        }
                    }
                }

                Projectile.netUpdate = true;
            }

            if (player.itemAnimation == 2)
            {
                Projectile.Kill();
                player.reuseDelay = 2;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 origin = player.RotatedRelativePoint(player.MountedCenter);

            float animProgress = player.itemAnimation / (float)player.itemAnimationMax;
            float reach = MathHelper.Lerp(minReach, maxReach, (float)Math.Sin(animProgress * Math.PI));
            float velRot = Projectile.velocity.ToRotation();
            float velLen = Projectile.velocity.Length();

            Vector2 spinningpoint = new Vector2(1.5f, 0f)
                .RotatedBy(Math.PI + (1f - animProgress) * (Math.PI * 3f)) 
                * new Vector2(velLen, Projectile.ai[0]);

            Vector2 spearTip = origin + spinningpoint.RotatedBy(velRot)
                + new Vector2(velLen + reach, 0f).RotatedBy(velRot);

            float collisionPoint = 0f;

            if (Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(), targetHitbox.Size(),
                origin, spearTip,
                24f * Projectile.scale,
                ref collisionPoint))
                return true;

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.direction = 
                (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : -1;

            for (int i = 0; i < 10; i++)
            {
                Vector2 dustPos = target.Center + Main.rand.NextVector2Circular(target.width / 2, target.height / 2);
        
                Dust hitDust = Dust.NewDustPerfect(
                    dustPos,
                    DustID.GemEmerald,
                    Main.rand.NextVector2Circular(4, 4),
                    180,
                    Color.Lerp(jadeColor, Color.White, 0.2f),
                    Main.rand.NextFloat(1f, 1.8f)
                );
                hitDust.noGravity = true;
   
                if (i < 4)
                {
                    Dust energy = Dust.NewDustPerfect(
                        dustPos,
                        DustID.GreenFairy,
                        Main.rand.NextVector2Circular(2, 2),
                        150,
                        jadeGlow,
                        Main.rand.NextFloat(1.2f, 2f)
                    );
                    energy.noGravity = true;
                    energy.fadeIn = 1.5f;
                }
            }

            SoundEngine.PlaySound(SoundID.NPCHit5 with { Pitch = 0.3f }, target.Center);

            soulTimer++;
            if (soulTimer >= 10)
            {
                soulTimer = 0;
                if (Main.myPlayer == Projectile.owner)
                {
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        target.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<ValhalliteSoul>(),
                        Projectile.damage / 2,
                        1f,
                        Projectile.owner
                    );
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                Projectile.direction =
                    (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : -1;
      
                for (int i = 0; i < 5; i++)
                {
                    Dust pvpDust = Dust.NewDustDirect(
                        target.Center, 10, 10,
                        DustID.GemEmerald,
                        0f, 0f, 100
                    );
                    pvpDust.velocity = Main.rand.NextVector2Circular(3, 3);
                    pvpDust.scale = Main.rand.NextFloat(0.8f, 1.2f);
                    pvpDust.noGravity = true;
                    pvpDust.color = jadeColor;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            SpriteBatch spriteBatch = Main.spriteBatch;
            

            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            Vector2 drawOrigin = new Vector2(
                (Projectile.spriteDirection == 1) ? (texture.Width + 8f) : (-8f),
                (player.gravDir == 1f) ? (-8f) : (texture.Height + 8f)
            );

            Vector2 position = Projectile.position 
                + new Vector2(Projectile.width, Projectile.height) / 2f 
                + Vector2.UnitY * Projectile.gfxOffY
                - Main.screenPosition;

            float rotation = Projectile.rotation;

            var spriteEffects = 
                (player.direction > 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            if (player.gravDir == -1f)
            {
                spriteEffects = SpriteEffects.FlipVertically;
                rotation += (float)Math.PI / 2f * Projectile.spriteDirection;
            }

            Texture2D trailTexture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_1").Value;
    
            for (int k = 0; k < Projectile.oldPos.Length - 1; k++)
            {
                if (Projectile.oldPos[k] == Vector2.Zero)
                    continue;

                Vector2 drawPos = new Vector2(Projectile.width, Projectile.height) / 2f
                    + Vector2.UnitY * Projectile.gfxOffY
                    - Main.screenPosition;

       
                float trailRotation = (float)Math.Atan2(
                    Projectile.oldPos[k].Y - Projectile.oldPos[k + 1].Y,
                    Projectile.oldPos[k].X - Projectile.oldPos[k + 1].X
                );

                float opacity = (1f - k / (float)Projectile.oldPos.Length) * 0.7f;
                Color trailColor = jadeGlow * opacity;
                trailColor.A = (byte)(180 * opacity);

                float scale = Projectile.scale * (1f - k / (float)Projectile.oldPos.Length * 0.6f);

                spriteBatch.Draw(
                    trailTexture,
                    Projectile.oldPos[k] + drawPos,
                    null,
                    trailColor,
                    trailRotation,
                    new Vector2(trailTexture.Width / 2, trailTexture.Height / 2),
                    scale,
                    spriteEffects,
                    0f
                );

                spriteBatch.Draw(
                    trailTexture,
                    (Projectile.oldPos[k] + Projectile.oldPos[k + 1]) * 0.5f + drawPos,
                    null,
                    trailColor * 0.6f,
                    trailRotation + 0.5f,
                    new Vector2(trailTexture.Width / 2, trailTexture.Height / 2),
                    scale * 0.8f,
                    spriteEffects,
                    0f
                );
            }

            Texture2D glowTexture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Extra_89").Value;
            if (glowTexture != null)
            {
                glowRotation += 0.12f;
                
                Color glowColor = jadeLight;
                glowColor.A = 80;
                
                float pulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3f) * 0.1f + 0.9f;
          
                spriteBatch.Draw(
                    glowTexture,
                    position,
                    null,
                    glowColor,
                    glowRotation,
                    new Vector2(glowTexture.Width / 2, glowTexture.Height / 2),
                    Projectile.scale * 0.7f * pulse,
                    spriteEffects,
                    0f
                );

                spriteBatch.Draw(
                    glowTexture,
                    position,
                    null,
                    jadeGlow * 0.5f,
                    -glowRotation * 0.8f,
                    new Vector2(glowTexture.Width / 2, glowTexture.Height / 2),
                    Projectile.scale * 0.4f,
                    spriteEffects,
                    0f
                );
            }

            Color finalColor = Color.Lerp(lightColor, jadeColor, 0.6f);
            finalColor = Color.Lerp(finalColor, Color.White, 0.2f);
            
            spriteBatch.Draw(texture, position, null, finalColor,
                rotation, drawOrigin, Projectile.scale, spriteEffects, 0f);

            spriteBatch.Draw(texture, position, null, 
                jadeGlow * 0.3f,
                rotation, drawOrigin, Projectile.scale * 1.05f, spriteEffects, 0f);

            return false;
        }

        public override void PostDraw(Color lightColor)
        {

            if (Main.rand.NextBool(8))
            {
                Player player = Main.player[Projectile.owner];
                SpriteBatch spriteBatch = Main.spriteBatch;
                
                Vector2 position = Projectile.position 
                    + new Vector2(Projectile.width, Projectile.height) / 2f 
                    + Vector2.UnitY * Projectile.gfxOffY
                    - Main.screenPosition;

                float rotation = Projectile.rotation;
                
                var spriteEffects = 
                    (player.direction > 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                if (player.gravDir == -1f)
                {
                    spriteEffects = SpriteEffects.FlipVertically;
                    rotation += (float)Math.PI / 2f * Projectile.spriteDirection;
                }

                Texture2D energyTexture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Extra_89").Value;
                
                if (energyTexture != null)
                {
                    Vector2 energyPos = position + Main.rand.NextVector2Circular(25, 25);
                    Color energyColor = Color.Lerp(jadeGlow, Color.White, Main.rand.NextFloat(0.3f));
                    energyColor.A = 100;
                    
                    spriteBatch.Draw(
                        energyTexture,
                        energyPos,
                        null,
                        energyColor,
                        Main.GlobalTimeWrappedHourly * 2f,
                        new Vector2(energyTexture.Width / 2, energyTexture.Height / 2),
                        Main.rand.NextFloat(0.05f, 0.15f),
                        SpriteEffects.None,
                        0f
                    );
                }
            }
        }
    }
}