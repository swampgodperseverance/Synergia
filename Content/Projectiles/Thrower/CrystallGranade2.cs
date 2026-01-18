using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using NewHorizons.Content.Projectiles.Ranged;

namespace Synergia.Content.Projectiles.Thrower
{
    internal class CrystallGranade2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 14;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.timeLeft = 80;
            Projectile.friendly = true;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            for (int i = 0; i < 20; i++)
            {
                int d = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    31,
                    0f,
                    0f,
                    100,
                    default,
                    1.5f
                );
                Main.dust[d].noGravity = true;
            }

            for (int i = 0; i < 4; i++)
            {
                int g = Gore.NewGore(
                    Projectile.GetSource_FromThis(),
                    Projectile.position,
                    Vector2.Zero,
                    Main.rand.Next(61, 64),
                    1f
                );
                Main.gore[g].velocity *= 0.3f;
            }

            if (Projectile.owner == Main.myPlayer)
            {
                int count = Main.rand.Next(5, 8); // 5–7
                float speed = 7f;

                for (int i = 0; i < count; i++)
                {
                    Vector2 velocity = Main.rand.NextVector2CircularEdge(1f, 1f) * speed;

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        velocity,
                        ModContent.ProjectileType<CrystalGrenadeShardProj>(),
                        (int)(Projectile.damage * 0.6f),
                        Projectile.knockBack,
                        Projectile.owner
                    );
                }
            }

            Projectile.netUpdate = true;
        }
    }
}
