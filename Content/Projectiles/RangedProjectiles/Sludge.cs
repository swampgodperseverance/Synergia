using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System;
using Terraria.Audio;
using Terraria.ModLoader;
using Avalon.Dusts;
using Terraria.GameContent;

namespace Synergia.Content.Projectiles.RangedProjectiles
{
    public class Sludge : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = 1;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;
            Projectile.scale = 1f;
            Projectile.alpha = 255;
        }

        private Player player => Main.player[Projectile.owner];

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.15f, 0.35f, 0.12f);

            Projectile.spriteDirection = Projectile.direction;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.ai[0]++;

            if (Projectile.ai[0] == 4)
            {
                Projectile.alpha = 0;

                for (int i = 0; i < 12; i++)
                {
                    int dust = Dust.NewDust(
                        Projectile.position,
                        Projectile.width,
                        Projectile.height,
                        ModContent.DustType<ContagionWaterSplash>(),
                        Projectile.oldVelocity.X * 0.4f,
                        Projectile.oldVelocity.Y * 0.4f,
                        0,
                        new Color(30, 90, 40),
                        1.2f
                    );

                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.4f;
                }
            }

            if (Projectile.ai[0] > 10)
                Projectile.velocity.Y += Projectile.ai[0] / 100f;

            if (Projectile.velocity.Length() >= 20f)
                Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 20f;

            if (Projectile.ai[0] >= 4)
            {
                int dust = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    ModContent.DustType<ContagionWaterSplash>(),
                    Projectile.oldVelocity.X * 0.1f,
                    Projectile.oldVelocity.Y * 0.1f,
                    0,
                    new Color(20, 80, 35),
                    0.9f
                );

                Main.dust[dust].noGravity = true;
                Main.dust[dust].fadeIn = 0.7f;
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 5)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }

            if (Projectile.frame >= 3)
                Projectile.frame = 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];

            Rectangle frame = new Rectangle(
                0,
                frameHeight * Projectile.frame,
                texture.Width,
                frameHeight
            );

            Vector2 origin = new Vector2(texture.Width / 2f, frameHeight / 2f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            float glowIntensity = 0.6f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 8f) * 0.15f; 

            for (int i = 0; i < 2; i++)
            {
                float offset = (i + 1) * 0.8f; 
                Vector2 glowOffset = new Vector2(offset, offset);

                Main.EntitySpriteDraw(
                    texture,
                    drawPos + glowOffset,
                    frame,
                    new Color(30, 150, 50, 25) * glowIntensity, 
                    Projectile.rotation,
                    origin,
                    Projectile.scale * 1.07f,
                    Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                    0
                );

                Main.EntitySpriteDraw(
                    texture,
                    drawPos - glowOffset,
                    frame,
                    new Color(30, 150, 50, 25) * glowIntensity,
                    Projectile.rotation,
                    origin,
                    Projectile.scale * 1.07f,
                    Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                    0
                );
            }

            for (int i = 0; i < 4; i++) 
            {
                float angle = i * MathHelper.TwoPi / 4f;
                Vector2 radialOffset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 1.2f; 

                Main.EntitySpriteDraw(
                    texture,
                    drawPos + radialOffset,
                    frame,
                    new Color(50, 200, 70, 35) * (glowIntensity * 0.7f),
                    Projectile.rotation,
                    origin,
                    Projectile.scale * 1.05f, 
                    Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                    0
                );
            }

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                frame,
                new Color(80, 255, 100, 50) * glowIntensity, 
                Projectile.rotation,
                origin,
                Projectile.scale * 1.08f, 
                Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0
            );

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                frame,
                new Color(200, 255, 180, 40) * (glowIntensity * 0.5f), 
                Projectile.rotation,
                origin,
                Projectile.scale * 1.03f, 
                Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0
            );

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                frame,
                Color.Lerp(lightColor, new Color(60, 180, 90, 255), 0.7f),
                Projectile.rotation,
                origin,
                Projectile.scale,
                Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0
            );

            return false;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int size = Projectile.width * 2;
            hitbox.Inflate(size / 2, size / 2);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath9, Projectile.position);

            for (int i = 0; i < 40; i++)
            {
                int dust = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    ModContent.DustType<ContagionWaterSplash>(),
                    Main.rand.NextFloat(-2f, 2f),
                    Main.rand.NextFloat(-2f, 2f),
                    0,
                    new Color(25, 100, 50),
                    Main.rand.NextFloat(1f, 1.6f)
                );

                Main.dust[dust].noGravity = false;
            }
        }
    }
}