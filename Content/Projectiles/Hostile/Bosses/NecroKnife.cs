using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;
using Synergia.Helpers; // Для использования EaseFunctions

namespace Synergia.Content.Projectiles.Hostile.Bosses
{
    public class NecroKnife : ModProjectile
    {
        private float appearProgress = 0f;
        private bool hasAppeared = false;
        private const int appearDuration = 20; // Плавное появление в течение 20 тиков
        private const int fadeOutDuration = 30;
        private int timer = 0;

        public static readonly SoundStyle NecroSword = new("Synergia/Assets/Sounds/NecroSword"); // Звук появления

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 64;
            Projectile.width = 22;
            Projectile.height = 48;
            Projectile.aiStyle = 0;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
        }

        public override void AI()
        {
            timer++;

            if (!hasAppeared)
            {
                appearProgress = MathHelper.Clamp(timer / (float)appearDuration, 0f, 1f);
                Projectile.alpha = (int)(255 * (1f - appearProgress));

                // Эффект появления с пыльцой
                if (Main.rand.NextBool(5))
                {
                    int dust = Dust.NewDust(Projectile.Center - new Vector2(8), 16, 16,
                        DustID.PurpleTorch, Main.rand.NextFloat(-0.4f, 0.4f), Main.rand.NextFloat(-0.6f, 0.6f),
                        150, new Color(200, 100, 255), 1.3f);
                    Main.dust[dust].velocity *= 0.6f;
                    Main.dust[dust].noGravity = true;
                }

                // Когда полностью появляется, начинаем взлёт
                if (appearProgress >= 1f)
                {
                    hasAppeared = true;
                    SoundEngine.PlaySound(NecroSword, Projectile.Center); // Новый звук
                }
            }
            else
            {
                // 🚀 Применяем более резкое ускорение с EaseOutQuint
                float t = (timer / (float)(Projectile.timeLeft - fadeOutDuration)); // Прогресс от начала до фейда
                float easedT = EaseFunctions.EaseOutQuint(t); // Применяем более агрессивное ускорение
                Projectile.velocity.Y = -22f * easedT; // Применяем ускорение вверх

                Projectile.rotation = -MathHelper.PiOver2; // Поворачиваем нож

                Lighting.AddLight(Projectile.Center, new Vector3(0.6f, 0.2f, 0.8f));

                if (Main.rand.NextBool(4))
                {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                        DustID.PurpleTorch, 0f, -1.4f, 150, new Color(220, 120, 255), 1.2f);
                    Main.dust[dust].velocity *= 0.3f;
                    Main.dust[dust].noGravity = true;
                }

                // 💫 Fade-out под конец
                if (Projectile.timeLeft < fadeOutDuration)
                {
                    float fade = Projectile.timeLeft / (float)fadeOutDuration;
                    Projectile.alpha = (int)(255 * (1f - fade));
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() * 0.5f;
            Color drawColor = Projectile.GetAlpha(lightColor);

            // Трейл (следы)
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float t = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Main.EntitySpriteDraw(
                    texture,
                    Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition,
                    null,
                    new Color(220, 100, 255) * t * 0.5f,
                    -MathHelper.PiOver2,
                    origin,
                    Projectile.scale,
                    SpriteEffects.None,
                    0);
            }

            // Основная отрисовка
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                new Color(255, 180, 255),
                -MathHelper.PiOver2,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0);

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 15; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.PurpleTorch, Projectile.oldVelocity.X * 0.3f, Projectile.oldVelocity.Y * 0.3f,
                    150, new Color(220, 120, 255), 1.4f);
                Main.dust[dust].velocity *= 2.5f;
                Main.dust[dust].noGravity = true;
            }
            SoundEngine.PlaySound(SoundID.Item9 with { Pitch = 0.3f }, Projectile.Center);
        }
    }
}
