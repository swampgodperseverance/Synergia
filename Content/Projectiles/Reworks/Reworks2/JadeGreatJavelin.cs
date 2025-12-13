using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.Audio;
using Terraria.GameContent;
using Synergia.Trails;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class JadeGreatJavelin : ModProjectile
    {
        private PrimDrawer trailDrawer;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
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
            
            MiscShaderData jadeShader = null;
            string[] shaderKeys = { "FlameLashTrailColorGradient", "FlameLashTrailShape", "FlameLashTrailErosion" };
            foreach (var key in shaderKeys)
            {
                if (GameShaders.Misc.TryGetValue(key, out jadeShader))
                    break;
            }

            if (jadeShader != null)
            {
                jadeShader.UseImage1("Images/Misc/noise");
                jadeShader.UseOpacity(0.85f);
                jadeShader.UseColor(new Color(50, 255, 80));    
                jadeShader.UseSecondaryColor(new Color(100, 255, 140)); 
            }

            trailDrawer = new PrimDrawer(
                widthFunc: t => MathHelper.Lerp(2.8f, 0.4f, t),
                colorFunc: t =>
                {
                    Color start = new Color(50, 255, 80);
                    Color end = new Color(100, 255, 140);
                    Color c = Color.Lerp(start, end, t);
                    c *= (1f - t * 0.7f);
                    return c;
                },
                shader: jadeShader
            );

            Player player = Main.player[Projectile.owner];
            Vector2 mouse = Main.MouseWorld;

            float offsetX = Main.rand.NextFloat(-60f, 60f);

            Projectile.position = new Vector2(
                mouse.X + offsetX - Projectile.width / 2f,
                Main.screenPosition.Y - 200f 
            );

           
            Projectile.velocity = new Vector2(0, 45f);
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);

            for (int i = 0; i < 25; i++)
            {
                Vector2 speed = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.5f, 4f);
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, speed.X, speed.Y, 100, new Color(50, 255, 80), 1.4f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1.2f;
            }

            Lighting.AddLight(Projectile.Center, 0.3f, 0.7f, 0.4f);
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
