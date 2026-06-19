using System;
using System.IO;
using Avalon.Common;
using Avalon.Common.Templates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class GranitburRework : MaceTemplate
    {
        private bool projectileSpawned = false;

        public override string Texture => "ValhallaMod/Items/Weapons/Melee/ChannelMelee/Granitbur";
        public override string TrailTexture => Texture;
        public override LocalizedText DisplayName => Language.GetText("Mods.ValhallaMod.Content.Items.Granitbur.DisplayName");

        public override float ScaleMult => Projectile.ai[2] < 2 ? 1.05f : 0.9f;
        public override float MaxRotation => Projectile.ai[2] < 2 ? 4.2f : 2.8f;
        public override float SwingRadius => Projectile.ai[2] < 2 ? 90f : 60f;

        public override float StartScaleTime => 0.4f;
        public override float StartScaleMult => 1f;
        public override float EndScaleTime => 0.3f;
        public override float EndScaleMult => 1f;

        public override Color? TrailColor => new Color(0.2f, 0.6f, 1f, 0.3f);
        public override Func<float, float> EasingFunc => rot => Easings.PowInOut(rot, 5f);
        public override int TrailLength => 12;

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D glowTexture = ModContent.Request<Texture2D>("Synergia/Content/Projectiles/Reworks/Reworks2/GranitburGlow", AssetRequestMode.ImmediateLoad).Value;
            if (glowTexture != null)
            {
                Vector2 drawPosition = Projectile.Center - Main.screenPosition;
                float rotation = Projectile.rotation;
                Vector2 origin = glowTexture.Size() / 2f;
                float scale = Projectile.scale * 1.2f;

                for (int i = 0; i < 8; i++)
                {
                    float angle = i * MathHelper.PiOver4;
                    Vector2 offset = new Vector2(4f * Projectile.scale, 0f).RotatedBy(angle);
                    Color outlineColor = new Color(0.1f, 0.5f, 1f, 0.3f) * 0.5f;

                    Main.spriteBatch.Draw(
                        glowTexture,
                        drawPosition + offset,
                        null,
                        outlineColor,
                        rotation,
                        origin,
                        scale,
                        SpriteEffects.None,
                        0f
                    );
                }

                Main.spriteBatch.Draw(
                    glowTexture,
                    drawPosition,
                    null,
                    new Color(0.3f, 0.7f, 1f, 0.6f),
                    rotation,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0f
                );
            }

            return base.PreDraw(ref lightColor);
        }

        public override void EmitDust(Vector2 handPosition, float swingRadius, float rotationProgress, float easedRotationProgress)
        {
            if (Projectile.localAI[2] != 1 && easedRotationProgress > 0.1f)
            {
                Projectile.localAI[2] = 1;
                SoundEngine.PlaySound(SoundID.Item1 with { Volume = 0.9f, Pitch = Projectile.ai[2] < 2 ? 0f : 0.3f }, Projectile.position);
            }

            float speedMultiplier = Math.Clamp(Math.Abs(Projectile.oldRot[0] - Projectile.rotation), 0f, 1f);
            if (speedMultiplier > 0.15f)
            {
                Vector2 offsetFromHand = Projectile.Center - handPosition;
                float dirMod = SwingDirection * Owner.gravDir;
                Dust d = Dust.NewDustPerfect(
                    Vector2.Lerp(Projectile.Center, handPosition, Main.rand.NextFloat(0.2f, 0.5f)),
                    DustID.Electric,
                    Vector2.Normalize(offsetFromHand * dirMod).RotatedBy(MathHelper.PiOver2 * Owner.direction) * speedMultiplier * 3f,
                    Scale: 0.8f,
                    Alpha: 80
                );
                d.noGravity = true;
                d.fadeIn = 1.3f;
                d.color = new Color(0.3f, 0.7f, 1f);

                if (Main.rand.NextBool(3))
                {
                    Dust d2 = Dust.NewDustPerfect(
                        Projectile.Center + Main.rand.NextVector2Circular(20f, 20f),
                        DustID.GemSapphire,
                        Main.rand.NextVector2Circular(2f, 2f),
                        Scale: 0.5f,
                        Alpha: 150
                    );
                    d2.noGravity = true;
                    d2.color = new Color(0.5f, 0.8f, 1f);
                }
            }

            if (easedRotationProgress > 0.3f && easedRotationProgress < 0.7f && Projectile.ai[2] >= 2 && !projectileSpawned)
            {
                if (Main.rand.NextBool(3))
                {
                    Vector2 directionToCursor = Main.MouseWorld - Projectile.Center;
                    directionToCursor.Normalize();

                    Vector2 spawnPos = Projectile.Center + directionToCursor * 40f;
                    Vector2 velocity = directionToCursor * 16f;

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        spawnPos,
                        velocity,
                        ModContent.ProjectileType<GranitburProjectile>(),
                        Projectile.damage / 2,
                        Projectile.knockBack * 0.5f,
                        Projectile.owner
                    );
                }

                projectileSpawned = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            projectileSpawned = false;
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnKill(int timeLeft)
        {
            projectileSpawned = false;
            base.OnKill(timeLeft);
        }
    }

    public class GranitburProjectile : ModProjectile
    {
        private const int TotalTime = 60;
        private float alpha = 0f;

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = TotalTime;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            float timer = TotalTime - Projectile.timeLeft;
            float p = MathHelper.Clamp(timer / TotalTime, 0f, 1f);
            p = p * p;
            alpha = p;

            Projectile.alpha = (int)(255f * alpha);

            Projectile.velocity *= 0.92f;

            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;

            if (Main.rand.NextBool(2) && alpha < 0.8f)
            {
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(Projectile.width / 2, Projectile.height / 2),
                    DustID.Electric,
                    Main.rand.NextVector2Circular(0.8f, 0.8f),
                    Scale: 0.8f + Main.rand.NextFloat(0.4f) * (1f - alpha),
                    Alpha: (int)(60 * alpha)
                );
                d.noGravity = true;
                d.color = new Color(0.2f, 0.6f, 1f) * (1f - alpha);
            }

            if (Main.rand.NextBool(3) && alpha < 0.8f)
            {
                Dust d2 = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(15f, 15f),
                    DustID.GemSapphire,
                    Main.rand.NextVector2Circular(1.5f, 1.5f),
                    Scale: 0.5f * (1f - alpha),
                    Alpha: (int)(80 * alpha)
                );
                d2.noGravity = true;
                d2.color = new Color(0.4f, 0.7f, 1f) * (1f - alpha);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("Synergia/Content/Projectiles/Reworks/Reworks2/GranitburProjectile").Value;
            if (texture != null)
            {
                Vector2 drawPosition = Projectile.Center - Main.screenPosition;
                float pulse = (float)(Math.Sin(Main.GameUpdateCount * 0.05f) * 0.15f + 0.85f);
                float glowIntensity = (1f - alpha) * pulse;

                float mainAlpha = MathHelper.Lerp(1f, 0.3f, alpha) * (0.7f + 0.3f * pulse);
                Color mainColor = new Color(0.2f, 0.6f, 1f, mainAlpha);

                float outlineSize = 3f * Projectile.scale * (0.8f + 0.4f * pulse);
                float outlineAlpha = (1f - alpha * 0.5f) * (0.5f + 0.3f * pulse);

                Vector2[] outlineOffsets = new Vector2[]
                {
                new Vector2(outlineSize, 0),
                new Vector2(-outlineSize, 0),
                new Vector2(0, outlineSize),
                new Vector2(0, -outlineSize),
                new Vector2(outlineSize * 0.7f, outlineSize * 0.7f),
                new Vector2(-outlineSize * 0.7f, -outlineSize * 0.7f),
                new Vector2(outlineSize * 0.7f, -outlineSize * 0.7f),
                new Vector2(-outlineSize * 0.7f, outlineSize * 0.7f),
                };
                float[] alphaLayers = new float[] { 0.3f, 0.2f, 0.1f };
                float[] sizeMultipliers = new float[] { 1f, 1.5f, 2f };

                foreach (float sizeMult in sizeMultipliers)
                {
                    foreach (Vector2 offset in outlineOffsets)
                    {
                        float currentAlpha = outlineAlpha * alphaLayers[Array.IndexOf(sizeMultipliers, sizeMult)];
                        Color outlineColor = new Color(0.1f, 0.5f, 1f, currentAlpha * 0.3f);

                        Main.spriteBatch.Draw(
                            texture,
                            drawPosition + offset * sizeMult,
                            null,
                            outlineColor,
                            Projectile.rotation,
                            texture.Size() / 2f,
                            Projectile.scale * (0.9f + 0.1f * pulse),
                            SpriteEffects.None,
                            0f
                        );
                    }
                }

                float glowSize = 6f * Projectile.scale * (0.8f + 0.3f * pulse);
                Color glowColor = new Color(0.0f, 0.3f, 0.8f, outlineAlpha * 0.1f);

                for (int i = 0; i < 8; i++)
                {
                    float angle = i * MathHelper.PiOver4 + Main.GameUpdateCount * 0.02f;
                    Vector2 glowOffset = new Vector2(glowSize, 0).RotatedBy(angle);
                    Main.spriteBatch.Draw(
                        texture,
                        drawPosition + glowOffset,
                        null,
                        glowColor,
                        Projectile.rotation,
                        texture.Size() / 2f,
                        Projectile.scale * (1.1f + 0.1f * pulse),
                        SpriteEffects.None,
                        0f
                    );
                }

                Main.spriteBatch.Draw(
                    texture,
                    drawPosition,
                    null,
                    mainColor * (1.2f * glowIntensity),
                    Projectile.rotation,
                    texture.Size() / 2f,
                    Projectile.scale * (1f - alpha * 0.2f),
                    SpriteEffects.None,
                    0f
                );
            }

            return false;
        }

        public override void Kill(int timeLeft)
        {
        }
    }
}