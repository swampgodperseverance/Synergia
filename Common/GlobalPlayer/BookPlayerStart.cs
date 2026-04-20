using Bismuth.Content.Items.Other;
using Synergia.Content.Items;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Synergia.Common.ModPlayers
{
    public class VanillaPlayer : ModPlayer
    {
        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath) {
            if (!mediumCoreDeath) {
                yield return new Item(ItemType<OldTales>());
            }
        }
        public override void ModifyStartingInventory(IReadOnlyDictionary<string, List<Item>> itemsByMod, bool mediumCoreDeath) {
            itemsByMod["Terraria"].RemoveAll(item => item.type == ItemID.CopperShortsword);
            itemsByMod["Terraria"].Insert(0, new Item(ItemType<ClassEngraving>()));
            itemsByMod["Bismuth"].RemoveAll(item => item.type == ItemType<ClassEngraving>());
        }
    }
}