using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Helpers;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using System;

namespace Synergia.Content.Projectiles.Thrower
{
    public sealed class ThunderChakramProj : ModProjectile
    {
        private static VertexStrip _vertexStrip = new VertexStrip();

        private bool hasSlowed = false;
        private float initialSpeed = 8f;
        private float slowStartTime = 60f;
        private float glowIntensity = 0f;
        private bool isGlowing = false;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 180;
            Projectile.extraUpdates = 1;
            Projectile.alpha = 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var shader = GameShaders.Misc["LightDisc"];
            float finalGlow = glowIntensity * 1.2f;

            shader.UseSaturation(-2f);
            shader.UseOpacity(0.8f + finalGlow);
            shader.Apply(null);

            _vertexStrip.PrepareStripWithProceduralPadding(
                Projectile.oldPos,
                Projectile.oldRot,
                StripColors,
                StripWidth,
                -Main.screenPosition + Projectile.Size / 2f,
                false,
                true
            );
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = texture.Size() / 2f;
            Vector2 screenPos = Projectile.Center - Main.screenPosition;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            float glowSize = 1f + glowIntensity * 1.2f;

            for (int i = 0; i < 6; i++)
            {
                float rot = i * MathHelper.PiOver2 + Main.GlobalTimeWrappedHourly * 2f;
                Vector2 offset = new Vector2(2.5f, 0f).RotatedBy(rot);
                Color glowColor = new Color(255, 215, 0, 80) * (0.4f + glowIntensity * 1.2f);

                Main.EntitySpriteDraw(
                    texture,
                    screenPos + offset,
                    null,
                    glowColor,
                    Projectile.rotation,
                    origin,
                    Projectile.scale * glowSize,
                    SpriteEffects.None
                );
            }

            Main.EntitySpriteDraw(
                texture,
                screenPos,
                null,
                Color.White,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None
            );

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        private Color StripColors(float progress)
        {
            float inv = 1f - progress;
            Color startColor = new Color(255, 200, 50);
            Color endColor = new Color(255, 140, 30);
            Color color = Color.Lerp(startColor, endColor, progress);
            return color * (inv * 1.1f);
        }

        private float StripWidth(float progress)
        {
            return 10f * (1f - progress);
        }

        public override void AI()
        {
            Projectile.rotation += 0.2f * Projectile.direction;

            Lighting.AddLight(Projectile.Center, 1f, 0.7f, 0.2f);

            if (!hasSlowed && Projectile.timeLeft < slowStartTime)
            {
                hasSlowed = true;
                initialSpeed = Projectile.velocity.Length();
            }

            if (hasSlowed)
            {
                float slowProgress = 1f - (Projectile.timeLeft / (float)slowStartTime);
                float eased = EaseFunctions.EaseOutQuad(slowProgress);
                float currentSpeed = initialSpeed * (1f - eased * 0.9f);
                if (currentSpeed < 1f) currentSpeed = 1f;

                Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * currentSpeed;

                if (slowProgress > 0.8f && !isGlowing)
                {
                    isGlowing = true;
                }

                if (isGlowing)
                {
                    glowIntensity += 0.05f;
                    if (glowIntensity > 1f) glowIntensity = 1f;
                }
            }

            Projectile.spriteDirection = Projectile.direction;

            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldFlame, 0f, 0f, 100, default, 1f);
                Main.dust[dust].velocity *= 0.2f;
                Main.dust[dust].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.velocity *= 0.85f;

            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldFlame, Main.rand.NextFloat(-1.5f, 1.5f), Main.rand.NextFloat(-1.5f, 1.5f), 100, default, 1f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 25; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldFlame, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f), 100, default, 1.5f);
            }

            Vector2[] directions = new Vector2[]
            {
                new Vector2(-1, 0),
                new Vector2(1, 0),
                new Vector2(0, -1),
                new Vector2(0, 1)
            };

            for (int i = 0; i < 4; i++)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    directions[i] * 7f,
                    ModContent.ProjectileType<ThunderSpike2>(),
                    Projectile.damage / 2,
                    Projectile.knockBack * 0.8f,
                    Projectile.owner
                );
            }
        }
    }
}