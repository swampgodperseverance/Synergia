using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace Synergia.Common
{
    public class LavaGradientRarity : ModRarity
    {
        public override Color RarityColor
        {
            get
            {
                float t = (float)(Main.GameUpdateCount % 120) / 120f;
                return Color.Lerp(new Color(255, 50, 30), new Color(255, 140, 30), (float)Math.Sin(t * MathHelper.TwoPi) * 0.5f + 0.5f);
            }
        }
    }
}
