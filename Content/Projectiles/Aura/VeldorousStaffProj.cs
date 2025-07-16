using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ValhallaMod.Projectiles.AI;
using static Terraria.ModLoader.ModContent;

namespace Vanilla.Content.Projectiles.Aura
{
    [ExtendsFromMod("ValhallaMod")]
    public class VeldorousStaffProj : AuraAI
    {
        public override void SetDefaults()
        {
            Projectile.alpha = 255;
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = false;
            Projectile.damage = 10; // низкий урон

            // Цвет ауры
            auraColor = new Color(0, 80, 0, 160);
            auraColor2 = new Color(0, 40, 0, 120); // дополнительный цвет (можно убрать)

            // Радиус ауры
            distanceMax = 240f;

            // Орбы вокруг ауры
            orbCount = 6;
            orbSpeed = 1.2f;
            orbTrailCount = 10;
            orbTrailGap = 0.02f;

            // Отключаем выстрелы
            shootSpawnStyle = AuraShootStyles.None;

            // Бонусы для игроков
            buffType = BuffID.Regeneration;
            buffType2 = BuffID.ManaRegeneration;

            // Дебаффов нет — можно оставить 0 или удалить строки

            // Spectre cut: отключён
            spectreCut = false;
        }

        public override void PostAI()
        {
            foreach (Player player in Main.player)
            {
                if (player.active && Vector2.Distance(player.Center, Projectile.Center) < distanceMax)
                {
                    player.AddBuff(BuffID.Regeneration, 10);
                    player.AddBuff(BuffID.ManaRegeneration, 10);
                    player.statDefense += 7;

                    Mod RoA = ModLoader.GetMod("RoA");
                    if (RoA != null)
                    {
                        int resilienceBuffType = RoA.Find<ModBuff>("Resilience").Type;
                        player.AddBuff(resilienceBuffType, 10);
                    }
                }
            }
        }
    }
}