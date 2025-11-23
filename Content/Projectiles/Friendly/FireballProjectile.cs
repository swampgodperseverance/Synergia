using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using System;

namespace Synergia.Content.Projectiles.Friendly
{
    public class FireballProjectile : ModProjectile
    {
        private float gravity = 0.2f;
        private float maxSpeed = 12f; 

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 6;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 90;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 300;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Lighting.AddLight(Projectile.Center, 1f, 0.5f, 0f);

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position, 
                    Projectile.width, 
                    Projectile.height, 
                    Main.rand.NextBool() ? DustID.Torch : DustID.Lava, 
                    0f, 0f, 100);
                
                dust.noGravity = true;
                dust.scale = 1.5f;
                
                dust.velocity = -Projectile.velocity * 0.2f;
            }

            Projectile.velocity.Y += gravity;
            
            if (Projectile.velocity.Length() > maxSpeed)
            {
                Projectile.velocity = Vector2.Normalize(Projectile.velocity) * maxSpeed;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300);
            
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position, 
                    Projectile.width, 
                    Projectile.height, 
                    DustID.SolarFlare, 
                    0f, 0f, 100);
                
                dust.noGravity = true;
                dust.scale = 1.5f;
                dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
            }
            
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position, 
                    Projectile.width, 
                    Projectile.height, 
                    DustID.SolarFlare, 
                    0f, 0f, 100);
                
                dust.noGravity = true;
                dust.scale = 1.5f;
                dust.velocity = Main.rand.NextVector2Circular(5f, 5f);
            }
            
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 200, 100, 150); 
        }
         public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            SpriteEffects spriteEffects = Projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 drawOrigin = new(Projectile.spriteDirection == 1 ? texture.Width : 0f, texture.Height);
            Color drawColor = Color.White * Projectile.Opacity;
            Color trailColor = drawColor with { A = 0 };
            Color trailColor2 = Color.Red with { A = 0 } * Projectile.Opacity;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                trailColor *= 0.91f;
                trailColor2 *= 0.91f;
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, trailColor * 0.5f, Projectile.oldRot[i], drawOrigin, Projectile.scale, spriteEffects, 0f);
            }

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, drawColor, Projectile.rotation, drawOrigin, Projectile.scale, spriteEffects, 0f);
            return false;
        }
    }
}