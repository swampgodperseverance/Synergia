using Synergia.Content.Items;
using System.Collections.Generic;
using Terraria;

namespace Synergia.Common.ModPlayers
{
    public class VanillaPlayer : ModPlayer
    {
        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath) {
            if (!mediumCoreDeath) {
                yield return new Item(ItemType<OldTales>());
            }
        }
    }
}