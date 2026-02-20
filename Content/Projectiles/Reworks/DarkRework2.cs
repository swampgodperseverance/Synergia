using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks
{
    public class DarkRework2 : ModProjectile
    {
        private float glowRotation;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            base.Projectile.width = 20;
            base.Projectile.height = 20;
            base.Projectile.aiStyle = 19;
            base.Projectile.penetrate = -1;
            base.Projectile.scale = 1f;
            base.Projectile.alpha = 0;
            base.Projectile.hide = true;
            base.Projectile.ownerHitCheck = true;
            base.Projectile.DamageType = DamageClass.Melee;
            base.Projectile.tileCollide = false;
            base.Projectile.friendly = true;
            base.Projectile.usesLocalNPCImmunity = true;
            base.Projectile.localNPCHitCooldown = 60;
            base.Projectile.timeLeft = 300;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            float rotation = base.Projectile.velocity.ToRotation();
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), base.Projectile.Center + rotation.ToRotationVector2() * 15f, base.Projectile.Center + rotation.ToRotationVector2() * -88f, 24f * base.Projectile.scale, ref point))
            {
                return new bool?(true);
            }
            return base.Colliding(projHitbox, targetHitbox);
        }

        public override bool PreAI()
        {
            if (this.runOnce)
            {
                this.originalVelo = base.Projectile.velocity;
                this.runOnce = false;
            }
            int useTime = (int)base.Projectile.ai[0];
            Player player = Main.player[base.Projectile.owner];
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (player.Center - base.Projectile.Center).ToRotation() + 1.5707964f);
            base.Projectile.direction = player.direction;
            float degrees = 360f * (this.counter / (float)useTime);
            this.counter += 1f;
            Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true, true);
            Vector2 toMouse = new Vector2(-40f, 0f).RotatedBy((double)this.originalVelo.ToRotation(), default(Vector2)) + new Vector2(32f, 0f).RotatedBy((double)(MathHelper.ToRadians(degrees * (float)base.Projectile.direction) + this.originalVelo.ToRotation()), default(Vector2));
            base.Projectile.velocity = new Vector2(-base.Projectile.velocity.Length(), 0f).RotatedBy((double)toMouse.ToRotation(), default(Vector2));
            base.Projectile.direction = player.direction;
            player.heldProj = base.Projectile.whoAmI;
            player.itemTime = 3;
            player.itemAnimation = 3;
            base.Projectile.position.X = ownerMountedCenter.X - (float)(base.Projectile.width / 2);
            base.Projectile.position.Y = ownerMountedCenter.Y - (float)(base.Projectile.height / 2);

            // Сохраняем позиции для трейла
            if (Projectile.oldPos.Length > 0)
            {
                for (int i = Projectile.oldPos.Length - 1; i > 0; i--)
                {
                    Projectile.oldPos[i] = Projectile.oldPos[i - 1];
                }
                Projectile.oldPos[0] = Projectile.position;
            }

            if (!player.frozen)
            {
                float sin = (float)Math.Sin((double)(this.counter / (float)useTime * 3.1415927f));
                this.movementFactor = MathHelper.Lerp(10f, 24f, sin);
            }
            base.Projectile.position += base.Projectile.velocity * this.movementFactor;

            if (this.counter >= (float)useTime)
            {
                base.Projectile.Kill();
            }
            base.Projectile.rotation = (base.Projectile.Center - player.Center).ToRotation() + MathHelper.ToRadians(135f);
            base.Projectile.spriteDirection = -base.Projectile.direction;
            if (base.Projectile.spriteDirection == -1)
            {
                base.Projectile.rotation -= MathHelper.ToRadians(90f);
            }

            glowRotation += 0.1f;
            if (glowRotation > MathHelper.TwoPi)
                glowRotation -= MathHelper.TwoPi;

            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            // Рисуем трейл
            Texture2D glowTexture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_1").Value;
            Vector2 glowOrigin = new Vector2(glowTexture.Width / 2, glowTexture.Height / 2);

            Vector2 drawPos = Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;

            var spriteEffects = player.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            if (player.gravDir == -1f)
            {
                spriteEffects = SpriteEffects.FlipVertically;
            }

            // Рисуем трейл по старым позициям
            for (int k = 0; k < Projectile.oldPos.Length - 1; k++)
            {
                if (Projectile.oldPos[k] != Vector2.Zero && Projectile.oldPos[k + 1] != Vector2.Zero)
                {
                    Vector2 trailPos = Projectile.oldPos[k] + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
                    float trailRot = (float)Math.Atan2(
                        Projectile.oldPos[k].Y - Projectile.oldPos[k + 1].Y,
                        Projectile.oldPos[k].X - Projectile.oldPos[k + 1].X
                    );

                    Color trailColor = new Color(80 - k * 10, 0, 120 + k * 10, 100 - k * 10);
                    float trailScale = Projectile.scale * (1f - k / (float)Projectile.oldPos.Length);

                    spriteBatch.Draw(glowTexture, trailPos, null, trailColor, trailRot, glowOrigin, trailScale, spriteEffects, 0f);
                }
            }

            // Рисуем основную текстуру
            Vector2 drawOrigin = new Vector2(
                (Projectile.spriteDirection == 1) ? (texture.Width + 8f) : (-8f),
                (player.gravDir == 1f) ? (-8f) : (texture.Height + 8f)
            );

            Color darkPurpleColor = lightColor;
            darkPurpleColor.R = (byte)(darkPurpleColor.R * 0.5f);
            darkPurpleColor.G = (byte)(darkPurpleColor.G * 0.2f);
            darkPurpleColor.B = (byte)(darkPurpleColor.B * 1.2f);

            spriteBatch.Draw(texture, drawPos, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, spriteEffects, 0f);

            // Рисуем дополнительное свечение
            spriteBatch.Draw(glowTexture, drawPos, null, new Color(120, 0, 180, 100), glowRotation, glowOrigin, Projectile.scale * 1.2f, spriteEffects, 0f);

            return false;
        }

        public float movementFactor;
        private Vector2 originalVelo = Vector2.Zero;
        private bool runOnce = true;
        private float counter;
    }
}