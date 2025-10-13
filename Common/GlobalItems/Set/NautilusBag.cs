using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using ValhallaMod.Items.Weapons.Javelin;
using Synergia.Content.Items.Accessories;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.GlobalItems
{
	public class NautilusBag : GlobalItem
	{
		public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
		{
			if (item.ModItem != null && item.ModItem.Name == "DreadBag")
			{
				itemLoot.Add(
					ItemDropRule.Common(
						ItemType<BloodyNecklace>(),
						chanceDenominator: 2,
						minimumDropped: 1,
						maximumDropped: 1
					)
				);
			}
		}
	}
}