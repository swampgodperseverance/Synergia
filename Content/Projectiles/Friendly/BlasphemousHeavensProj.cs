using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using ValhallaMod.Projectiles.AI;
using System;

namespace Vanilla.Content.Projectiles.Friendly
{
    public class BlasphemousHeavensProj : ValhallaGlaive
    {
        private int trailCounter = 0;
        private const int TrailInterval = 3; 
        
        public override void SetDefaults()
        {
            Projectile.width = 30;
            DrawOffsetX = 4;
            Projectile.height = 30;
            DrawOriginOffsetY = -0;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.alpha = 0;
            Projectile.penetrate = 106;
            Projectile.scale = 0.9f;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.aiStyle = -1;
            
            bounces = 8;
            timeFlying = 30;
            speedHoming = 17f;
            speedFlying = 17f;
            speedComingBack = 28f;
            homingDistanceMax = 600f;
            homingStyle = HomingStyle.BasicGlaive;
            homingStart = true;
            homingIgnoreTile = true;
        }

        public override void AI()
        {
            base.AI();
            
            trailCounter++;
            if (trailCounter >= TrailInterval)
            {
                trailCounter = 0;
                
                Dust electricDust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Electric,
                    Scale: 1.4f
                );
                electricDust.noGravity = true;
                electricDust.velocity = Projectile.velocity * 0.5f;
                
                if (Main.rand.NextFloat() < 0.3f)
                {
                    Dust.NewDustPerfect(
                        Projectile.Center,
                        DustID.BlueTorch,
                        Vector2.Zero,
                        100,
                        new Color(100, 150, 255),
                        1.2f
                    );
                }
            }
            
            Lighting.AddLight(Projectile.Center, 0.3f, 0.3f, 0.8f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Vector2 spawnPos = target.Center - new Vector2(0, 500);
            Vector2 velocity = Vector2.UnitY * 25f;

            Projectile.NewProjectile(
                Projectile.GetSource_OnHit(target),
                spawnPos,
                velocity,
                ModContent.ProjectileType<LightningStrike>(),
                damageDone / 2,
                0f,
                Projectile.owner
            );

            for (int i = 0; i < 12; i++)
            {
                Vector2 dustVel = new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
                Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Electric,
                    dustVel,
                    100,
                    Color.White,
                    1.5f
                );
            }
            
            SoundEngine.PlaySound(SoundID.Item122 with { Pitch = -0.2f }, target.Center);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 25; i++)
            {
                Vector2 dustVel = new Vector2(Main.rand.NextFloat(-8f, 8f), Main.rand.NextFloat(-8f, 8f));
                Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Electric,
                    dustVel,
                    150,
                    new Color(200, 230, 255),
                    2f
                );
            }
            
            SoundEngine.PlaySound(SoundID.Item93 with { Volume = 0.7f, Pitch = 0.3f }, Projectile.Center);
        }
    }
}