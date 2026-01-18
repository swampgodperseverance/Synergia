using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace Synergia.Content.Projectiles.Thrower
{
    public class Aeglos2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
          
            ProjectileID.Sets.TrailCacheLength[Type] = 3;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.aiStyle = 1;
            Projectile.DamageType = DamageClass.Throwing;

            Projectile.penetrate = -1; 
            Projectile.timeLeft = 300;

            Projectile.extraUpdates = 1;
            AIType = ProjectileID.BoneJavelin;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.spriteDirection = Projectile.velocity.X >= 0f ? 1 : -1;

            if (Projectile.velocity.Y < 0)
                Projectile.velocity.Y -= 0.06f;
            else
                Projectile.velocity.Y *= 1.02f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 origin = texture.Size() / 2f;

          
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float alpha = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Color color = lightColor * alpha * 0.6f;

                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;

                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    null,
                    color,
                    Projectile.rotation,
                    origin,
                    Projectile.scale,
                    Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                    0
                );
            }

            return true; 
        }

        public override void OnKill(int timeLeft)
        {
            const int dustCount = 10;

            for (int i = 0; i < dustCount; i++)
            {
                Vector2 velocity = Projectile.velocity * Main.rand.NextFloat(0.2f, 1.4f);
                int dust = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Iron,
                    velocity.X,
                    velocity.Y,
                    100,
                    default,
                    1.4f
                );

                Main.dust[dust].noLight = true;
            }

            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
        }
    }
}
