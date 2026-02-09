// Code by 𝒜𝑒𝓇𝒾𝓈
using Synergia.Common.ModSystems;
using Synergia.Common.ModSystems.WorldGens;
using Synergia.Content.Buffs;
using Synergia.Helpers;
using Terraria;

namespace Synergia.Common.Biome {
    public class SnowVillage : ModBiome {
        public override bool IsBiomeActive(Player player) {
            bool active = WorldHelper.CheckBiome(player, 103, 25, SynergiaGenVars.SnowVillagePositionX, SynergiaGenVars.SnowVillagePositionY - 25, BuffType<SnowVillageBuff>());
            if (active && !SynergiaWorld.FirstEnterInSnowVillage) { SynergiaWorld.FirstEnterInSnowVillage = true; }
            return active;
        }
    }
}