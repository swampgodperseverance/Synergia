// Code by SerNik
using Avalon.Tiles.Furniture;
using Avalon.Tiles.Furniture.Coughwood;
using Bismuth.Content.Tiles;
using Synergia.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.WorldBuilding;
using static Synergia.Common.ModSystems.WorldGens.SynergiaGenVars;

namespace Synergia.Common.ModSystems.WorldGens.HoodlumsHeadquarter {
    public class SettingHeadquarter : BaseWorldGens {
        bool swampGen;

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

            // OnTable
            WorldGen.PlaceOnTable1x1(HLOX + 35, HLTY + 13, TileID.Bottles, style: 5);
            WorldGen.PlaceOnTable1x1(HLOX + 36, HLTY + 13, TileID.Candles, style: 0);
            WorldGen.PlaceOnTable1x1(HLOX + 20, HLTY + 15, TileID.Candles, style: 0);

            // 1X1
            WorldGen.PlaceTile(HLOX + 30, HLTY + 10, TileID.FoodPlatter, style: 0);
            WorldGen.PlaceTile(HLOX + 31, HLTY + 10, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 26, HLTY + 15, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 27, HLTY + 15, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 28, HLTY + 16, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 28, HLTY + 15, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 27, HLTY + 9, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 28, HLTY + 9, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 23, HLTY + 10, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 23, HLTY + 9, TileID.SilverCoinPile, style: 0);
            WorldGen.PlaceTile(HLOX + 22, HLTY + 10, TileID.SilverCoinPile, style: 0);

            // Painting3X3
            WorldGen.Place3x3Wall(HLOX + 32, HLTY + 13, TileID.Painting3X3, style: 42);
            WorldGen.Place3x3Wall(HLOX + 28, HLTY + 13, TileID.Painting3X3, style: 45);
            WorldGen.Place3x3Wall(HLOX + 22, HLTY + 14, TileID.Painting3X3, style: 43);

            // Banners
            WorldGen.PlaceObject(HLOX + 30, HLTY + 12, TileID.Banners, style: 34);
            WorldGen.PlaceObject(HLOX + 26, HLTY + 12, TileID.Banners, style: 34);

            // Fishing Crate
            WorldGen.PlaceObject(HLOX + 25, HLTY + 10, TileID.FishingCrate, style: 0);

            // Beds
            WorldGen.PlaceObject(HLOX + 31, HLTY + 8, TileID.Beds, style: 21);
            WorldGen.PlaceObject(HLOX + 26, HLTY + 7, TileID.Beds, style: 21);
            WorldGen.PlaceObject(HLOX + 21, HLTY + 7, TileID.Beds, style: 21);
            WorldGen.PlaceObject(HLOX + 16, HLTY + 7, TileID.Beds, style: 21);
            WorldGen.PlaceObject(HLOX + 11, HLTY + 8, TileID.Beds, style: 21);

            // Chest
            int chest = WorldGen.PlaceChest(HLOX - 2, HLTY + 34, (ushort)TileType<SwampChest>(), false, 0);
            if (chest != -1) { SwampChest(Main.chest[chest].item, 0); }
            chest = WorldGen.PlaceChest(HLOX - 75, HLTY + 17, 21, false, 2);
            if (chest != -1) { GoldChestLeft(Main.chest[chest].item, 0); }
            chest = WorldGen.PlaceChest(HLOX + 66, HLTY + 17, 21, false, 2); 
            if (chest != -1) { GoldChestRight(Main.chest[chest].item, 0); }
        }
        public override void PostUpdateWorld() {
            // WorldGen.PlaceObject(HLOX + 25, HLTY + 10, TileID.FishingCrate, style: 0);
            //WorldGen.Place2x1(HLOX + 26, HLTY + 16, (ushort)TileType<CoughwoodWorkBench>(), style: 0);
            //WorldGen.PlaceObject(HLOX + 11, HLTY + 8, TileID.Beds, style: 21);
            //WorldGen.PlaceObject(HLOX + 26, HLTY + 7, TileID.Beds, style: 21);
            //WorldGen.PlaceObject(HLOX + 25, HLTY + 10, TileID.FishingCrate, style: 0);
            //WorldGen.Place3x3Wall(HLOX + 22, HLTY + 14, TileID.Painting3X3, style: 43);
            //WorldGen.Place2x1(HLOX + 20, HLTY + 16, (ushort)TileType<CoughwoodWorkBench>(), style: 0);
            //WorldGen.PlaceOnTable1x1(HLOX + 20, HLTY + 15, TileID.Candles, style: 0);

            //WorldGen.PlaceTile(HLOX + 30, HLTY + 13, TileID.Adamantite);
        }
        static void SwampChest(Item[] chestInventory, int chestIndex) { }
        static void GoldChestLeft(Item[] chestInventory, int chestIndex) { }
        static void GoldChestRight(Item[] chestInventory, int chestIndex) {
            WorldHelper.LootInContainers(chestInventory, ref chestIndex, 151);
        }
    }
}