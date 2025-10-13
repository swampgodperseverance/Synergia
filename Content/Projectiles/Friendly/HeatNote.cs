using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Friendly;

namespace Synergia.Content.Projectiles.Friendly
{
    public class HeatNote : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120; // 2 секунды (60 = 1с)
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {

            // Пульсация (масштаб туда-сюда)
            Projectile.scale = 1f + 0.1f * (float)System.Math.Sin(Main.GameUpdateCount / 10f);

            // Свет
            Lighting.AddLight(Projectile.Center, 1.2f, 0.4f, 0.1f);

            // Пыль вокруг
            if (Main.rand.NextBool(10))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch,
                    Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f), 150, default, 1.2f);
            }

            // Когда время почти закончилось → взрыв
            if (Projectile.timeLeft == 1)
            {
                Explode();
            }
        }

        private void Explode()
        {
            SoundEngine.PlaySound(SoundID.Item14 with { Volume = 1.2f, Pitch = -0.2f }, Projectile.Center);

            // Взрывная пыль
            for (int i = 0; i < 40; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(4f, 4f);
                Dust.NewDustPerfect(Projectile.Center, DustID.Firework_Yellow, speed * Main.rand.NextFloat(0.5f, 1.5f), 150, default, 1.5f).noGravity = true;
            }

            if (Main.myPlayer == Projectile.owner)
            {
                int count = Main.rand.Next(7, 10); // 2–4 LyreProj2
                for (int i = 0; i < count; i++)
                {
                    Vector2 velocity = Main.rand.NextVector2Circular(3f, 3f);
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        velocity,
                        ModContent.ProjectileType<LyreProj2>(),
                        Projectile.damage,
                        Projectile.knockBack,
                        Projectile.owner
                    );
                }
            }
        }
    }
}
