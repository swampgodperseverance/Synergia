using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Synergia
{
    public class MyPlayer : ModPlayer
    {
        public int SwordCombo;
        public int SwordComboSlash;
        public int SwordComboR;

        public override void PostUpdate()
        {
            if (SwordComboR <= 0)
            {
                SwordCombo = 0;
                SwordComboR = 0;
            }
            else
            {
                SwordComboR--;
            }
        }
    }
}
