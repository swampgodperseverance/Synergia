using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Friendly
{
    public class CleavageSpear : ModProjectile 
    {
        private float glowRotation;
        private int comboCounter;
        private bool empowered;
        
        public override void SetStaticDefaults() 
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10; 
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults() 
        {
            Projectile.width = Projectile.height = 22;
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
        }

        public override void AI() 
        {
            Player player = Main.player[Projectile.owner];
            Vector2 center = player.RotatedRelativePoint(player.MountedCenter);
            
            Projectile.direction = player.direction;
            player.heldProj = Projectile.whoAmI;
            Projectile.Center = center;
            
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
                Projectile.localAI[0] = Math.Max(0, Projectile.localAI[0] - 1f);

                float animationProgress = player.itemAnimation / (float)player.itemAnimationMax;
                float reverseProgress = 1f - animationProgress;
                float rotation = Projectile.velocity.ToRotation();
                float length = Projectile.velocity.Length();
                

                Vector2 spinningOffset = new Vector2(1.5f, 0f).RotatedBy((float)Math.PI + reverseProgress * ((float)Math.PI * 3f)) * new Vector2(length, Projectile.ai[0]);
                Projectile.position += spinningOffset.RotatedBy(rotation) + new Vector2(length + 84f, 0f).RotatedBy(rotation);
                
                Vector2 targetPos = center + spinningOffset.RotatedBy(rotation) + new Vector2(length + 124f, 0f).RotatedBy(rotation);
                Projectile.rotation = center.AngleTo(targetPos) + (float)Math.PI / 4f * player.direction;
                
                if (Projectile.spriteDirection == -1)
                    Projectile.rotation += (float)Math.PI;

 
                if (Main.netMode != NetmodeID.Server) 
                {
                    Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.UnitY);

                    int dustType = empowered 
                        ? DustID.LavaMoss 
                        : (Main.rand.NextBool(3) ? DustID.Torch : DustID.Lava);

                    for (int i = 0; i < 3; i++) {
                        Dust dust = Dust.NewDustPerfect(
                            Projectile.Center + direction.RotatedBy(
                                reverseProgress * MathHelper.TwoPi * 2f + i / 3f * MathHelper.TwoPi * 8f),
                            dustType,
                            null,
                            100,
                            default,
                            Main.rand.NextFloat(0.8f, 1.4f));
        
                        dust.velocity = center.DirectionTo(dust.position) * 2f + direction * 3f;
                        dust.noGravity = true;
                        dust.noLight = false;

                        if (empowered) {
                            dust.velocity *= 1.5f;
                            dust.scale *= 1.3f;
                        }
                    }
                    
                    // attempt to create smth like overheating
                    if (comboCounter >= 3 && Main.rand.NextBool(5))
                    {
                        Dust.NewDustPerfect(
                            Projectile.Center + Main.rand.NextVector2Circular(10, 10),
                            DustID.SolarFlare,
                            Projectile.velocity * 0.5f + Main.rand.NextVector2Circular(2, 2),
                            150,
                            new Color(255, 200, 50),
                            1.2f);
                    }
                }
                
                Projectile.netUpdate = true;
            }
            
            if (player.itemAnimation == 2) 
            {
                Projectile.Kill();
                player.reuseDelay = 2;
                
                if (player.itemAnimationMax > 20)
                    comboCounter = 0;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
        {
            float rotation = Projectile.rotation - (float)Math.PI / 4f * Math.Sign(Projectile.velocity.X) + 
                           ((Projectile.spriteDirection == -1) ? (float)Math.PI : 0f);
                           
            float collisionPoint = 0f;
            float length = empowered ? -120f : -95f; 
            
            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(), 
                targetHitbox.Size(), 
                Projectile.Center, 
                Projectile.Center + rotation.ToRotationVector2() * length, 
                empowered ? 30f : 23f * Projectile.scale,
                ref collisionPoint);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) 
        {
            Projectile.direction = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : -1;
            
            comboCounter++;
            if (comboCounter >= 3)
            {
                empowered = true;
                SoundEngine.PlaySound(SoundID.Item45 with { Pitch = 0.5f }, Projectile.position);
            }
            
            if (empowered)
            {
                target.AddBuff(BuffID.OnFire3, 300); 
                
                if (hit.Crit)
                {
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        target.Center,
                        Vector2.Zero,
                        ProjectileID.DD2ExplosiveTrapT3Explosion,
                        Projectile.damage / 2,
                        5f,
                        Projectile.owner);
                }
            }
            else
            {
                target.AddBuff(BuffID.OnFire, 180);
            }
            
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustPerfect(
                    target.Center + Main.rand.NextVector2Circular(target.width, target.height) * 0.5f,
                    DustID.Torch,
                    Main.rand.NextVector2Circular(3, 3),
                    100,
                    default,
                    1.5f);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) 
        {
            if (info.PvP)
            {
                Projectile.direction = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : -1;
                target.AddBuff(empowered ? BuffID.OnFire3 : BuffID.OnFire, 180);
            }
        }

        public override bool PreDraw(ref Color lightColor) 
        {
            Player player = Main.player[Projectile.owner];
            SpriteBatch spriteBatch = Main.spriteBatch;

            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = new Vector2(
                (Projectile.spriteDirection == 1) ? (texture.Width + 8f) : (-8f),
                (player.gravDir == 1f) ? (-8f) : (texture.Height + 8f));

            Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);
            float rotation = Projectile.rotation;

            SpriteEffects effects = player.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            if (player.gravDir == -1f) 
            {
                effects |= SpriteEffects.FlipVertically;
                rotation += (float)Math.PI / 2f * Projectile.spriteDirection;
            }

            Texture2D trailTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_1").Value; //asset from consolaria
            Vector2 trailOrigin = trailTexture.Size() / 2f;
            glowRotation += 0.1f;

            for (int k = 0; k < Projectile.oldPos.Length - 1; k++) 
            {
                float progress = 1f - k / (float)Projectile.oldPos.Length;
                Vector2 trailPos = Projectile.oldPos[k] + Projectile.Size / 2f - Main.screenPosition;
                float trailRot = (float)Math.Atan2(Projectile.oldPos[k].Y - Projectile.oldPos[k + 1].Y, 
                                              Projectile.oldPos[k].X - Projectile.oldPos[k + 1].X);

                Color trailColor = empowered ? 
                    new Color(255, 150 + k * 10, 0, (int)(100 * progress)) : 
                    new Color(255, 100 + k * 20, 0, (int)(100 * progress));

                spriteBatch.Draw(
                    trailTexture,
                    trailPos,
                    null,
                    trailColor,
                    trailRot,
                    trailOrigin,
                    Projectile.scale * progress,
                    effects,
                    0f);
            }

            spriteBatch.Draw(
                texture,
                position,
                null,
                lightColor,
                rotation,
                origin,
                Projectile.scale,
                effects,
                0f);

            if (empowered)
            {
                Color glowColor = new Color(255, 200, 50, 100);
                spriteBatch.Draw(
                    texture,
                    position,
                    null,
                    glowColor,
                    rotation,
                    origin,
                    Projectile.scale * 1.1f,
                    effects,
                    0f);
            }

            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];
            if (player.HasBuff(ModContent.BuffType<Buffs.Hellborn>()))
            {
                modifiers.SourceDamage *= 1.13f;
            }
        }

       
    }
}