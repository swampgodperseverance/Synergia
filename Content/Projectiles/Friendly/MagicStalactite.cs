using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using System;
using Terraria.ModLoader;

namespace Vanilla.Content.Projectiles.Friendly
{
    public sealed class MagicStalactite : ModProjectile //Roa inspired
    {
        private const int TrailLength = 10;
        private const int FadeInDuration = 60; 
        private const int DustSpawnRate = 4;
        
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = TrailLength;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.alpha = 255; 
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Projectile.alpha = Math.Max(Projectile.alpha - (255 / FadeInDuration), 0);

            // Включить столкновение с тайлами через 1 секунду
            if (Projectile.timeLeft <= 180 - FadeInDuration)
                Projectile.tileCollide = true;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Main.rand.NextBool(DustSpawnRate))
            {
                SpawnTravelDust();
            }
        }

        private void SpawnTravelDust()
        {
            int dustType = DustID.Torch;
            float scale = Main.rand.NextFloat(1.0f, 1.4f);
            
            Dust dust = Dust.NewDustPerfect(
                Projectile.Center + Main.rand.NextVector2Circular(Projectile.width/2, Projectile.height/2),
                dustType,
                Projectile.velocity * 0.1f + Main.rand.NextVector2Circular(0.3f, 0.3f),
                100,
                default,
                scale
            );
            
            dust.noGravity = true;
            dust.fadeIn = 0.5f;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 10;
            fallThrough = false;
            return Projectile.tileCollide;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27 with { Volume = 0.7f }, Projectile.Center);

            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Torch,
                    Main.rand.NextVector2Circular(3f, 3f),
                    100,
                    default,
                    Main.rand.NextFloat(1.2f, 1.8f)
                );
                dust.noGravity = true;
                dust.fadeIn = 1.2f;
            }

            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Firework_Red,
                    Main.rand.NextVector2Circular(2f, 2f),
                    0,
                    default,
                    Main.rand.NextFloat(0.8f, 1.2f)
                );
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() * 0.5f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) continue;

                float progress = 1f - (i / (float)Projectile.oldPos.Length);
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
                Color trailColor = new Color(255, 160, 60, 0) * progress * 0.7f;
                float scale = Projectile.scale * (0.8f + progress * 0.2f);

                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    null,
                    trailColor,
                    Projectile.rotation,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0
                );
            }

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                Projectile.GetAlpha(lightColor) * 1.2f,
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