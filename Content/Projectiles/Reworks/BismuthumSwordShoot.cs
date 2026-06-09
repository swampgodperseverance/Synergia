using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks
{
    public class BismuthumSwordShoot : ModProjectile
    {
        private const int FadeInTime = 10;
        private const int FadeOutTime = 15;

        private float flashIntensity = 1f;
        private bool flashActive = true;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 60;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 25;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                Vector2 mouseWorld = Main.MouseWorld;
                Projectile.position = mouseWorld;
                Projectile.velocity = new Vector2(0f, -22f);
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Projectile.timeLeft > FadeOutTime)
            {
                Projectile.alpha -= (int)(255f / FadeInTime);
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;
            }
            else
            {
                Projectile.alpha += (int)(255f / FadeOutTime);
                if (Projectile.alpha > 255)
                    Projectile.alpha = 255;
            }

            if (flashActive)
            {
                flashIntensity -= 0.1f;
                if (flashIntensity <= 0f)
                {
                    flashIntensity = 0f;
                    flashActive = false;
                }
            }

            Lighting.AddLight(Projectile.Center, 0.4f, 0.4f, 0.4f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] + new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;
                float opacity = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Color color = new Color(200, 200, 200) * opacity * (1f - Projectile.alpha / 255f);
                Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            }

            Color mainColor = new Color(255, 255, 255, 255 - Projectile.alpha);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, mainColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);

            if (flashIntensity > 0f)
            {
                float easeIn = (float)Math.Pow(flashIntensity, 0.3f);
                float easeOut = 1f - (float)Math.Pow(1f - flashIntensity, 3f);
                float combinedEase = easeIn * easeOut * 1.2f;

                if (combinedEase > 1f) combinedEase = 1f;

                BlendState oldBlend = Main.graphics.GraphicsDevice.BlendState;
                Main.graphics.GraphicsDevice.BlendState = BlendState.Additive;

                Texture2D glowTex = ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_1").Value;
                float baseRotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

                float intensity = combinedEase * 0.85f;
                float pulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 15f) * 0.1f + 0.9f;

                Color coreColor = new Color(255, 255, 255) * intensity * pulse;
                Color bloomColor = new Color(255, 255, 255) * intensity * 0.6f * pulse;
                Color softColor = new Color(255, 255, 255) * intensity * 0.35f * pulse;

                float coreScale = 0.5f + intensity * 1.8f;
                float bloomScale = coreScale * 1.4f;
                float softScale = coreScale * 0.9f;

                float bloomRotation = baseRotation + Main.GlobalTimeWrappedHourly * 2f;

                Main.EntitySpriteDraw(
                    glowTex,
                    Projectile.Center - Main.screenPosition,
                    null,
                    softColor,
                    baseRotation,
                    glowTex.Size() / 2f,
                    softScale,
                    SpriteEffects.None,
                    0
                );

                Main.EntitySpriteDraw(
                    glowTex,
                    Projectile.Center - Main.screenPosition,
                    null,
                    bloomColor,
                    bloomRotation,
                    glowTex.Size() / 2f,
                    bloomScale,
                    SpriteEffects.None,
                    0
                );

                Main.EntitySpriteDraw(
                    glowTex,
                    Projectile.Center - Main.screenPosition,
                    null,
                    coreColor,
                    baseRotation,
                    glowTex.Size() / 2f,
                    coreScale,
                    SpriteEffects.None,
                    0
                );

                for (int i = 0; i < 3; i++)
                {
                    float angleOffset = i * MathHelper.TwoPi / 3f;
                    float offsetDist = coreScale * 8f;
                    Vector2 offset = new Vector2(
                        (float)Math.Cos(baseRotation + angleOffset) * offsetDist,
                        (float)Math.Sin(baseRotation + angleOffset) * offsetDist
                    );

                    Main.EntitySpriteDraw(
                        glowTex,
                        Projectile.Center + offset - Main.screenPosition,
                        null,
                        coreColor * 0.5f,
                        baseRotation + angleOffset,
                        glowTex.Size() / 2f,
                        coreScale * 0.4f,
                        SpriteEffects.None,
                        0
                    );
                }

                Main.graphics.GraphicsDevice.BlendState = oldBlend;
            }

            return false;
        }
    }
}