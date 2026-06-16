using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Trails;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.LunaticCultistBuff
{
    public class FallingStar : ModProjectile
    {
        private PrimDrawer trailDrawer;

        public override string Texture => "Terraria/Images/Projectile_538";
        public override string GlowTexture => "Terraria/Images/Extra_91";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 180;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            MiscShaderData starShader = null;
            string[] shaderKeys = { "FlameLashTrailColorGradient", "FlameLashTrailShape", "FlameLashTrailErosion" };
            foreach (var key in shaderKeys)
            {
                if (GameShaders.Misc.TryGetValue(key, out starShader))
                    break;
            }

            if (starShader != null)
            {
                starShader.UseImage1("Images/Misc/noise");
                starShader.UseOpacity(0.8f);
                starShader.UseColor(new Color(0, 200, 255));
                starShader.UseSecondaryColor(new Color(100, 0, 255));
            }

            trailDrawer = new PrimDrawer(
                widthFunc: (t) => MathHelper.Lerp(8f, 1f, t),
                colorFunc: (t) =>
                {
                    Color start = new Color(0, 200, 255);
                    Color end = new Color(100, 0, 255);
                    Color c = Color.Lerp(start, end, t);
                    c *= (1f - t * 0.7f);
                    return c;
                },
                shader: starShader
            );
        }

        public override void AI()
        {
            Projectile.rotation += Projectile.direction * 0.1f;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.localAI[2] = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Lighting.AddLight(Projectile.Center, 0.2f, 0.5f, 0.8f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (trailDrawer != null && Projectile.oldPos.Length > 1)
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

            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Texture2D glow = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor with { A = 0 }, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, (Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally), 0);
            Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, null, Color.Cyan with { A = 0 } * 0.5f, Projectile.localAI[2], new Vector2(glow.Width) * 0.5f, Projectile.scale, SpriteEffects.None, 0);

            for (int i = 0; i < 3; i++)
                Main.EntitySpriteDraw(glow, Projectile.Center + Vector2.UnitY.RotatedBy(MathHelper.TwoPi * i / 3f - Projectile.rotation) * 4f - Main.screenPosition, null, Color.Magenta with { A = 0 } * 0.5f, Projectile.localAI[2], new Vector2(glow.Width) * 0.5f, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }
}