using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.GameContent;
using Synergia.Trails;
using System.Collections.Generic;
using System.Linq;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class WaterGunBubble2 : ModProjectile
    {
        private PrimDrawer trailDrawer;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; 
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 60;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = 0;
            Projectile.scale = 1.1f;
        }

        public override void OnSpawn(IEntitySource source)
        {
            MiscShaderData shader = null;

            // ищем доступный градиент
            string[] shaderKeys = {
                "FlameLashTrailColorGradient",
                "FlameLashTrailShape",
                "FlameLashTrailErosion"
            };

            foreach (string k in shaderKeys)
            {
                if (GameShaders.Misc.TryGetValue(k, out shader))
                    break;
            }

            if (shader != null)
            {
                shader.UseImage1("Images/Misc/noise");
                shader.UseColor(new Color(80, 180, 255));    
                shader.UseSecondaryColor(new Color(150, 230, 255));
                shader.UseOpacity(0.75f);
            }

            trailDrawer = new PrimDrawer(
                widthFunc: t => MathHelper.Lerp(3.0f, 0.4f, t),
                colorFunc: t =>
                {
                    Color start = new Color(80, 180, 255);
                    Color end = new Color(150, 230, 255);
                    Color c = Color.Lerp(start, end, t);
                    return c * (1f - t * 0.55f);
                },
                shader: shader
            );
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.position, 0.1f, 0.4f, 0.55f);

            if (Projectile.velocity.LengthSquared() > 0.1f)
                Projectile.rotation = Projectile.velocity.ToRotation();

            if (Main.rand.NextBool(5))
            {
                int d = Dust.NewDust(Projectile.position, 6, 6, DustID.Wet,
                    Projectile.velocity.X * 0.05f, Projectile.velocity.Y * 0.05f);
                Main.dust[d].noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath9, Projectile.position);

            for (int i = 0; i < 8; i++)
            {
                int d = Dust.NewDust(Projectile.position, 8, 8, DustID.Wet);
                Main.dust[d].velocity *= 1.2f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (trailDrawer != null)
            {
                List<Vector2> points = Projectile.oldPos
                    .Where(pos => pos != Vector2.Zero)
                    .Select(pos => pos + Projectile.Size / 2f)
                    .ToList();

                if (points.Count > 1)
                {
                    trailDrawer.DrawPrims(points, -Main.screenPosition, 8);
                }
            }

            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation,
                tex.Size() / 2f,
                Projectile.scale,
                SpriteEffects.None
            );

            return false;
        }
    }
}
