// Code by SerNik
using Bismuth.Content.Tiles;
using Synergia.Content.Items.Weapons.Summon;
using Synergia.Helpers;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using static Bismuth.Utilities.BismuthWorld;
using static Synergia.Common.ModSystems.WorldGens.SynergiaGenVars;
using static Synergia.Common.ModSystems.WorldGens.SynergiaGenVars;
using static Synergia.Lists.Items;

namespace Synergia.Common.ModSystems {
    public class SynergiaWorld : ModSystem {
        public static Dictionary<int, (int, int)> BannerType { get; private set; } = [];
        public static bool FirstEnterInSnowVillage { get; internal set; }
        public static bool FirstEnterInHellVillage { get; internal set; }
        public static bool SpawnDwarf { get; internal set; }

        public override void OnWorldLoad() {
            FirstEnterInSnowVillage = false;
            FirstEnterInHellVillage = false;
            SpawnDwarf = false;
        }
        public override void SaveWorldData(TagCompound tag) {
            tag["FirstEnterInSnowVillage"] = FirstEnterInSnowVillage;
            tag["FirstEnterInHellVillage"] = FirstEnterInHellVillage;
            tag["SpawnDwarf"] = SpawnDwarf;
        }
        public override void LoadWorldData(TagCompound tag) {
            FirstEnterInSnowVillage = tag.GetBool("FirstEnterInSnowVillage");
            FirstEnterInHellVillage = tag.GetBool("FirstEnterInHellVillage");
            SpawnDwarf = tag.GetBool("SpawnDwarf");
        }
        sealed public override void NetSend(BinaryWriter writer) {
            writer.Write(FirstEnterInSnowVillage);
            writer.Write(FirstEnterInHellVillage);
            writer.Write(SpawnDwarf);
        }
        sealed public override void NetReceive(BinaryReader reader) {
            FirstEnterInSnowVillage = reader.ReadBoolean();
            FirstEnterInHellVillage = reader.ReadBoolean();
            SpawnDwarf = reader.ReadBoolean();
        }
        public override void PostWorldGen() {
            WorldHelper.AddContainersLoot(13, 3, SkyChest, ItemType<Starcaller>());
            WorldHelper.CleaningLiquid(HellVillageX - 220, HellVillageY - 115, HellVillageX - 57, HellVillageY - 67);
            WorldHelper.CleaningLiquid(HellLakeX - 214, HellVillageY - 112, HellLakeX, HellVillageY - 80);
        }
        public override void PostUpdateWorld() {
            if (!SnowVillageGen) { FirstEnterInSnowVillage = true; }
            if (!HellVillageGen) { FirstEnterInHellVillage = true; }
            //int baseY = (int)Main.rockLayer;
            //int startY = 0;
            //for (int i = 0; i < 1000;) {
            //    Tile tile = Main.tile[HLOX, baseY - i];
            //    if (tile.TileType != TileType<SwampMud>()) {
            //        i++;
            //    }
            //    else { startY = baseY - i; break; }
            //}
            //if (startY != 0) {
            //    Main.NewText("Its swamp block");
            //    WorldHelper.Cleaning(HLOX + 80, startY - 100, HLOX - 20, startY  - 20, TileType<SwampMud>(), 0, 1, 66, 165); // + вниз - верх
            //}

            //WorldHelper.Cleaning(HLOX + 80, HLOY + 20, HLOX - 80, HLOY - 60, TileType<SwampMud>(), 0, 1, 66, 165);
            //Main.NewText(Main.LocalPlayer.Center.X);
            //Main.LocalPlayer.position = new Point16(Main.maxTilesX - Main.maxTilesX + 3000, (int)Main.rockLayer-250).ToWorldCoordinates();
            //for (int i = 0; i < 10; i++) {
            //    for (int k = 0; k < 10; k++) {
            //        //if (Main.tile[SwampCenterX + i, Main.UnderworldLayer - k].HasTile)
            //        //WorldGen.PlaceTile(Main.maxTilesX - Main.maxTilesX + 3000 + i, (int)Main.rockLayer - 250, TileID.Adamantite);
            //    }
            //}
        }
    }
}