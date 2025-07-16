using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Vanilla.Content.Dusts
{
    public class MercuriumSparkDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = false;
            dust.scale = Main.rand.NextFloat(1.1f, 1.6f);
            dust.velocity *= 0.5f;  // Направление и скорость пыли
        }

        public override bool Update(Dust dust)
        {
            // Плавное мерцание
            Lighting.AddLight(dust.position, 0.2f, 0.3f, 0.7f); // голубовато-синее свечение

            dust.rotation += 0.1f; // Поворот пыли
            dust.scale *= 0.97f;   // Плавное уменьшение размера
            dust.velocity *= 0.96f; // Плавное замедление

            if (dust.scale < 0.5f)
                dust.active = false;  // Деактивировать пыль, если она слишком мала

            return false;
        }
    }
}