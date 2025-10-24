using Synergia.Common.WorldGenSystem;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

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
        /// <summary> Проверяет, находится ли игрок внутри прямоугольной области. Если находится — опционально добавляет бафф.</summary>
        public static bool CheakBiome(Player player, int width, int height, int left, int top, int buff = 1)
        {
            Point pos = player.Center.ToTileCoordinates();

            int right = left + width;
            int bottom = top + height;

            if (pos.X >= left && pos.X < right && pos.Y >= top && pos.Y < bottom)
            {
                if (buff != 1) player.AddBuff(buff, 3);
                return true;
            }

            return false;
        }
        /// <summary> Если нужно добавить один предмет</summary>
        public static void LootInContainers(Item[] ChestInventory, ref int Index, int Item, int Min = 1, int Max = 1) 
        {
            ChestInventory[Index].SetDefaults(Item); Random(ChestInventory, ref Index, Min, Max); Index++;
        }
        /// <summary> Если нужно добавить один случаеный предмет из списка ну например zenith, wood</summary>
        public static void RandomLootInCoutainer(Item[] ChestInventory, ref int Index, int Min = 1, int Max = 1, params int[] Items)
        { 
            ChestInventory[Index].SetDefaults(Utils.SelectRandom(WorldGen.genRand, Items));
            Random(ChestInventory, ref Index, Min, Max); Index++;
        }
        /// <summary>  </summary>
        /// <param name="Tire">1:Copper, 2:Iron, 3:Silver, 4:Gold, 5: Copper </param>
        public static void IfOreTireLoot(Item[] ChestInventory, ref int Index, int Tire, int IfTrue, int Else, int Min = 1, int Max = 1)
        {
            bool ThisWoroldHasOre = Tire switch
            {
                1 => WorldGen.SavedOreTiers.Copper == TileID.Copper,
                2 => WorldGen.SavedOreTiers.Iron == TileID.Iron,
                3 => WorldGen.SavedOreTiers.Silver == TileID.Silver,
                4 => WorldGen.SavedOreTiers.Gold == TileID.Gold,
                _ => WorldGen.SavedOreTiers.Copper == TileID.Copper,
            };

            int itemToGive;

            if (ThisWoroldHasOre) itemToGive = IfTrue;
            else itemToGive = Else;

            ChestInventory[Index].SetDefaults(itemToGive); Random(ChestInventory, ref Index, Min, Max); Index++;
        }
        static void Random(Item[] ChestInventory, ref int Index, int Min, int Max) => ChestInventory[Index].stack = Main.rand.Next(Min, Max + 1);
    }
}