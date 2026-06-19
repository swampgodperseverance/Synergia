// Code by SerNik
using Avalon.Tiles.Furniture;
using Avalon.Tiles.Furniture.Coughwood;
using Avalon.Tiles.Herbs;
using Bismuth.Content.Items.Other;
using Bismuth.Content.Tiles;
using Bismuth.Utilities;
using Synergia.Content.Items.Weapons.Melee;
using Synergia.Content.Items.Weapons.Ranged;
using Synergia.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.WorldBuilding;
using ValhallaMod.Items.Accessory;
using ValhallaMod.Items.Accessory.Shield;
using ValhallaMod.Tiles.Furnitures;
using static Synergia.Common.ModSystems.WorldGens.SynergiaGenVars;

namespace Synergia.Common.ModSystems.WorldGens.HoodlumsHeadquarter {
    public class SettingHeadquarter : BaseWorldGens {
        bool swampGen;
        static int chestValue = 0;

        public override string NameGen => "[Synergia] Setting struct in swamp";
        public override int Index => 1;
        public override bool GensBool { get => swampGen; set => swampGen = value; }
        public override bool Do_MakeGen(GenerationProgress progress) => HeadquarterLayerOne.GenHeadquarter(progress); // BismuthWorld.WorldSize != 1 ? HeadquarterLayerOne.GenHeadquarter(progress) : SwampCave.GenCave(progress);
        public override void PostWorldGen() {
            //Goblen
            WorldGen.PlaceObject(HLOX - 70, HLTY + 33, 105, style: 5, direction: +1);
            WorldGen.PlaceObject(HLOX - 53, HLTY + 38, 105, style: 5, direction: +1);
            WorldGen.PlaceObject(HLOX - 05, HLTY + 34, 105, style: 5, direction: -1);
            WorldGen.PlaceObject(HLOX + 03, HLTY + 34, 105, style: 5, direction: +1);
            WorldGen.PlaceObject(HLOX + 51, HLTY + 38, 105, style: 5, direction: -1);
            WorldGen.PlaceObject(HLOX + 68, HLTY + 33, 105, style: 5, direction: -1);
            WorldGen.PlaceObject(HLOX + 2, HLTY + 13, TileID.Lamps);
            WorldGen.PlaceObject(HLOX + 1, HLTY + 13, 105, style: 5, direction: +1);
            WorldGen.PlaceObject(HLOX - 3, HLTY + 13, 105, style: 5, direction: -1);
            WorldGen.PlaceObject(HLOX - 5, HLTY + 13, TileID.Lamps);

            // Brass Lantern
            WorldGen.PlaceObject(HLOX - 83, HLTY + 28, 42, style: 1, direction: -1);
            WorldGen.PlaceObject(HLOX - 39, HLTY + 17, 42, style: 1, direction: -1);
            WorldGen.PlaceObject(HLOX - 31, HLTY + 31, 42, style: 1, direction: -1);
            WorldGen.PlaceObject(HLOX - 27, HLTY + 27, 42, style: 1, direction: -1);
            WorldGen.PlaceObject(HLOX - 05, HLTY + 16, 42, style: 1, direction: -1);
            WorldGen.PlaceObject(HLOX - 07, HLTY + 32, 42, style: 1, direction: -1);
            WorldGen.PlaceObject(HLOX + 04, HLTY + 32, 42, style: 1, direction: -1);
            WorldGen.PlaceObject(HLOX + 05, HLTY + 25, 42, style: 1, direction: -1);
            WorldGen.PlaceObject(HLOX + 15, HLTY + 27, 42, style: 1, direction: -1);
            WorldGen.PlaceObject(HLOX + 30, HLTY + 30, 42, style: 1, direction: -1);
            WorldGen.PlaceObject(HLOX + 80, HLTY + 27, 42, style: 1, direction: -1);

            // Hanging Pots && Hanging Waterleaf
            WorldGen.PlaceObject(HLOX - 34, HLTY + 27, (ushort)TileType<HangingPots>(), style: 0, direction: -1);
            WorldGen.PlaceObject(HLOX - 25, HLTY + 31, (ushort)TileType<HangingPots>(), style: 0, direction: -1);
            WorldGen.PlaceObject(HLOX - 03, HLTY + 23, (ushort)TileType<HangingPots>(), style: 0, direction: -1);
            WorldGen.PlaceObject(HLOX + 17, HLTY + 27, (ushort)TileType<HangingPots>(), style: 0, direction: -1);
            WorldGen.PlaceObject(HLOX + 26, HLTY + 30, (ushort)TileType<HangingPots>(), style: 0, direction: -1);
            WorldGen.PlaceObject(HLOX + 10, HLTY + 10, (ushort)TileType<HangingPots>(), style: 0, direction: -1);
            WorldGen.PlaceObject(HLOX - 26, HLTY + 12, (ushort)TileType<HangingPots>(), style: 0, direction: -1);
            WorldGen.PlaceObject(HLOX - 23, HLTY + 27, TileID.PotsSuspended, style: 3, direction: -1);
            WorldGen.PlaceObject(HLOX - 07, HLTY + 23, TileID.PotsSuspended, style: 3, direction: -1);
            WorldGen.PlaceObject(HLOX - 03, HLTY + 23, TileID.PotsSuspended, style: 3, direction: -1);
            WorldGen.PlaceObject(HLOX + 07, HLTY + 25, TileID.PotsSuspended, style: 3, direction: -1);
            WorldGen.PlaceObject(HLOX + 34, HLTY + 30, TileID.PotsSuspended, style: 3, direction: -1);

            // 3X2
            WorldGen.Place3x2(HLOX - 24, HLTY + 25, (ushort)TileType<ContagionCampfire>(), 0);
            WorldGen.Place3x2(HLOX + 30, HLTY + 28, (ushort)TileType<ContagionCampfire>(), 0);

            // Coughwood Work Bench
            WorldGen.Place2x1(HLOX + 35, HLTY + 14, (ushort)TileType<CoughwoodWorkBench>(), style: 0);
            WorldGen.Place2x1(HLOX + 26, HLTY + 16, (ushort)TileType<CoughwoodWorkBench>(), style: 0);
            WorldGen.Place2x1(HLOX + 20, HLTY + 16, (ushort)TileType<CoughwoodWorkBench>(), style: 0);
            WorldGen.Place2x1(HLOX + 10, HLTY + 15, (ushort)TileType<CoughwoodWorkBench>(), style: 0);
            WorldGen.Place2x1(HLOX + 05, HLTY + 14, (ushort)TileType<CoughwoodWorkBench>(), style: 0);
            WorldGen.Place2x1(HLOX - 09, HLTY + 14, (ushort)TileType<CoughwoodWorkBench>(), style: 0);
            WorldGen.Place2x1(HLOX - 16, HLTY + 15, (ushort)TileType<CoughwoodWorkBench>(), style: 0);
            WorldGen.Place2x1(HLOX - 24, HLTY + 16, (ushort)TileType<CoughwoodWorkBench>(), style: 0);
            WorldGen.Place2x1(HLOX - 36, HLTY + 15, (ushort)TileType<CoughwoodWorkBench>(), style: 0);
            WorldGen.Place2x1(HLOX - 33, HLTY + 16, (ushort)TileType<CoughwoodWorkBench>(), style: 0);

            // OnTable
            WorldGen.PlaceOnTable1x1(HLOX + 35, HLTY + 13, TileID.Bottles, style: 5);
            WorldGen.PlaceOnTable1x1(HLOX + 36, HLTY + 13, TileID.Candles, style: 0);
            WorldGen.PlaceOnTable1x1(HLOX + 20, HLTY + 15, TileID.Candles, style: 0);
            WorldGen.PlaceOnTable1x1(HLOX + 10, HLTY + 14, TileID.Bottles, style: 4);
            WorldGen.PlaceOnTable1x1(HLOX + 11, HLTY + 14, TileID.Bottles, style: 5);
            WorldGen.PlaceOnTable1x1(HLOX + 05, HLTY + 13, TileID.Candles, style: 0);
            WorldGen.PlaceOnTable1x1(HLOX + 06, HLTY + 13, TileID.Bottles, style: 5);
            WorldGen.PlaceOnTable1x1(HLOX - 08, HLTY + 13, TileID.Bottles, style: 4);
            WorldGen.PlaceOnTable1x1(HLOX - 15, HLTY + 14, TileID.Candles, style: 0);
            WorldGen.PlaceOnTable1x1(HLOX - 24, HLTY + 15, TileID.WaterCandle, style: 0);
            WorldGen.PlaceOnTable1x1(HLOX - 23, HLTY + 15, TileID.Bottles, style: 5);
            WorldGen.PlaceOnTable1x1(HLOX - 36, HLTY + 14, TileID.Candles, style: 0);
            WorldGen.PlaceOnTable1x1(HLOX - 35, HLTY + 14, TileID.Bottles, style: 5);
            WorldGen.PlaceOnTable1x1(HLOX - 32, HLTY + 15, TileID.Bottles, style: 5);

            // 1X1
            WorldGen.PlaceTile(HLOX + 30, HLTY + 10, TileID.FoodPlatter, style: 0);
            WorldGen.PlaceTile(HLOX + 31, HLTY + 10, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 26, HLTY + 15, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 27, HLTY + 15, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 28, HLTY + 16, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 28, HLTY + 15, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 27, HLTY + 09, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 28, HLTY + 09, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 23, HLTY + 10, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 23, HLTY + 09, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 22, HLTY + 10, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 18, HLTY + 10, (ushort)TileType<BarfbushPlanterBox>(), style: 0);
            WorldGen.PlaceTile(HLOX + 17, HLTY + 10, (ushort)TileType<BarfbushPlanterBox>(), style: 0);
            WorldGen.PlaceTile(HLOX + 16, HLTY + 09, TileID.FoodPlatter, style: 0);
            WorldGen.PlaceTile(HLOX + 14, HLTY + 16, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 13, HLTY + 15, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 14, HLTY + 15, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 13, HLTY + 10, TileID.FoodPlatter, style: 0);
            WorldGen.PlaceTile(HLOX + 12, HLTY + 10, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX - 10, HLTY + 14, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX - 11, HLTY + 14, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX - 15, HLTY + 10, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX - 16, HLTY + 10, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX - 18, HLTY + 09, TileID.Bottles, style: 1);
            WorldGen.PlaceTile(HLOX - 19, HLTY + 09, TileID.Bottles, style: 1);
            WorldGen.PlaceTile(HLOX - 25, HLTY + 10, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX - 26, HLTY + 10, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX - 25, HLTY + 9, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX - 26, HLTY + 9, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX - 26, HLTY + 16, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX - 25, HLTY + 16, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX - 25, HLTY + 15, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX - 30, HLTY + 09, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX - 31, HLTY + 09, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX - 33, HLTY + 10, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX - 34, HLTY + 10, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX - 34, HLTY + 15, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX - 33, HLTY + 15, TileID.SilverCoinPile, style: 0);

            // Painting3X3
            WorldGen.Place3x3Wall(HLOX + 32, HLTY + 13, TileID.Painting3X3, style: 42);
            WorldGen.Place3x3Wall(HLOX + 28, HLTY + 13, TileID.Painting3X3, style: 45);
            WorldGen.Place3x3Wall(HLOX + 22, HLTY + 14, TileID.Painting3X3, style: 43);
            WorldGen.Place3x3Wall(HLOX + 16, HLTY + 14, TileID.Painting3X3, style: 44);
            WorldGen.Place3x3Wall(HLOX + 06, HLTY + 10, TileID.Painting3X3, style: 42);
            WorldGen.Place3x3Wall(HLOX - 10, HLTY + 12, TileID.Painting3X3, style: 43);
            WorldGen.Place3x3Wall(HLOX - 17, HLTY + 13, TileID.Painting3X3, style: 44);
            WorldGen.Place3x3Wall(HLOX - 41, HLTY + 12, TileID.Painting3X3, style: 42);
            WorldGen.Place3x3Wall(HLOX - 31, HLTY + 13, TileID.Painting3X3, style: 45);

            // Banners
            WorldGen.PlaceObject(HLOX + 30, HLTY + 12, TileID.Banners, style: 34);
            WorldGen.PlaceObject(HLOX + 26, HLTY + 12, TileID.Banners, style: 34);
            WorldGen.PlaceObject(HLOX + 18, HLTY + 12, TileID.Banners, style: 34);
            WorldGen.PlaceObject(HLOX + 13, HLTY + 12, TileID.Banners, style: 34);
            WorldGen.PlaceObject(HLOX - 13, HLTY + 10, TileID.Banners, style: 34);
            WorldGen.PlaceObject(HLOX - 21, HLTY + 12, TileID.Banners, style: 34);
            WorldGen.PlaceObject(HLOX - 24, HLTY + 11, TileID.Banners, style: 34);
            WorldGen.PlaceObject(HLOX - 28, HLTY + 12, TileID.Banners, style: 34);
            WorldGen.PlaceObject(HLOX - 36, HLTY + 10, TileID.Banners, style: 34);

            // Fishing Crate
            WorldGen.PlaceObject(HLOX + 25, HLTY + 10, TileID.FishingCrate, style: 0);
            WorldGen.PlaceObject(HLOX - 21, HLTY + 10, TileID.CookingPots);

            // Beds
            WorldGen.PlaceObject(HLOX + 31, HLTY + 8, TileID.Beds, style: 21);
            WorldGen.PlaceObject(HLOX + 26, HLTY + 7, TileID.Beds, style: 21);
            WorldGen.PlaceObject(HLOX + 21, HLTY + 7, TileID.Beds, style: 21);
            WorldGen.PlaceObject(HLOX + 16, HLTY + 7, TileID.Beds, style: 21);
            WorldGen.PlaceObject(HLOX + 11, HLTY + 8, TileID.Beds, style: 21);
            WorldGen.PlaceObject(HLOX - 15, HLTY + 8, TileID.Beds, style: 21);
            WorldGen.PlaceObject(HLOX - 20, HLTY + 7, TileID.Beds, style: 21);
            WorldGen.PlaceObject(HLOX - 25, HLTY + 7, TileID.Beds, style: 21);
            WorldGen.PlaceObject(HLOX - 30, HLTY + 7, TileID.Beds, style: 21);
            WorldGen.PlaceObject(HLOX - 35, HLTY + 8, TileID.Beds, style: 21);

            // Lavaflyina Bottle
            WorldGen.PlaceObject(HLOX + 31, HLTY + 5, TileID.LavaflyinaBottle);
            WorldGen.PlaceObject(HLOX + 27, HLTY + 4, TileID.LavaflyinaBottle);
            WorldGen.PlaceObject(HLOX + 20, HLTY + 4, TileID.LavaflyinaBottle);
            WorldGen.PlaceObject(HLOX + 17, HLTY + 4, TileID.LavaflyinaBottle);
            WorldGen.PlaceObject(HLOX + 10, HLTY + 5, TileID.LavaflyinaBottle);
            WorldGen.PlaceObject(HLOX - 13, HLTY + 5, TileID.LavaflyinaBottle);
            WorldGen.PlaceObject(HLOX - 19, HLTY + 4, TileID.LavaflyinaBottle);
            WorldGen.PlaceObject(HLOX - 23, HLTY + 4, TileID.LavaflyinaBottle);
            WorldGen.PlaceObject(HLOX - 30, HLTY + 4, TileID.LavaflyinaBottle);
            WorldGen.PlaceObject(HLOX - 33, HLTY + 5, TileID.LavaflyinaBottle);

            // Chest
            int chest = WorldGen.PlaceChest(HLOX - 2, HLTY + 34, (ushort)TileType<SwampChest>(), false, 0);
            if (chest != -1) { SwampChest(Main.chest[chest].item, 0); }
            chest = WorldGen.PlaceChest(HLOX - 75, HLTY + 17, 21, false, 2);
            if (chest != -1) { GoldChestLeft(Main.chest[chest].item, 0); }
            chest = WorldGen.PlaceChest(HLOX + 66, HLTY + 17, 21, false, 2); 
            if (chest != -1) { GoldChestRight(Main.chest[chest].item, 0); }
            WorldGen.PlaceChest(HLOX - 29, HLTY + 10, 21, false, 5);

            // 3
            WorldGen.PlaceObject(HLOX - 6, HLTY - 3, (ushort)TileType<LiquidTankLava>());
            WorldGen.PlaceObject(HLOX + 3, HLTY - 3, (ushort)TileType<LiquidTankLava>());
            WorldGen.PlaceObject(HLOX + 11, HLTY - 3, 390);
            WorldGen.PlaceObject(HLOX - 14, HLTY - 3, 390);
            WorldGen.PlaceObject(HLOX - 15, HLTY - 14, TileID.LavaflyinaBottle);
            WorldGen.PlaceObject(HLOX + 10, HLTY - 14, TileID.LavaflyinaBottle);
            WorldGen.PlaceObject(HLOX + 17, HLTY - 12, TileID.LavaflyinaBottle);
            WorldGen.PlaceObject(HLOX + 1, HLTY - 18, TileID.LavaflyinaBottle);
            WorldGen.PlaceObject(HLOX - 9, HLTY - 16, TileID.LavaflyinaBottle);

            WorldGen.PlaceObject(HLOX + 24, HLTY - 3, TileID.FishingCrate, style: 0);
            WorldGen.PlaceObject(HLOX + 26, HLTY - 3, TileID.FishingCrate, style: 0);
            WorldGen.PlaceObject(HLOX + 25, HLTY - 5, TileID.FishingCrate, style: 0);
            WorldGen.PlaceTile(HLOX + 24, HLTY - 5, TileID.Candles);
            WorldGen.PlaceObject(HLOX - 35, HLTY - 2, TileID.FishingCrate, style: 0);
            WorldGen.PlaceObject(HLOX - 37, HLTY - 1, TileID.FishingCrate, style: 0);
            WorldGen.PlaceTile(HLOX - 36, HLTY - 3, TileID.WaterCandle, style: 0);

        }

