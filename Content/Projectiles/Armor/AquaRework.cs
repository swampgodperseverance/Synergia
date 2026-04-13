using System;
using System.Collections.Generic;
using ReLogic.Content;
using Synergia.Helpers;
using Terraria;
using Terraria.ID;

namespace Synergia.Content.Projectiles.Armor
{
    public class AquaRework : ModProjectile
    {
        private const float OrbitDuration = 140f;
        private const float OrbitRadius = 42f;
        private const float InitialAngularSpeed = 0.28f;
        private const float FinalAngularSpeed = 0.015f;

        private float currentAngle = 0f;
        private float angularSpeed = InitialAngularSpeed;
        private float timer = 0f;
        private Vector2 targetDirection;
        private float randomOffset;
        private float rayRandomSeed;
        private float[] rayRotations;
        private float[] rayScaleX;
        private float[] rayScaleY;
        private float[] rayPulseSpeed;
        private float[] rayAppearDelay;

        private HashSet<int> affectedProjectiles = new HashSet<int>();

        private static Asset<Texture2D> rayTexture;

        public override void Load()
        {
            rayTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ray");
        }

        public override void Unload()
        {
            rayTexture = null;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
            Projectile.alpha = 255;
            Projectile.light = 0.65f;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.active)
            {
                Projectile.Kill();
                return;
            }

            if (timer == 0)
            {
                randomOffset = Main.rand.NextFloat(MathHelper.TwoPi);
                rayRandomSeed = Main.rand.NextFloat(100f);
                rayRotations = new float[6];
                rayScaleX = new float[6];
                rayScaleY = new float[6];
                rayPulseSpeed = new float[6];
                rayAppearDelay = new float[6];

                for (int i = 0; i < 6; i++)
                {
                    rayRotations[i] = Main.rand.NextFloat(-0.2f, 0.2f);
                    rayScaleX[i] = Main.rand.NextFloat(0.4f, 0.6f);
                    rayScaleY[i] = Main.rand.NextFloat(0.7f, 1.0f);
                    rayPulseSpeed[i] = Main.rand.NextFloat(1.2f, 2.0f);
                    rayAppearDelay[i] = Main.rand.NextFloat(0f, 8f);
                }
                Projectile.position += Main.rand.NextVector2Circular(8f, 8f);
            }

            timer++;

            if (timer <= OrbitDuration)
            {
                float progress = timer / OrbitDuration;
                float eased = EaseFunctions.EaseOutQuint(progress);
                angularSpeed = MathHelper.Lerp(InitialAngularSpeed, FinalAngularSpeed, eased);

                currentAngle += angularSpeed;

                Vector2 offset = new Vector2(
                    (float)Math.Cos(currentAngle + randomOffset) * OrbitRadius,
                    (float)Math.Sin(currentAngle + randomOffset) * OrbitRadius
                );

                Projectile.Center = owner.Center + offset;
                Projectile.rotation = 0f;

                if (timer < 25)
                    Projectile.alpha = (int)(255 * (1f - timer / 25f));
                else
                    Projectile.alpha = 0;
            }
            else
            {
                if (timer == OrbitDuration + 1)
                {
                    Vector2 toCursor = Main.MouseWorld - Projectile.Center;
                    toCursor.Normalize();
                    targetDirection = toCursor * (19f + Main.rand.NextFloat(-2f, 2f));
                }

                float flyProgress = (timer - OrbitDuration) / 50f;

                if (flyProgress < 1f)
                {
                    float speedFactor = EaseFunctions.EaseOutCubic(flyProgress);
                    Projectile.velocity = targetDirection * (1f - speedFactor * (0.88f + Main.rand.NextFloat(-0.05f, 0.05f)));
                }
                else
                {
                    Projectile.velocity *= 0.90f;
                    Projectile.alpha += 6;
                    if (Projectile.alpha >= 255)
                        Projectile.Kill();
                }

                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }

            HandleProjectileInteraction();

            Lighting.AddLight(Projectile.Center, 0.25f, 0.65f, 1.1f);
        }

