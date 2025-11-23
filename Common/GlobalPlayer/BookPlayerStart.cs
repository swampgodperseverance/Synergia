using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Synergia.Content.Items;

namespace Synergia.Common.ModPlayers
{
    public class VanillaPlayer : ModPlayer
    {
        private bool receivedOldTales = false;

        public override void OnEnterWorld()
        {
            if (!receivedOldTales)
            {
                int itemType = ModContent.ItemType<OldTales>();
                Player.QuickSpawnItem(null, itemType, 1);
                receivedOldTales = true;
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["receivedOldTales"] = receivedOldTales;
        }

        public override void LoadData(TagCompound tag)
        {
            receivedOldTales = tag.GetBool("receivedOldTales");
        }
    }
}
