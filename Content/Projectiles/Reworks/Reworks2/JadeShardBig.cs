using Synergia.Helpers;
using Avalon.Particles;

using ValhallaMod.Dusts;
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class JadeShardBig : ModProjectile
    {
        private Vector2 initialVelocity;
        private float travelTime = 120f; 
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
          
            Dust jadeTrail = Dust.NewDustPerfect(
                Projectile.Center,
                ModContent.DustType<JadeDust>()
            );
            jadeTrail.velocity += Projectile.velocity * -0.5f;
            jadeTrail.velocity *= 0.25f;
            jadeTrail.scale = 1.1f;
            jadeTrail.noGravity = true;


   
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
      
            for (int i = 0; i < 12; i++)
            {
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center,
                    ModContent.DustType<JadeDust>()
                );
                d.velocity = Main.rand.NextVector2Circular(3f, 3f);
                d.scale = Main.rand.NextFloat(1.0f, 1.4f);
                d.noGravity = true;
            }

        
            ParticleSystem.AddParticle(
                new EnergyRevolverParticle(),
                Projectile.Center,
                default,
                new Color(0, 255, 120),
                0,
                0.9f,
                22
            );


            float baseRotation = Projectile.velocity.ToRotation();
            float spread = MathHelper.ToRadians(30f); 

            for (int i = -1; i <= 1; i++)
            {
                float rot = baseRotation + spread * (i * 0.5f);

                Vector2 vel = rot.ToRotationVector2() * 10f; 

                Projectile.NewProjectile(
                    Projectile.GetSource_Death(),
                    Projectile.Center,
                    vel,
                    ModContent.ProjectileType<JadeShard>(),
                    (int)(Projectile.damage * 0.8f), 
                    Projectile.knockBack,
                    Projectile.owner
                );
            }

            Projectile.Kill();
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explode();
            return false;
        }
    }
}
