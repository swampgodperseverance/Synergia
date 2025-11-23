using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Avalon.Items.Accessories.PreHardmode; 

namespace Synergia.Common.GlobalItems
{
    public class PygmyAccessoryRestriction : GlobalItem
    {
        public override bool CanEquipAccessory(Item item, Player player, int slot, bool modded)
        {
            if (item.type == ItemID.PygmyNecklace)
            {
                for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
                {
                    Item equippedItem = player.armor[i];
                    if (equippedItem != null && equippedItem.type == ModContent.ItemType<PygmyShield>())
                    {
                        return false; 
                    }
                }
            }

            if (item.type == ModContent.ItemType<PygmyShield>())
            {
                for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
                {
                    Item equippedItem = player.armor[i];
                    if (equippedItem != null && equippedItem.type == ItemID.PygmyNecklace)
                    {
                        return false; 
                    }
                }
            }

            return true;
        }
    }
}
