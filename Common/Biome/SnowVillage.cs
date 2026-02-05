// Code by 𝒜𝑒𝓇𝒾𝓈
using Synergia.Common.ModSystems.WorldGens;
using Synergia.Content.Buffs;
using Synergia.Helpers;
using Terraria;

namespace Synergia.Common.Biome {
    public class SnowVillage : ModBiome {
        public override bool IsBiomeActive(Player player) => WorldHelper.CheckBiome(player, 103, 25, BaseWorldGens.SnowVilagePositionX, BaseWorldGens.SnowVilagePositionY - 25, BuffType<SnowVillageBuff>());
    }
}