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
    public class JadeShard : ModProjectile
    {
        private Vector2 initialVelocity;
        private float travelTime = 60f; 
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
            // == Trail (Jade Dust) ==
            Dust jadeTrail = Dust.NewDustPerfect(
                Projectile.Center,
                ModContent.DustType<JadeDust>()
            );
            jadeTrail.velocity += Projectile.velocity * -0.5f;
            jadeTrail.velocity *= 0.25f;
            jadeTrail.scale = 1.1f;
            jadeTrail.noGravity = true;


            // == Movement ==
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
            // == Jade visual explosion ==
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

            // Optionally можно оставить эффект из твоего мода
            ParticleSystem.AddParticle(
                new EnergyRevolverParticle(),
                Projectile.Center,
                default,
                new Color(0, 255, 120), // зелёный джейд
                0,
                0.9f,
                22
            );

            Projectile.Kill();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explode();
            return false;
        }
    }
}
