using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Friendly
{
    public class LyreProj2 : ModProjectile
    {
        private float spinSpeed = 0.35f; // начальная скорость вращения

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Main.projFrames[Projectile.type] = 6; 
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            // Анимация (6 кадров)
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 6;
            }

            // Таймер жизни
            Projectile.ai[0]++;

            if (Projectile.ai[0] <= 60) // первые ~1 секунда
            {
                // Постепенно замедляется по скорости
                Projectile.velocity *= 0.96f;
            }
            else
            {
                // Находим цель
                NPC target = FindTarget();
                if (target != null)
                {
                    Vector2 toTarget = target.Center - Projectile.Center;
                    float speed = 10f;
                    Vector2 desiredVelocity = toTarget.SafeNormalize(Vector2.Zero) * speed;

                    // Плавное ускорение в сторону врага
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.1f);
                }
            }

            // Постепенное замедление вращения (эффект "затухания")
            if (spinSpeed > 0.05f) // чтобы не остановился полностью
                spinSpeed *= 0.98f; // коэффициент плавного замедления

            Projectile.rotation += spinSpeed * Projectile.direction;

            // Красивый след из лавовых частиц
            if (Main.rand.NextBool(3))
            {
                Dust d = Dust.NewDustDirect(
                    Projectile.position, 
                    Projectile.width, 
                    Projectile.height, 
                    DustID.Lava, // 🔥 лавовые частицы
                    Projectile.velocity.X * 0.2f, 
                    Projectile.velocity.Y * 0.2f, 
                    100, 
                    default, 
                    1.2f
                );
                d.noGravity = true;
                d.velocity *= 0.6f;
            }
        }

        private NPC FindTarget()
        {
            NPC closest = null;
            float maxDist = 500f;
            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy(this))
                {
                    float dist = Vector2.Distance(npc.Center, Projectile.Center);
                    if (dist < maxDist)
                    {
                        maxDist = dist;
                        closest = npc;
                    }
                }
            }
            return closest;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            for (int i = 0; i < 12; i++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(2.5f, 2.5f);
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center, 
                    DustID.Lava, // 🔥 при взрыве тоже лава
                    speed, 
                    120, 
                    default, 
                    1.5f
                );
                d.noGravity = true;
            }
        }
    }
}
