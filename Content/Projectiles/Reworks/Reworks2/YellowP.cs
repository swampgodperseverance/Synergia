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
    public class YellowP : ModProjectile
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

            MiscShaderData yellowShader = null;
            string[] shaderKeys = { "FlameLashTrailColorGradient", "FlameLashTrailShape", "FlameLashTrailErosion" };
            foreach (var key in shaderKeys)
            {
                if (GameShaders.Misc.TryGetValue(key, out yellowShader))
                    break;
            }

            if (yellowShader != null)
            {
                yellowShader.UseImage1("Images/Misc/noise");
                yellowShader.UseOpacity(0.9f);
                yellowShader.UseColor(new Color(255, 230, 50));          
                yellowShader.UseSecondaryColor(new Color(255, 255, 150)); 
            }

            trailDrawer = new PrimDrawer(
                widthFunc: (t) => MathHelper.Lerp(3f, 0.4f, t),
                colorFunc: (t) =>
                {
                    Color start = new Color(255, 220, 70);
                    Color end = new Color(255, 255, 160);
                    Color c = Color.Lerp(start, end, t);
                    c *= (1f - t * 0.8f);
                    return c;
                },
                shader: yellowShader
            );
        }

        public override void AI()
        {

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Lighting.AddLight(Projectile.Center, 0.9f, 0.85f, 0.3f);

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

    
            if (Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldFlame, 0f, 0f, 150, default, 1.3f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.6f;
            }

            Projectile.netUpdate = true;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

            for (int i = 0; i < 25; i++)
            {
                Vector2 speed = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 4.5f);
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldFlame, speed.X, speed.Y, 100, new Color(255, 230, 80), 1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1.4f;
            }
            Lighting.AddLight(Projectile.Center, 1.2f, 1.0f, 0.3f);
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
