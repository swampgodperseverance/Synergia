using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Synergia.Trails;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks
{
    public class BatbowProjectile : ModProjectile
    {
        private float flightTimer;
        private Vector2 initialVelocity;
        private PrimDrawer trailDrawer;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8; // короче хвост
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
        }

        public override void OnSpawn(IEntitySource source)
        {
            initialVelocity = Projectile.velocity;

            // ——— розовый градиентный шейдер ———
            MiscShaderData pinkShader = null;
            string[] shaderKeys = { "FlameLashTrailColorGradient", "FlameLashTrailShape", "FlameLashTrailErosion" };
            foreach (var key in shaderKeys)
            {
                if (GameShaders.Misc.TryGetValue(key, out pinkShader))
                    break;
            }

            if (pinkShader != null)
            {
                pinkShader.UseImage1("Images/Misc/noise");
                pinkShader.UseOpacity(0.8f);
                pinkShader.UseColor(new Color(255, 120, 220));
                pinkShader.UseSecondaryColor(new Color(255, 200, 255));
            }

            trailDrawer = new PrimDrawer(
                widthFunc: (float t) => MathHelper.Lerp(3.5f, 0.5f, t), // короче и тоньше
                colorFunc: (float t) =>
                {
                    Color start = new Color(255, 120, 220);
                    Color end = new Color(200, 100, 255);
                    Color c = Color.Lerp(start, end, t);
                    c *= (1f - t * 1.2f);
                    return c;
                },
                shader: pinkShader
            );
        }

        public override void AI()
        {
            flightTimer++;

            // лёгкое ускорение
            Projectile.velocity *= 1.015f;
            if (Projectile.velocity.Length() > initialVelocity.Length() * 2.5f)
                Projectile.velocity = Vector2.Normalize(Projectile.velocity) * initialVelocity.Length() * 2.5f;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            // немного свечения
            Lighting.AddLight(Projectile.Center, 1.1f, 0.3f, 0.8f);

            // розовые частицы
            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(
                    Projectile.Center - new Vector2(4f),
                    8, 8,
                    DustID.PinkTorch,
                    -Projectile.velocity.X * 0.2f,
                    -Projectile.velocity.Y * 0.2f,
                    100,
                    default,
                    1.1f
                );
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.PinkTorch,
                    Main.rand.NextFloat(-2f, 2f),
                    Main.rand.NextFloat(-2f, 2f),
                    150,
                    default,
                    1.4f
                );
                Main.dust[dust].noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (trailDrawer != null)
            {
                List<Vector2> points = Projectile.oldPos
                    .Where(v => v != Vector2.Zero)
                    .Select(v => v + Projectile.Size / 2f)
                    .ToList();

                if (points.Count > 1)
                {
                    Vector2 offset = -Main.screenPosition;
                    trailDrawer.DrawPrims(points, offset, totalTrailPoints: 20);
                }
            }

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                lightColor,
                Projectile.rotation,
                texture.Size() / 2f,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
}
