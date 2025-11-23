using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace Synergia.Common
{
    public class CoreburnedRarity : ModRarity
    {
        public override Color RarityColor
        {
            get
            {
                // Базовый параметр времени (цикл около 4 секунд)
                float t = (float)(Main.GameUpdateCount % 240) / 240f;

                // Многослойное дыхание — основная волна и более быстрая пульсация
                float basePulse = (float)(Math.Sin(t * MathHelper.TwoPi) * 0.5f + 0.5f);
                float microPulse = (float)(Math.Sin(t * MathHelper.TwoPi * 3f) * 0.2f + 0.8f);
                float combined = MathHelper.Clamp(basePulse * microPulse, 0f, 1f);

                // Базовые цвета
                Color ashCold = new Color(90, 85, 95);     // холодный серо-фиолетовый пепел
                Color emberDim = new Color(160, 70, 60);   // тусклый красно-коричневый жар
                Color emberGlow = new Color(255, 130, 70); // короткая вспышка лавы

                // Плавный переход между слоями — пепел оживает изнутри
                Color emberBlend = Color.Lerp(emberDim, emberGlow, (float)Math.Pow(combined, 1.5f));
                Color ashenGlow = Color.Lerp(ashCold, emberBlend, combined * 0.9f);

                // Имитация дыхания света — лёгкое “мерцание” яркости
                float brightness = 0.85f + 0.15f * (float)Math.Sin(t * MathHelper.TwoPi * 0.5f + 1f);
                Color final = new Color(
                    (int)(ashenGlow.R * brightness),
                    (int)(ashenGlow.G * brightness),
                    (int)(ashenGlow.B * brightness)
                );

                return final;
            }
        }
    }
}
