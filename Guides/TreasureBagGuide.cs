using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Synergia.Content.Items.Weapons.Throwing;
using static Terraria.ModLoader.ModContent;

namespace Synergiaanilla.Common.GlobalItems
{
	public class CarnageBag : GlobalItem //drop modification inside a treasure bag
	{
		public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
		{
			if (item.ModItem != null && item.ModItem.Name == "PirateSquidBag") //internal name of treasure bag
			{
				itemLoot.Add(
					ItemDropRule.Common(
						ItemType<GoldGlove>(), //item that you want to add
						chanceDenominator: 3, // 100/3 = 33%
						minimumDropped: 1,
						maximumDropped: 1
					)
				);
			}
          // public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
       // {
            //if (item.type == ItemID.GolemBossBag) //vanilla bags
		}
	}
}
