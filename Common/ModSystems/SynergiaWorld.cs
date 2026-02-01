// Code by 𝒜𝑒𝓇𝒾𝓈
using StramsSurvival.Items;
using Synergia.Content.Items.Weapons.Summon;
using Synergia.Helpers;
using System.Collections.Generic;
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
        }
        public override void PostUpdateWorld() {
            EditsArena(HellArenaPositionX - 198, HellArenaPositionY);   
            EditsVillage(HellVillageX - 280, HellVillageY);
            EditsLake(HellLakeX - 236, HellLakeY);
        }
        static void EditsArena(int x, int y) {
            //WorldGen.PlaceTile(x + 111, y - 28, 120);
        }
        static void EditsVillage(int x, int y) {
            // WorldGen.PlaceObject(x + 2, y - 99, TileID.Statues, false, 15);
        }
        static void EditsLake(int x, int y) {
            //WorldGen.PlaceChest(x + 132, y - 12, TileID.Containers, false, 2);
            //WorldGen.PlaceWall(x + 93, y - 30, ModList.Roa.Find<ModWall>("GrimstoneWall").Type);
        }
    }
}