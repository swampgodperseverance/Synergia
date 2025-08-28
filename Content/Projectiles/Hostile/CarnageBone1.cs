using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile
{
    public class CarnageBone1 : ModProjectile
    {
        private enum BoneState { SlowSpin, Accelerating }
        private BoneState currentState = BoneState.SlowSpin;
        private int timer = 0;
        private const int SpinTime = 60; 
        private Player target;
        private const float MaxSpeed = 18f; 

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.damage = 18;
            Projectile.tileCollide = true;
            Projectile.timeLeft = SpinTime + 90; 
        }

        public override void AI()
        {
            timer++;

            // Интенсивная золотая пыль
            if (Main.rand.NextBool(5))
            {
                Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(12, 12),
                    DustID.GoldFlame,
                    Projectile.velocity * 0.4f + Main.rand.NextVector2Circular(2f, 2f),
                    150,
                    default,
                    1.3f
                ).noGravity = true;
            }

            if (target == null || !target.active || target.dead)
                target = FindNearestPlayer();

            switch (currentState)
            {
                case BoneState.SlowSpin:
                    Projectile.rotation += 0.2f * Projectile.direction; 
                    Projectile.velocity *= 0.95f; 

                    if (timer >= SpinTime)
                    {
                        currentState = BoneState.Accelerating;
                        timer = 0;
                    }
                    break;

                case BoneState.Accelerating:
                    if (target != null)
                    {
                        Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                        float acceleration = MathHelper.Lerp(0.3f, 0.6f, timer / 45f); 
                        Projectile.velocity += direction * acceleration;
                    }

                    Projectile.rotation += 0.5f * Projectile.direction; 

                    if (Projectile.velocity.Length() > MaxSpeed)
                        Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * MaxSpeed;

                    if (Main.rand.NextBool(3))
                    {
                        Dust.NewDustPerfect(
                            Projectile.Center,
                            DustID.GoldFlame,
                            Projectile.velocity * 0.5f + Main.rand.NextVector2Circular(2f, 2f),
                            180,
                            default,
                            1.5f
                        ).noGravity = true;
                    }
                    break;
            }
        }

        private Player FindNearestPlayer()
        {
            Player nearestPlayer = null;
            float minDistance = float.MaxValue;

            foreach (Player player in Main.player)
            {
                if (player.active && !player.dead)
                {
                    float distance = Vector2.DistanceSquared(player.Center, Projectile.Center);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestPlayer = player;
                    }
                }
            }

            return nearestPlayer;
        }
    }
}