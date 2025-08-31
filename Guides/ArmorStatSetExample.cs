using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;
using System.Collections.Generic;

namespace Synergia.Common.GlobalItems.Set
{
	public class ArmorStatSet : GlobalItem
	{
		private static Mod consolaria = ModLoader.GetMod("Consolaria"); //Write here ur mod like these examples
		private static Mod roa = ModLoader.GetMod("RoA");

		public override void UpdateEquip(Item item, Player player)
		{

			if (consolaria != null)
			{
				if (item.type == consolaria.Find<ModItem>("OstaraHat").Type)
				{	// most useful methods, we are not deleting old stats, we just add new ones
					player.GetDamage(DamageClass.Throwing) += 0.11f;
					player.GetCritChance(DamageClass.Throwing) += 5;	
					//player.GetAttackSpeed(DamageClass.Ranged) += 5f;
					//player.GetArmorPenetration(DamageClass.Throwing) += ThrowingArmorPenetration;
					//player.GetKnockback<ThrowingDamageClass>() += 5f;
					//player.GetModPlayer<YourStatBonusAccessoryPlayer>().YourStatBonusAccessory = true;
				}
				if (item.type == consolaria.Find<ModItem>("OstaraJacket").Type)
				{ //same
					player.GetAttackSpeed(DamageClass.Throwing) += 0.13f;
				}
				if (item.type == consolaria.Find<ModItem>("OstaraBoots").Type)
				{ // with functions from example mod for example
					player.jumpSpeedBoost += 2.4f;
	 							//player.waterWalk2 = true; // Allows walking on all liquids without falling into it
								//player.waterWalk = true; // Allows walking on water, honey, and shimmer without falling into it
								//player.iceSkate = true; // Grant the player improved speed on ice and not breaking thin ice when falling onto it
								//player.desertBoots = true; // Grants the player increased movement speed while running on sand
								//player.fireWalk = true; // Grants the player immunity from Meteorite and Hellstone tile damage
								//player.noFallDmg = true; // Grants the player the Lucky Horseshoe effect of nullifying fall damage
								//player.lavaRose = true; // Grants the Lava Rose effect
								//player.lavaMax += LavaImmunityTime * 60;
				}
				
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			void Add(string modName, string itemName, string tooltipKey)
			{
				Mod m = ModLoader.GetMod(modName);
				if (m != null && item.type == m.Find<ModItem>(itemName).Type)//add localization
					ReplaceTooltip(tooltips, "Tooltip0", Language.GetTextValue($"Mods.Synergia.{tooltipKey}"));//add localization
			}

			Add("Consolaria", "OstaraHat", "Items.OstaraHat.Tooltip");//add localization
			Add("Consolaria", "OstaraJacket", "Items.OstaraJacket.Tooltip");//add localization
			Add("Consolaria", "OstaraBoots", "Items.OstaraBoots.Tooltip");//add localization
   
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
