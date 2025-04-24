using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace Vanilla.Common.GlobalItems.Acsrs
{
	public class BacchusBootsEdit : GlobalItem
	{
		public override bool AppliesToEntity(Item item, bool lateInstantiation)
		{
			// find this boots
			return item.ModItem?.Mod?.Name == "Avalon" && item.ModItem?.Name == "BacchusBoots";
		}

		public override void UpdateAccessory(Item item, Player player, bool hideVisual)
		{
				// increasing damage  +10%
			player.GetDamage(DamageClass.Summon) += 0.10f;
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			// Remove old toltip
			tooltips.RemoveAll(line => line.Text.Trim() == "8% increased summon damage");

			// 10+8=18, i'm not deleted old 8% damage
			tooltips.Add(new TooltipLine(Mod, "VanillaBuffedSummon", "+18% summon damage")
			{
				OverrideColor = Microsoft.Xna.Framework.Color.LightGoldenrodYellow
			});
		}
	}
}
