using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.AI;
using Vanilla.Content.Dusts;
using static Terraria.ModLoader.ModContent;

namespace Vanilla.Content.Projectiles.Aura
{
    [ExtendsFromMod("ValhallaMod")]
    public class MercuriumAura : AuraAI
    { 
        public override void SetDefaults()
        {
            Projectile.alpha = 255;
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = false;

            distanceMax = 160f;

            // Баффы союзникам
            buffType2 = BuffID.ManaRegeneration;

            // Дебафф врагам
            debuffType = BuffID.Electrified;
            debuffType2 = 0;

            // Отключение стрельбы
            shootSpawnStyle = AuraShootStyles.None;
            shootType = 0;
            shootCount = 0;
            shootSpeed = 0f;
            shootCooldown = 0;
            shootDirectionStyle = AuraShootDirection.Random;
            shootStep = 0f;

            // Визуальные настройки
            orbCount = 12;
            orbSpeed = 0.2f;
            orbTrailCount = 10;
            orbTrailGap = 0.01f;

            auraColor = new Color(20, 40, 255, 150); // тёмно-синий
            auraColor2 = new Color(0, 0, 100, 150);

            spectreCut = true;
            spectreCutDust = ModContent.DustType<MercuriumSparkDust>();
            spectreCutDustScale = 4.0f;
            spectreCutCooldown = 20;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
