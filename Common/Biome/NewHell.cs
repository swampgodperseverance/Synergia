// Code by 𝒜𝑒𝓇𝒾𝓈
using Synergia.Common.GlobalPlayer;
using Synergia.Common.ModSystems.WorldGens;
using Synergia.Helpers;
using Terraria;

namespace Synergia.Common.Biome {
    public class NewHell : ModBiome {
        public override bool IsBiomeActive(Player player) {
            int width = 237 + BaseWorldGens.HellArenaPositionX - BaseWorldGens.HellLakeX;
            bool fullHell = WorldHelper.CheckBiome(player, width, 119, BaseWorldGens.HellLakeX - 236, BaseWorldGens.HellLakeY - 119);
            if (fullHell) {
                bool arena = WorldHelper.CheckBiome(player, 199, 119, BaseWorldGens.HellArenaPositionX - 199, BaseWorldGens.HellArenaPositionY - 119);
                bool village = WorldHelper.CheckBiome(player, 281, 119, BaseWorldGens.HellVillageX - 280, BaseWorldGens.HellVillageY - 119);
                bool lake = WorldHelper.CheckBiome(player, 215, 119, BaseWorldGens.HellLakeX - 236, BaseWorldGens.HellLakeY - 119);
                BiomePlayer bPlayer = player.GetModPlayer<BiomePlayer>();
                bPlayer.arenaBiome = arena;
                bPlayer.villageBiome = village;
                bPlayer.lakeBiome = lake;
                return fullHell;
            }
            return false;
        }
    }
}