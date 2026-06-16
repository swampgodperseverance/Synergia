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
using Synergia.Helpers;

namespace Synergia.Content.Projectiles.Boss.OcramBuff
{
    public class OcramKnife : ModProjectile {
        private PrimDrawer trailDrawer;
        private Vector2 dashDirection;
        private float spawnTimer = 0f;
        private bool hasDashed = false;
        private Vector2 startPos;
        private Vector2 targetPos;
        private float movementProgress;
        private float startRotation;
        private float dashTimer = 0f;
        public override string Texture => "Synergia/Content/Projectiles/Boss/OcramBuff/OcramKnife";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 240;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Default;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.scale = 0.1f;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            if (player != null && player.active)
            {
                Vector2 offset = Main.rand.NextVector2Unit() * Main.rand.NextFloat(100f, 170f);
                Projectile.Center = player.Center + offset;
                targetPos = player.Center;
                startPos = Projectile.Center;
                startRotation = (player.Center - Projectile.Center).ToRotation() + MathHelper.PiOver2;
                Projectile.rotation = startRotation;
            }

            movementProgress = 0f;

            MiscShaderData purpleShader = null;
            string[] shaderKeys = { "FlameLashTrailColorGradient", "FlameLashTrailShape", "FlameLashTrailErosion" };
            foreach (var key in shaderKeys)
            {
                if (GameShaders.Misc.TryGetValue(key, out purpleShader))
                    break;
            }

            if (purpleShader != null)
            {
                purpleShader.UseImage1("Images/Misc/noise");
                purpleShader.UseOpacity(0.82f);
                purpleShader.UseColor(new Color(155, 55, 235));
                purpleShader.UseSecondaryColor(new Color(235, 130, 255));
            }

            trailDrawer = new PrimDrawer(
                widthFunc: (t) => MathHelper.Lerp(8.5f, 1.6f, t),
                colorFunc: (t) =>
                {
                    Color start = new Color(175, 65, 255);
                    Color end = new Color(255, 175, 255);
                    Color c = Color.Lerp(start, end, t);
                    c *= (1f - t * 0.8f);
                    return c;
                },
                shader: purpleShader
            );
        }

        public override void AI()
        {
            spawnTimer++;
            Player player = Main.player[Projectile.owner];

            if (player == null || !player.active)
            {
                Projectile.Kill();
                return;
            }

            if (!hasDashed)
            {
                if (spawnTimer < 38)
                {
                    Projectile.scale = MathHelper.Lerp(0.1f, 1f, spawnTimer / 38f);
                    Projectile.alpha = (int)MathHelper.Lerp(255f, 0f, spawnTimer / 38f);
                    Projectile.velocity *= 0.85f;
                    Projectile.rotation = startRotation;
                }
                else if (spawnTimer >= 38 && spawnTimer < 55)
                {
                    Projectile.velocity *= 0.92f;
                    Projectile.rotation = startRotation;
                }
                else
                {
                    hasDashed = true;
                    dashDirection = player.Center - Projectile.Center;
                    if (dashDirection != Vector2.Zero)
                    {
                        dashDirection.Normalize();
                        dashDirection = dashDirection.RotatedByRandom(0.45f);
                    }
                    movementProgress = 0f;
                    startPos = Projectile.Center;
                    targetPos = player.Center + dashDirection * 200f;
                    dashTimer = 0f;
                }
            }
            else
            {
                dashTimer++;
                movementProgress = Math.Min(1f, movementProgress + 0.025f);
                float easedProgress = EaseFunctions.EaseInOutCubic(movementProgress);
                Projectile.Center = Vector2.Lerp(startPos, targetPos, easedProgress);

                if (movementProgress < 0.95f)
                {
                    Vector2 direction = (targetPos - startPos).SafeNormalize(Vector2.Zero);
                    Projectile.rotation = direction.ToRotation() + MathHelper.PiOver2;
                }

                if (movementProgress >= 1f)
                {
                    Projectile.velocity = dashDirection * 21f;
                }

                if (dashTimer >= 20)
                {
                    Projectile.Kill();
                }
            }

            if (hasDashed && movementProgress < 0.95f)
            {
                Projectile.velocity = Vector2.Zero;
            }

            Lighting.AddLight(Projectile.Center, 0.75f, 0.3f, 1.05f);

            if (spawnTimer > 225)
                Projectile.Kill();
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);

            for (int i = 0; i < 42; i++)
            {
                Vector2 speed = Main.rand.NextVector2Unit() * Main.rand.NextFloat(3.2f, 7f);
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch,
                    speed.X, speed.Y, 100, new Color(190, 85, 255), 1.6f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1.45f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D projTex = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D glowTex = ModContent.Request<Texture2D>("Synergia/Content/Projectiles/Boss/OcramBuff/OcramKnife_Glow").Value;

            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            if (trailDrawer != null)
            {
                List<Vector2> points = Projectile.oldPos
                    .Where(v => v != Vector2.Zero)
                    .Select(v => v + Projectile.Size / 2f)
                    .ToList();

                if (points.Count > 1)
                {
                    Vector2 offset = -Main.screenPosition;
                    trailDrawer.DrawPrims(points, offset, totalTrailPoints: 24);
                }
            }

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            float pulse = 1f + MathF.Sin(spawnTimer * 0.32f) * 0.2f;

            for (int i = 0; i < 5; i++)
            {
                float angleOffset = i * MathHelper.TwoPi / 5f + spawnTimer * 0.1f;
                Vector2 offset = angleOffset.ToRotationVector2() * (4.5f + pulse * 3f);
                float scaleMod = 1.4f + i * 0.07f;

                Color glowColor = new Color(150, 60, 240) * (0.8f - i * 0.12f);
                sb.Draw(glowTex, drawPos + offset, null, glowColor, Projectile.rotation, glowTex.Size() / 2f,
                    Projectile.scale * scaleMod * pulse, SpriteEffects.None, 0f);
            }

            sb.Draw(glowTex, drawPos, null, new Color(220, 130, 255) * 0.98f, Projectile.rotation,
                glowTex.Size() / 2f, Projectile.scale * 1.62f * pulse, SpriteEffects.None, 0f);

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            sb.Draw(projTex, drawPos, null, Color.White * (1f - Projectile.alpha / 255f), Projectile.rotation,
                projTex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);

            return false;
        }
    }
}