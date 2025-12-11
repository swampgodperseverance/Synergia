using Microsoft.Xna.Framework;

namespace Synergia.Common.Rarities {
    public class NULLItemRarity : BaseRarity {
        static Color color = new(100, 54, 75);
        static Color color2 = new(178, 82, 102);
        static Color color3 = new(226, 114, 133);

        static Color color4 = new(89, 207, 147);
        static Color color5 = new(66, 164, 89);
        static Color color6 = new(61, 111, 67);

        static Color color7 = new(100, 135, 140);
        static Color color8 = new(138, 196, 195);
        static Color color9 = new(175, 233, 223);

        static Color color10 = new(192, 144, 169);
        static Color color11 = new(169, 104, 136);
        static Color color12 = new(101, 73, 86);

        static readonly Color[] colors = [color, color2, color3, color4, color5, color6, color7, color8, color9, color10, color11, color12];

        public static Color GetColor() {
            return AnimatedColor(colors, 45);
        }
    }
}
