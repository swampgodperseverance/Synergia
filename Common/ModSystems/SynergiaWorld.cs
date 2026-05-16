// Code by SerNik
using Synergia.Content.Items.Weapons.Summon;
using Synergia.Content.NPCs;
using Synergia.Helpers;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader.IO;
using static Synergia.Common.ModSystems.WorldGens.SynergiaGenVars;
using static Synergia.Lists.Items;

namespace Synergia.Common.ModSystems {
    public class SynergiaWorld : ModSystem {
        public static Dictionary<int, (int, int)> BannerType { get; private set; } = [];
        public static bool FirstEnterInSnowVillage { get; internal set; }
        public static bool FirstEnterInHellVillage { get; internal set; }
        public static bool SpawnDwarf { get; internal set; }
        public static bool SpawnCristal { get; set; } 

        public override void ClearWorld() {
            FirstEnterInSnowVillage = false;
            FirstEnterInHellVillage = false;
            SpawnDwarf = false;
            SpawnCristal = false;
        }
        public override void OnWorldLoad() {
            FirstEnterInSnowVillage = false;
            FirstEnterInHellVillage = false;
            SpawnDwarf = false;
            SpawnCristal = false;
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
            writer.Write(SpawnCristal);
        }
        sealed public override void NetReceive(BinaryReader reader) {
            FirstEnterInSnowVillage = reader.ReadBoolean();
            FirstEnterInHellVillage = reader.ReadBoolean();
            SpawnDwarf = reader.ReadBoolean();
            SpawnCristal = reader.ReadBoolean();
        }
        public override void PostWorldGen() {
            WorldHelper.AddContainersLoot(13, 3, SkyChest, ItemType<Starcaller>());
            WorldHelper.CleaningLiquid(HellVillageX - 220, HellVillageY - 115, HellVillageX - 57, HellVillageY - 67);
            WorldHelper.CleaningLiquid(HellLakeX - 214, HellVillageY - 112, HellLakeX, HellVillageY - 80);
        }
        public override void PostUpdateWorld() {
            if (!SnowVillageGen) { FirstEnterInSnowVillage = true; }
            if (!HellVillageGen) { FirstEnterInHellVillage = true; }
            if (NPC.downedGolemBoss && !DownedBossSystem.DownedSinlordBoss && !SpawnCristal) {
                SynegiaHelper.SpawnNPC((HellArenaPositionX - 198 + 110) * 16, (HellArenaPositionY - 28) * 16, NPCType<HellheartMonolith>());
                SpawnCristal = true;
            }
        }
    }
}