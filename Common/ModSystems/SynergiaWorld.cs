// Code by 𝒜𝑒𝓇𝒾𝓈
using StramsSurvival.Items;
using Synergia.Common.GlobalPlayer;
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

        public override void OnWorldLoad() {
            FirstEnterInSnowVillage = false;
            FirstEnterInHellVillage = false;
        }
        public override void SaveWorldData(TagCompound tag) {
            tag["FirstEnterInSnowVillage"] = FirstEnterInSnowVillage;
            tag["FirstEnterInHellVillage"] = FirstEnterInHellVillage;
        }
        public override void LoadWorldData(TagCompound tag) {
            FirstEnterInSnowVillage = tag.GetBool("FirstEnterInSnowVillage");
            FirstEnterInHellVillage = tag.GetBool("FirstEnterInHellVillage");
        }
        sealed public override void NetSend(BinaryWriter writer) {
            BitsByte Flags = new(); 
            Flags[0] = FirstEnterInSnowVillage;
            Flags[1] = FirstEnterInHellVillage;
        }
        sealed public override void NetReceive(BinaryReader reader) {
            BitsByte Flags = reader.ReadByte();
            FirstEnterInSnowVillage = Flags[0];
            FirstEnterInHellVillage = Flags[0];
        }
        public override void PostWorldGen() {
            WorldHelper.AddContainersLoot(13, 3, SkyChest, ItemType<Starcaller>());
            WorldHelper.DestroyerContainersLoot(13, ItemType<JungleSeedPacket>());
            WorldHelper.AddContainersLoot(13, 1, ItemType<JungleSeedPacket>(), 1, 3);
            WorldHelper.CleaningLiquid(HellVillageX - 220, HellVillageY - 115, HellVillageX - 57, HellVillageY - 67);
            WorldHelper.CleaningLiquid(HellLakeX - 214, HellVillageY - 112, HellLakeX, HellVillageY - 80);
        }
        public override void PostUpdateWorld() {
            EditsArena(HellArenaPositionX - 198, HellArenaPositionY);   
            EditsVillage(HellVillageX - 280, HellVillageY);
            EditsLake(HellLakeX - 236, HellLakeY);
        }
        static void EditsArena(int x, int y) {
            if (Main.LocalPlayer.GetModPlayer<BiomePlayer>().arenaBiome) {
                SynegiaHelper.SpawnNPC((x + 110) * 16, (y - 28) * 16, NPCType<HellheartMonolith>());
            }
        }
        static void EditsVillage(int x, int y) { }
        static void EditsLake(int x, int y) {}
    }
}