using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;
using System.Collections.Generic;
using ValhallaMod; // AuraPlayer
using ValhallaMod.Projectiles.AI;
using static ValhallaMod.Projectiles.AI.AuraAI;
using Avalon.Buffs;

namespace Synergia.Common.GlobalItems.Set
{
	public class AccessoryStatSet : GlobalItem
	{
		private static Mod consolaria = ModLoader.GetMod("Consolaria");
		private static Mod roa = ModLoader.GetMod("RoA");
		private static Mod horizons = ModLoader.GetMod("NewHorizons");
		private static Mod valhalla = ModLoader.GetMod("ValhallaMod");
		private static Mod avalon = ModLoader.GetMod("Avalon");
		private static Mod bismuth = ModLoader.GetMod("Bismuth");

		public override void UpdateEquip(Item item, Player player)
		{
			var auraPlayer = player.GetModPlayer<AuraPlayer>();

			//Bismuth
			if (bismuth != null) 
			{
				if (item.type == bismuth.Find<ModItem>("BerserksRing").Type)
				{
					player.GetDamage(DamageClass.Generic) -= 0.20f;
				}

			}
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			void Add(string modName, string itemName, string tooltipKey)
			{
				Mod m = ModLoader.GetMod(modName);
				if (m != null && item.type == m.Find<ModItem>(itemName).Type)
					ReplaceTooltip(tooltips, "Tooltip0", Language.GetTextValue($"Mods.Synergia.{tooltipKey}"));
			}

			Add("Bismuth", "BerserksRing", "Items.BerserksRing.Tooltip");
		}

		private void ReplaceTooltip(List<TooltipLine> tooltips, string tooltipName, string newText)
		{
			foreach (var line in tooltips)
			{
				if (line.Name == tooltipName && line.Mod == "Terraria")
				{
					line.Text = newText;
					break;
				}
			}
		}
	}
}