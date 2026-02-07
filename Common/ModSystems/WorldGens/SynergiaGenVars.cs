using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader.IO;

namespace Synergia.Common.ModSystems.WorldGens {
    /// TODO: Need fix 
    public class SynergiaGenVars: ModSystem {
        public static int SnowVilagePositionX { get; set; }
        public static int SnowVilagePositionY { get; set; }
        public static List<Vector2> VilageTiles { get; set; } = [];
        public static List<Vector2> VilageWalles { get; set; } = [];
        public static int HellArenaPositionX { get; set; }
        public static int HellArenaPositionY { get; set; }
        public static List<Vector2> ArenaTiles { get; set; } = [];
        public static List<Vector2> ArenaWalles { get; set; } = [];
        public static int HellVillageX { get; set; }
        public static int HellVillageY { get; set; }
        public static List<Vector2> HellVillageTilesVector { get; set; } = [];
        public static List<Vector2> HellVillageWallesVector { get; set; } = [];
        public static int HellLakeX { get; set; }
        public static int HellLakeY { get; set; }
        public static List<Vector2> HellLakeTilesVector { get; set; } = [];
        public static List<Vector2> HellLakeWallesVector { get; set; } = [];
        public static Rectangle ArenaTilesMP { get; set; } = new();

        public override void OnWorldLoad() {
            HellArenaPositionX = 0;
            HellArenaPositionY = 0;
            SnowVilagePositionX = 0;
            SnowVilagePositionY = 0;
            HellVillageX = 0;
            HellVillageY = 0;
            HellLakeX = 0;
            HellLakeY = 0;
            VilageTiles.Clear();
            VilageWalles.Clear();
            ArenaTiles.Clear();
            ArenaWalles.Clear();
            HellVillageTilesVector.Clear();
            HellVillageWallesVector.Clear();
            HellLakeTilesVector.Clear();
            HellLakeWallesVector.Clear();
        }
        public override void SaveWorldData(TagCompound tag) {
            tag["HellArenaPositionX"] = HellArenaPositionX;
            tag["HellArenaPositionY"] = HellArenaPositionY;
            tag["ArenaTiles"] = ArenaTiles;
            tag["ArenaWales"] = ArenaWalles;
            tag["SnowVilagePositionX"] = SnowVilagePositionX;
            tag["SnowVilagePositionY"] = SnowVilagePositionY;
            tag["VilageTiles"] = VilageTiles;
            tag["VilageWalles"] = VilageWalles;
            tag["hellVillageX"] = HellVillageX;
            tag["hellVillageY"] = HellVillageY;
            tag["hellVillageTilesVector"] = HellVillageTilesVector;
            tag["HellVillageWallesVector"] = HellVillageWallesVector;
            tag["HellLakeX"] = HellLakeX;
            tag["HellLakeY"] = HellLakeY;
            tag["HellLakeTilesVector"] = HellLakeTilesVector;
            tag["HellLakeWallesVector"] = HellLakeWallesVector;
        }
        public override void LoadWorldData(TagCompound tag) {
            HellArenaPositionX = tag.GetInt("HellArenaPositionX");
            HellArenaPositionY = tag.GetInt("HellArenaPositionY");
            ArenaTiles = tag.Get<List<Vector2>>("ArenaTiles");
            ArenaWalles = tag.Get<List<Vector2>>("ArenaWales");
            SnowVilagePositionX = tag.GetInt("SnowVilagePositionX");
            SnowVilagePositionY = tag.GetInt("SnowVilagePositionY");
            VilageTiles = tag.Get<List<Vector2>>("VilageTiles");
            VilageWalles = tag.Get<List<Vector2>>("VilageWalles");
            HellVillageX = tag.GetInt("hellVillageX");
            HellVillageY = tag.GetInt("hellVillageY");
            HellVillageTilesVector = tag.Get<List<Vector2>>("hellVillageTilesVector");
            HellVillageWallesVector = tag.Get<List<Vector2>>("HellVillageWallesVector");
            HellLakeX = tag.GetInt("HellLakeX");
            HellLakeY = tag.GetInt("HellLakeY");
            HellLakeTilesVector = tag.Get<List<Vector2>>("HellLakeTilesVector");
            HellLakeWallesVector = tag.Get<List<Vector2>>("HellLakeWallesVector");
        }
        public override void NetSend(BinaryWriter writer) {
            writer.Write(VilageTiles.Count);
            foreach (Vector2 v in VilageTiles) writer.WriteVector2(v);
            writer.Write(VilageWalles.Count);
            foreach (Vector2 v in VilageWalles) writer.WriteVector2(v);
            //writer.Write(ArenaTiles.Count);
            //foreach (Vector2 v in ArenaTiles) writer.WriteVector2(v);
            //writer.Write(ArenaWalles.Count);
            //foreach (Vector2 v in ArenaWalles) writer.WriteVector2(v);
            //writer.Write(HellVillageTilesVector.Count);
            //foreach (Vector2 v in HellVillageTilesVector) writer.WriteVector2(v);
            //writer.Write(HellVillageWallesVector.Count);
            //foreach (Vector2 v in HellVillageWallesVector) writer.WriteVector2(v);
            //writer.Write(HellLakeTilesVector.Count);
            //foreach (Vector2 v in HellLakeTilesVector) writer.WriteVector2(v);
            //writer.Write(HellLakeWallesVector.Count);
            //foreach (Vector2 v in HellLakeWallesVector) writer.WriteVector2(v);

            writer.Write(SnowVilagePositionX);
            writer.Write(SnowVilagePositionY);
            writer.Write(HellArenaPositionX);
            writer.Write(HellArenaPositionY);
            writer.Write(HellVillageX);
            writer.Write(HellVillageY);
            writer.Write(HellLakeX);
            writer.Write(HellLakeY);
        }
        public override void NetReceive(BinaryReader reader) {
            int count = reader.ReadInt32();  VilageTiles.Clear();
            for (int i = 0; i < count; i++)  { VilageTiles.Add(reader.ReadVector2()); }
            int count2 = reader.ReadInt32(); VilageWalles.Clear();
            for (int i = 0; i < count2; i++) { VilageWalles.Add(reader.ReadVector2()); }
            //int count3 = reader.ReadInt32(); ArenaTiles.Clear();
            //for (int i = 0; i < count3; i++) { ArenaTiles.Add(reader.ReadVector2()); }
            //int count4 = reader.ReadInt32(); ArenaWalles = [];
            //for (int i = 0; i < count4; i++) { ArenaWalles.Add(reader.ReadVector2()); }
            //int count5 = reader.ReadInt32(); HellVillageTilesVector = [];
            //for (int i = 0; i < count5; i++) { HellVillageTilesVector.Add(reader.ReadVector2()); }
            //int count6 = reader.ReadInt32(); HellVillageWallesVector = [];
            //for (int i = 0; i < count6; i++) { HellVillageWallesVector.Add(reader.ReadVector2()); }
            //int count7 = reader.ReadInt32(); HellLakeTilesVector = [];
            //for (int i = 0; i < count7; i++) { HellLakeTilesVector.Add(reader.ReadVector2()); }
            //int count8 = reader.ReadInt32(); HellLakeWallesVector = [];
            //for (int i = 0; i < count8; i++) { HellLakeWallesVector.Add(reader.ReadVector2()); }

            SnowVilagePositionX = reader.ReadInt32();
            SnowVilagePositionY = reader.ReadInt32();
            HellArenaPositionX = reader.ReadInt32();
            HellArenaPositionY = reader.ReadInt32();
            HellVillageX = reader.ReadInt32();
            HellVillageY = reader.ReadInt32();
            HellLakeX = reader.ReadInt32();
            HellLakeY = reader.ReadInt32();
        }
    }
}
