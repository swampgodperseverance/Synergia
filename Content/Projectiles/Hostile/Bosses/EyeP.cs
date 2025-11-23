using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile.Bosses
{
    public class EyeP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;


            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.light = 0.5f;
        }

        private float waveCounter = 0f; 

        public override void AI()
        {

            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
            }

            waveCounter += 0.2f; 
            float waveOffset = (float)Math.Sin(waveCounter) * 1.5f; 

            float speed = Projectile.velocity.Length();
            Vector2 dir = Projectile.velocity.SafeNormalize(Vector2.UnitX);
            dir = dir.RotatedBy(waveOffset * 0.05f);
            Projectile.velocity = dir * speed;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Lighting.AddLight(Projectile.Center, new Vector3(0.9f, 0.2f, 0.2f));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / Main.projFrames[Projectile.type] / 2f);

 
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 oldPos = Projectile.oldPos[i];
                if (oldPos == Vector2.Zero)
                    continue;

                float fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Color trailColor = Color.White * fade * 0.5f;

                int frameHeight = texture.Height / Main.projFrames[Projectile.type];
                Rectangle sourceRect = new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight);

                Main.EntitySpriteDraw(
                    texture,
                    oldPos + Projectile.Size / 2f - Main.screenPosition,
                    sourceRect,
                    trailColor,
                    Projectile.rotation, 
                    origin,
                    Projectile.scale,
                    SpriteEffects.None,
                    0
                );
            }


            int height = texture.Height / Main.projFrames[Projectile.type];
            Rectangle rect = new Rectangle(0, height * Projectile.frame, texture.Width, height);
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                rect,
                Color.White,
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