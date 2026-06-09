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
    public class ObsidianSealRework : ModProjectile
    {
        private PrimDrawer trailDrawer;
        private Vector2 targetPosition;
        private bool hasReachedTarget = false;
        private float deathTimer = 0f;
        private float spawnProgress = 0f;
        private bool initialized = false;
        private const float SPAWN_DURATION = 15f;

        public override string Texture
        {
            get
            {
                return "ValhallaMod/Projectiles/Magic/Tomes/ObsidianSkull";
            }
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.alpha = 255;
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
                widthFunc: (t) => MathHelper.Lerp(4f, 0f, t),
                colorFunc: (t) =>
                {
                    Color start = new Color(255, 220, 0);
                    Color end = new Color(255, 180, 50);
                    Color c = Color.Lerp(start, end, t);
                    c *= (1f - t * 0.7f);
                    return c;
                },
                shader: yellowShader
            );
        }

        public override void AI()
        {
            if (!initialized)
            {
                initialized = true;

                targetPosition = Main.MouseWorld;

                Vector2 spawnPosition = new Vector2(
                    targetPosition.X + Main.rand.Next(-200, 201),
                    Main.screenPosition.Y - 100
                );

                Projectile.Center = spawnPosition;
                Projectile.rotation = MathF.Atan2(targetPosition.Y - Projectile.Center.Y,
                                                  targetPosition.X - Projectile.Center.X);

                Vector2 direction = targetPosition - Projectile.Center;
                float distance = direction.Length();
                float speed = MathHelper.Clamp(distance / 30f, 8f, 20f);
                Projectile.velocity = direction.SafeNormalize(Vector2.Zero) * speed;
            }

            if (spawnProgress < 1f)
            {
                spawnProgress += 1f / SPAWN_DURATION;
                Projectile.alpha = (int)(255 * (1f - spawnProgress));
                Projectile.scale = MathHelper.Lerp(0.5f, 1f, spawnProgress);
            }

            if (!hasReachedTarget)
            {
                Vector2 toTarget = targetPosition - Projectile.Center;

                if (toTarget.Length() < 15f ||
                    Vector2.Dot(Projectile.velocity, toTarget) < 0)
                {
                    hasReachedTarget = true;
                    deathTimer = 0f;
                    Projectile.velocity = Vector2.Zero;

                    for (int i = 0; i < 8; i++)
                    {
                        Dust.NewDust(Projectile.Center, 10, 10,
                            DustID.GoldFlame,
                            Main.rand.NextFloat(-3f, 3f),
                            Main.rand.NextFloat(-3f, 3f),
                            100, default, 1.2f);
                    }

                    SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
                }
                else
                {
                    Vector2 newDirection = toTarget.SafeNormalize(Vector2.Zero);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity,
                        newDirection * Projectile.velocity.Length(), 0.1f);

                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                }
            }
            else
            {
                deathTimer += 2f;

                if (deathTimer % 6 == 0 && deathTimer < 20)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Dust dust = Dust.NewDustDirect(Projectile.Center, 10, 10,
                            DustID.GoldFlame,
                            Main.rand.NextFloat(-2f, 2f),
                            Main.rand.NextFloat(-2f, 2f),
                            100, default, 0.8f);
                        dust.noGravity = true;
                    }
                }

                Projectile.scale = MathHelper.Lerp(1f, 0f, deathTimer / 20f);
                Projectile.alpha = (int)(255 * (deathTimer / 20f));

                if (deathTimer >= 20)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        Dust dust = Dust.NewDustDirect(Projectile.Center, 10, 10,
                            DustID.GoldFlame,
                            Main.rand.NextFloat(-4f, 4f),
                            Main.rand.NextFloat(-4f, 4f),
                            100, default, 1.2f);
                        dust.noGravity = true;
                        dust.velocity *= 1.2f;
                    }

                    Projectile.Kill();
                }
            }

            Lighting.AddLight(Projectile.Center, 0.9f, 0.7f, 0.2f);

            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldFlame, 0f, 0f, 150, default, 1.2f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.5f;
            }

            if (!hasReachedTarget)
            {
                Projectile.oldPos[Projectile.oldPos.Length - 1] = Projectile.Center;
                for (int i = Projectile.oldPos.Length - 1; i > 0; i--)
                {
                    Projectile.oldPos[i] = Projectile.oldPos[i - 1];
                }
                Projectile.oldRot[Projectile.oldRot.Length - 1] = Projectile.rotation;
                for (int i = Projectile.oldRot.Length - 1; i > 0; i--)
                {
                    Projectile.oldRot[i] = Projectile.oldRot[i - 1];
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (trailDrawer != null && !hasReachedTarget)
            {
                List<Vector2> points = Projectile.oldPos
                    .Where(v => v != Vector2.Zero)
                    .Select(v => v + Projectile.Size / 2f)
                    .ToList();

                if (points.Count > 1)
                {
                    Vector2 offset = -Main.screenPosition;
                    trailDrawer.DrawPrims(points, offset, totalTrailPoints: 30);
                }
            }

            Texture2D mainTexture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Vector2 mainOrigin = new Vector2(mainTexture.Width * 0.5f, mainTexture.Height * 0.5f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            for (int i = 0; i < 8; i++)
            {
                Vector2 offset = new Vector2(
                    (float)Math.Cos(i * MathHelper.PiOver4) * 2f,
                    (float)Math.Sin(i * MathHelper.PiOver4) * 2f
                );

                Main.EntitySpriteDraw(
                    mainTexture,
                    drawPos + offset,
                    null,
                    new Color(255, 220, 0, 60),
                    Projectile.rotation,
                    mainOrigin,
                    Projectile.scale * 1.05f,
                    SpriteEffects.None,
                    0
                );
            }

            float glowIntensity = hasReachedTarget ?
                MathHelper.Lerp(1f, 0f, deathTimer / 20f) :
                (1f + (float)Math.Sin(Main.GameUpdateCount * 0.3f)) * 0.5f;

            Color glowColor = new Color(255, 200, 0, 80) * glowIntensity;

            for (int i = 0; i < 4; i++)
            {
                Vector2 offset = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
                Main.EntitySpriteDraw(
                    mainTexture,
                    drawPos + offset,
                    null,
                    glowColor * 0.4f,
                    Projectile.rotation,
                    mainOrigin,
                    Projectile.scale * 1.15f,
                    SpriteEffects.None,
                    0
                );
            }

            Color drawColor = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(
                mainTexture,
                drawPos,
                null,
                drawColor,
                Projectile.rotation,
                mainOrigin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (hasReachedTarget) return false;

            float point = 0f;
            Vector2 start = Projectile.Center - Projectile.velocity * 2;
            Vector2 end = Projectile.Center;

            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(),
                targetHitbox.Size(),
                start,
                end,
                Projectile.width,
                ref point
            );
        }
    }
}