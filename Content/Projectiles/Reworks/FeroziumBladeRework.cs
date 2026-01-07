using Avalon.Common;
using Avalon.Common.Extensions;
using Avalon.Common.Templates;
using Avalon.Data.Sets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using System;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks
{
    public class FeroziumBladeRework : MaceTemplate
    {
        public override string Texture => "Avalon/Items/Weapons/Melee/Hardmode/FeroziumIceSword/FeroziumIceSword";
        public override string TrailTexture => "Synergia/Content/Projectiles/Reworks/FeroziumBladeRework_Trail";

        private const string GlowTexturePath = "Synergia/Assets/Textures/LightTrail_1";

        public override float ScaleMult => Projectile.ai[2] < 1 ? 0.8f : 0.6f;
        public override float MaxRotation => MathHelper.TwoPi;
        public override float SwingRadius => Projectile.ai[2] < 2 ? 65f : 55f;
        public override float StartScaleTime => 0.5f;
        public override float StartScaleMult => 0.6f;
        public override float EndScaleTime => 0.35f;
        public override float EndScaleMult => 0.6f;
        public override Color? TrailColor => null;
        public override Func<float, float> EasingFunc => rot => Easings.PowInOut(rot, 4f);
        public override int TrailLength => 6;

        private float glowRotation = 0f;
        private Texture2D glowTexture;
        private Texture2D originalTexture;

        private float nextSlashProgress = 0f;
        private const int GlowTrailLength = 15;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 70;
            Projectile.height = 70;
        }

        public override void AI()
        {
            base.AI();
            if (Projectile.ai[1] == 0f)
            {
                nextSlashProgress = 1f / 7f;
            }
            glowRotation += 0.12f;
            if (glowRotation > MathHelper.TwoPi)
                glowRotation -= MathHelper.TwoPi;
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
                    DustID.IceTorch,
                    Vector2.Normalize(offsetFromHand * dirMod).RotatedBy(MathHelper.PiOver2 * Owner.direction) * speedMultiplier * 2.5f,
                    Scale: 0.9f,
                    Alpha: 100
                );
                d.noGravity = true;
                d.fadeIn = 1.2f;
            }

            if (easedRotationProgress >= nextSlashProgress)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    int projType = ModContent.ProjectileType<FeroziumIcicle>();
                    Vector2 toCursor = Main.MouseWorld - Projectile.Center;
                    if (toCursor.Length() < 20f)
                        toCursor = Vector2.UnitX * Owner.direction;
                    Vector2 velocity = toCursor.SafeNormalize(Vector2.Zero) * 36f;
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        velocity,
                        projType,
                        (int)(Projectile.damage * 0.8f),
                        Projectile.knockBack * 1.2f,
                        Projectile.owner
                    );
                }
                nextSlashProgress += 1f / 7f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                hit.Knockback *= 1.2f;
            }
            target.AddBuff(BuffID.Frostburn2, 60);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (glowTexture == null)
                glowTexture = ModContent.Request<Texture2D>(GlowTexturePath).Value;
            
            if (originalTexture == null)
                originalTexture = ModContent.Request<Texture2D>(Texture).Value;

            SpriteBatch spriteBatch = Main.spriteBatch;
            
            Texture2D trailTex = ModContent.Request<Texture2D>(TrailTexture).Value;
            Vector2 trailOrigin = new Vector2(trailTex.Width / 2f, trailTex.Height / 2f);
            Vector2 glowOrigin = new Vector2(glowTexture.Width / 2f, glowTexture.Height / 2f);

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (k >= TrailLength || Projectile.oldPos[k] == Vector2.Zero) continue;

                Vector2 drawPos = Projectile.oldPos[k] + Projectile.Size / 2f - Main.screenPosition;
                float progress = 1f - k / (float)TrailLength;
                
                if (k == 0) continue;
                
                Color trailColor = new Color(0.6f, 0.9f, 1f, 0.3f * progress);
                
                spriteBatch.Draw(
                    trailTex,
                    drawPos,
                    null,
                    trailColor,
                    Projectile.oldRot[k],
                    trailOrigin,
                    Projectile.scale * progress * 0.8f,
                    SpriteEffects.None,
                    0f
                );
            }

            for (int k = 1; k < GlowTrailLength; k += 2)
            {
                if (k >= Projectile.oldPos.Length || Projectile.oldPos[k] == Vector2.Zero) continue;

                Vector2 drawPos = Projectile.oldPos[k] + Projectile.Size / 2f - Main.screenPosition;
                float progress = 1f - k / (float)GlowTrailLength;

                Color glowColor = new Color(0.3f, 0.7f, 1f, 0.5f) * progress;
                float rotatedGlow = glowRotation + k * 0.15f;

                spriteBatch.Draw(
                    glowTexture,
                    drawPos,
                    null,
                    glowColor,
                    Projectile.oldRot[k] + rotatedGlow,
                    glowOrigin,
                    Projectile.scale * progress * 1.1f,
                    SpriteEffects.None,
                    0f
                );
            }

            for (int k = 2; k < GlowTrailLength; k += 3)
            {
                if (k >= Projectile.oldPos.Length || Projectile.oldPos[k] == Vector2.Zero) continue;

                Vector2 drawPos = Projectile.oldPos[k] + Projectile.Size / 2f - Main.screenPosition;
                float progress = 1f - k / (float)GlowTrailLength;

                Color outerGlowColor = new Color(0.2f, 0.5f, 0.9f, 0.3f) * progress;
                float outerRotatedGlow = glowRotation + k * -0.1f;

                spriteBatch.Draw(
                    glowTexture,
                    drawPos,
                    null,
                    outerGlowColor,
                    Projectile.oldRot[k] + outerRotatedGlow,
                    glowOrigin,
                    Projectile.scale * progress * 1.4f,
                    SpriteEffects.None,
                    0f
                );
            }

            Vector2 position = Projectile.Center - Main.screenPosition;
            Rectangle? sourceRectangle = null;
            Color drawColor = lightColor;
            Vector2 origin = originalTexture.Size() / 2f;
            
            spriteBatch.Draw(
                originalTexture,
                position,
                sourceRectangle,
                drawColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );

            return false;
        }
    }
}