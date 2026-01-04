using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Trails;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class GreenP : ModProjectile
    {
        private PrimDrawer trailDrawer;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 350;
            Projectile.penetrate = 2;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void OnSpawn(IEntitySource source)
        {
            MiscShaderData greenShader = null;
            string[] shaderKeys = { "FlameLashTrailColorGradient", "FlameLashTrailShape", "FlameLashTrailErosion" };
            foreach (var key in shaderKeys)
            {
                if (GameShaders.Misc.TryGetValue(key, out greenShader))
                    break;
            }

            if (greenShader != null)
            {
                greenShader.UseImage1("Images/Misc/noise");
                greenShader.UseOpacity(0.9f);
                greenShader.UseColor(new Color(50, 255, 120));
                greenShader.UseSecondaryColor(new Color(120, 255, 180));
            }

            trailDrawer = new PrimDrawer(
                widthFunc: (t) => MathHelper.Lerp(2.5f, 0.3f, t),
                colorFunc: (t) =>
                {
                    Color start = new Color(60, 255, 140);
                    Color end = new Color(160, 255, 190);
                    Color c = Color.Lerp(start, end, t);
                    c *= (1f - t * 0.8f);
                    return c;
                },
                shader: greenShader
            );
        }

        public override void AI()
        {

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Lighting.AddLight(Projectile.Center, 0.1f, 0.8f, 0.3f);

            Vector2 vector = new Vector2(Projectile.ai[0], Projectile.ai[1]) - Projectile.Center;

            if (Projectile.timeLeft < 275)
                Projectile.Kill();

            if (vector.Length() < Projectile.velocity.Length())
                Projectile.Kill();
            else
            {
                vector.Normalize();
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, vector * 11.2f, 0.1f);
            }

            Projectile.netUpdate = true;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);


            for (int i = 0; i < 20; i++)
            {
                Vector2 speed = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 4f);
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GreenTorch, speed.X, speed.Y, 100, new Color(50, 255, 120), 1.2f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1.5f;
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
