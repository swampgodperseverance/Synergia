using System;
using Avalon.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Helpers;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.RangedProjectiles
{
    public class PhoenixProj : ModProjectile
    {
        private Vector2 initialVelocity;
        private float travelTime = 80f;
        private float elapsed = 0f;
        private float curveOffset; 
        private float curveDirection; 

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

        public override void OnSpawn(IEntitySource source)
        {
            curveOffset = Main.rand.NextFloat(-0.8f, 0.8f);
            curveDirection = Main.rand.NextFloat(0.5f, 1.5f);
        }

        public override void AI()
        {
            if (elapsed == 0f)
            {
                initialVelocity = Projectile.velocity;
            }

            elapsed++;

            float t = MathHelper.Clamp(elapsed / travelTime, 0f, 1f);
            Vector2 newVelocity = initialVelocity * (1f - EaseFunctions.EaseOutCubic(t));
            float curveAmount = MathHelper.Clamp(1f - t, 0f, 1f); 
            float sinCurve = (float)Math.Sin(elapsed * 0.1f * curveDirection) * curveOffset * curveAmount;
            Vector2 curvedVelocity = newVelocity.RotatedBy(sinCurve * 0.05f);
            Projectile.velocity = curvedVelocity;

            if (Projectile.velocity.LengthSquared() > 0.1f)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
            if (Main.rand.NextBool(3))
            {
                Dust d = Dust.NewDustDirect(Projectile.position - new Vector2(4, 4), Projectile.width + 8, Projectile.height + 8, DustID.Torch, 0, 0, 100, default, Main.rand.NextFloat(0.8f, 1.3f));
                d.velocity = -Projectile.velocity * 0.2f + Main.rand.NextVector2Circular(1f, 1f);
                d.noGravity = true;
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
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Lava, 0, 0, 100, default, Main.rand.NextFloat(1f, 1.6f));
                d.velocity = Main.rand.NextVector2Circular(3f, 3f);
                d.noGravity = true;
            }

            for (int i = 0; i < 8; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.SolarFlare, 0, 0, 100, default, Main.rand.NextFloat(0.8f, 1.4f));
                d.velocity = Main.rand.NextVector2Circular(2.5f, 2.5f);
                d.noGravity = true;
            }

            ParticleSystem.AddParticle(new EnergyRevolverParticle(), Projectile.Center, default, new Color(255, 100, 0), 0, 0.8f, 20);
            ParticleSystem.AddParticle(new EnergyRevolverParticle(), Projectile.Center, default, new Color(255, 50, 0), 0, 0.6f, 25);

            Projectile.Kill();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explode();
            return false;
        }

      
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Rectangle frame = texture.Frame(1, 1, 0, 0);
            Vector2 origin = frame.Size() / 2f;

            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Color outlineColor = new Color(255, 80, 0, 150);
            Color innerGlow = new Color(255, 180, 40, 100);

            for (int i = 0; i < 4; i++)
            {
                float angle = i * MathHelper.PiOver2;
                Vector2 offset = angle.ToRotationVector2() * 1.5f;

                Main.spriteBatch.Draw(texture, drawPosition + offset, frame, outlineColor, Projectile.rotation, origin, Projectile.scale, effects, 0f);
            }

            float pulse = 0.8f + (float)Math.Sin(Main.GameUpdateCount * 0.3f) * 0.3f;
            Vector2[] corners = new Vector2[]
            {
                new Vector2(-1.2f, -1.2f), new Vector2(1.2f, -1.2f),
                new Vector2(-1.2f, 1.2f), new Vector2(1.2f, 1.2f)
            };

            foreach (Vector2 corner in corners)
            {
                Main.spriteBatch.Draw(texture, drawPosition + corner * pulse, frame, innerGlow, Projectile.rotation, origin, Projectile.scale, effects, 0f);
            }

            Main.spriteBatch.Draw(texture, drawPosition, frame, Color.White, Projectile.rotation, origin, Projectile.scale, effects, 0f);

            return false;
        }
    }
}