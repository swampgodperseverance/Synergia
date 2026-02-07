using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.RangedProjectiles
{
    public class EnferFlames : ModProjectile
    {
        private const int FadeInTime = 18;
        private const int FadeOutTime = 20;
        private int spawnTime;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 60;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.light = 0.6f;
            Projectile.alpha = 255;
        }

        public override void OnSpawn(IEntitySource source)
        {
            spawnTime = Projectile.timeLeft;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            Projectile.velocity *= 0.96f;
            Projectile.velocity.Y -= 0.02f;

            Lighting.AddLight(Projectile.Center, 1.1f, 0.4f, 0.1f);

            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                Main.dust[dust].scale = Main.rand.NextFloat(1f, 1.4f);
                Main.dust[dust].velocity *= 0.3f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].fadeIn = 0.9f;
            }

            int elapsed = spawnTime - Projectile.timeLeft;

            if (elapsed < FadeInTime)
            {
                float progress = (float)elapsed / FadeInTime;
                Projectile.alpha = (int)(255 * (1f - progress));
            }
            else if (Projectile.timeLeft <= FadeOutTime)
            {
                float progress = (float)Projectile.timeLeft / FadeOutTime;
                Projectile.alpha = (int)(255 * (1f - progress));
                Projectile.scale = MathHelper.Lerp(0.6f, 1f, progress);
            }
            else
            {
                Projectile.alpha = 50;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 120);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item74, Projectile.position);

            for (int i = 0; i < 12; i++)
            {
                Vector2 dir = Main.rand.NextVector2Circular(1f, 1f);
                int d = Dust.NewDust(Projectile.Center, 0, 0, DustID.Torch, dir.X * 2.2f, dir.Y * 2.2f, 100, default, 1.4f);
                Main.dust[d].noGravity = true;
                Main.dust[d].fadeIn = 1.0f;
            }

            for (int i = 0; i < 6; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1.3f;
                Main.dust[dust].scale *= 1.1f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            Rectangle source = new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight);

            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Color drawColor = lightColor * ((255f - Projectile.alpha) / 255f);

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                source,
                drawColor,
                Projectile.rotation,
                new Vector2(texture.Width / 2f, frameHeight / 2f),
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            if (Projectile.timeLeft <= FadeOutTime + 8 && Projectile.timeLeft > 2)
            {
                Texture2D ring = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ring").Value;
                if (ring != null)
                {
                    float ringProgress = (float)Projectile.timeLeft / (FadeOutTime + 8);
                    // Scaling NOT MAIN
                    float ringScale = 0.15f + ringProgress * 0.15f;
                    float ringAlpha = ringProgress * 0.55f;

                    Main.EntitySpriteDraw(
                        ring,
                        drawPos,
                        null,
                        new Color(255, 180, 80, 0) * ringAlpha,
                        0f,
                        ring.Size() / 2f,
                        ringScale * 0.6f, // Flash scaling MAIN
                        SpriteEffects.None,
                        0
                    );
                }
            }
        

            return false;
        }
    }
}