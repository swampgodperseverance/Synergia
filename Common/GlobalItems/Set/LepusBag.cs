using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using ValhallaMod.Items.Weapons.Javelin;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.GlobalItems
{
	public class LepusBag : GlobalItem
	{
		public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
		{
			if (item.ModItem != null && item.ModItem.Name == "LepusBag")
			{
				itemLoot.Add(
					ItemDropRule.Common(
						ItemType<CarrotDagger>(),
						chanceDenominator: 2,
						minimumDropped: 150,
						maximumDropped: 150
					)
				);
			}
		}
	}
}