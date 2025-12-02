using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.Summoner;

namespace Synergia.Content.Projectiles.Armor
{
    public class ValhalliteKnight : ModProjectile
    {
        private float alpha = 1f; 
        private float shootTimer = 0f;
        private const float FadeInTime = 30f;
        private const float FadeOutTime = 30f;
        private const float Lifetime = 600f; 
        private bool fadingOut = false;

        public override void SetStaticDefaults() { }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = (int)Lifetime;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            Vector2 offset = new Vector2(0, -60f);
            Projectile.Center = Vector2.Lerp(Projectile.Center, player.Center + offset, 0.15f);

            // Поворот в сторону игрока
            Projectile.spriteDirection = player.direction;

            // Плавное появление
            if (!fadingOut && alpha > 0f)
                alpha -= 1f / FadeInTime;
            alpha = Math.Clamp(alpha, 0f, 1f);

            // Плавное исчезновение
            if (Projectile.timeLeft < FadeOutTime)
            {
                fadingOut = true;
                alpha += 1f / FadeOutTime;
                alpha = Math.Clamp(alpha, 0f, 1f);
            }

            // Парение
            Projectile.position.Y += (float)Math.Sin(Main.GameUpdateCount * 0.1f) * 0.3f;

            // Стрельба
            shootTimer++;
            if (shootTimer >= 60f)
            {
                shootTimer = 0f;
                NPC target = FindClosestEnemy(player.Center, 600f);

                if (target != null && Main.myPlayer == Projectile.owner)
                {
                    Vector2 shootDir = Vector2.Normalize(target.Center - Projectile.Center) * 12f;

                    // Создаём проектиль
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromAI(),
                        Projectile.Center,
                        shootDir,
                        ModContent.ProjectileType<NimbusFlakeSum>(),
                        Projectile.damage,
                        0f,
                        player.whoAmI
                    );

                    // Морозный звук выстрела
                    SoundEngine.PlaySound(SoundID.Item27 with { Volume = 0.7f, PitchVariance = 0.2f }, Projectile.Center);

                    // Эффект отдачи
                    Projectile.velocity -= shootDir * 0.15f; // немного отбрасывается назад

                    // Снежная частица выстрела
                    for (int i = 0; i < 3; i++)
                    {
                        Dust.NewDust(Projectile.Center, 2, 2, DustID.Frost, shootDir.X * -0.3f, shootDir.Y * -0.3f, 100, default, 1.2f);
                    }
                }
            }

            // Свет
            Lighting.AddLight(Projectile.Center, 0.4f, 0.6f, 1.2f);
        }

        private NPC FindClosestEnemy(Vector2 center, float maxDistance)
        {
            NPC closest = null;
            float sqrMax = maxDistance * maxDistance;
            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy(this))
                {
                    float sqrDist = Vector2.DistanceSquared(center, npc.Center);
                    if (sqrDist < sqrMax)
                    {
                        sqrMax = sqrDist;
                        closest = npc;
                    }
                }
            }
            return closest;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Color drawColor = Color.White * (1f - alpha);
            drawColor.A = 0;

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                drawColor,
                0f,
                texture.Size() / 2f,
                Projectile.scale,
                effects,
                0
            );

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            // Морозные частицы при исчезновении
            for (int i = 0; i < 15; i++)
            {
                Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Frost,
                    Main.rand.NextFloat(-2f, 2f),
                    Main.rand.NextFloat(-2f, 2f),
                    150,
                    default,
                    1.4f
                );
            }

            // Морозный звук при смерти
            SoundEngine.PlaySound(SoundID.Item30 with { Volume = 0.6f, Pitch = -0.2f }, Projectile.Center);
        }
    }
}
