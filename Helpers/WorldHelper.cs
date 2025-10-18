using System;
using System.Collections.Generic;
using Terraria;

namespace Synergia.Helpers
{
    public class WorldHelper
    {
        public static void Cleaning(int startX, int startY, int endX, int endY, params int[] type)
        {
            int minX = Math.Min(startX, endX);
            int maxX = Math.Max(startX, endX);
            int minY = Math.Min(startY, endY);
            int maxY = Math.Max(startY, endY);

            HashSet<int> tileTypesToDestroy = [.. type];

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    if (WorldGen.InWorld(x, y, 10) && Main.tile[x, y].HasTile && tileTypesToDestroy.Contains(Main.tile[x, y].TileType))
                    {
                        Tile tile = Main.tile[x, y];
                        tile.HasTile = false;
                    }
                }
            }
        }
        /// <summary> Если нужно добавить один предмет</summary>
        public static void LootInContainers(Item[] ChestInventory, ref int Index, int Item) { ChestInventory[Index++].SetDefaults(Item); }
        /// <summary>Если нужно добавить один случаеный предмет из списка ну например zenith, wood</summary>
        public static void RandomLootInCoutainer(Item[] ChestInventory, ref int Index, params int[] Items) { ChestInventory[Index++].SetDefaults(Utils.SelectRandom(WorldGen.genRand, Items)); }
    }
}