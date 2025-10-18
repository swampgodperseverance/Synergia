using System;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Synergia.Helpers;

namespace Synergia.Content.Projectiles.Reworks.AltUse
{
    public class NarsilAlt : ModProjectile
    {
        private float spinSpeed = 0.4f; // начальная скорость вращения
        private bool exploded = false;
        //private bool hasStoppedSpinning = false;
        private bool isAcceleratingDown = false;
        private float acceleration = 0f;
        private float appearanceTimer = 0f;
        private float spinStopTimer = 0f;
        private Vector2 originalVelocity;

        // Состояния снаряда
        private enum ProjectileState
        {
            Appearing,
            Spinning,
            Stopping,
            AcceleratingDown
        }
        
        private ProjectileState currentState = ProjectileState.Appearing;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 70;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.light = 1f;
        }

        public override bool? CanDamage() => !exploded;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // ✅ Только один активный снаряд
            if (player.ownedProjectileCounts[Projectile.type] > 1)
            {
                Projectile.Kill();
                return;
            }

            // ПКМ — телепорт и взрыв
            if (player.controlUseTile && player.releaseUseTile && !exploded)
            {
                exploded = true;
                player.Teleport(Projectile.Center, 1, 0);
                OnKill(0);
                Projectile.Kill();
                return;
            }

