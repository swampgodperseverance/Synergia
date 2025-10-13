using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Synergia.Common.GlobalPlayer
{
    public class CommandPlayer : ModPlayer
    {
        public bool ViewCoordinates = false;
        public int Timer = 0;

        public override void PostUpdate()
        {
            Point mousePos = Main.MouseScreen.ToPoint();
            if (ViewCoordinates)
            {
                if (Timer == 0) Main.NewText($"X: {mousePos.X}, Y {mousePos.Y}");
                if (Timer > 0) Timer--;
            }
        }
    }
}