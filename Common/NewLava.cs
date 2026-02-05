// Code by 𝒜𝑒𝓇𝒾𝓈
using ModLiquidLib.ModLoader;
using Synergia.Common.Biome;
using Synergia.Common.GlobalPlayer;
using Terraria;

namespace Synergia.Common {
    public class NewLava : GlobalLiquid {
        public override void OnPlayerCollision(Player player, int type) {
            if (player.InModBiome<NewHell>()) {
                SynergiaPlayer sPlayer = player.GetModPlayer<SynergiaPlayer>();
                if (!sPlayer.IsEquippedUprateLavaCharm) {
                    player.AddBuff(22, 2);
                }
            }
        }
    }
}