            // Белый свет
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 1.3f);

            // Обработка состояний
            switch (currentState)
            {
                case ProjectileState.Appearing:
                    HandleAppearance();
                    break;
                    
                case ProjectileState.Spinning:
                    HandleSpinning();
                    break;
                    
                case ProjectileState.Stopping:
                    HandleStopping();
                    break;
                    
                case ProjectileState.AcceleratingDown:
                    HandleAcceleratingDown();
                    break;
            }
        }

        private void HandleAppearance()
        {
            appearanceTimer += 0.02f;
            float progress = EasingFunction.OutBack(appearanceTimer);
            
            // Масштабирование при появлении
            Projectile.scale = EasingFunction.OutBack(Math.Min(appearanceTimer * 1.5f, 1f));
            
            // Эффект "появления из ничего"
            if (appearanceTimer < 0.3f)
            {
                for (int i = 0; i < 3; i++)
                {
                    Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(16, 16), 32, 32, DustID.WhiteTorch);
                    d.velocity = Main.rand.NextVector2Circular(3f, 3f);
                    d.scale = Main.rand.NextFloat(0.8f, 1.5f) * progress;
                    d.noGravity = true;
                }
            }
            
            // Вращение во время появления
            Projectile.rotation += spinSpeed * Projectile.direction;
            
            if (appearanceTimer >= 1f)
            {
                currentState = ProjectileState.Spinning;
                originalVelocity = Projectile.velocity;
                Projectile.velocity *= 0.3f; // Замедляем после появления
            }
        }

        private void HandleSpinning()
        {
            // Продолжаем вращение с замедлением
            Projectile.rotation += spinSpeed * Projectile.direction;
            if (spinSpeed > 0.02f)
                spinSpeed *= 0.98f;

            // Постепенно уменьшаем горизонтальное движение
            Projectile.velocity.X *= 0.95f;
            Projectile.velocity.Y += 0.1f; // Легкая гравитация

            // Переход к остановке вращения
            if (spinSpeed <= 0.05f && spinStopTimer == 0f)
            {
                currentState = ProjectileState.Stopping;
            }
        }

        private void HandleStopping()
        {
            spinStopTimer += 0.02f;
            float progress = EasingFunction.OutCubic(Math.Min(spinStopTimer * 2f, 1f));
            
            // Плавный поворот к вертикальному положению (смотрящим вниз)
            float targetRotation = MathHelper.PiOver2; // 90 градусов - смотрит вниз
            if (Projectile.direction == -1)
                targetRotation = -MathHelper.PiOver2;
                
            Projectile.rotation = MathHelper.Lerp(Projectile.rotation, targetRotation, progress);
            
            // Легкое парение перед падением
            Projectile.velocity.Y *= 0.8f;
            Projectile.velocity.X *= 0.9f;
            
            // Эффекты при остановке
            if (Main.rand.NextBool(5))
            {
                Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(8, 8), 16, 16, DustID.Silver);
                d.velocity = Vector2.UnitY * -1f;
                d.scale = Main.rand.NextFloat(0.8f, 1.2f);
                d.noGravity = true;
            }
            
            if (spinStopTimer >= 0.5f)
            {
                currentState = ProjectileState.AcceleratingDown;
                isAcceleratingDown = true;
                acceleration = 0f;
                
                // Звуковой эффект начала падения
                SoundEngine.PlaySound(SoundID.Item1 with { Pitch = -0.3f }, Projectile.Center);
            }
        }

        private void HandleAcceleratingDown()
        {
            acceleration += 0.2f; // Увеличиваем ускорение
            Projectile.velocity.Y += acceleration; // Применяем ускорение
            
            // Ограничение максимальной скорости
            if (Projectile.velocity.Y > 15f)
                Projectile.velocity.Y = 15f;
                
            // Сохраняем вертикальную ориентацию
            Projectile.rotation = MathHelper.PiOver2;
            if (Projectile.direction == -1)
                Projectile.rotation = -MathHelper.PiOver2;
            
            // Эффекты при быстром падении
            if (Main.rand.NextBool(3))
            {
                Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4), 8, 8, DustID.WhiteTorch);
                d.velocity = Vector2.UnitY * 2f + Main.rand.NextVector2Circular(1f, 1f);
                d.scale = Main.rand.NextFloat(0.5f, 1f);
                d.noGravity = false;
            }
            
            // Свет становится интенсивнее при ускорении
            float lightIntensity = MathHelper.Clamp(acceleration * 0.1f, 1f, 2f);
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * lightIntensity);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            // Визуальные эффекты взрыва
            for (int i = 0; i < 25; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(16, 16), 32, 32, DustID.WhiteTorch);
                d.velocity = Main.rand.NextVector2Circular(5f, 5f);
                d.scale = Main.rand.NextFloat(1.2f, 2.3f);
                d.noGravity = true;
            }

            // Урон от взрыва
            int radius = 80;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.Distance(Projectile.Center) < radius)
                {
                    int dmg = Projectile.damage * 2;
             
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = new Color(255, 255, 255, 220);
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(tex.Width * 0.5f, Projectile.height * 0.5f);
            Rectangle src = new Rectangle(0, 0, tex.Width, tex.Height);
            SpriteEffects fx = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            // Корректировка масштаба
            Vector2 drawScale = new Vector2(Projectile.scale, Projectile.scale);

            // Следы (только в состояниях движения)
            if (currentState != ProjectileState.Appearing)
            {
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    if (Projectile.oldPos[k] == Vector2.Zero)
                        continue;
                        
                    Vector2 pos = Projectile.oldPos[k] - Main.screenPosition + origin;
                    float trailAlpha = (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length * 0.6f;
                    Color trailColor = Color.White * trailAlpha;
                    
                    Main.spriteBatch.Draw(tex, pos, src, trailColor, Projectile.oldRot[k], origin, drawScale, fx, 0f);
                }
            }

            // Основной спрайт
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, src, lightColor, Projectile.rotation, origin, drawScale, fx, 0f);
            
            // Эффект свечения при ускорении
            if (isAcceleratingDown && acceleration > 5f)
            {
                float glowIntensity = (acceleration - 5f) / 10f;
                Color glowColor = Color.Lerp(Color.White, Color.Cyan, glowIntensity) * 0.3f;
                Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, src, glowColor, Projectile.rotation, origin, drawScale * 1.1f, fx, 0f);
            }
            
            return false;
        }
    }
}