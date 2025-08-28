using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile {
    public class LavaBone : ModProjectile {
        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults() {
            int width = 20; int height = 40;
            Projectile.Size = new Vector2(width, height);

            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.aiStyle = 2;
            Projectile.timeLeft = 180;

            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
        }

        public override void AI() {
            Projectile.rotation += 0.35f * Projectile.direction;

            if (Main.rand.NextBool(2)) {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Lava, 
                    Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1.2f);
            }

            if (Projectile.timeLeft < 60) {
                Projectile.tileCollide = true; 
            }

            if (Projectile.aiStyle == -1) {
                float scaleFactor3 = 18f;
                int num203 = Player.FindClosest(Projectile.Center, 1, 1);

                Vector2 vector20 = Main.player[num203].Center - Projectile.Center;
                vector20.Normalize();
                vector20 *= scaleFactor3;
                int num204 = 70;
                Projectile.velocity = (Projectile.velocity * (num204 - 1) + vector20) / num204;
                if (Projectile.velocity.Length() < 14f) {
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= 18f;
                }
            }
        }

        public override void OnKill(int timeLeft) {
            if (Main.netMode != NetmodeID.Server) {
                for (int k = 0; k < 10; k++) {
                    Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height,
                        DustID.Lava, Projectile.oldVelocity.X * 0.3f, Projectile.oldVelocity.Y * 0.3f);
                }
            }
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }

        public override bool PreDraw(ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            SpriteEffects effects = (Projectile.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            for (int k = 0; k < Projectile.oldPos.Length; k++) {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Color.OrangeRed * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length * 0.5f);
                float rotation;
                if (k + 1 >= Projectile.oldPos.Length) {
                    rotation = (Projectile.position - Projectile.oldPos[k]).ToRotation() + MathHelper.PiOver2;
                }
                else {
                    rotation = (Projectile.oldPos[k + 1] - Projectile.oldPos[k]).ToRotation() + MathHelper.PiOver2;
                }
                spriteBatch.Draw(texture, drawPos, null, color, rotation, drawOrigin, Projectile.scale - k / (float)Projectile.oldPos.Length, effects, 0f);
            }
            return true;
        }
    }
}
