using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile
{
    public class CarnageBone2 : ModProjectile
    {
        private enum BoneState { SlowSpin, Accelerating }
        private BoneState currentState = BoneState.SlowSpin;
        private int timer = 0;
        private const int SpinTime = 45; 
        private Player target;
        private const float MaxSpeed = 22f; 

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.damage = 18;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = SpinTime + 75;
        }

        public override void AI()
        {
            timer++;

            if (Main.rand.NextBool(3))
            {
                Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(15, 15),
                    DustID.GoldFlame,
                    Projectile.velocity * 0.6f + Main.rand.NextVector2Circular(2.5f, 2.5f),
                    200,
                    default,
                    1.7f
                ).noGravity = true;
            }

            if (target == null || !target.active || target.dead)
                target = FindNearestPlayer();

            switch (currentState)
            {
                case BoneState.SlowSpin:
                    Projectile.rotation += 0.25f * Projectile.direction; 
                    Projectile.velocity *= 0.9f; 

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
                        float acceleration = MathHelper.Lerp(0.4f, 0.8f, timer / 30f);
                        Projectile.velocity += direction * acceleration;
                    }

                    Projectile.rotation += 0.7f * Projectile.direction; 

                    if (Projectile.velocity.Length() > MaxSpeed)
                        Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * MaxSpeed;

                    if (Main.rand.NextBool(2))
                    {
                        Dust.NewDustPerfect(
                            Projectile.Center,
                            DustID.GoldFlame,
                            Projectile.velocity * 0.7f + Main.rand.NextVector2Circular(3f, 3f),
                            200,
                            default,
                            2f
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