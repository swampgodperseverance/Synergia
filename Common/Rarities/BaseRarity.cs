using Terraria;
using Microsoft.Xna.Framework;

namespace Synergia.Common.Rarities {
    public class BaseRarity {
        public static Color AnimatedColor(Color[] colors, byte time = 60) {
            int transitionTime = time;
            int colorCount = colors.Length;
            int totalTime = transitionTime * colorCount;

            int timer = (int)(Main.GameUpdateCount % totalTime);
            int index = timer / transitionTime;
            float t = timer % transitionTime / (float)transitionTime;

            Color from = colors[index % colorCount];
            Color to = colors[(index + 1) % colorCount];

            return Color.Lerp(from, to, t);
        }
    }
}