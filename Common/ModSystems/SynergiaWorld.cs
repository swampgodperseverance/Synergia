// Code by 𝒜𝑒𝓇𝒾𝓈
using StramsSurvival.Items;
using Synergia.Content.Items.Weapons.Summon;
using Synergia.Helpers;
using System.Collections.Generic;
using Terraria;
using static Synergia.Common.ModSystems.WorldGens.BaseWorldGens;
using static Synergia.Lists.Items;

namespace Synergia.Common.ModSystems {
    public class SynergiaWorld : ModSystem {
        public static Dictionary<int, (int, int)> BannerType { get; private set; } = [];

        public override void PostWorldGen() {
            WorldHelper.AddContainersLoot(13, 3, SkyChest, ItemType<Starcaller>());
            WorldHelper.DestroyerContainersLoot(13, ItemType<JungleSeedPacket>());
            WorldHelper.AddContainersLoot(13, 1, ItemType<JungleSeedPacket>(), 1, 3);
            WorldHelper.CleaningLiquid(HellVillageX - 220, HellVillageY - 115, HellVillageX - 57, HellVillageY - 67);
            WorldHelper.CleaningLiquid(HellLakeX - 214, HellVillageY - 112, HellLakeX, HellVillageY - 80);
            AddWall(HellVillageX - 280, HellVillageY);
        }
        public override void PostUpdateWorld() {
            EditsArena(HellArenaPositionX - 198, HellArenaPositionY);   
            EditsVillage(HellVillageX - 280, HellVillageY);
            EditsLake(HellLakeX - 236, HellLakeY);
        }
        static void EditsArena(int x, int y) {
            //SynegiaHelper.SpawnNPC((x + 110) * 16, (y - 28) * 16, NPCType<HellheartMonolith>());
        }
        static void EditsVillage(int x, int y) { }
        static void EditsLake(int x, int y) {}
        static void AddWall(int x, int y) {
            ushort wall = ModList.Roa.Find<ModWall>("GrimstoneBrickWall").Type;
            WorldGen.PlaceWall(x + 220, y - 56, wall);
            WorldGen.PlaceWall(x + 220, y - 55, wall);
            WorldGen.PlaceWall(x + 219, y - 55, wall);
            WorldGen.PlaceWall(x + 218, y - 55, wall);
            WorldGen.PlaceWall(x + 217, y - 55, wall);
            WorldGen.PlaceWall(x + 217, y - 54, wall);
            WorldGen.PlaceWall(x + 216, y - 53, wall);
            WorldGen.PlaceWall(x + 216, y - 52, wall);
            WorldGen.PlaceWall(x + 215, y - 52, wall);
            WorldGen.PlaceWall(x + 216, y - 51, wall);
            WorldGen.PlaceWall(x + 215, y - 51, wall);
            WorldGen.PlaceWall(x + 215, y - 50, wall);
            WorldGen.PlaceWall(x + 214, y - 50, wall);
            WorldGen.PlaceWall(x + 213, y - 49, wall);
            WorldGen.PlaceWall(x + 213, y - 48, wall);
            WorldGen.PlaceWall(x + 212, y - 47, wall);
        }
    }
}