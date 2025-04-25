using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace Vanilla
{
	public class Vanilla : GlobalItem
	{
		string internalName = "";

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (item.ModItem != null)
			{
				internalName = item.ModItem.Name;
			}
			else
			{
			}

			tooltips.Add(new TooltipLine(Mod, "InternalName", $"[i:{item.type}] [c/FFFF00:{internalName} (ID: {item.type})]"));
		}

		public override bool InstancePerEntity => true;
	}
}
