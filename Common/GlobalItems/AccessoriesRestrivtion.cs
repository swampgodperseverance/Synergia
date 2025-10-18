using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Accessories.PreHardmode;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalItems
{
    public class AccessoryRestriction : AccessoryСhanges
    {
        public override bool CanEquipAccessory(Item item, Player player, int slot, bool modded)
        {
            #region SummonerAccessory
            if (!DesiredType(item, player, ItemID.SummonerEmblem, ModContent.ItemType<SummonerScroll>())) return false;
            if (!DesiredType(item, player, ItemID.PapyrusScarab, ModContent.ItemType<SummonerScroll>())) return false;
            #endregion
            #region PygmyAccessory
            if (!DesiredType(item, player, ItemID.PygmyNecklace, ModContent.ItemType<PygmyShield>())) return false;
            #endregion
            return true; 
        }
    }
}