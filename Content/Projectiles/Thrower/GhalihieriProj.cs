using System;
using Microsoft.Xna.Framework;
using Synergia.Helpers;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Thrower
{
    public class GhalihieriProj : ModProjectile
    {
        private static VertexStrip _vertexStrip = new VertexStrip();

        private const int HEAT_UP_TIME = 70;
        private const int TURN_START_TIME = 60;
        private const int SLOW_DOWN_DURATION = 35;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            int age = 600 - Projectile.timeLeft;

            if (Projectile.velocity != Vector2.Zero && age < TURN_START_TIME - 10)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            }

            if (age >= TURN_START_TIME && age < TURN_START_TIME + SLOW_DOWN_DURATION)
            {
                float progress = (age - TURN_START_TIME) / (float)SLOW_DOWN_DURATION;
                float eased = EaseFunctions.EaseOutQuint(progress);

                Projectile.velocity *= 0.85f - eased * 0.65f;

                float targetRotation = MathHelper.ToRadians(90f) + MathHelper.ToRadians(45f);
                Projectile.rotation = MathHelper.Lerp(Projectile.rotation, targetRotation, eased * 0.9f);

                if (progress > 0.6f)
                {
                    Projectile.velocity.Y += 0.4f * (progress - 0.6f) / 0.4f;
                }

                if (Main.rand.NextBool(3))
                {
                    Dust d = Dust.NewDustPerfect(
                        Projectile.Center + Main.rand.NextVector2Circular(14f, 14f),
                        DustID.Torch,
                        Vector2.Zero,
                        100,
                        default,
                        1.4f + progress * 0.8f
                    );
                    d.noGravity = true;
                    d.fadeIn = 1.1f;
                }
            }
            else if (age >= TURN_START_TIME + SLOW_DOWN_DURATION)
            {
                Projectile.velocity.X *= 0.94f;
                Projectile.velocity.Y += 1.1f;
                Projectile.velocity.Y = MathHelper.Clamp(Projectile.velocity.Y, -14f, 28f);

                Projectile.velocity.X += (float)Math.Sin(age * 0.35f) * 0.08f;

                if (Main.rand.NextBool(2))
                {
                    Dust d = Dust.NewDustPerfect(
                        Projectile.Center,
                        DustID.Torch,
                        Projectile.velocity * Main.rand.NextFloat(0.25f, 0.55f),
                        70,
                        default,
                        Main.rand.NextFloat(1.3f, 2.0f)
                    );
                    d.noGravity = true;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawTrail();
            return true;
        }

        private void DrawTrail()
        {
            var shader = GameShaders.Misc["LightDisc"];
            float pulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 4f) * 0.3f + 0.7f;

            shader.UseSaturation(-2.5f);
            shader.UseOpacity(1.6f * pulse);
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
        }

        private Color StripColors(float progress)
        {
            float inv = 1f - progress;
            int age = 600 - Projectile.timeLeft;
            float heatProgress = MathHelper.Clamp(age / (float)HEAT_UP_TIME, 0f, 1f);
            heatProgress = EaseFunctions.EaseOutCubic(heatProgress);

            Color cold = new Color(35, 12, 3);
            Color hot = new Color(255, 145, 15);

            Color baseColor = Color.Lerp(cold, hot, heatProgress);
            float pulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 6f + progress * 7f) * 0.5f + 0.5f;
            Color glow = Color.Lerp(baseColor, hot, pulse * 0.65f);

            Color result = glow * (inv * inv * 1.35f);
            result.A = 0;
            return result;
        }

        private float StripWidth(float progress)
        {
            float pulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 5f + progress * 12f) * 0.25f + 1f;
            return 13f * (1f - progress) * pulse;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.Kill();
            HitEffect(Projectile.Center);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.Kill();
            HitEffect(Projectile.Center);
        }

        private void HitEffect(Vector2 position)
        {
            for (int i = 0; i < 18; i++)
            {
                Vector2 vel = Main.rand.NextVector2Circular(6f, 6f);
                Dust d = Dust.NewDustPerfect(position, DustID.Torch, vel, 60, default, 1.6f + Main.rand.NextFloat(0.8f));
                d.noGravity = true;
            }

            for (int i = 0; i < 9; i++)
            {
                Vector2 vel = Main.rand.NextVector2Circular(4.5f, 4.5f);
                Dust.NewDustPerfect(position, DustID.Smoke, vel * 0.7f, 120, default, 1.2f);
            }

            for (int i = 0; i < 6; i++)
            {
                Gore.NewGore(Projectile.GetSource_Death(), position, Main.rand.NextVector2Circular(3f, 3f), 61 + Main.rand.Next(3));
            }
        }
    }

    public class Ghalihieri2 : ModProjectile
    {
        private static VertexStrip _vertexStrip = new VertexStrip();

        private const int HEAT_UP_TIME = 70;
        private const int MAX_LIFE = 110;

        private bool hasDoneLoop;
        private int loopTimer;
        private const int LOOP_DURATION = 24;       
        private float loopAngularSpeed;                 

        private bool accelerating;
        private int accelTimer;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = 1;
            Projectile.timeLeft = MAX_LIFE;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            int age = MAX_LIFE - Projectile.timeLeft;

            if (age < 6)
            {
                Projectile.velocity *= 0.90f;
                if (age == 5)
                {
                    loopAngularSpeed = MathHelper.ToRadians(360f / LOOP_DURATION);
                }
            }
            if (!hasDoneLoop && age >= 10 && age < 10 + LOOP_DURATION)
            {
                loopTimer = age - 10;
                Projectile.velocity = Projectile.velocity.RotatedBy(loopAngularSpeed);
                if (loopTimer > LOOP_DURATION * 0.3f && loopTimer < LOOP_DURATION * 0.7f)
                {
                    Projectile.velocity *= 0.985f;
                }
            }
            if (!hasDoneLoop && age >= 10 + LOOP_DURATION)
            {
                hasDoneLoop = true;
                accelerating = true;
                accelTimer = 0;
            }

       
            if (accelerating)
            {
                accelTimer++;

                if (accelTimer <= 20)
                {
                    float progress = accelTimer / 20f;
                    float eased = EaseFunctions.EaseOutQuad(progress);

                    Vector2 forward = Projectile.velocity.SafeNormalize(Vector2.Zero);
                    float addedSpeed = 4.5f + eased * 9f; 

                    Projectile.velocity += forward * addedSpeed * 0.12f;
                }
                else
                {
                    accelerating = false;
                }
                Projectile.velocity *= 0.995f;
            }
            if (Projectile.velocity.LengthSquared() > 0.04f)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            }
            if (Main.rand.NextBool(4))
            {
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(8f, 8f),
                    DustID.Torch,
                    Projectile.velocity * 0.3f + Main.rand.NextVector2Circular(0.8f, 0.8f),
                    90,
                    default,
                    1.1f + age / 80f
                );
                d.noGravity = true;
                d.fadeIn = 0.9f;
            }

            Lighting.AddLight(Projectile.Center, 0.9f, 0.4f, 0.08f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var shader = GameShaders.Misc["LightDisc"];
            float pulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 5.2f) * 0.25f + 0.75f;
            shader.UseSaturation(-2.8f);
            shader.UseOpacity(1.4f * pulse);
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
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;
            Vector2 screenPos = Projectile.Center - Main.screenPosition;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < 4; i++)
            {
                float rot = i * MathHelper.PiOver2;
                Vector2 offset = new Vector2(2.2f, 0f).RotatedBy(rot + Main.GlobalTimeWrappedHourly * 2.4f);
                Color glowColor = new Color(255, 180, 60, 140) * (0.65f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 9f) * 0.35f);

                Main.EntitySpriteDraw(
                    texture,
                    screenPos + offset,
                    null,
                    glowColor,
                    Projectile.rotation,
                    origin,
                    Projectile.scale * 1.14f,
                    SpriteEffects.None
                );
            }

            Main.EntitySpriteDraw(
                texture,
                screenPos,
                null,
                Color.White * 0.88f,
                Projectile.rotation,
                origin,
                Projectile.scale * 1.07f,
                SpriteEffects.None
            );

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        private Color StripColors(float progress)
        {
            float inv = 1f - progress;
            int age = MAX_LIFE - Projectile.timeLeft;
            float heat = MathHelper.Clamp(age / (float)HEAT_UP_TIME, 0f, 1f);
            heat = EaseFunctions.EaseOutCubic(heat);

            Color cold = new Color(30, 10, 2);
            Color hot = new Color(255, 150, 10);

            Color baseC = Color.Lerp(cold, hot, heat);
            float pulseVal = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 7f + progress * 8f) * 0.5f + 0.5f;
            Color glow = Color.Lerp(baseC, hot, pulseVal * 0.7f);

            Color result = glow * (inv * inv * 1.4f);
            result.A = 0;
            return result;
        }

        private float StripWidth(float progress)
        {
            float pulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 6.5f + progress * 14f) * 0.3f + 1f;
            return 5.5f * (1f - progress) * pulse;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.Kill();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.Kill();
        }
    }
}