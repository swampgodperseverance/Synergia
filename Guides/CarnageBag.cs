using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using ValhallaMod.Items.Weapons.Javelin;
using Synergia.Content.Items.Weapons.Throwing;
using static Terraria.ModLoader.ModContent;

namespace Synergiaanilla.Common.GlobalItems
{
	public class CarnageBag : GlobalItem
	{
		public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
		{
			if (item.ModItem != null && item.ModItem.Name == "PirateSquidBag")
			{
				itemLoot.Add(
					ItemDropRule.Common(
						ItemType<GoldGlove>(),
						chanceDenominator: 3,
						minimumDropped: 1,
						maximumDropped: 1
					)
				);
			}
		}
	}
}