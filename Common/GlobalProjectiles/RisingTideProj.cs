using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Shaders;
using Synergia.Trails;

namespace Synergia.Common.GlobalProjectiles
{
    public class RisingTideGP : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        private PrimDrawer visualTrail;
        private float waveOffset;

        public override void AI(Projectile projectile)
        {
            // Проверяем, что это именно RisingTide из ValhallaMod
            if (projectile.ModProjectile != null &&
                projectile.ModProjectile.Mod.Name == "ValhallaMod" &&
                projectile.ModProjectile.Name == "RisingTide")
            {
                waveOffset += 0.08f; // для "живого" переливания трейла

                // Инициализация трейла (один раз)
                if (visualTrail == null)
                {
                    MiscShaderData shader = null;
                    string[] keys = { "FlameLashTrailColorGradient", "FlameLashTrailShape", "FlameLashTrailErosion" };
                    foreach (var k in keys)
                        if (GameShaders.Misc.TryGetValue(k, out shader))
                            break;

                    if (shader != null)
                    {
                        shader.UseImage1("Images/Misc/noise");
                        shader.UseOpacity(0.9f);
                        shader.UseColor(new Color(0, 180, 255)); // Глубокий океан
                        shader.UseSecondaryColor(new Color(180, 250, 255)); // Морская пена
                    }

                    visualTrail = new PrimDrawer(
                        widthFunc: t =>
                        {
                            // ширина немного пульсирует, имитируя волны
                            float wave = (float)System.Math.Sin(t * 6f + waveOffset) * 0.3f;
                            return MathHelper.Lerp(8f, 1.5f, t) + wave;
                        },
                        colorFunc: t =>
                        {
                            // Цвет переливается от тёмно-бирюзового к светло-голубому и белому
                            Color deepSea = new Color(0, 100, 200);
                            Color aqua = new Color(0, 200, 255);
                            Color foam = new Color(220, 250, 255);

                            // смешиваем цвета с небольшим динамическим “дыханием”
                            float dynamic = 0.5f + 0.5f * (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 2f + t * 5f);
                            Color mix = Color.Lerp(deepSea, aqua, t);
                            mix = Color.Lerp(mix, foam, dynamic * t * 0.8f);

                            // мягкое затухание прозрачности
                            mix *= (1f - t * 0.9f);
                            return mix;
                        },
                        shader: shader
                    );

                    ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
                    ProjectileID.Sets.TrailingMode[projectile.type] = 2;
                }

                // Эффект мягкого голубого света вокруг
                Lighting.AddLight(projectile.Center, 0.1f, 0.4f, 0.6f);

                // Пузырьки и капельки
                if (Main.rand.NextBool(4))
                {
                    int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.WaterCandle,
                        -projectile.velocity.X * 0.1f, -projectile.velocity.Y * 0.1f, 120, default, 1.3f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 0.4f;
                    Main.dust[d].scale *= 0.9f;
                }

                // Иногда всплески — белая пена
                if (Main.rand.NextBool(12))
                {
                    int d = Dust.NewDust(projectile.Center, 10, 10, DustID.Smoke,
                        Main.rand.NextFloat(-1.5f, 1.5f),
                        Main.rand.NextFloat(-1.5f, 1.5f),
                        150, new Color(220, 240, 255), 1.2f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 0.6f;
                }
            }
        }

        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            // Только для RisingTide
            if (projectile.ModProjectile != null &&
                projectile.ModProjectile.Mod.Name == "ValhallaMod" &&
                projectile.ModProjectile.Name == "RisingTide" &&
                visualTrail != null)
            {
                List<Vector2> points = projectile.oldPos
                    .Where(v => v != Vector2.Zero)
                    .Select(v => v + projectile.Size / 2f)
                    .ToList();

                if (points.Count > 1)
                {
                    Vector2 offset = -Main.screenPosition;
                    visualTrail.DrawPrims(points, offset, totalTrailPoints: 18);
                }
            }

            return base.PreDraw(projectile, ref lightColor);
        }
    }
}
