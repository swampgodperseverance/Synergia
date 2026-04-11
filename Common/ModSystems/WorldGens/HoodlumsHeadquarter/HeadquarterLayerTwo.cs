using Avalon.Tiles.Contagion.Coughwood;
using Avalon.Walls;
using Bismuth.Content.Tiles;
using Bismuth.Content.Walls;
using Terraria;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace Synergia.Common.ModSystems.WorldGens.HoodlumsHeadquarter {
    public class HeadquarterLayerTwo {
        // 0 - empty, 1 swamp mud, 2 - 273, 3 - 472, 4 - SwampWood, 5 - CoughwoodTile, 6 - Chain 
        static readonly byte[,] Tiles = new byte[,] {
            {1,2,2,0,6,6,0,0,2,3,0,0,0,3,0,0,0,0,0,0,2,2,0,2,2,0,0,0,0,0,0,0,0,0,2,0,2,0,0,0,2,3,0,0,0,0,0,0,4,5,4,4,4,5,5,4,5,5,1,5,5,4,4,4,5,4,4,4,5,5,4,1,4,4,5,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,5,4,4,5,5,4,4,4,1,4,5,5,4,4,4,1,1,4,4,5,5,4,5,5,4,4,4,0,0,0,0,0,0,2,3,0,0,0,0,0,0,0,0,0,0,2,2,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,2,3,0,6,6,6,4,4,1,1,1,},
        };
        // 0 - empty, 1 swamp mud, 2 - CoughwoodWall, 3 - IronBrick, 4 - StoneSlab, 5 - CoughwoodFence, 6 - SwampWoodWall, 7 - IronFence
        static readonly byte[,] Wales = new byte[,] {
            {1,0,2,2,3,4,0,0,0,3,0,0,0,0,0,4,4,4,4,0,0,7,0,7,0,0,0,0,0,0,0,4,3,0,4,0,0,0,0,0,3,0,0,0,0,5,5,5,5,5,5,5,5,5,5,0,5,0,1,0,0,0,5,0,5,0,5,0,0,1,0,5,5,0,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,5,5,5,5,0,0,0,0,0,0,0,5,0,1,0,5,0,0,0,5,1,1,0,5,0,5,0,0,0,0,5,5,5,5,5,0,0,0,0,3,0,7,7,7,0,4,4,3,4,7,7,0,0,0,0,0,0,0,0,0,0,4,0,0,3,0,0,0,0,0,3,0,0,0,4,6,2,0,0,1,1,1,},
        };
        // 0 - empty, 1 - hamer, 2 - /|, 3 - |/, 4 - \|, 5 - |\
        static readonly byte[,] Slope = new byte[,] {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,5,0,2,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        };
        //
        static readonly byte[,] Liquid = new byte[,] {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        };

        public static bool GenHeadquarter(GenerationProgress progress, int x2, int y2) {
            if (progress != null) {
                progress.Message = "Localization";
                progress.Set(0.66f);
            }

            int width = Tiles.GetLength(1);
            int height = Tiles.GetLength(0);

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    int worldX = x2 + x;
                    int worldY = y2 - y;

                    if (!WorldGen.InWorld(worldX, worldY, 10)) { continue; }

                    Tile tile = Framing.GetTileSafely(worldX, worldY);
                    tile.ClearEverything();

                    switch (Tiles[y, x]) {
                        case 0: break;
                        case 1: tile.TileType = (ushort)TileType<SwampMud>(); tile.HasTile = true; break;
                        case 2: tile.TileType = TileID.IronBrick; tile.HasTile = true; break;
                        case 3: tile.TileType = TileID.StoneSlab; tile.HasTile = true; break;
                        case 4: tile.TileType = (ushort)TileType<SwampWood>(); tile.HasTile = true; break;
                        case 5: tile.TileType = (ushort)TileType<CoughwoodTile>(); tile.HasTile = true; break;
                        case 6: tile.TileType = TileID.Chain; tile.HasTile = true; break;
                    }
                    // 0 - empty, 1 swamp mud, 2 - CoughwoodWall, 3 - IronBrick, 4 - StoneSlab, 5 - CoughwoodFence
                    switch (Wales[y, x]) {
                        case 0: break;
                        case 1: tile.WallType = (ushort)WallType<SwampWall>(); break;
                        case 2: tile.WallType = (ushort)WallType<CoughwoodWall>(); break;
                        case 3: tile.WallType = WallID.IronBrick; break;
                        case 4: tile.WallType = WallID.StoneSlab; break;
                        case 5: tile.WallType = (ushort)WallType<CoughwoodFence>(); break;
                        case 6: tile.WallType = (ushort)WallType<SwampWoodWall>(); break;
                        case 7: tile.WallType = WallID.IronFence; break;
                    }
                    switch (Slope[y, x]) {
                        case 0: break;
                        case 1: tile.IsHalfBlock = true; break;
                        case 2: tile.Slope = SlopeType.SlopeDownRight; break;
                        case 3: tile.Slope = SlopeType.SlopeUpLeft; break;
                        case 4: tile.Slope = SlopeType.SlopeUpRight; break;
                        case 5: tile.Slope = SlopeType.SlopeDownLeft; break;
                    }
                    switch (Liquid[y, x]) {
                        case 0: tile.LiquidAmount = 0; break;
                        case 1: tile.LiquidType = LiquidID.Lava; tile.LiquidAmount = 255; break;
                    }
                }
            }
            return true; // Layer 3 :)
        }
    }
}
