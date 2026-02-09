//using Bismuth.Content.Tiles;
//using Synergia.Helpers;
//using System.Collections.Generic;
//using System.IO;
//using Terraria;
//using Terraria.GameContent.Generation;
//using Terraria.ModLoader;
//using Terraria.ModLoader.IO;
//using Terraria.WorldBuilding;
//using static Bismuth.Utilities.BismuthWorld;

//namespace Synergia.Common.ModSystems.WorldGens;

//public class WorldGenInSwampBiom : ModSystem {

//    bool cavesGenerated = false;
//    public static int swampCavenX;
//    public static int swampCavenY;

//    static readonly int[,] SwampTiles = new int[,] 
//    {
//        {1,1,1,1,1,1,1,1,1,1,1},
//        {1,0,0,0,0,0,0,0,0,0,1},
//        {1,1,1,1,1,1,1,1,1,1,1}
//    };
//    public override void OnWorldLoad() {
//        cavesGenerated = false;
//        swampCavenX = 0;
//        swampCavenY = 0;
//    }
//    public override void SaveWorldData(TagCompound tag) {
//        var Generated = new BitsByte(); Generated[0] = cavesGenerated;
//        tag["swampCavenX"] = swampCavenX;
//        tag["hellVillageY"] = swampCavenY;
//    }
//    public override void LoadWorldData(TagCompound tag) {
//        var Generated = (BitsByte)tag.GetByte("Generated"); cavesGenerated = Generated[0];
//        swampCavenX = tag.GetInt("swampCavenX");
//        swampCavenY = tag.GetInt("swampCavenY");
//    }
//    public override void NetSend(BinaryWriter writer) {
//        BitsByte Flags = new(); Flags[0] = cavesGenerated;
//        writer.Write(swampCavenX);
//        writer.Write(swampCavenY);
//    }
//    public override void NetReceive(BinaryReader reader) {
//        BitsByte Flags = reader.ReadByte(); cavesGenerated = Flags[0];
//        swampCavenX = reader.ReadInt32();
//        swampCavenY = reader.ReadInt32();
//    }

//    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
//        int index = tasks.FindIndex(x => x.Name == "Final Cleanup");
//        if (index != -1) {
//            tasks.Insert(index, new PassLegacy("[Synergia] Swamp Caves", (progress, config) => AddCaven(progress)));
//        }
//    }
//    void AddCaven(GenerationProgress progress = null) {
//        if (cavesGenerated) return;
//        bool Success = Do_MakeCaven(progress);
//        if (Success) cavesGenerated = true;
//    }
//    static bool Do_MakeCaven(GenerationProgress progress) {
//        if (progress != null) {
//            progress.Message = "";
//            progress.Set(0.33f);
//        }
//        swampCavenX = SwampCenterX;
//        swampCavenY = SwampStartY + 20;

//        WorldHelper.Cleaning(swampCavenX + 40, swampCavenY, swampCavenX, swampCavenY + 50, ModContent.TileType<SwampMud>());
//        int width = SwampTiles.GetLength(1);
//        int height = SwampTiles.GetLength(0);

//        for (int y = 0; y < height; y++) {
//            for (int x = 0; x < width; x++) {
//                int worldx = swampCavenX - 20 + x;
//                int worldY = swampCavenY - y;

//                if (!WorldGen.InWorld(worldx, worldY, 10)) {
//                    continue;
//                }

//                Tile tile = Framing.GetTileSafely(worldx, worldY);
//                tile.ClearEverything();

//                switch (SwampTiles[y, x]) {
//                    case 0: break;
//                    case 1: tile.TileType = 1; tile.HasTile = true; break;
//                }
//            }
//        }
//        return true;
//    }
//}