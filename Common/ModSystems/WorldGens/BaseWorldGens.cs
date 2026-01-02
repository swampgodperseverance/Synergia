using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace Synergia.Common.ModSystems.WorldGens {
    public abstract class BaseWorldGens : ModSystem {
        public static int HellArenaPositionX { get; protected set; }
        public static int HellArenaPositionY { get; protected set; }
        public static List<Vector2> ArenaTiles { get; protected set; } = [];
        public static int BasePositionArena { get; protected set; }
        public static int SnowVilagePositionX { get; protected set; }
        public static int SnowVilagePositionY { get; protected set; }
        public static List<Vector2> VilageTiles { get; protected set; } = [];
        public static int HellVillageX { get; protected set; }
        public static int HellVillageY { get; protected set; }
        public static List<Vector2> HellVillageTilesVector { get; protected set; } = [];
        public static int HellLakeX { get; protected set; }
        public static int HellLakeY { get; protected set; }
        public static List<Vector2> HellLakeTilesVector { get; protected set; } = [];

        public const byte a = 10, b = 11, c = 12, d = 13, e = 14, f = 15, g = 16, h = 17, i = 18, j = 19, k = 20, l = 21;

        public abstract bool GensBool { get; set; }
        public virtual string NameGen { get; }
        public string Favorit = "Final Cleanup";
        public virtual string VanillaIndexName => "Final Cleanup";

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
            int index = tasks.FindIndex(x => x.Name == VanillaIndexName);
            if (index != -1) {
                tasks.Insert(index, new PassLegacy(NameGen, (progress, config) => AddGen(progress)));
            }
        }
        protected void AddGen(GenerationProgress progress = null) {
            if (GensBool) return;
            bool Success = Do_MakeGen(progress);
            if (Success) GensBool = true;
        }
        public abstract bool Do_MakeGen(GenerationProgress progress);
        sealed public override void OnWorldLoad() {
            HellArenaPositionX = 0;
            HellArenaPositionY = 0;
            SnowVilagePositionX = 0;
            SnowVilagePositionY = 0;
            HellVillageX = 0;
            HellVillageY = 0;
            HellLakeX = 0;
            HellLakeY = 0;
            HellLakeTilesVector.Clear();
            VilageTiles.Clear();
            ArenaTiles.Clear();
            HellVillageTilesVector.Clear();
            GensBool = false;
        }
        sealed public override void SaveWorldData(TagCompound tag) {
            tag["HellArenaPositionX"] = HellArenaPositionX;
            tag["HellArenaPositionY"] = HellArenaPositionY;
            tag["ArenaTiles"] = ArenaTiles;
            tag["SnowVilagePositionX"] = SnowVilagePositionX;
            tag["SnowVilagePositionY"] = SnowVilagePositionY;
            tag["VilageTiles"] = VilageTiles;
            tag["hellVillageX"] = HellVillageX;
            tag["hellVillageY"] = HellVillageY;
            tag["hellVillageTilesVector"] = HellVillageTilesVector;
            tag["HellLakeX"] = HellLakeX;
            tag["HellLakeY"] = HellLakeY;
            tag["HellLakeTilesVector"] = HellLakeTilesVector;
            BitsByte Generated2 = new();
            Generated2[0] = GensBool;
        }
        sealed public override void LoadWorldData(TagCompound tag) {
            HellArenaPositionX = tag.GetInt("HellArenaPositionX");
            HellArenaPositionY = tag.GetInt("HellArenaPositionY");
            ArenaTiles = tag.Get<List<Vector2>>("ArenaTiles");
            BitsByte Generated2 = (BitsByte)tag.GetByte("Generated2");
            SnowVilagePositionX = tag.GetInt("SnowVilagePositionX");
            SnowVilagePositionY = tag.GetInt("SnowVilagePositionY");
            VilageTiles = tag.Get<List<Vector2>>("VilageTiles");
            HellVillageX = tag.GetInt("hellVillageX");
            HellVillageY = tag.GetInt("hellVillageY");
            HellVillageTilesVector = tag.Get<List<Vector2>>("hellVillageTilesVector");
            HellLakeX = tag.GetInt("HellLakeX");
            HellLakeY = tag.GetInt("HellLakeY");
            HellLakeTilesVector = tag.Get<List<Vector2>>("HellLakeTilesVector");
            GensBool = Generated2[0];
        }
        sealed public override void NetSend(BinaryWriter writer) {
            writer.Write(HellArenaPositionX);
            writer.Write(HellArenaPositionY);
            writer.Write(ArenaTiles.Count);
            writer.Write(SnowVilagePositionX);
            writer.Write(SnowVilagePositionY);
            writer.Write(VilageTiles.Count);
            writer.Write(HellVillageX);
            writer.Write(HellVillageY);
            writer.Write(HellVillageTilesVector.Count);
            writer.Write(HellLakeX);
            writer.Write(HellLakeY);
            writer.Write(HellLakeTilesVector.Count);
            foreach (Vector2 v in ArenaTiles) writer.WriteVector2(v);
            foreach (Vector2 v in VilageTiles) writer.WriteVector2(v);
            foreach (Vector2 v in HellVillageTilesVector) writer.WriteVector2(v);
            foreach (Vector2 v in HellLakeTilesVector) writer.WriteVector2(v);
            BitsByte Flags = new(); Flags[0] = GensBool;
        }
        sealed public override void NetReceive(BinaryReader reader) {
            HellArenaPositionX = reader.ReadInt32();
            HellArenaPositionY = reader.ReadInt32();
            SnowVilagePositionX = reader.ReadInt32();
            SnowVilagePositionY = reader.ReadInt32();
            HellVillageX = reader.ReadInt32();
            HellVillageY = reader.ReadInt32();
            HellLakeX = reader.ReadInt32();
            HellLakeY = reader.ReadInt32();
            ArenaTiles.Clear();
            VilageTiles.Clear();
            HellVillageTilesVector.Clear();
            HellLakeTilesVector.Clear();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++) {
                ArenaTiles.Add(reader.ReadVector2());
                VilageTiles.Add(reader.ReadVector2());
                HellVillageTilesVector.Add(reader.ReadVector2());
                HellLakeTilesVector.Add(reader.ReadVector2());
            }
            BitsByte Flags = reader.ReadByte();
            GensBool = Flags[0];
        }
    }
}