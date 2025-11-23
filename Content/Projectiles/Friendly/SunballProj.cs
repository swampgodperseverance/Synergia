using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.ModLoader;
using Synergia.Helpers;
using Avalon.Particles;

namespace Synergia.Content.Projectiles.Friendly
{
    public class SunballProj : ModProjectile
    {
        private Vector2 initialVelocity;
        private float travelTime = 60f; // Увелчи и будет лететь дальше
        private float elapsed = 0f;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 4;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300; 
            Projectile.extraUpdates = 1;
            Projectile.alpha = 0;
        }

        public override void AI()
        {

            if (elapsed == 0f)
            {
                initialVelocity = Projectile.velocity;
            }

            elapsed++;

            float t = MathHelper.Clamp(elapsed / travelTime, 0f, 1f);

            Projectile.velocity = initialVelocity * (1f - EaseFunctions.EaseOutCubic(t));

            if (Projectile.velocity.LengthSquared() > 0.1f)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }

            if (Projectile.velocity.Length() < 0.5f)
            {
                Explode();
            }
        }

        private void Explode()
        {
            
            for (int i = 0; i < 8; i++) 
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Lava);
                d.velocity = Main.rand.NextVector2Circular(2f, 2f); 
                d.scale = Main.rand.NextFloat(0.8f, 1.2f);          
                d.noGravity = true;
            }

            ParticleSystem.AddParticle(new EnergyRevolverParticle(), Projectile.Center, default, new Color(255, 140, 0), 0, 0.8f, 20);

            Projectile.Kill();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explode();
            return false;
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
