// Code by 𝒜𝑒𝓇𝒾𝓈
using Synergia.Content.Items.Weapons.Summon;
using Synergia.Helpers;
using System.Collections.Generic;
using System.IO;
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
            writer.Write(FirstEnterInSnowVillage);
            writer.Write(FirstEnterInHellVillage);
        }
        sealed public override void NetReceive(BinaryReader reader) {
            FirstEnterInSnowVillage = reader.ReadBoolean();
            FirstEnterInHellVillage = reader.ReadBoolean();
        }
        public override void PostWorldGen() {
            WorldHelper.AddContainersLoot(13, 3, SkyChest, ItemType<Starcaller>());
            WorldHelper.CleaningLiquid(HellVillageX - 220, HellVillageY - 115, HellVillageX - 57, HellVillageY - 67);
            WorldHelper.CleaningLiquid(HellLakeX - 214, HellVillageY - 112, HellLakeX, HellVillageY - 80);
        }
    }
}