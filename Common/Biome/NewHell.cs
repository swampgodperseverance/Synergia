// Code by 𝒜𝑒𝓇𝒾𝓈
using Synergia.Common.GlobalPlayer;
using Synergia.Common.ModSystems;
using Synergia.Common.ModSystems.WorldGens;
using Synergia.Helpers;
using Terraria;

namespace Synergia.Common.Biome {
    public class NewHell : ModBiome {
        public override bool IsBiomeActive(Player player) {
            int width = 237 + SynergiaGenVars.HellArenaPositionX - SynergiaGenVars.HellLakeX;
            bool fullHell = WorldHelper.CheckBiome(player, width, 119, SynergiaGenVars.HellLakeX - 236, SynergiaGenVars.HellLakeY - 119);
            if (fullHell) {
                bool arena = WorldHelper.CheckBiome(player, 199, 119, SynergiaGenVars.HellArenaPositionX - 199, SynergiaGenVars.HellArenaPositionY - 119);
                bool village = WorldHelper.CheckBiome(player, 281, 119, SynergiaGenVars.HellVillageX - 280, SynergiaGenVars.HellVillageY - 119);
                bool lake = WorldHelper.CheckBiome(player, 215, 119, SynergiaGenVars.HellLakeX - 236, SynergiaGenVars.HellLakeY - 119);
                BiomePlayer bPlayer = player.GetModPlayer<BiomePlayer>();
                bPlayer.arenaBiome = arena;
                bPlayer.villageBiome = village;
                bPlayer.lakeBiome = lake;
                if (village && !SynergiaWorld.FirstEnterInHellVillage) { SynergiaWorld.FirstEnterInHellVillage = true; }
                return fullHell;
            }
            return false;
        }
    }
}