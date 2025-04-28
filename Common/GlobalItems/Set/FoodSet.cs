using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;
using System.Collections.Generic;
using StramsSurvival;

namespace Vanilla.Common.GlobalItems.Set
{
	public class FoodSet : GlobalItem
	{
		// Set Mods
		private static Mod roa = ModLoader.GetMod("RoA");

		// Set Changes
		public override void SetDefaults(Item item)
		{
			//RoA
			if (item.type == roa.Find<ModItem>("AlmondMilk").Type)
			{
				item.ApplyCookingRarityValue(7);
				// ApplyCookingRarityValue() - редкость продукта (стоимость продажи)
				// 0 - 10 меди
				// 1 - 25 меди
				// 2 - 70 меди
				// 3 - 1 серебро
				// 4 - 5 серебра
				// 5 - 20 серебра
				// 6 - 60 серебра
				// 7 - 80 серебра
				// 8 - 1 золото
				// 9 - 1 золото 15 серебра
				// 10- 1 золото 50 серебра
				item.QuickFood(19, 14, useTime: 96, tier: 4);
				//30 (hungerRestored) – сколько восстанавливает сытости
				//14 (thirstRestored) – сколько восстанавливает жажды
				//96 (useTime)        – время использования (в тиках, 60 = 1 секунда)
				//4 (tier)            – тир еды
				//Про тир еды
				//1, 2 = 1 тир
				//3, 4 = 2 тир
				//5 = 3 тир
				//Параметры какая это еда влияют на разные баффы, снизу все параметры
				//Можно указывать несколько = несколько баффов

				//item.makeDairy();     //Есть молоко
				//item.makeVegetable(); //Есть овощи
				//item.makeProtein();   //Есть белок(мясо и т.д)
				//item.makeFruit();     //Есть фрукты
				//item.makeGrain();     //Есть зерно(мучные продукты и т.д)
				//item.makeSugar();     //Есть сахар
				//item.makeAlcohol();   //Есть алкоголь

				//Дальше DefaultToFood обычно всегда прописан у еды, т.к это ванильный параметр
				//item.DefaultToFood(30, 26, 1, false, 17);
				//30 (healLife) – сколько здоровья восстанавливает
				//26 (useTime) – время использования (в тиках, 60 = 1 секунда)
				//1 (buffType) – тип баффа (например, BuffID.WellFed для сытости)
				//false - хз
				//17 (buffTime) – длительность баффа (в тиках, 60 * 60 = 1 минута) тут 61200 тиков = 17 минут
			}
		}
	}
}