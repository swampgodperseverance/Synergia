using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Weapons.Ranged
{
	public class ArmsDealerShop : GlobalNPC
	{
		public override void ModifyShop(NPCShop shop)
		{

			if (shop.NpcType == 19)
			{
				shop.Add<Blunderbussin>(new Condition[]
				{
					Condition.DownedPlantera
				});
			}
		}
	}
}
