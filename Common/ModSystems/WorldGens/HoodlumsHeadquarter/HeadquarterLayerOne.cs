// Code by SerNik
using Avalon.Tiles.Ores;
using Bismuth.Content.Tiles;
using Synergia.Helpers;
using Terraria;
using Terraria.WorldBuilding;
using static Bismuth.Utilities.BismuthWorld;
using static Synergia.Common.ModSystems.WorldGens.SynergiaGenVars;

namespace Synergia.Common.ModSystems.WorldGens.HoodlumsHeadquarter {
    // Arena 
    public class HeadquarterLayerOne {
        // Size X: 160, Y: 80 - ? 
        static readonly byte[,] SwampTiles = new byte[,]
        {
            {1,1,1,1,1,1,1,1,1,1,1},
            {1,0,0,0,0,0,0,0,0,0,1},
            {1,1,1,1,1,1,1,1,1,1,1}
        };

        public static bool GenHeadquarter(GenerationProgress progress) {
            if (progress != null) {
                progress.Message = "Localization";
                progress.Set(0.33f);
            }

            HLOX = SwampCenterX;
            int baseY = (int)Main.rockLayer;
            for (int i = 0; i < 1000;) {
                if (Main.tile[HLOX, baseY - i].TileType != TileType<SwampMud>()) { i++; }
                else { HLOY = baseY - i; break; }
            }

            WorldHelper.Cleaning(HLOX + 80, HLOY - 109, HLOX - 80, HLOY - 30, TileType<SwampMud>(), TileType<PeatBlock>(), TileType<ZincOre>(), 0, 1, 28, 48, 66, 165, 169); // + вниз - верх

            int width = SwampTiles.GetLength(1);
            int height = SwampTiles.GetLength(0);

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    int worldX = HLOX - 80 + x;
                    int worldY = HLOY - 30 - y;

                    if (!WorldGen.InWorld(worldX, worldY, 10)) { continue; }

                    Tile tile = Framing.GetTileSafely(worldX, worldY);
                    tile.ClearEverything();

                    switch (SwampTiles[y, x]) {
                        case 0: break;
                        case 1: tile.TileType = 1; tile.HasTile = true; break;
                    }
                }
            }
            return true;
        }
    }
}