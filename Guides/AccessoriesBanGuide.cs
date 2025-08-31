using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Avalon.Items.Accessories.Hardmode;

namespace Synergia.Common.GlobalItems
{
    public class SummonerAccessoryRestriction : GlobalItem //perfect for balancing options, when in craft trees player cant equip old imbalance accessories
    {
        public override bool CanEquipAccessory(Item item, Player player, int slot, bool modded)
        {

            if (item.type == ItemID.SummonerEmblem) // here summoner emblem
            {

                for (int i = 3; i < 8 + player.extraAccessorySlots; i++) 
                {
                    Item equippedItem = player.armor[i]; // equip checker
                    if (equippedItem != null && equippedItem.type == ModContent.ItemType<SummonerScroll>()) // ban
                    {
                        return false; 
                    }
                }
            }

            if (item.type == ModContent.ItemType<SummonerScroll>()) // same here but vice versa
            {
                for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
                {
                    Item equippedItem = player.armor[i];
                    if (equippedItem != null && equippedItem.type == ItemID.SummonerEmblem)
                    {
                        return false; 
                    }
                }
            }

            if (item.type == ModContent.ItemType<SummonerScroll>()) //etc
            {
                for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
                {
                    Item equippedItem = player.armor[i];
                    if (equippedItem != null && equippedItem.type == ItemID.PapyrusScarab)
                    {
                        return false; 
                    }
                }
            }
            if (item.type == ItemID.PapyrusScarab)
            {
                for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
                {
                    Item equippedItem = player.armor[i];
                    if (equippedItem != null && equippedItem.type == ModContent.ItemType<SummonerScroll>())
                    {
                        return false; 
                    }
                }
            }

            return true; 
        }
    }
}

