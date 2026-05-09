using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Helpers;
using System;

namespace Synergia.Content.Projectiles.Thrower
{
    public sealed class ThunderChakramProj : ModProjectile
    {
        private bool hasSlowed = false;
        private float initialSpeed = 8f;
        private float slowStartTime = 60f;
        private float glowIntensity = 0f;
        private bool isGlowing = false;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
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
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D sparkleTexture = ModContent.Request<Texture2D>(
                "RoA/Resources/Textures/VisualEffects/DefaultSparkle"
            ).Value;
            Texture2D mainTexture = ModContent.Request<Texture2D>(Texture).Value;

            Vector2 sparkleOrigin = sparkleTexture.Size() * 0.5f;
            Vector2 mainOrigin = mainTexture.Size() / 2f;
            Vector2 screenPos = Projectile.Center - Main.screenPosition;
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (Projectile.oldPos[k] == Vector2.Zero) continue;

                Vector2 drawPos = Projectile.oldPos[k] + Projectile.Size / 2f - Main.screenPosition;
                float progress = k / (float)Projectile.oldPos.Length;

                Color color = Color.Lerp(
                    new Color(255, 230, 100),
                    new Color(255, 150, 50),
                    progress
                ) * (1f - progress * 0.5f);

                float scale = Projectile.scale * (1f - progress * 0.3f);
                float rotation = Projectile.oldRot[k] + MathHelper.PiOver2;

                spriteBatch.Draw(
                    sparkleTexture,
                    drawPos,
                    null,
                    color,
                    rotation,
                    sparkleOrigin,
                    scale,
                    effects,
                    0f
                );
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            float glowSize = 1f + glowIntensity * 0.6f;

            for (int i = 0; i < 8; i++)
            {
                float rot = i * MathHelper.PiOver4 + Main.GlobalTimeWrappedHourly * 3f;
                Vector2 offset = new Vector2(3f, 0f).RotatedBy(rot);
                Color glowColor = new Color(255, 200, 50, 60) * (0.5f + glowIntensity * 0.6f);

                Main.EntitySpriteDraw(
                    mainTexture,
                    screenPos + offset,
                    null,
                    glowColor,
                    Projectile.rotation,
                    mainOrigin,
                    Projectile.scale * glowSize,
                    SpriteEffects.None
                );
            }

            Main.EntitySpriteDraw(
                mainTexture,
                screenPos,
                null,
                Color.White,
                Projectile.rotation,
                mainOrigin,
                Projectile.scale,
                SpriteEffects.None
            );

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        public override void AI()
        {
            Projectile.rotation += 0.3f * Projectile.direction;

            Lighting.AddLight(Projectile.Center, 1f, 0.6f, 0.2f);

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

                if (slowProgress > 0.7f && !isGlowing)
                {
                    isGlowing = true;
                }

                if (isGlowing)
                {
                    glowIntensity += 0.04f;
                    if (glowIntensity > 0.8f) glowIntensity = 0.8f;
                }
            }

            Projectile.spriteDirection = Projectile.direction;

            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldFlame, 0f, 0f, 100, default, 1f);
                Main.dust[dust].velocity *= 0.2f;
                Main.dust[dust].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.velocity *= 0.85f;

            for (int i = 0; i < 8; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldFlame, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 100, default, 1.2f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldFlame, Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f), 100, default, 1.5f);
            }

            Vector2[] directions = new Vector2[]
            {
                new Vector2(-1, 0),
                new Vector2(1, 0),
                new Vector2(0, -1),
                new Vector2(0, 1),
                new Vector2(-0.7f, -0.7f),
                new Vector2(0.7f, -0.7f),
                new Vector2(-0.7f, 0.7f),
                new Vector2(0.7f, 0.7f)
            };

            for (int i = 0; i < 8; i++)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    directions[i] * 6f,
                    ModContent.ProjectileType<ThunderSpike2>(),
                    Projectile.damage / 4,
                    Projectile.knockBack * 0.6f,
                    Projectile.owner
                );
            }
        }
    }
}