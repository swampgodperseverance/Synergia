using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ValhallaMod.Projectiles.AI;

namespace Vanilla.Content.Projectiles.Aura
{
    [ExtendsFromMod("ValhallaMod")]
    public class UnderwaterAuraScytheSupport : AuraAI
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            
            
            Projectile.friendly = false; 
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.alpha = 100; // Делаем более видимым

            // Только баффы (без дебаффов)
            buffType = BuffID.Calm;       // Спокойствие (уменьшает агрессию мобов)
            buffType2 = BuffID.Fishing;   // Улучшенная рыбалка
            debuffType = 0;               // Отключаем дебаффы
            debuffType2 = 0;

            // Отключаем атакующие параметры
            shootSpawnStyle = AuraShootStyles.None;
            shootType = 0;               // Нет выстрелов
            shootSpeed = 0f;
            spectreCut = false;           // Отключаем атакующие эффекты

            // Увеличиваем параметры ауры
            distanceMax = 200f;          // Больший радиус
            orbCount = 16;               // Больше частиц
            orbSpeed = 0.15f;            // Медленное движение
            
            // Мягкие голубые тона
            auraColor = new Color(70, 130, 230, 120); 
            auraColor2 = new Color(30, 80, 180, 150);  
        }

        public override void AI()
        {
            base.AI();
            
            // Дополнительные визуальные эффекты (пузырьки)
            if (Main.rand.NextBool(15))
            {
                Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(distanceMax, distanceMax), 
                    DustID.Water, 
                    Vector2.Zero, 
                    150, 
                    default, 
                    1f
                );
            }
        }
    }
}