using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ValhallaMod.Projectiles.AI;

namespace Vanilla.Content.Projectiles.Aura
{
    public class UnderwaterAuraScytheAttack : AuraAI
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            
            // Настройки урона
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Summon;

            // Только дебаффы
            buffType = 0;
            buffType2 = 0;
            debuffType = BuffID.Weak;
            debuffType2 = BuffID.Chilled;

            // Атакующие параметры
            shootSpawnStyle = AuraShootStyles.None;
            shootType = ProjectileID.WaterStream;
            shootSpeed = 6f;

            // Визуальные настройки
            auraColor = new Color(80, 120, 255, 150);
            auraColor2 = new Color(0, 80, 160, 150);
            spectreCut = true;
        }
    }
}