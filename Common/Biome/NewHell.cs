using Synergia.Common.ModSystems;
using Synergia.Common.ModSystems.WorldGens;
using Synergia.Helpers;
using Terraria;

namespace Synergia.Common.Biome {
    public class NewHell : ModBiome {
        public override bool IsBiomeActive(Player player) {
            bool Arena = WorldHelper.CheakBiome(player, 199, 119, BaseWorldGens.HellArenaPositionX - 199, BaseWorldGens.HellArenaPositionY - 119);
            bool Village = WorldHelper.CheakBiome(player, 281, 119, BaseWorldGens.HellVillageX - 280, BaseWorldGens.HellVillageY - 119);
            SynergiaWorld sWorld = GetInstance<SynergiaWorld>();
            if (Arena) { sWorld.Arena(); }
            if (Village) { sWorld.Village(); }
            return Arena || Village;
        }
    }
}