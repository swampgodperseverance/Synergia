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

        private const float minReach = 40f;
        private const float maxReach = 140f;

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
            Vector2 origin = player.RotatedRelativePoint(player.MountedCenter);
            Projectile.direction = player.direction;
            player.heldProj = Projectile.whoAmI;
            Projectile.Center = origin;

            if (Projectile.ai[0] == 1)
            {
                SoundEngine.PlaySound(SoundID.Item45 with { Volume = 0.9f, PitchVariance = 0.15f }, Projectile.Center);
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
                Projectile.localAI[0] = Math.Max(0, Projectile.localAI[0] - 1f);

                float animProgress = player.itemAnimation / (float)player.itemAnimationMax;
                float reverseProgress = 1f - animProgress;
                float reach = MathHelper.Lerp(minReach, maxReach, (float)Math.Sin(animProgress * Math.PI));

                float velRot = Projectile.velocity.ToRotation();
                float velLen = Projectile.velocity.Length();

                Vector2 spinningpoint = new Vector2(1.5f, 0f)
                    .RotatedBy(Math.PI + reverseProgress * (Math.PI * 3f))
                    * new Vector2(velLen, Projectile.ai[0]);

                Projectile.position += spinningpoint.RotatedBy(velRot)
                                     + new Vector2(velLen + reach, 0f).RotatedBy(velRot);

                Vector2 target = origin + spinningpoint.RotatedBy(velRot)
                               + new Vector2(velLen + reach + 40f, 0f).RotatedBy(velRot);

                Projectile.rotation = origin.AngleTo(target) + (float)Math.PI / 4f * player.direction;
                if (Projectile.spriteDirection == -1)
                    Projectile.rotation += (float)Math.PI;

                if (Main.netMode != NetmodeID.Server)
                {
                    Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.UnitY);

                    // Улучшенные частицы как во втором копье
                    int dustType = empowered 
                        ? DustID.LavaMoss 
                        : (Main.rand.NextBool(3) ? DustID.Torch : DustID.Lava);

                    for (int i = 0; i < 3; i++)
                    {
                        Dust dust = Dust.NewDustPerfect(
                            Projectile.Center + direction.RotatedBy(
                                reverseProgress * MathHelper.TwoPi * 2f + i / 3f * MathHelper.TwoPi * 8f),
                            dustType,
                            null,
                            100,
                            default,
                            Main.rand.NextFloat(0.8f, 1.4f));
        
                        dust.velocity = origin.DirectionTo(dust.position) * 2f + direction * 3f;
                        dust.noGravity = true;
                        dust.noLight = false;

                        if (empowered)
                        {
                            dust.velocity *= 1.5f;
                            dust.scale *= 1.3f;
                        }
                    }
                    
                    // Эффект перегрева при комбо
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

                    Lighting.AddLight(Projectile.Center, 1.2f, 0.4f, 0.05f);
                }
            }

            if (player.itemAnimation == 2)
            {
                Projectile.Kill();
                player.reuseDelay = 2;
                
                // Сброс комбо при медленной анимации
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
            Player player = Main.player[Projectile.owner];

            // Система комбо как во втором копье
            comboCounter++;
            if (comboCounter >= 3)
            {
                empowered = true;
                SoundEngine.PlaySound(SoundID.Item45 with { Pitch = 0.5f }, Projectile.position);
            }
            
            // Улучшенные эффекты попадания
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

            for (int i = 0; i < 20; i++)
            {
                Dust d = Dust.NewDustDirect(target.position, target.width, target.height, 
                    empowered ? DustID.SolarFlare : DustID.Lava,
                    Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f), 120, default, 
                    Main.rand.NextFloat(1.2f, 1.8f));
                d.noGravity = true;
                d.velocity *= 0.7f;
                
                if (empowered)
                {
                    d.scale *= 1.3f;
                    d.velocity *= 1.2f;
                }
            }

            Lighting.AddLight(target.Center, empowered ? 1.8f : 1.4f, empowered ? 0.8f : 0.6f, empowered ? 0.2f : 0.1f);

            // Оригинальная механика с SpinCleava
            if (Main.player[Projectile.owner] == Main.LocalPlayer) 
            {
                int hitCounter = (int)(Main.GameUpdateCount % 10);
                hitCounter++;
        
                if (hitCounter >= 10)
                {
                    Vector2 spawnPos = player.Center + new Vector2(0f, -60f);
                    Vector2 initialVelocity = new Vector2(0f, -5f);

                    if (Main.myPlayer == player.whoAmI)
                    {
                        int p = Projectile.NewProjectile(
                            Projectile.GetSource_FromThis(),
                            spawnPos,
                            initialVelocity,
                            ModContent.ProjectileType<SpinCleava>(),
                            Projectile.damage * 2,
                            4f,
                            Projectile.owner
                        );

                        if (p >= 0)
                        {
                            Main.projectile[p].timeLeft = 300;
                            Main.projectile[p].netUpdate = true;
                            SoundEngine.PlaySound(SoundID.Item74 with { Volume = 1f, Pitch = -0.3f }, spawnPos);

                            for (int i = 0; i < 30; i++)
                            {
                                Dust lava = Dust.NewDustDirect(spawnPos - new Vector2(10, 10), 20, 20, 
                                    empowered ? DustID.SolarFlare : DustID.Lava,
                                    Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f), 120, default, 
                                    Main.rand.NextFloat(1.2f, 2.0f));
                                lava.noGravity = true;
                                lava.velocity *= 0.6f;
                            }
                        }
                    }
                }
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
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Vector2 drawOrigin = new Vector2(
                (Projectile.spriteDirection == 1) ? (texture.Width + 8f) : (-8f),
                (player.gravDir == 1f) ? (-8f) : (texture.Height + 8f)
            );
            Vector2 position = Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2f
                             + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
            float rotation = Projectile.rotation;
            var spriteEffects = player.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            if (player.gravDir == -1f)
            {
                spriteEffects = SpriteEffects.FlipVertically;
                rotation += (float)Math.PI / 2f * Projectile.spriteDirection;
            }

            // Улучшенный трейл как во втором копье
            Texture2D trail = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_1").Value;
            Vector2 trailOrigin = new Vector2(trail.Width / 2, trail.Height / 2);
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
                    trail,
                    trailPos,
                    null,
                    trailColor,
                    trailRot,
                    trailOrigin,
                    Projectile.scale * progress,
                    spriteEffects,
                    0f);
            }

            // Основная текстура
            spriteBatch.Draw(texture, position, null, lightColor, rotation, drawOrigin, Projectile.scale, spriteEffects, 0f);

            // Эффект свечения при усилении
            if (empowered)
            {
                Color glowColor = new Color(255, 200, 50, 100);
                spriteBatch.Draw(
                    texture,
                    position,
                    null,
                    glowColor,
                    rotation,
                    drawOrigin,
                    Projectile.scale * 1.1f,
                    spriteEffects,
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
            
            if (empowered)
            {
                modifiers.SourceDamage *= 1.2f;
            }
        }
    }
}