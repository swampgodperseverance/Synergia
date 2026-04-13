using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Content.Dusts
{
    public class Steam : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            // Анимация: 3 кадра
            int frame = Main.rand.Next(0, 3);
            dust.frame = new Rectangle(0, frame * 24, 24, 24);

            dust.noGravity = true;
            dust.noLight = false;

            // Начальные параметры
            dust.scale *= Main.rand.NextFloat(0.95f, 1.35f);
            dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);

            // Начальный цвет с небольшой вариацией
            dust.color = new Color(95, 95, 100, 0); // начинаем полностью прозрачными
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return dust.color;
        }

        public override bool Update(Dust dust)
        {
            // Плавное появление и исчезновение
            float lifeProgress = 1f - (float)dust.alpha / 255f;     // от 1.0 → 0.0
            float opacity = Utils.GetLerpValue(0f, 0.25f, lifeProgress, true); // быстрое появление

            // Базовый цвет дыма (холодный серо-бежевый)
            Color baseColor = new Color(72, 74, 78);

            // Освещение + прозрачность
            Color litColor = Lighting.GetColor((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f));

            dust.color = litColor.MultiplyRGB(baseColor) * opacity * 0.92f;
            dust.color.A = (byte)(110 * opacity); // контроль альфы отдельно для мягкости

            // Движение
            dust.position += dust.velocity * 0.85f;        // чуть медленнее, чем было

            // Постепенное замедление
            dust.velocity *= 0.96f;

            // Лёгкий подъём вверх + небольшой дрейф
            dust.velocity.Y -= 0.085f;
            dust.velocity.X *= 0.93f;

            // Случайное покачивание в стороны
            if (Main.rand.NextBool(5))
                dust.velocity.X += Main.rand.NextFloat(-0.12f, 0.12f);

            // Уменьшение размера
            dust.scale *= 0.978f;

            // Постепенное исчезновение
            dust.alpha += 2; // было 3 — стало мягче

            // Убираем пыль, когда она почти исчезла
            if (dust.alpha >= 245 || dust.scale < 0.35f)
                dust.active = false;

            return false;
        }
    }
}