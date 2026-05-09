using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;

namespace Synergia.Content.Projectiles.Thrower
{
    public class SolarDisc2 : ModProjectile
    {
        private bool hasReversed = false;
        private bool hasArced = false;
        private int stateTimer = 0;
        private float flashIntensity = 0f;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.extraUpdates = 2;
        }

        public override void AI()
        {
            stateTimer++;
            Projectile.rotation += 0.28f * (hasReversed ? -1f : 1f);

            if (stateTimer < 28)
            {
                Projectile.velocity *= 0.97f;
                Projectile.velocity.Y += 0.11f;
            }
            else if (stateTimer == 28)
            {
                Vector2 dir = Projectile.velocity.SafeNormalize(Vector2.Zero);
                float speed = Projectile.velocity.Length() * 0.92f;
                Projectile.velocity = dir.RotatedBy(MathHelper.Pi) * speed;
                hasReversed = true;
            }
            else if (stateTimer < 65)
            {
                Projectile.velocity *= 0.965f;
                Projectile.velocity.Y -= 0.09f;
            }
            else if (!hasArced)
            {
                hasArced = true;
                Projectile.velocity.Y = -11.5f;
                Projectile.velocity.X *= 0.55f;
            }
            else
            {
                Projectile.velocity.Y += 0.32f;
                Projectile.velocity.X *= 0.982f;
            }

            if (stateTimer >= 28)
                Lighting.AddLight(Projectile.Center, 1.35f, 0.85f, 0.25f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Vector2 origin = tex.Size() / 2f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) continue;
                float fade = 1f - (float)i / Projectile.oldPos.Length;
                float scale = Projectile.scale * (1f - i * 0.038f);
                Main.EntitySpriteDraw(
                    tex,
                    Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition,
                    null,
                    new Color(255, 180, 60, 0) * fade * 0.75f,
                    Projectile.oldRot[i],
                    origin,
                    scale,
                    SpriteEffects.None,
                    0
                );
            }

            BlendState oldBlend = Main.graphics.GraphicsDevice.BlendState;

            if (hasReversed && stateTimer > 28)
            {
                float outlineAlpha = 0.65f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 12f) * 0.15f;
                Main.graphics.GraphicsDevice.BlendState = BlendState.Additive;

                for (int i = 0; i < 4; i++)
                {
                    Vector2 offset = new Vector2(2f, 0f).RotatedBy(i * MathHelper.PiOver2);
                    Main.EntitySpriteDraw(
                        tex,
                        Projectile.Center - Main.screenPosition + offset,
                        null,
                        new Color(255, 240, 150) * outlineAlpha * 0.4f,
                        Projectile.rotation,
                        origin,
                        Projectile.scale * 1.08f,
                        SpriteEffects.None,
                        0
                    );
                }
            }

            if (flashIntensity > 0f)
            {
                float easeIn = (float)Math.Pow(flashIntensity, 0.3f);
                float easeOut = 1f - (float)Math.Pow(1f - flashIntensity, 3f);
                float combinedEase = easeIn * easeOut * 1.4f;
                if (combinedEase > 1f) combinedEase = 1f;

                Main.graphics.GraphicsDevice.BlendState = BlendState.Additive;

                Texture2D glowTex = ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_1").Value;
                Texture2D ringTex = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ring").Value;

                float baseRotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                float intensity = combinedEase * 1.15f;
                float pulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 20f) * 0.1f + 0.95f;

                Color coreColor = new Color(255, 230, 90) * intensity * pulse;
                Color bloomColor = new Color(255, 170, 50) * intensity * 0.75f * pulse;
                Color softColor = new Color(255, 245, 140) * intensity * 0.45f * pulse;

                float coreScale = 0.65f + intensity * 2.4f;
                float bloomScale = coreScale * 1.65f;
                float softScale = coreScale * 1.15f;

                float bloomRotation = baseRotation + Main.GlobalTimeWrappedHourly * 3.2f;

                Main.EntitySpriteDraw(ringTex, Projectile.Center - Main.screenPosition, null, softColor * 0.85f, Main.GlobalTimeWrappedHourly * 1.6f, ringTex.Size() / 2f, coreScale * 0.9f, SpriteEffects.None, 0);

                Main.EntitySpriteDraw(glowTex, Projectile.Center - Main.screenPosition, null, softColor, baseRotation, glowTex.Size() / 2f, softScale, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(glowTex, Projectile.Center - Main.screenPosition, null, bloomColor, bloomRotation, glowTex.Size() / 2f, bloomScale, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(glowTex, Projectile.Center - Main.screenPosition, null, coreColor, baseRotation, glowTex.Size() / 2f, coreScale, SpriteEffects.None, 0);

                Main.graphics.GraphicsDevice.BlendState = oldBlend;
            }

            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);
            flashIntensity = 1.35f;

            for (int i = 0; i < 26; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.SolarFlare, 0f, 0f, 0, default, 1.65f);
                d.noGravity = true;
                d.velocity = Main.rand.NextVector2Circular(8.5f, 8.5f);
            }

            for (int i = 0; i < 13; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.OrangeTorch, 0f, 0f, 0, default, 1.35f);
                d.noGravity = true;
                d.velocity *= 2.8f;
            }
        }
    }
}