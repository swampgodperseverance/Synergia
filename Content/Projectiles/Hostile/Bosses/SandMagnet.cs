using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile.Bosses
{
    public class SandMagnet : ModProjectile
    {
        private const float MaxScale = 1.2f;
        private const float MinScale = 0.8f;
        private const float PulseSpeed = 0.01f;
        private const float RotationSpeed = 0.05f;
        private const float AttractionForce = 0.9f;
        private const float AttractionRadius = 1000f;
        private const int LifeTime = 300; 

        private float currentScale;
        private bool scalingUp = true;

        private bool rising = true;
        private float riseTimer = 0f;
        private const float RiseDuration = 90f; 
        private const float RiseHeight = 80f; 

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = LifeTime;
            Projectile.penetrate = -1;
            Projectile.alpha = 255; 
            Projectile.scale = MinScale;
            currentScale = MinScale;
        }

        public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
        {
            Terraria.Audio.SoundEngine.PlaySound(
                new Terraria.Audio.SoundStyle("Synergia/Assets/Sounds/SandSphere") 
                { 
                    Volume = 1.1f, 
                    PitchVariance = 0.15f 
                }, 
                Projectile.Center
            );

            for (int i = 0; i < 40; i++)
            {
                int dust = Dust.NewDust(Projectile.Center - new Vector2(10), 20, 20, DustID.Sandnado, 
                    Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3), 150, default, 1.4f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override void AI()
        {
            if (Projectile.timeLeft > LifeTime - 60)
            {
                Projectile.alpha -= 4;
                if (Projectile.alpha < 0) Projectile.alpha = 0;
            }


            if (Projectile.timeLeft < 60)
            {
                Projectile.alpha += 4;
                if (Projectile.alpha > 255) Projectile.alpha = 255;
            }

            if (rising)
            {
                riseTimer++;
                float progress = MathHelper.Clamp(riseTimer / RiseDuration, 0f, 1f);
                Projectile.position.Y -= (RiseHeight / RiseDuration); 

                if (progress >= 1f)
                    rising = false;
            }

            if (scalingUp)
            {
                currentScale += PulseSpeed;
                if (currentScale >= MaxScale)
                {
                    currentScale = MaxScale;
                    scalingUp = false;
                }
            }
            else
            {
                currentScale -= PulseSpeed;
                if (currentScale <= MinScale)
                {
                    currentScale = MinScale;
                    scalingUp = true;
                }
            }
            Projectile.scale = currentScale;

            Projectile.rotation += RotationSpeed;

            AttractPlayers();

            SpawnDustRing();
            if (Projectile.timeLeft == 1)
                FinalBurst();
        }

        private void AttractPlayers()
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (!player.active || player.dead)
                    continue;

                float distance = Vector2.Distance(player.Center, Projectile.Center);
                if (distance <= AttractionRadius)
                {
                    Vector2 dir = Vector2.Normalize(Projectile.Center - player.Center);
                    float forceMult = 1f - (distance / AttractionRadius);
                    Vector2 attraction = dir * AttractionForce * forceMult;

                    player.velocity += attraction;


                    float maxSpeed = 4f;
                    if (player.velocity.Length() > maxSpeed)
                        player.velocity = Vector2.Normalize(player.velocity) * maxSpeed;

                    for (int j = 0; j < 2; j++)
                    {
                        int dust = Dust.NewDust(player.position, player.width, player.height, DustID.Sandnado, 0f, 0f, 100, default, 1.3f);
                        Main.dust[dust].velocity *= 0.4f;
                        Main.dust[dust].noGravity = true;
                    }
                }
            }
        }

        private void SpawnDustRing()
        {
            if (Main.rand.NextBool(3))
            {
                float angle = Main.rand.NextFloat(0, MathHelper.TwoPi);
                float radius = 40f * Projectile.scale;
                Vector2 pos = Projectile.Center + angle.ToRotationVector2() * radius;

                Vector2 dustVel = (Projectile.Center - pos).SafeNormalize(Vector2.Zero) * 2f;
                Dust dust = Dust.NewDustPerfect(pos, DustID.Sandnado, dustVel, 100, default, 1.2f);
                dust.noGravity = true;
            }
        }

        private void FinalBurst()
        {
            for (int i = 0; i < 40; i++)
            {
                Vector2 vel = Main.rand.NextVector2Circular(6f, 6f);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Sandnado, vel, 150, default, 1.5f);
                dust.noGravity = true;
            }
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item70, Projectile.Center);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 255 - Projectile.alpha);
        }
    }
}
