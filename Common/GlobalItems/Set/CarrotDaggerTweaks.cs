using Terraria;
using Terraria.ModLoader;

namespace Vanilla.Common.GlobalItems
{
	public class CarrotDaggerTweaks : GlobalItem
	{
		public override void SetDefaults(Item item)
		{
			if (item.ModItem != null && item.ModItem.Name == "CarrotDagger")
			{
				item.damage = 15;
			}
		}
	}
}