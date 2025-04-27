using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace Vanilla.Common.GlobalItems.Acsrs
{
	public class BacchusBootsEdit : GlobalItem
	{
		private static Mod avalon = ModLoader.GetMod("Avalon");

		public override void UpdateAccessory(Item item, Player player, bool hideVisual)
		{
			if (item.type == avalon.Find<ModItem>("BacchusBoots").Type)
			{
				player.GetDamage(DamageClass.Summon) += 0.10f; // increasing damage  +10%
			}
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			// Remove old toltip

			if (item.type == avalon.Find<ModItem>("BacchusBoots").Type)
			{
				tooltips.RemoveAll(line => line.Text.Trim() == "8% increased summon damage");
				tooltips.Add(new TooltipLine(Mod, "VanillaBuffedSummon", "18% increased summon damage")
				{ OverrideColor = Microsoft.Xna.Framework.Color.LightGoldenrodYellow });
			}
		}
	}
}
