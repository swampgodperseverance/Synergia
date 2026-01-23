using StramsSurvival.Items;
using Synergia.Content.Items.Weapons.Summon;
using Synergia.Content.NPCs;
using Synergia.Helpers;
using System.Collections.Generic;
using static Synergia.Common.ModSystems.WorldGens.BaseWorldGens;
using static Synergia.Helpers.SynegiaHelper;
using static Synergia.Lists.Items;

namespace Synergia.Common.ModSystems {
    public class SynergiaWorld : ModSystem {
        public static Dictionary<int, (int, int)> BannerType { get; private set; } = [];

        public override void PostWorldGen() {
            WorldHelper.AddContainersLoot(13, 3, SkyChest, ItemType<Starcaller>());
            WorldHelper.DestroyerContainersLoot(13, ItemType<JungleSeedPacket>());
            WorldHelper.AddContainersLoot(13, 1, ItemType<JungleSeedPacket>(), 1, 3);
        }
        public override void PostUpdateWorld() {
            EditsArena(HellArenaPositionX - 198, HellArenaPositionY);
            EditsVillage(HellVillageX - 280, HellVillageY);
        }
        static void EditsArena(int x, int y) {
            //WorldGen.PlaceTile(x + 111, y - 28, 120);
        }
        public void Arena() {
            // тут твой код если персонаж в арене
            SpawnNPC((HellArenaPositionX - 198 + 110) * 16, (HellArenaPositionY - 28) * 16, NPCType<HellheartMonolith>());
        }
        static void EditsVillage(int x, int y) {
            // WorldGen.PlaceObject(x + 2, y - 99, TileID.Statues, false, 15);
        }
        public void Village() {
            // тут твой код если персонаж в деревни
        }
    }
}