        public override void PostUpdateWorld() {
            Main.LocalPlayer.GetModPlayer<BismuthPlayer>().IsBoSRead = true;
        }
        static void SwampChest(Item[] chestInventory, int chestIndex) {
            int[] items = [ItemType<BlastProtectionVest>(), ItemType<NecroBuckler>()];
            WorldHelper.LootInContainers(chestInventory, ref chestIndex, ItemType<UnchargedElessar>());
            if (Main.chest[SynergiaWorld.SwampChestindex] != null) {
                if (Main.tile[Main.chest[SynergiaWorld.SwampChestindex].x, Main.chest[SynergiaWorld.SwampChestindex].y].TileType != TileType<SwampChest>()) { WorldHelper.RandomLootInCoutainer(chestInventory, ref chestIndex, 1, 1, items); }
                else { Main.chest[SynergiaWorld.SwampChestindex].item[0].SetDefaults(Utils.SelectRandom(WorldGen.genRand, items)); }
            }
            else { WorldHelper.RandomLootInCoutainer(chestInventory, ref chestIndex, 1, 1, items); }
        }
        static void ChestLoot(Item[] chestInventory, ref int chestIndex) {
            WorldHelper.RandomLootInCoutainer(chestInventory, ref chestIndex, 2, 5, ItemID.RegenerationPotion, ItemID.ShinePotion, ItemID.NightOwlPotion, ItemID.SwiftnessPotion, ItemID.GillsPotion, ItemID.HunterPotion, ItemID.MiningPotion, 2329);
            WorldHelper.LootInContainers(chestInventory, ref chestIndex, ItemID.HealingPotion, 3, 5);
            WorldHelper.LootInContainers(chestInventory, ref chestIndex, ItemID.ManaPotion, 3, 5);
        }
        static void GoldChestLeft(Item[] chestInventory, int chestIndex) {
            WorldHelper.LootInContainers(chestInventory, ref chestIndex, ItemType<Anaconda>());
            WorldHelper.LootInContainers(chestInventory, ref chestIndex, ItemID.HermesBoots);
            WorldHelper.LootInContainers(chestInventory, ref chestIndex, ItemID.ArcheryPotion);
            ChestLoot(chestInventory, ref chestIndex);
            WorldHelper.LootInContainers(chestInventory, ref chestIndex, ItemID.GoldCoin, 1, 10);
        }
        static void GoldChestRight(Item[] chestInventory, int chestIndex) {
            WorldHelper.LootInContainers(chestInventory, ref chestIndex, ItemType<StoneAge>());
            WorldHelper.LootInContainers(chestInventory, ref chestIndex, ItemID.BandofRegeneration);
            WorldHelper.LootInContainers(chestInventory, ref chestIndex, ItemID.IronskinPotion);
            ChestLoot(chestInventory, ref chestIndex);
            WorldHelper.LootInContainers(chestInventory, ref chestIndex, ItemID.GoldCoin, 1, 10);
        }
    }
}