using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;
using System.Collections.Generic;
using StramsSurvival;

namespace Synergia.Common.GlobalItems.Set
{
	public class FoodSet : GlobalItem
	{
		// Set Mods
		private static Mod roa = ModLoader.GetMod("RoA");
		private static Mod consolaria = ModLoader.GetMod("Consolaria");
		private static Mod avalon = ModLoader.GetMod("Avalon");
		private static Mod newhorizons = ModLoader.GetMod("NewHorizons");
		private static Mod valhalla = ModLoader.GetMod("ValhallaMod");

		// Set Changes
		public override void SetDefaults(Item item)
		{
			//RoA
			if (item.type == roa.Find<ModItem>("AlmondMilk").Type)
			{
				item.ApplyCookingRarityValue(5);
				// ApplyCookingRarityValue() - Item Value (Cost)
				// 0 - 10 copper coins
				// 1 - 25 copper coins
				// 2 - 70 copper coins
				// 3 - 1 silver coin
				// 4 - 5 silver coins
				// 5 - 20 silver coins
				// 6 - 60 silver coins
				// 7 - 80 silver coins
				// 8 - 1 golden coin
				// 9 - 1 golden coin 15 silver coins
				// 10- 1 golden coin 50 silver coins
				item.QuickFood(0, 7, useTime: 96, tier: 3);
				item.makeDairy();
				//0 (hungerRestored) – how much hunger restored
				//7 (thirstRestored) – how much thirst restored
				//96 (useTime)        – use time (in ticks, 60 = 1 sec)
				//3 (tier)            – food tier
				//food tier = buff level
				//1, 2 = 1 tier (1 level)
				//3, 4 = 2 tier (2 level)
				//5 = 3 tier (3 level)
				//Parametres below, all have their own buffs
				//2 or more parametres = 2 or more buffs

				//item.makeDairy();     
				//item.makeVegetable();
				//item.makeProtein();   
				//item.makeFruit();     
				//item.makeGrain();     
				//item.makeSugar();     
				//item.makeAlcohol();  

				//next DefaultToFood it's a vanilla parametre. Must be written by default in all foood
				//item.DefaultToFood(30, 26, 1, false, 17);
				//30 (healLife) – how much heals
				//26 (useTime) – use time (in ticks, 60 = 1 sec)
				//1 (buffType) – Buff type (Ex., BuffID.WellFed )
				//false - idk lol
				//17 (buffTime) – how long buff (in ticks, 60 = 1 sec) here 61200 ticks = 17 minutes
			}
			if (item.type == roa.Find<ModItem>("Almond").Type)
			{
				item.ApplyCookingRarityValue(4);
				item.QuickFood(4, 0, useTime: 96, tier: 1);
				item.makeFruit();
			}
			if (item.type == roa.Find<ModItem>("Cloudberry").Type)
			{
				item.ApplyCookingRarityValue(3);
				item.QuickFood(3, 0, useTime: 96, tier: 1);
				item.makeFruit();
			}
			if (item.type == roa.Find<ModItem>("SherwoodShake").Type)
			{
				item.ApplyCookingRarityValue(6);
				item.QuickFood(0, 9, useTime: 96, tier: 5);
				item.makeAlcohol();
				item.makeSugar();
			}
   		}
	}
}
