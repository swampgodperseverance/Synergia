using Synergia.Common.ModSystems.WorldGens;
using Synergia.Helpers;
using Terraria;

namespace Synergia.Common.Biome {
    public class UndergraundSwamp : ModBiome {
        public override bool IsBiomeActive(Player player) {
            if (player.InModBiome<Bismuth.Utilities.ZoneSwamp>()) {
                int pos = System.Math.Abs(SynergiaGenVars.HLTY - SynergiaGenVars.HLOY);
                if (WorldHelper.CheckBiome(player, 171, pos, SynergiaGenVars.HLTX - 171, SynergiaGenVars.HLOY - pos - 31)) { return true; }
                else { return false; }
            }
            else { return false; }
        }
    }
}
