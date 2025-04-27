using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;
using System.Collections.Generic;

namespace Vanilla.Common.GlobalItems.Set
{
	public class ArmorStatSet : GlobalItem
	{
		// Set Mods
		private static Mod consolaria = ModLoader.GetMod("Consolaria");
		private static Mod roa = ModLoader.GetMod("RoA");
		private static Mod valhalla = ModLoader.GetMod("ValhallaMod");

		// Set Changes
		public override void UpdateEquip(Item item, Player player)
		{
			//CONSOLARIA
			if (item.type == consolaria.Find<ModItem>("OstaraHat").Type)
			{
				player.GetDamage(DamageClass.Throwing) += 0.11f;
				player.GetCritChance(DamageClass.Throwing) += 5;
			}
			if (item.type == consolaria.Find<ModItem>("OstaraJacket").Type)
			{
				player.GetAttackSpeed(DamageClass.Throwing) += 0.13f;
			}
			if (item.type == consolaria.Find<ModItem>("OstaraBoots").Type)
			{
				player.jumpSpeedBoost += 2.4f;
			}

			//ROA
			if (item.type == roa.Find<ModItem>("SentinelHelmet").Type)
			{
				player.GetDamage(DamageClass.Throwing) += 0.15f;
				player.GetCritChance(DamageClass.Throwing) += 5;
			}
			if (item.type == roa.Find<ModItem>("SentinelBreastplate").Type)
			{
				player.GetAttackSpeed(DamageClass.Throwing) += 0.1f;
			}
			if (item.type == roa.Find<ModItem>("SentinelLeggings").Type)
			{
				player.moveSpeed += 0.2f;
			}

			//VALHALLA
			if (item.type == valhalla.Find<ModItem>("GreediestHead").Type)
			{
				player.endurance += 0.13f;
				player.GetDamage(DamageClass.Throwing) += 0.25f;
			}
			if (item.type == valhalla.Find<ModItem>("GreediestBody").Type)
			{
				player.endurance += 0.14f;
				player.GetCritChance(DamageClass.Throwing) += 8;
			}
			if (item.type == valhalla.Find<ModItem>("GreediestLegs").Type)
			{
				player.GetAttackSpeed(DamageClass.Throwing) += 0.09f;
			}
		}

		// Set Tooltip
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			//CONSOLARIA
			if (item.type == consolaria.Find<ModItem>("OstaraHat").Type)
			{
				ReplaceTooltip(tooltips, "Tooltip0", "11% Increased throwing damage and crit chance by 5%");
			}
			if (item.type == consolaria.Find<ModItem>("OstaraJacket").Type)
			{
				ReplaceTooltip(tooltips, "Tooltip0", "Throwing speed increased by 13% ");
			}
			if (item.type == consolaria.Find<ModItem>("OstaraBoots").Type)
			{
				ReplaceTooltip(tooltips, "Tooltip0", "Jump height increased");
			}

			//ROA
			if (item.type == roa.Find<ModItem>("SentinelHelmet").Type)
			{
				ReplaceTooltip(tooltips, "Tooltip0", "15% Increased throwing damage and crit chance by 5%");
			}
			if (item.type == roa.Find<ModItem>("SentinelBreastplate").Type)
			{
				ReplaceTooltip(tooltips, "Tooltip0", "Throwing speed increased by 10% ");
			}
			if (item.type == roa.Find<ModItem>("SentinelLeggings").Type)
			{
				ReplaceTooltip(tooltips, "Tooltip0", "Running speed increased by 20%");
			}
			
			//VALHALLA
			if (item.type == valhalla.Find<ModItem>("GreediestHead").Type)
			{
				ReplaceTooltip(tooltips, "Tooltip0", "14% more damage resistance and 25% throwing damage");
			}
			if (item.type == valhalla.Find<ModItem>("GreediestBody").Type)
			{
				ReplaceTooltip(tooltips, "Tooltip0", "13% more damage resistance and +8 crit");
			}
			if (item.type == valhalla.Find<ModItem>("GreediestLegs").Type)
			{
				ReplaceTooltip(tooltips, "Tooltip0", "+9% throwing speed ");
			}
		}

		private void ReplaceTooltip(List<TooltipLine> tooltips, string tooltipName, string newText)
		{
			for (int i = 0; i < tooltips.Count; i++)
			{
				if (tooltips[i].Name == tooltipName && tooltips[i].Mod == "Terraria")
				{
					tooltips[i].Text = newText;
					break;
				}
			}
		}
	}
}