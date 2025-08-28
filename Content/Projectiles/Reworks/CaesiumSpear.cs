using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;
using Terraria;
using Avalon.Dusts;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks
{
    public class CaesiumSpear : ModProjectile
    {
        private float glowRotation;
        private const float maxReach = 260f; 
        private const float minReach = 45f;  

        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults() {
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

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            Vector2 origin = player.RotatedRelativePoint(player.MountedCenter);
            Projectile.direction = player.direction;
            player.heldProj = Projectile.whoAmI;
            Projectile.Center = origin;

            if (Projectile.ai[0] == 1) {
                SoundStyle spearSound = new SoundStyle("Synergia/Assets/Sounds/SpearSound") {
                    Volume = 0.5f,
                    PitchVariance = 0.2f
                };
                SoundEngine.PlaySound(spearSound, Projectile.Center);
            }
            if (player.dead) {
                Projectile.Kill();
                return;
            }

            Projectile.ai[0]++;

            if (!player.frozen) {
                Projectile.spriteDirection = Projectile.direction = player.direction;
                Projectile.alpha = Math.Max(0, Projectile.alpha - 127);
                if (Projectile.localAI[0] > 0f) Projectile.localAI[0] -= 1f;

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
                               + new Vector2(velLen + reach + 40f, 0f).RotatedBy(velRot);

                Projectile.rotation = origin.AngleTo(target) + (float)Math.PI / 4f * player.direction;
                if (Projectile.spriteDirection == -1)
                    Projectile.rotation += (float)Math.PI;
                if (Main.netMode != NetmodeID.Server) {
                    Vector2 normVel = Projectile.velocity.SafeNormalize(Vector2.UnitY);
                    for (int i = 0; i < 2; i++) {
                        Dust dust = Dust.NewDustDirect(Projectile.Center, 14, 14, ModContent.DustType<CaesiumDust>(), 0f, 0f, 120);
                        dust.velocity = origin.DirectionTo(dust.position) * 2f;
                        dust.position = Projectile.Center + normVel.RotatedBy((1f - animProgress) * ((float)Math.PI * 2f) * 2f
                            + i / 2f * ((float)Math.PI * 2f)) * 8f;
                        dust.scale = Main.rand.NextFloat(0.8f, 1.4f);
                        dust.velocity += normVel * 3f;
                        dust.noGravity = true;
                        dust.noLight = false;
                        dust.color = Color.Lerp(Color.Orange, Color.Green, Main.rand.NextFloat());
                    }
                }

                Projectile.netUpdate = true;
            }

            if (player.itemAnimation == 2) {
                Projectile.Kill();
                player.reuseDelay = 2;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Rectangle hitbox = new Rectangle(
                (int)Projectile.position.X,
                (int)Projectile.position.Y,
                Projectile.width,
                Projectile.height
            );

            return hitbox.Intersects(targetHitbox);
        }



        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            Projectile.direction = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : -1;
            CreateExplosionEffect(target.Center);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) {
            if (info.PvP) {
                Projectile.direction = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : -1;
                CreateExplosionEffect(target.Center);
            }
        }

        private void CreateExplosionEffect(Vector2 position) {
            for (int i = 0; i < 30; i++) {
                Dust dust = Dust.NewDustDirect(position, 0, 0, DustID.Smoke);
                dust.velocity = Main.rand.NextVector2Circular(6f, 6f);
                dust.scale = Main.rand.NextFloat(1f, 1.8f);
                dust.noGravity = true;
                dust.color = Color.Lerp(Color.Orange, Color.Green, Main.rand.NextFloat());
            }

            for (int i = 0; i < 15; i++) {
                Dust spark = Dust.NewDustDirect(position, 0, 0, DustID.FireworkFountain_Green);
                spark.velocity = Main.rand.NextVector2Circular(4f, 4f);
                spark.scale = Main.rand.NextFloat(1f, 1.4f);
                spark.noGravity = true;
            }

            for (int i = 0; i < 10; i++) {
                Dust flash = Dust.NewDustDirect(position, 0, 0, DustID.MagicMirror);
                flash.velocity = Main.rand.NextVector2Circular(2f, 2f);
                flash.scale = Main.rand.NextFloat(1.5f, 2.2f);
                flash.noGravity = true;
                flash.color = Color.Lerp(Color.Orange, Color.Lime, Main.rand.NextFloat());
            }

            Lighting.AddLight(position, 1.5f, 1.5f, 0.8f);
        }


        public override bool PreDraw(ref Color lightColor) {
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
            if (player.gravDir == -1f) {
                spriteEffects = SpriteEffects.FlipVertically;
                rotation += (float)Math.PI / 2f * Projectile.spriteDirection;
            }

            Texture2D glow = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_1").Value;
            Vector2 glowOrigin = new Vector2(glow.Width / 2, glow.Height / 2);
            glowRotation += 0.1f;
            if (glowRotation > Math.PI * 2f) glowRotation -= (float)Math.PI * 2f;

            for (int k = 0; k < Projectile.oldPos.Length - 1; k++) {
                Vector2 glowPosition = new Vector2(Projectile.width, Projectile.height) / 2f
                                     + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
                float glowRot = (float)Math.Atan2(
                    Projectile.oldPos[k].Y - Projectile.oldPos[k + 1].Y,
                    Projectile.oldPos[k].X - Projectile.oldPos[k + 1].X
                );


                Color glowColor = Color.Lerp(
                    Color.Orange,
                    Color.Green,
                    k / (float)(Projectile.oldPos.Length - 1)
                );
                glowColor.A = 20; 

                spriteBatch.Draw(glow, Projectile.oldPos[k] + glowPosition, null, glowColor, glowRot, glowOrigin, Projectile.scale - k / (float)Projectile.oldPos.Length, spriteEffects, 0f);
                spriteBatch.Draw(glow, (Projectile.oldPos[k] + Projectile.oldPos[k + 1]) * 0.5f + glowPosition, null, glowColor, glowRot, glowOrigin, Projectile.scale - k / (float)Projectile.oldPos.Length, spriteEffects, 0f);
            }

            spriteBatch.Draw(texture, position, null, lightColor, rotation, drawOrigin, Projectile.scale, spriteEffects, 0f);
            return false;
        }
    }
}
