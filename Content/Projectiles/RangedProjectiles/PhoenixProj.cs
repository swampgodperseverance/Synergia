using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Helpers;
using Avalon.Particles;

namespace Synergia.Content.Projectiles.RangedProjectiles
{
    public class PhoenixProj : ModProjectile
    {
        private Vector2 initialVelocity;
        private float travelTime = 80f; // Увелчи и будет лететь дальше
        private float elapsed = 0f;

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
    }
}
