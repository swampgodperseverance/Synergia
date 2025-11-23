using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Friendly
{
    public class BismuthumShrapnel : ModProjectile
    {
        private bool isLooping = false;
        private int loopTick = 0;
        private int loopLength = 12;
        private float speedPulse = 0f;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0; 
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 150; 
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 0; 
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void AI()
        {
            Lighting.AddLight(
                (int)(Projectile.position.X / 16f),
                (int)(Projectile.position.Y / 16f),
                0.36f, 0.15f, 0.5f
            );

            Projectile.rotation = Projectile.velocity.ToRotation() + 1.12f;

            speedPulse += 0.07f;
            float speedFactor = 1f + (float)Math.Sin(speedPulse) * 0.025f;
            Projectile.velocity *= speedFactor;

            if (!isLooping)
            {
                Vector2 vel = Projectile.velocity;
                Projectile.velocity = vel.RotatedBy(Main.rand.Next(-1, 2) * (Math.PI / 50)); // поворачивает слабее
            }

            if (Main.rand.NextBool(120) && !isLooping)
            {
                loopTick = 0;
                isLooping = true;
                loopLength = Main.rand.Next(18, 26); 
            }

            if (isLooping)
            {
                Vector2 vel = Projectile.velocity;
                Projectile.velocity = vel.RotatedBy(Math.PI / (loopLength + 2));
                loopTick++;

                if (loopTick >= loopLength * 2)
                {
                    isLooping = false;
                    loopLength = Main.rand.Next(10, 15);
                }
            }

            if (Main.rand.NextBool(20))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 150, default, 0.7f);
                Main.dust[dust].velocity *= 0.2f;
                Main.dust[dust].scale = 0.8f;
                Main.dust[dust].noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 150, default, 1.1f);
                Main.dust[dust].velocity *= 1.3f;
                Main.dust[dust].scale = Main.rand.NextFloat(0.9f, 1.2f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }
    }
}
