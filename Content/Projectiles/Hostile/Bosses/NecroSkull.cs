using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Audio;

namespace Synergia.Content.Projectiles.Hostile.Bosses
{
    public class NecroSkull : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 32;
            Projectile.aiStyle = 0;
            Main.projFrames[Projectile.type] = 13;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.damage = 0;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // 💀 Череп немного влево и над головой игрока
            Projectile.Center = new Vector2(player.Center.X - 8f, player.Center.Y - 60f);

            Projectile.frameCounter++;
            if (Projectile.frameCounter % 6 == 0)
                Projectile.frame++;

            if (Projectile.frame >= 13)
            {
                // 💥 При исчезновении спавним 5 ножей под черепом (на 400 пикселей ниже)
                int count = 5;
                float spacing = 40f;
                Vector2 startPos = Projectile.Center + new Vector2(-(count - 1) * spacing / 2f, 750f);

                for (int i = 0; i < count; i++)
                {
                    Vector2 spawnPos = startPos + new Vector2(i * spacing, 0f);
                    Vector2 velocity = new Vector2(0f, -14f); // строго вверх

                    int knife = Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        spawnPos,
                        velocity,
                        ModContent.ProjectileType<NecroKnife>(),
                        40,
                        0f,
                        Projectile.owner
                    );

                    Main.projectile[knife].ai[0] = 1;
                }

                // ✨ Эффект перед исчезновением
                for (int d = 0; d < 25; d++)
                {
                    int dust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.PurpleTorch,
                        Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f),
                        150, new Color(200, 100, 255), 1.5f);
                    Main.dust[dust].noGravity = true;
                }

                SoundEngine.PlaySound(SoundID.Item8 with { Pitch = 0.2f }, Projectile.Center);
                Projectile.Kill();
            }

            // фиолетовое свечение вокруг черепа
            Lighting.AddLight(Projectile.Center, new Vector3(0.6f, 0.2f, 0.8f));
        }
    }
}
