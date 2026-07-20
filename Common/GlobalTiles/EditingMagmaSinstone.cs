using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalTiles
{
    public class EditingMS : GlobalTile
    {
        private int msType;

        public override void Load()
        {
            msType = ModContent.Find<ModTile>("ValhallaMod", "SinstoneMagma").Type;
        }

        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            if (type == msType)
            {
                Player player = Main.LocalPlayer;   
                if (player.HeldItem.pick < 205)
                {
                    blockDamaged = false;
                    return false;
                }
            }

            return base.CanKillTile(i, j, type, ref blockDamaged);
        }
    }
}