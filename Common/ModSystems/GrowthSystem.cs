using Synergia.Content.Tiles.WorldGen;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Synergia.ModList;

namespace Synergia.Common.ModSystems {
    public class GrowthSystem : ModSystem {
        const int GroundGrowthTicks = 3600;
        const int ScanPeriod = 3600;

        double lastGrowthTime = 0;

        public override void PostUpdateWorld() {
            Player player = Main.LocalPlayer;
            if (player.GetModPlayer<QuestSystem.QuestBoolean>().HunterQuest) {
                return;
            }
            if (Main.GameUpdateCount % ScanPeriod != 0) {
                return;
            }

            double now = Main.GameUpdateCount;

            if (now - lastGrowthTime < GroundGrowthTicks) {
                return;
            }

            lastGrowthTime = now;

            List<(int x, int y)> spots = [];

            for (int i = 0; i < Main.maxTilesX; i++) {
                for (int j = 0; j < Main.maxTilesY; j++) {

                    Tile tile = Framing.GetTileSafely(i, j);

                    if (tile.HasTile && tile.TileType == Roa.Find<ModTile>("BackwoodsGrass").Type) {
                        Tile above = Framing.GetTileSafely(i, j - 1);

                        if (!above.HasTile) {
                            spots.Add((i, j));
                        }
                    }
                }
            }

            if (spots.Count == 0) {
                return;
            }

            var (x, y) = spots[Main.rand.Next(spots.Count)];

            WorldGen.PlaceTile(x, y - 1, ModContent.TileType<Reed3>(), mute: true);
        }
    }
}
