using Avalon.Common;
using Avalon.Common.Extensions;
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

namespace Synergia.Content.Projectiles.Thrower
{
    public class IcePieces : ModProjectile
    {
        private PrimDrawer trailDrawer;
        private int frame;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 2;
            Projectile.scale = 1.2f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            frame = Main.rand.Next(3);

            MiscShaderData iceShader = null;
            string[] keys = {
                "FlameLashTrailColorGradient",
                "FlameLashTrailShape",
                "FlameLashTrailErosion"
            };

            foreach (var k in keys)
            {
                if (GameShaders.Misc.TryGetValue(k, out iceShader))
                    break;
            }

            if (iceShader != null)
            {
                iceShader.UseImage1("Images/Misc/noise");
                iceShader.UseOpacity(0.8f);
                iceShader.UseColor(new Color(120, 200, 255));
                iceShader.UseSecondaryColor(new Color(200, 240, 255));
            }

            trailDrawer = new PrimDrawer(
                t => MathHelper.Lerp(2.4f, 0.3f, t),
                t =>
                {
                    Color c = Color.Lerp(
                        new Color(120, 210, 255),
                        new Color(200, 240, 255),
                        t
                    );
                    c *= (1f - t * 0.75f);
                    return c;
                },
                iceShader
            );
        }

        public override void AI()
        {
            Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.03f * Projectile.direction;

            Projectile.ai[0]++;
            if (Projectile.ai[0] > 45)
                Projectile.velocity.Y += 0.15f;

            Projectile.velocity *= 0.99f;

            Lighting.AddLight(Projectile.Center, 0.25f, 0.55f, 1.0f);
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
                    trailDrawer.DrawPrims(points, -Main.screenPosition, 20);
            }

            Texture2D tex = TextureAssets.Projectile[Type].Value;
            int frameHeight = tex.Height / 3;
            Rectangle source = new Rectangle(0, frame * frameHeight, tex.Width, frameHeight);

            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                source,
                lightColor,
                Projectile.rotation,
                source.Size() / 2f,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);

            for (int i = 0; i < 18; i++)
            {
                Vector2 v = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.2f, 3.8f);
                int d = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.IceTorch,
                    v.X,
                    v.Y,
                    100,
                    new Color(150, 220, 255),
                    1.3f
                );
                Main.dust[d].noGravity = true;
            }
        }
    }
}
