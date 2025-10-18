using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Helpers;

namespace Synergia.Content.Items.Weapons.Melee
{
    public class Seaborn : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Seaborn");
            // Tooltip.SetDefault("A graceful blade from the depths");
        }

        public override void SetDefaults()
        {
            Item.damage = 45;
            Item.DamageType = DamageClass.Melee;
            Item.width = 64;
            Item.height = 64;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = false;
            Item.noUseGraphic = false;
            Item.shoot = ModContent.ProjectileType<SeabornSlash>();
            Item.shootSpeed = 8f;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<SeabornSlash>()] < 1;
        }

        public override void UseAnimation(Player player)
        {
            // Определяем направление взмаха
            if (player.GetModPlayer<CorrectSwing>().SwingChangeBool)
            {
                player.GetModPlayer<CorrectSwing>().SwingChange++;
            }
        }

        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            // Корректируем хитбокс в зависимости от анимации
            base.UseItemHitbox(player, ref hitbox, ref noHitbox);
        }
    }

    public class SeabornSlash : ModProjectile
    {
        private Vector2[] oldPos = new Vector2[10];
        private float[] oldRot = new float[10];
        private int swingDirection;
        private float progress;
        private float scaleProgress;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Seaborn Slash");
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 120;
            Projectile.height = 120;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 25;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            
            // Инициализация направления взмаха
            if (Projectile.timeLeft == 25)
            {
                swingDirection = player.direction * (player.GetModPlayer<CorrectSwing>().SwingChange % 2 == 0 ? 1 : -1);
                progress = 0f;
                scaleProgress = 0f;
                
                // Звук взмаха
                SoundEngine.PlaySound(SoundID.Item60 with { Pitch = 0.3f }, player.Center);
            }

            // Обновление прогресса анимации
            progress = 1f - (Projectile.timeLeft / 25f);
            
            // Анимация масштаба: увеличиваем, затем уменьшаем
            scaleProgress = EaseFunctions.EaseInOutSine(progress < 0.5f ? progress * 2f : (1f - progress) * 2f);
            
            // Базовый масштаб с анимацией
            float baseScale = 0.8f + scaleProgress * 0.6f;
            
            // Определение угла в зависимости от направления и прогресса
            float angle = CalculateSwingAngle(progress, swingDirection);
            
            // Длина "рукояти" от игрока
            float distance = 40f + progress * 30f;
            
            // Позиция проекции
            Vector2 offset = angle.ToRotationVector2() * distance;
            Projectile.Center = player.MountedCenter + offset;
            Projectile.rotation = angle + MathHelper.PiOver4;
            
            // Настройка масштаба
            Projectile.scale = baseScale * (0.7f + 0.3f * (float)Math.Sin(progress * MathHelper.Pi));
            
            // Обновление массива старых позиций для трейла
            UpdateOldPositions();
            
            // Синхронизация с игроком
            Projectile.spriteDirection = player.direction;
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            
            // Визуальные эффекты
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(50f, 50f) * Projectile.scale,
                    DustID.Flare_Blue,
                    Vector2.Zero,
                    0,
                    new Color(100, 150, 255, 100),
                    1f
                );
                dust.noGravity = true;
                dust.fadeIn = 1.2f;
            }
        }

        private float CalculateSwingAngle(float progress, int direction)
        {
            // Разные easing функции для разных фаз анимации
            float easedProgress;
            
            if (direction > 0)
            {
                // Взмах сверху вниз
                easedProgress = EaseFunctions.EaseInOutBack(progress);
                return MathHelper.Lerp(MathHelper.Pi * 0.7f, MathHelper.Pi * 0.1f, easedProgress);
            }
            else
            {
                // Взмах снизу вверх  
                easedProgress = EaseFunctions.EaseInOutBack(progress);
                return MathHelper.Lerp(MathHelper.Pi * 0.3f, MathHelper.Pi * 0.9f, easedProgress);
            }
        }

        private void UpdateOldPositions()
        {
            for (int i = oldPos.Length - 1; i > 0; i--)
            {
                oldPos[i] = oldPos[i - 1];
                oldRot[i] = oldRot[i - 1];
            }
            oldPos[0] = Projectile.Center;
            oldRot[0] = Projectile.rotation;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(0, texture.Height / 2f);
            
            // Рисуем трейл
            DrawTrail(texture, origin);
            
            // Рисуем основной спрайт
            Color drawColor = lightColor;
            drawColor.A = 150;
            
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                drawColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0
            );
            
            // Добавляем свечение
            DrawGlowEffect(texture, origin);
            
            return false;
        }

        private void DrawTrail(Texture2D texture, Vector2 origin)
        {
            for (int i = 0; i < oldPos.Length - 1; i++)
            {
                if (oldPos[i] == Vector2.Zero || oldPos[i + 1] == Vector2.Zero)
                    continue;
                    
                float progress = (float)i / oldPos.Length;
                float scale = Projectile.scale * (1f - progress) * 0.5f;
                Color trailColor = Color.Lerp(new Color(100, 150, 255, 100), new Color(50, 100, 200, 50), progress);
                
                Main.EntitySpriteDraw(
                    texture,
                    oldPos[i] - Main.screenPosition,
                    null,
                    trailColor * (1f - progress),
                    oldRot[i],
                    origin,
                    scale,
                    Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                    0
                );
            }
        }

        private void DrawGlowEffect(Texture2D texture, Vector2 origin)
        {
            Texture2D glowTexture = ModContent.Request<Texture2D>("Synergia/Content/Items/Weapons/Melee/Seaborn_Glow").Value;
            
            Color glowColor = Color.Lerp(Color.Cyan, Color.Blue, (float)Math.Sin(Main.GlobalTimeWrappedHourly * 4f) * 0.5f + 0.5f);
            glowColor.A = 0;
            
            float glowScale = Projectile.scale * (1.1f + 0.1f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 6f));
            
            Main.EntitySpriteDraw(
                glowTexture,
                Projectile.Center - Main.screenPosition,
                null,
                glowColor * 0.7f,
                Projectile.rotation,
                origin,
                glowScale,
                Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0
            );
        }


        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // Кастомная коллизия для лучшего попадания
            float collisionPoint = 0f;
            Vector2 start = Main.player[Projectile.owner].MountedCenter;
            Vector2 end = start + Projectile.rotation.ToRotationVector2() * 100f * Projectile.scale;
            
            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(), targetHitbox.Size(),
                start, end, 30f * Projectile.scale, ref collisionPoint
            );
        }
    }
}