        private void HandleProjectileInteraction()
        {
            bool canReflect = (timer > OrbitDuration && Projectile.timeLeft <= 30);

            foreach (Projectile other in Main.projectile)
            {
                if (!other.active || other == Projectile) continue;
                if (!other.hostile || other.damage <= 0) continue;
                if (affectedProjectiles.Contains(other.whoAmI)) continue;

                float distance = Vector2.Distance(Projectile.Center, other.Center);
                float radius = Math.Max(Projectile.width, Projectile.height) / 2f + Math.Max(other.width, other.height) / 2f;

                if (distance <= radius)
                {
                    if (canReflect)
                    {
                        other.velocity = Vector2.Reflect(other.velocity, Vector2.Normalize(other.Center - Projectile.Center));
                        other.hostile = false;
                        other.friendly = true;
                        other.owner = Projectile.owner;

                        for (int i = 0; i < 8; i++)
                        {
                            Dust.NewDust(other.position, other.width, other.height, DustID.MagicMirror,
                                other.velocity.X * 0.5f, other.velocity.Y * 0.5f, 100, Color.Cyan, 1.2f);
                        }

                        affectedProjectiles.Add(other.whoAmI);
                    }
                    else
                    {
                        int newDamage = (int)(other.damage * 0.8f);
                        if (newDamage < 1) newDamage = 1;
                        other.damage = newDamage;

                        for (int i = 0; i < 5; i++)
                        {
                            Dust.NewDust(other.position, other.width, other.height, DustID.BubbleBlock,
                                other.velocity.X * 0.3f, other.velocity.Y * 0.3f, 80, Color.LightBlue, 0.8f);
                        }

                        affectedProjectiles.Add(other.whoAmI);
                    }
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 255 - Projectile.alpha);
        }

        public override void PostDraw(Color lightColor)
        {
            if (rayTexture == null || !rayTexture.IsLoaded)
                return;

            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawOrigin = texture.Size() / 2f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            for (int i = 0; i < 4; i++)
            {
                Vector2 offset = i switch
                {
                    0 => new Vector2(3, 0),
                    1 => new Vector2(-3, 0),
                    2 => new Vector2(0, 3),
                    3 => new Vector2(0, -3),
                    _ => Vector2.Zero
                };

                Color outline = new Color(60, 180, 255, (byte)(90 - Projectile.alpha / 3));
                Main.EntitySpriteDraw(texture, drawPos + offset, null, outline,
                    Projectile.rotation, drawOrigin, Projectile.scale * 1.15f, SpriteEffects.None, 0);
            }

            Color mainColor = new Color(140, 230, 255, (byte)(160 - Projectile.alpha / 2));
            Main.EntitySpriteDraw(texture, drawPos, null, mainColor,
                Projectile.rotation, drawOrigin, Projectile.scale * 1.05f, SpriteEffects.None, 0);

            if (timer > OrbitDuration)
            {
                float flyTime = timer - OrbitDuration;
                float totalTime = 50f;

                float rotationSpeed = 0.25f + rayRandomSeed * 0.008f;
                float globalRotation = Main.GlobalTimeWrappedHourly * rotationSpeed;

                Texture2D rayTex = rayTexture.Value;
                Vector2 rayOrigin = new Vector2(rayTex.Width / 2f, rayTex.Height);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp,
                    DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                int rayCount = 6;

                for (int i = 0; i < rayCount; i++)
                {
                    float appearStart = rayAppearDelay[i];
                    float appearEnd = appearStart + 30f;
                    float disappearStart = 70f;
                    float disappearEnd = 85f;
                    float appearProgress = MathHelper.Clamp((flyTime - appearStart) / (appearEnd - appearStart), 0f, 1f);
                    float disappearProgress = MathHelper.Clamp((flyTime - disappearStart) / (disappearEnd - disappearStart), 0f, 1f);

                    float intensityMultiplier = 1f;
                    if (flyTime < appearEnd)
                        intensityMultiplier = appearProgress;
                    else if (flyTime > disappearStart)
                        intensityMultiplier = 1f - disappearProgress;
                    else
                        intensityMultiplier = 1f;

                    float rayIntensity = intensityMultiplier * (1f - Projectile.alpha / 255f);

                    if (rayIntensity <= 0.02f) continue;

                    float angle = MathHelper.TwoPi / rayCount * i + globalRotation + rayRotations[i];

                    float pulseScale = 0.85f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * rayPulseSpeed[i] + i) * 0.08f;

                    float scaleMultiplier = MathHelper.Lerp(0.3f, 1f, appearProgress);
                    if (flyTime > disappearStart)
                        scaleMultiplier = MathHelper.Lerp(1f, 0.3f, disappearProgress);

                    Color rayColor = new Color(90, 200, 255) * (rayIntensity * 0.6f);

                    float scaleX = rayScaleX[i] * pulseScale * scaleMultiplier;
                    float scaleY = rayScaleY[i] * (0.8f + rayIntensity * 0.2f) * scaleMultiplier;

                    Main.EntitySpriteDraw(rayTex, drawPos, null, rayColor,
                        angle, rayOrigin,
                        new Vector2(scaleX, scaleY),
                        SpriteEffects.None, 0);
                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                    DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 172, Scale: 1.4f);
                d.noGravity = true;
                d.velocity *= 3.2f;
                d.fadeIn = 1.3f;
            }
        }
    }
}