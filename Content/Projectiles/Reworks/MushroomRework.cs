using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks
{
    public class MushroomRework : ModProjectile
    {
        private float glowRotation;
        private const float maxReach = 180f;
        private const float minReach = 40f;
        private bool hasSpawnedMushrooms = false;


        public override void SetStaticDefaults() 
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
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
                SoundEngine.PlaySound(SoundID.Item1 with { Volume = 0.8f, PitchVariance = 0.2f }, Projectile.Center);
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
                
                float animProgress = player.itemAnimation / (float)player.itemAnimationMax;
                float reach = MathHelper.Lerp(minReach, maxReach, (float)Math.Sin(animProgress * Math.PI));
                float velRot = Projectile.velocity.ToRotation();
                float velLen = Projectile.velocity.Length();

                Vector2 spinningpoint = new Vector2(1.5f, 0f)
                    .RotatedBy(Math.PI + (1f - animProgress) * (Math.PI * 3f)) 
                    * new Vector2(velLen, Projectile.ai[0]);

                Projectile.position += spinningpoint.RotatedBy(velRot) 
                                     + new Vector2(velLen + reach, 0f).RotatedBy(velRot);

                Vector2 tipPosition = origin + spinningpoint.RotatedBy(velRot) 
                                   + new Vector2(velLen + reach + 40f, 0f).RotatedBy(velRot);

                Projectile.rotation = origin.AngleTo(tipPosition) + (float)Math.PI / 4f * player.direction;
                if (Projectile.spriteDirection == -1)
                    Projectile.rotation += (float)Math.PI;

                // Спавн грибов на кончике копья
                if (animProgress > 0.5f && !hasSpawnedMushrooms) 
                {
                    SpawnMushroomLine(origin, tipPosition);
                    hasSpawnedMushrooms = true;
                }
                else if (animProgress <= 0.5f) 
                {
                    hasSpawnedMushrooms = false;
                }

                // Грибные частицы
                if (Main.netMode != NetmodeID.Server) 
                {
                    Vector2 normVel = Projectile.velocity.SafeNormalize(Vector2.UnitY);
                    for (int i = 0; i < 2; i++) 
                    {
                        Dust dust = Dust.NewDustDirect(Projectile.Center, 14, 14, DustID.MushroomSpray, 0f, 0f, 120);
                        dust.color = new Color(200, 200, 200, 100);
                        dust.velocity = origin.DirectionTo(dust.position) * 2f;
                        dust.position = Projectile.Center + normVel.RotatedBy((1f - animProgress) * ((float)Math.PI * 2f) * 2f 
                            + i / 2f * ((float)Math.PI * 2f)) * 8f;
                        dust.scale = Main.rand.NextFloat(0.8f, 1.4f);
                        dust.velocity += normVel * 3f;
                        dust.noGravity = true;
                        dust.noLight = false;
                    }
                }
            }

            if (player.itemAnimation == 2) 
            {
                Projectile.Kill();
                player.reuseDelay = 2;
            }
        }

        private void SpawnMushroomLine(Vector2 origin, Vector2 tipPosition)
        {
            if (Main.myPlayer != Projectile.owner)
                return;

            Vector2 direction = (tipPosition - origin).SafeNormalize(Vector2.Zero);
            float distance = Vector2.Distance(origin, tipPosition);
            int mushroomCount = 9;
            float spacing = distance / (mushroomCount - 1);

            for (int i = 0; i < mushroomCount; i++)
            {
                Vector2 spawnPos = origin + direction * (spacing * i);
                Vector2 velocity = direction * 36f; // Увеличенная скорость в 3 раза (было 12f)

                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    spawnPos,
                    velocity,
                    ProjectileID.Mushroom,
                    (int)(Projectile.damage * 0.6f),
                    Projectile.knockBack * 0.4f,
                    Projectile.owner,
                    0f,
                    1f);
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
        {
            return projHitbox.Intersects(targetHitbox);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) 
        {
            Projectile.direction = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : -1;
        }

        public override bool PreDraw(ref Color lightColor) 
        {
            Player player = Main.player[Projectile.owner];
            SpriteEffects spriteEffects = player.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawOrigin = new Vector2(
                Projectile.spriteDirection == 1 ? texture.Width + 8f : -8f,
                player.gravDir == 1f ? -8f : texture.Height + 8f
            );
            Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            float rotation = Projectile.rotation;

            if (player.gravDir == -1f) 
            {
                spriteEffects = SpriteEffects.FlipVertically;
                rotation += (float)Math.PI / 2f * Projectile.spriteDirection;
            }

            // Бледно-серый след
            Texture2D glow = ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_1").Value;
            Vector2 glowOrigin = new Vector2(glow.Width / 2, glow.Height / 2);
            glowRotation += 0.1f;
            if (glowRotation > Math.PI * 2f) 
                glowRotation -= (float)Math.PI * 2f;

            for (int k = 0; k < Projectile.oldPos.Length - 1; k++) 
            {
                float glowRot = (float)Math.Atan2(
                    Projectile.oldPos[k].Y - Projectile.oldPos[k + 1].Y,
                    Projectile.oldPos[k].X - Projectile.oldPos[k + 1].X
                );

                Color glowColor = new Color(180, 180, 180, 120);
                Vector2 glowPos = Projectile.oldPos[k] + Projectile.Size / 2f - Main.screenPosition;

                Main.spriteBatch.Draw(glow, glowPos, null, glowColor, glowRot, glowOrigin, 
                    Projectile.scale - k / (float)Projectile.oldPos.Length, spriteEffects, 0f);
            }

            Main.spriteBatch.Draw(texture, position, null, lightColor, rotation, drawOrigin, Projectile.scale, spriteEffects, 0f);
            return false;
        }
    }
}