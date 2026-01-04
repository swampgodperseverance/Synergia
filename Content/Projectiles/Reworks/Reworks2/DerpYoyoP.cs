using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Synergia.Trails;
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class DerpYoyoP : ModProjectile
    {
        private float flightTimer;
        private Vector2 initialVelocity;
        private PrimDrawer trailDrawer;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
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

            MiscShaderData blueShader = null;
            string[] shaderKeys = { "FlameLashTrailColorGradient", "FlameLashTrailShape", "FlameLashTrailErosion" };
            foreach (var key in shaderKeys)
            {
                if (GameShaders.Misc.TryGetValue(key, out blueShader))
                    break;
            }

            if (blueShader != null)
            {
                blueShader.UseImage1("Images/Misc/noise");
                blueShader.UseOpacity(0.8f);
                blueShader.UseColor(new Color(100, 200, 255));
                blueShader.UseSecondaryColor(new Color(150, 255, 255));
            }

            trailDrawer = new PrimDrawer(
                widthFunc: (float t) => MathHelper.Lerp(2f, 0.3f, t),
                colorFunc: (float t) =>
                {
                    Color start = new Color(100, 200, 255);
                    Color end = new Color(150, 255, 255);
                    Color c = Color.Lerp(start, end, t);
                    c *= (1f - t);
                    return c;
                },
                shader: blueShader
            );
        }

        public override void AI()
        {
            flightTimer++;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Lighting.AddLight(Projectile.Center, 0.3f, 0.7f, 1f);


            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(
                    Projectile.Center - new Vector2(4f),
                    8, 8,
                    DustID.IceTorch,
                    -Projectile.velocity.X * 0.1f,
                    -Projectile.velocity.Y * 0.1f,
                    100,
                    default,
                    1.1f
                );
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.2f;
            }


            if (flightTimer <= 60)
            {

            }
            else
            {
                NPC target = FindClosestNPC(Projectile.Center, 500f);
                if (target != null)
                {

                    Vector2 toTarget = target.Center - Projectile.Center;
                    toTarget.Normalize();
                    Projectile.velocity = toTarget * initialVelocity.Length();
                }
                else
                {

                    Vector2 toPlayer = Main.player[Projectile.owner].Center - Projectile.Center;
                    toPlayer.Normalize();
                    Projectile.velocity = toPlayer * initialVelocity.Length();
                }
            }
        }

        private NPC FindClosestNPC(Vector2 position, float maxDistance)
        {
            NPC closest = null;
            float closestDist = maxDistance;
            foreach (var npc in Main.npc)
            {
                if (npc.active && !npc.friendly && !npc.dontTakeDamage)
                {
                    float dist = Vector2.Distance(position, npc.Center);
                    if (dist < closestDist)
                    {
                        closest = npc;
                        closestDist = dist;
                    }
                }
            }
            return closest;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Ice,
                    Main.rand.NextFloat(-2f, 2f),
                    Main.rand.NextFloat(-2f, 2f),
                    150,
                    default,
                    1.2f
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
