using Synergia.Common.WorldGenSystem;
using Synergia.Content.Buffs;
using Synergia.Helpers;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.Biome
{
    public class SnowVillage : ModBiome
    {
        public override bool IsBiomeActive(Player player) => WorldHelper.CheakBiome(player, 103, 25, GenerationSnowVillage.SnowVilagePositionX, GenerationSnowVillage.SnowVilagePositionY - 25, ModContent.BuffType<SnowVillageBuff>());
    }
}