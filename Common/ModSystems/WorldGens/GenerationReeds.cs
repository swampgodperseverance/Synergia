using Synergia.Content.Tiles.WorldGen;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using static Synergia.ModList;

namespace Synergia.Common.ModSystems.WorldGens {
    public class GenerationReeds : ModSystem {
        bool GenerateCrystal;

        public override void OnWorldLoad() {
            GenerateCrystal = false;
        }
        public override void SaveWorldData(TagCompound tag) {
            var Generated = new BitsByte();
            Generated[3] = GenerateCrystal;
        }
        public override void LoadWorldData(TagCompound tag) {
            var Generated = (BitsByte)tag.GetByte("Generated");
            GenerateCrystal = Generated[3];
        }
        public override void NetSend(BinaryWriter writer) {
            BitsByte Flags1 = new BitsByte();
            Flags1[3] = GenerateCrystal;
        }
        public override void NetReceive(BinaryReader reader) {
            BitsByte Flags1 = reader.ReadByte();
            GenerateCrystal = Flags1[3];
        }
        public static bool Placement(int x, int y) {
            for (var i = x - 16; i < x + 16; i++) {
                for (var j = y - 16; j < y + 16; j++) {
                    if (Main.tile[i, j].LiquidAmount > 0) {
                        return false;
                    }
                    int[] TileArray = { TileID.BlueDungeonBrick, TileID.GreenDungeonBrick, TileID.PinkDungeonBrick, TileID.Cloud, TileID.RainCloud, 147, 53, 60, 40, 23, 199, 25, 203 };
                    for (var ohgodilovememes = 0; ohgodilovememes < TileArray.Length - 1; ohgodilovememes++) {
                        if (Main.tile[i, j].TileType == (ushort)TileArray[ohgodilovememes]) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
            int index = tasks.FindIndex(genPass => genPass.Name == "Gems");
            if (index != -1) {
                tasks.Add(new PassLegacy("[Synergia] Reads", (progress, config) => AddNatureCrystal(progress)));
            }
        }
        public void AddNatureCrystal(GenerationProgress progress = null) {
            if (GenerateCrystal) {
                return;
            }
            bool Success = do_MakeNatureCrystal(progress);
            if (Success) {
                GenerateCrystal = true;
            }
        }
        bool do_MakeNatureCrystal(GenerationProgress progress) {
            if (progress != null) {
                progress.Message = "";
                progress.Set(0.79f);
            }
            goto GenerateCrystal;

        GenerateCrystal:
            int X = 1;
            int Y = 1;
            float widthScale = (Main.maxTilesX / 4200f);
            int numberToGenerate = 3;
            for (int k = 0; k < numberToGenerate; k++) {
                bool placement = false;
                bool placed = false;
                while (!placed) {
                    bool success = false;
                    int attempts = 0;
                    while (!success) {
                        attempts++;
                        if (attempts > 1000) {
                            success = true;
                            continue;
                        }
                        int i = WorldGen.genRand.Next(200, Main.maxTilesX - 200);
                        if (i <= Main.maxTilesX / 2 - 50 || i >= Main.maxTilesX / 2 + 50) {
                            int j = 0;
                            while (!Main.tile[i, j].HasTile && (double)j < Main.worldSurface) {
                                j++;
                            }
                            if (Main.tile[i, j].TileType == Roa.Find<ModTile>("BackwoodsGrass").Type) {
                                j--;
                                if (j > 150) {
                                    placement = Placement(i, j);
                                    if (placement) {
                                        X = i;
                                        Y = j;
                                        for (int I = 0; I < 250; I++) {
                                            int why = Main.rand.Next(-15, 15);
                                            int ex = Main.rand.Next(-40, 40);
                                            WorldGen.PlaceObject(i + ex, j + why, (ushort)ModContent.TileType<Reed3>());
                                        }
                                        success = true;
                                        placed = true;
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
