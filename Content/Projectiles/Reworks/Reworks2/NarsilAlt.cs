using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace Synergia.Content.Projectiles.Reworks.AltUse
{
    public class NarsilAlt : ModProjectile
    {
        private enum NarsilState
        {
            Appearing,
            Spinning,
            AligningToCursor,
            Pulsing
        }

        private NarsilState currentState = NarsilState.Appearing;

        private float rotationSpeed = 0.35f;
        private int spinTimer = 0;
        private const int spinTime = 180;
        private float pulseCounter = 0f;
        private float appearProgress = 0f;
        private bool flashTriggered = false;
        private float targetRotation = 0f; // Добавляем переменную для хранения целевого угла

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 400;
            Projectile.tileCollide = false;
            Projectile.height = 35;
            Projectile.width = 35;
            Projectile.penetrate = 10;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.extraUpdates = 1;
        }

        public override bool PreAI()
        {
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 1.5f * Main.essScale);
            Projectile.velocity *= 0.97f;

            switch (currentState)
            {
                case NarsilState.Appearing:
                    appearProgress += 0.02f;
                    if (appearProgress > 1f)
                    {
                        appearProgress = 1f;
                        currentState = NarsilState.Spinning;
                    }

                    Projectile.alpha = (int)(255 - appearProgress * 255);
                    Projectile.rotation += 0.05f;
                    break;

                case NarsilState.Spinning:
                    spinTimer++;
                    Projectile.rotation += rotationSpeed;
                    rotationSpeed *= 0.985f;

                    if (spinTimer >= spinTime)
                    {
                        // Вычисляем направление к курсору
                        Vector2 directionToCursor = Main.MouseWorld - Projectile.Center;
                        targetRotation = directionToCursor.ToRotation(); // Получаем угол в радианах
                        
                        currentState = NarsilState.AligningToCursor;
                    }
                    break;

                case NarsilState.AligningToCursor:
                    // Плавно поворачиваем к целевому углу
                    float rotationDiff = MathHelper.WrapAngle(targetRotation - Projectile.rotation);
                    Projectile.rotation += rotationDiff * 0.15f; // Коэффициент для плавности поворота

                    // Если достаточно близко к целевому углу, переходим к пульсации
                    if (Math.Abs(rotationDiff) < 0.05f)
                    {
                        Projectile.rotation = targetRotation;
                        currentState = NarsilState.Pulsing;
                    }
                    break;

                case NarsilState.Pulsing:
                    pulseCounter += 0.04f;

                    if (!flashTriggered)
                    {
                        flashTriggered = true;

                        for (int i = 0; i < 20; i++)
                        {
                            Vector2 vel = Main.rand.NextVector2Circular(5f, 5f);
                            int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.WhiteTorch, vel.X, vel.Y, 150, Color.White, 1.8f);
                            Main.dust[dust].noGravity = true;
                        }

                        for (int i = 0; i < 3; i++)
                        {
                            Vector2 vel = Main.rand.NextVector2Circular(1.5f, 1.5f);
                            int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke, vel.X, vel.Y, 100, Color.White, 2.2f);
                            Main.dust[dust].noGravity = true;
                        }

                        for (int i = 0; i < 8; i++)
                        {
                            Vector2 vel = Main.rand.NextVector2Circular(3f, 3f);
                            int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.IceTorch, vel.X, vel.Y, 80, Color.White, 1.8f);
                            Main.dust[dust].noGravity = true;
                        }
                    }
                    break;
            }

            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() * 0.5f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Color baseColor = Color.White * Projectile.Opacity;

            switch (currentState)
            {
                case NarsilState.Appearing:
                    for (int i = 0; i < 10; i++)
                    {
                        float progress = 1f - appearProgress;
                        Vector2 offset = Main.rand.NextVector2CircularEdge(80f, 80f) * progress;
                        Color ghostColor = Color.White * (0.1f + 0.4f * progress);
                        Main.EntitySpriteDraw(
                            texture,
                            drawPos + offset,
                            null,
                            ghostColor,
                            Projectile.rotation,
                            origin,
                            Projectile.scale,
                            SpriteEffects.None,
                            0
                        );
                    }

                    Main.EntitySpriteDraw(
                        texture,
                        drawPos,
                        null,
                        Color.White * appearProgress,
                        Projectile.rotation,
                        origin,
                        Projectile.scale,
                        SpriteEffects.None,
                        0
                    );
                    break;

                case NarsilState.Spinning:
                case NarsilState.AligningToCursor: // Добавляем отрисовку для фазы выравнивания
                    Main.EntitySpriteDraw(
                        texture,
                        drawPos,
                        null,
                        baseColor,
                        Projectile.rotation,
                        origin,
                        Projectile.scale,
                        SpriteEffects.None,
                        0
                    );
                    break;

                case NarsilState.Pulsing:
                    float pulse = (float)Math.Sin(pulseCounter) * 0.5f + 0.5f;
                    float pulseScale = 1f + 0.1f * pulse;
                    int ghostCount = 8;

                    for (int i = 0; i < ghostCount; i++)
                    {
                        float angle = MathHelper.TwoPi * i / ghostCount;
                        Vector2 offset = angle.ToRotationVector2() * (6f + 3f * pulse);
                        Color ghostColor = Color.White * (0.15f + 0.25f * pulse);

                        Main.EntitySpriteDraw(
                            texture,
                            drawPos + offset,
                            null,
                            ghostColor,
                            Projectile.rotation,
                            origin,
                            Projectile.scale * pulseScale,
                            SpriteEffects.None,
                            0
                        );
                    }

                    Main.EntitySpriteDraw(
                        texture,
                        drawPos,
                        null,
                        Color.White,
                        Projectile.rotation,
                        origin,
                        Projectile.scale,
                        SpriteEffects.None,
                        0
                    );
                    break;
            }

            return false;
        }
    }
}