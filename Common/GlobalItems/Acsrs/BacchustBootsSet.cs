using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace Vanilla.Common.GlobalItems.Accessories
{
	public class BacchusBootsEdit : GlobalItem
	{
		public override bool AppliesToEntity(Item item, bool lateInstantiation)
		{
			// Проверяем, что это предмет из мода Avalon с нужным названием
			return item.ModItem?.Mod?.Name == "Avalon" && item.ModItem?.Name == "BacchusBoots";
		}

		public override void UpdateAccessory(Item item, Player player, bool hideVisual)
		{
			// Добавляем +18% урона призывателя
			player.GetDamage(DamageClass.Summon) += 0.10f;
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			// Удаляем старую строку с +8%
			tooltips.RemoveAll(line => line.Text.Trim() == "8% increased summon damage");

			// Добавляем новую строку
			tooltips.Add(new TooltipLine(Mod, "VanillaBuffedSummon", "+18% summon damage")
			{
				OverrideColor = Microsoft.Xna.Framework.Color.LightGoldenrodYellow
			});
		}
	}
}