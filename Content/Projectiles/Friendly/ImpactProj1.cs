using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Trails;
using Synergia.Content.Dusts;

namespace Synergia.Content.Projectiles.Friendly
{
    public class CogwormProj1 : ModProjectile
    {
        private PrimDrawer trailDrawer;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8; 
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.aiStyle = 3;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 700;
            Projectile.extraUpdates = 1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            MiscShaderData lavaShader = null;

            string[] shaderKeys = { "FlameLashTrailColorGradient", "FlameLashTrailShape", "FlameLashTrailErosion" };
            foreach (var key in shaderKeys)
            {
                if (GameShaders.Misc.TryGetValue(key, out lavaShader))
                {
                    Main.NewText($"[CogwormProj5] Используется шейдер: {key}", Color.Orange);
                    break;
                }
            }

            if (lavaShader != null)
            {
                try
                {
                    lavaShader.UseImage1("Images/Misc/noise");
                    lavaShader.UseOpacity(0.7f);
                    lavaShader.UseColor(Color.OrangeRed);
                    lavaShader.UseSecondaryColor(Color.Yellow);
                }
                catch { }
            }
            else
            {

            }
            trailDrawer = new PrimDrawer(
                widthFunc: (float t) => MathHelper.Lerp(8f, 1.5f, t), 
                colorFunc: (float t) =>
                {
                    Color start = new Color(255, 90, 30);
                    Color mid = new Color(255, 160, 60);
                    Color end = new Color(255, 230, 120);
                    Color fireColor = Color.Lerp(Color.Lerp(start, mid, t), end, t * 0.8f);
                    fireColor *= (1f - t * 0.9f) * 0.6f; 
                    return fireColor;
                },
                shader: lavaShader
            );
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 240);
        }

        public override void AI()
        {
            Projectile.rotation += 0.25f * Projectile.direction;
            if (Main.rand.NextBool(5))
            {
                int d = Dust.NewDust(
                    Projectile.Center - new Vector2(4f, 4f),
                    8, 8,
                    DustID.Torch,
                    Projectile.velocity.X * -0.3f,
                    Projectile.velocity.Y * -0.3f,
                    150,
                    default,
                    1.1f
                );
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 0.4f;
                Main.dust[d].scale *= Main.rand.NextFloat(0.9f, 1.3f);
                Main.dust[d].fadeIn = 1.1f;
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
                    trailDrawer.DrawPrims(points, offset, totalTrailPoints: 40);
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
