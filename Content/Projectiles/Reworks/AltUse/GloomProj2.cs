using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Helpers;
using Synergia.Trails;
using Terraria.Graphics.Shaders;
using Avalon.Particles;

namespace Synergia.Content.Projectiles.Reworks.AltUse
{
    public class GloomProj2 : ModProjectile
    {
        private float flightTimer;
        private Vector2 initialVelocity;
        private PrimDrawer trailDrawer;
        private const float homingStrength = 0.1f; // скорость самонаведения

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16; // длиннее хвост
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

            // Голубой градиент для шлейфа
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
                blueShader.UseColor(new Color(80, 180, 255));
                blueShader.UseSecondaryColor(new Color(150, 220, 255));
            }

            trailDrawer = new PrimDrawer(
                widthFunc: t => MathHelper.Lerp(2f, 0.2f, t), // тонкий
                colorFunc: t =>
                {
                    Color start = new Color(80, 180, 255);
                    Color end = new Color(150, 220, 255);
                    Color c = Color.Lerp(start, end, t);
                    c *= (1f - t * 1.2f);
                    return c;
                },
                shader: blueShader
            );
        }

        public override void AI()
        {
            flightTimer++;

            // Самонаводка на ближайшую NPC
            NPC target = null;
            float closestDist = 1000f;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.CanBeChasedBy())
                {
                    float dist = Vector2.Distance(npc.Center, Projectile.Center);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        target = npc;
                    }
                }
            }

            if (target != null)
            {
                Vector2 toTarget = target.Center - Projectile.Center;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, toTarget.SafeNormalize(Vector2.Zero) * initialVelocity.Length(), homingStrength);
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            // Голубое свечение
            Lighting.AddLight(Projectile.Center, 0.3f, 0.8f, 1.0f);

            // Голубые частицы
            if (Main.rand.NextBool(3))
            {
                Dust d = Dust.NewDustDirect(
                    Projectile.Center - new Vector2(4f),
                    8, 8,
                    DustID.IceTorch,
                    -Projectile.velocity.X * 0.2f,
                    -Projectile.velocity.Y * 0.2f,
                    100,
                    default,
                    1.1f
                );
                d.noGravity = true;
                d.velocity *= 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Голубой эффект при попадании
            for (int i = 0; i < 15; i++)
            {
                Dust d = Dust.NewDustDirect(
                    target.position,
                    target.width,
                    target.height,
                    DustID.IceTorch,
                    Main.rand.NextFloat(-3f, 3f),
                    Main.rand.NextFloat(-3f, 3f),
                    150,
                    default,
                    1.3f
                );
                d.noGravity = true;
            }
            SoundEngine.PlaySound(SoundID.Item27, target.position);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (int i = 0; i < 12; i++)
            {
                Dust d = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.IceTorch,
                    Main.rand.NextFloat(-2f, 2f),
                    Main.rand.NextFloat(-2f, 2f),
                    150,
                    default,
                    1.4f
                );
                d.noGravity = true;
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
                    trailDrawer.DrawPrims(points, offset, totalTrailPoints: 30); // длиннее
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
