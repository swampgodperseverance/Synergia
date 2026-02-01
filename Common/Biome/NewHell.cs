// Code by 𝒜𝑒𝓇𝒾𝓈
using Synergia.Common.GlobalPlayer;
using Synergia.Common.ModSystems.WorldGens;
using Synergia.Helpers;
using Terraria;

namespace Synergia.Common.Biome {
    public class NewHell : ModBiome {
        public override bool IsBiomeActive(Player player) {
            bool arena = WorldHelper.CheakBiome(player, 199, 119, BaseWorldGens.HellArenaPositionX - 199, BaseWorldGens.HellArenaPositionY - 119);
            bool village = WorldHelper.CheakBiome(player, 281, 119, BaseWorldGens.HellVillageX - 280, BaseWorldGens.HellVillageY - 119);
            bool lake = WorldHelper.CheakBiome(player, 215, 119, BaseWorldGens.HellLakeX - 236, BaseWorldGens.HellLakeY - 119);
            BiomePlayer bPlayr = player.GetModPlayer<BiomePlayer>();
            bPlayr.Arena(arena);
            bPlayr.Village(village);
            bPlayr.Lake(lake);
            return arena || village || lake;
        }
    }
}