using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalTiles
{
    public class EditingMercuiriumOre : GlobalTile
    {
        private int mercuriumType;

        public override void Load()
        {
            mercuriumType = ModContent.Find<ModTile>("RoA", "MercuriumOre").Type;
        }

        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            if (type == mercuriumType)
            {
                Player player = Main.LocalPlayer;   
                if (player.HeldItem.pick < 55)
                {
                    blockDamaged = false;
                    return false;
                }
            }

            return base.CanKillTile(i, j, type, ref blockDamaged);
        }
    }
}