using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Trails;

namespace Synergia.Content.Projectiles.RangedProjectiles
{
    public class EnferArrow : ModProjectile
    {
        private PrimDrawer trailDrawer;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 1;
            Projectile.light = 0.7f;
        }

        public override void OnSpawn(IEntitySource source)
        {
            MiscShaderData lavaShader = null;
            string[] shaderKeys = { "FlameLashTrailColorGradient", "FlameLashTrailShape", "FlameLashTrailErosion" };
            foreach (var key in shaderKeys)
            {
                if (GameShaders.Misc.TryGetValue(key, out lavaShader))
                {
                    break;
                }
            }

            // Настройка параметров шейдера
            if (lavaShader != null)
            {
                lavaShader.UseImage1("Images/Misc/noise");
                lavaShader.UseOpacity(0.8f);
                lavaShader.UseColor(Color.OrangeRed);
                lavaShader.UseSecondaryColor(Color.Yellow);
            }

            trailDrawer = new PrimDrawer(
                widthFunc: (float t) => MathHelper.Lerp(3.3f, 0.7f, t),
                colorFunc: (float t) =>
                {
                    Color start = new Color(255, 80, 20);
                    Color mid = new Color(255, 140, 50);
                    Color end = new Color(255, 220, 120);
                    Color result = Color.Lerp(Color.Lerp(start, mid, t), end, t * 0.8f);
                    result *= (1f - t * 0.85f) * 0.9f;
                    return result;
                },
                shader: lavaShader
            );
        } 

        public override void AI()
        {
            Projectile.velocity.Y *= 0.99f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(
                    Projectile.Center - new Vector2(4f),
                    8, 8,
                    DustID.Torch,
                    -Projectile.velocity.X * 0.3f,
                    -Projectile.velocity.Y * 0.3f,
                    150,
                    default,
                    1.2f
                );
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.4f;
                Main.dust[dust].scale *= Main.rand.NextFloat(0.9f, 1.3f);
                Main.dust[dust].fadeIn = 1.1f;
            }

            Lighting.AddLight(Projectile.Center, 1.3f, 0.5f, 0.1f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 240);

            for (int i = 0; i < Main.rand.Next(1, 3); i++)
            {
                Vector2 speed = new Vector2(
                    Main.rand.NextFloat(-2f, 2f),
                    Main.rand.NextFloat(-2f, -1f)
                );
                Projectile.NewProjectile(
                    Projectile.GetSource_Death(),
                    Projectile.Center,
                    speed,
                    ModContent.ProjectileType<EnferFlames>(),
                    Projectile.damage / 3,
                    0f,
                    Projectile.owner
                );
            }
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (int i = 0; i < 15; i++)
            {
                int dust = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Torch,
                    Main.rand.NextFloat(-2f, 2f),
                    Main.rand.NextFloat(-2f, 2f),
                    150,
                    default,
                    1.5f
                );
                Main.dust[dust].noGravity = true;
            }
             for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                Vector2 speed = new Vector2(
                    Main.rand.NextFloat(-2f, 2f),
                    Main.rand.NextFloat(-2f, -1f)
                );
                Projectile.NewProjectile(
                    Projectile.GetSource_Death(),
                    Projectile.Center,
                    speed,
                    ModContent.ProjectileType<EnferFlames>(),
                    Projectile.damage / 2,
                    0f,
                    Projectile.owner
                );
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
