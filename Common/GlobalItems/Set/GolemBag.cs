using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Avalon.Items.Material.OreChunks;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.GlobalItems
{
    public class GolemBag : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.GolemBossBag)
            {
                itemLoot.Add(
                    ItemDropRule.Common(
                        ItemType<CaesiumChunk>(),
                        chanceDenominator: 1,
                        minimumDropped: 60,
                        maximumDropped: 110
                    )
                );
            }
        }
    }
}