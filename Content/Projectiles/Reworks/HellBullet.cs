using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Helpers;
using Avalon.Particles;
using Synergia.Common.GlobalPlayer;

namespace Synergia.Content.Projectiles.Reworks
{
    public class Hellbullet : ModProjectile
    {
        private Vector2 initialVelocity;
        private float travelTime = 360f;
        private float elapsed = 0f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10; 
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0; 
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
                initialVelocity = Projectile.velocity;

            elapsed++;

            float t = MathHelper.Clamp(elapsed / travelTime, 0f, 1f);
            Projectile.velocity = initialVelocity * (1f - EaseFunctions.EaseOutCubic(t));

            if (Projectile.velocity.LengthSquared() > 0.1f)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Projectile.velocity.Length() < 0.5f)
                Explode();
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

            Player owner = Main.player[Projectile.owner];
            owner.GetModPlayer<ScreenShakePlayer>().TriggerShake(6, 0.3f);

            Projectile.Kill();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explode();
            return false;
        }



        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

            Vector2 origin = new Vector2(tex.Width / 2, tex.Height / 2);

            for (int i = 1; i <= 2; i++)
            {
                if (Projectile.oldPos.Length > i)
                {
                    Vector2 trailPos = Projectile.oldPos[i] + Projectile.Size / 2;
                    float alpha = 0.5f - i * 0.2f;   
                    float scale = Projectile.scale - i * 0.1f;

                    spriteBatch.Draw(
                        tex,
                        trailPos - Main.screenPosition,
                        null,
                        lightColor * alpha,
                        Projectile.rotation,
                        origin,
                        scale,
                        SpriteEffects.None,
                        0f
                    );
                }
            }

            spriteBatch.Draw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );

            return false;
        }
    }
}
