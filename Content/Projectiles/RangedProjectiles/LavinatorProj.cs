using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.RangedProjectiles
{
    public class LavinatorDart : ModProjectile
    {
        private const int MicroBurstCooldown = 8;
        private const int MicroBurstDamage = 10;
        private const float MicroBurstKnockback = 1f;

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.alpha = 255;
            Projectile.timeLeft = 3000;

            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 3f)
            {
                Projectile.alpha = 0;
            }

            if (Projectile.ai[0] >= 20f)
            {
                Projectile.ai[0] = 20f;
                Projectile.velocity.Y += 0.075f;
            }

            int dustIndex = Dust.NewDust(new Vector2(Projectile.Center.X - 4f, Projectile.Center.Y - 4f), 1, 1, 6, 0f, 0f, 100, default(Color), 1f);
            Dust dust = Main.dust[dustIndex];
            dust.velocity += Projectile.velocity * -0.5f;
            dust.velocity *= 0.25f;

            Projectile.localAI[1]++;

            if (Projectile.localAI[1] > MicroBurstCooldown && Main.rand.NextBool(2))
            {
                Projectile.localAI[1] = 0;
                SpawnMicroBurst();
            }

            SpawnTrailDust();
        }

        private void SpawnMicroBurst()
        {
            float distance = Main.rand.NextFloat(10f, 25f);
            float angle = Main.rand.NextFloat(MathHelper.TwoPi);

            Vector2 offset = new Vector2(
                (float)Math.Cos(angle) * distance,
                (float)Math.Sin(angle) * distance
            );

            Vector2 burstPosition = Projectile.Center + offset;

            int burstDamage = MicroBurstDamage;
            float burstKnockback = MicroBurstKnockback;

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                burstPosition,
                Vector2.Zero,
                ModContent.ProjectileType<LavinatorMicroBurst>(),
                burstDamage,
                burstKnockback,
                Projectile.owner,
                Main.rand.NextFloat(0.7f, 1.0f)
            );
        }

        private void SpawnTrailDust()
        {
            if (Main.rand.NextBool(2))
            {
                int trailDust = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Torch,
                    Projectile.velocity.X * -0.2f,
                    Projectile.velocity.Y * -0.2f,
                    100,
                    default(Color),
                    1.4f
                );

                Dust dust = Main.dust[trailDust];
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(1.0f, 1.5f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, (3000 - Projectile.timeLeft) * 4);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, (3000 - Projectile.timeLeft) * 4);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

            Vector2 vector2 = new Vector2(20f, 20f);
            for (int i = 0; i < 8; i++)
            {
                int index2 = Dust.NewDust(
                    new Vector2(Projectile.Center.X - vector2.X / 2f, Projectile.Center.Y - vector2.Y / 2f),
                    (int)vector2.X,
                    (int)vector2.Y,
                    6,
                    0f,
                    0f,
                    100,
                    default(Color),
                    1.4f
                );

                Dust dust = Main.dust[index2];
                dust.velocity = new Vector2(dust.velocity.X * 1.6f, dust.velocity.Y * 1.6f);
                dust.noGravity = true;
            }
        }
    }

    public class LavinatorMicroBurst : ModProjectile
    {
        private float Scale => Projectile.ai[0];

        public override string Texture => "Synergia/Assets/Textures/Ring";

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 14;
            Projectile.alpha = 0;
            Projectile.scale = 0.07f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Projectile.scale += 0.025f * Scale;
            Projectile.rotation += 0.15f;

            if (Projectile.timeLeft < 7)
            {
                Projectile.alpha += 35;
            }

            if (Projectile.timeLeft < 4)
            {
                Projectile.damage = 0;
            }

            Lighting.AddLight(Projectile.Center, 1.0f, 0.4f, 0.1f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D glowTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Glow").Value;
            Texture2D ringTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ring").Value;
            Texture2D coreTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/CoreGlow").Value;

            float opacity = 1f - Projectile.alpha / 255f;
            Color color = new Color(255, 80, 30) * opacity;
            Color orangeColor = new Color(255, 130, 30) * opacity * 0.6f;
            Color yellowColor = new Color(255, 180, 30) * opacity * 0.4f;

            Vector2 position = Projectile.Center - Main.screenPosition;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Main.spriteBatch.Draw(
                ringTexture,
                position,
                null,
                orangeColor,
                Projectile.rotation,
                ringTexture.Size() / 2f,
                Projectile.scale * 1.4f,
                SpriteEffects.None,
                0f
            );

            Main.spriteBatch.Draw(
                glowTexture,
                position,
                null,
                color,
                -Projectile.rotation * 0.5f,
                glowTexture.Size() / 2f,
                Projectile.scale * 1.0f,
                SpriteEffects.None,
                0f
            );

            Main.spriteBatch.Draw(
                coreTexture,
                position,
                null,
                yellowColor,
                Projectile.rotation * 0.3f,
                coreTexture.Size() / 2f,
                Projectile.scale * 0.6f,
                SpriteEffects.None,
                0f
            );

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0f;
            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(),
                targetHitbox.Size(),
                Projectile.Center,
                Projectile.Center,
                8 * Projectile.scale * 3,
                ref collisionPoint
            );
        }
    }
}