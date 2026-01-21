using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.CameraModifiers;
using ValhallaMod;
using Terraria.Audio;
using ValhallaMod.Projectiles.AI;
using Synergia.Content.Projectiles.Thrower;

namespace Synergia.Content.Projectiles.Thrower
{
    public class SnowGlaive2 : ValhallaGlaive
    {
        private bool shot;
        private int shootTimer;
        private float flashIntensity;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 6;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            DrawOffsetX = 4;
            DrawOriginOffsetY = -4;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 102;
            Projectile.scale = 0.9f;

            bounces = 2;
            quickComeBack = true;
            timeFlying = 30;

            speedFlying = 15f;
            speedHoming = 15f;
            speedComingBack = 20f;

            homingDistanceMax = 400f;
            homingStyle = 1;
            homingStart = false;
            homingIgnoreTile = false;
        }

        public override void AI()
        {
            base.AI();

            shootTimer++;

            if (!shot && shootTimer >= 30)
            {
                shot = true;
                flashIntensity = 1f;

                if (Main.myPlayer == Projectile.owner)
                {
                    Vector2 baseDir = Projectile.velocity.SafeNormalize(Vector2.UnitX);

                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 dir = baseDir.RotatedByRandom(0.35f);
                        dir *= Main.rand.NextFloat(6f, 9f);

                        Projectile.NewProjectile(
                            Projectile.GetSource_FromAI(),
                            Projectile.Center,
                            dir,
                            ModContent.ProjectileType<IcePieces>(),
                            Projectile.damage / 3,
                            1f,
                            Projectile.owner
                        );
                    }
                }

                Main.instance.CameraModifiers.Add(
                    new PunchCameraModifier(
                        Projectile.Center,
                        Main.rand.NextVector2Unit(),
                        6f,
                        10f,
                        18,
                        1000f
                    )
                );

                SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
            }

            if (flashIntensity > 0f)
            {
                flashIntensity -= 0.05f;
                if (flashIntensity < 0f) flashIntensity = 0f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Vector2 origin = tex.Size() / 2f;

            for (int i = 1; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    continue;

                float fade = 1f - i / (float)Projectile.oldPos.Length;
                Main.EntitySpriteDraw(
                    tex,
                    Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition,
                    null,
                    new Color(150, 220, 255, 0) * fade * 0.6f,
                    Projectile.rotation,
                    origin,
                    Projectile.scale * (1f - i * 0.05f),
                    SpriteEffects.None,
                    0
                );
            }

            BlendState oldBlend = Main.graphics.GraphicsDevice.BlendState;
            
            if (flashIntensity > 0f)
            {
                float easeIn = (float)Math.Pow(flashIntensity, 0.3f);
                float easeOut = 1f - (float)Math.Pow(1f - flashIntensity, 3f);
                float combinedEase = easeIn * easeOut * 1.2f;
                
                if (combinedEase > 1f) combinedEase = 1f;

                Main.graphics.GraphicsDevice.BlendState = BlendState.Additive;

                Texture2D glowTex = ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_1").Value;
                float baseRotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

                float intensity = combinedEase * 0.85f;
                float pulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 15f) * 0.1f + 0.9f;

                Color coreColor = new Color(180, 240, 255) * intensity * pulse;
                Color bloomColor = new Color(140, 200, 255) * intensity * 0.6f * pulse;
                Color softColor = new Color(210, 245, 255) * intensity * 0.35f * pulse;

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
    }
}