// Code by SerNik
using ModLiquidLib.ModLoader;
using Synergia.Common.Biome;
using Synergia.Common.GlobalPlayer;
using Terraria;
using Terraria.ID;

namespace Synergia.Common {
    public class NewLava : GlobalLiquid {
        public override void OnPlayerCollision(Player player, int type) {
            if (player.InModBiome<NewHell>()) {
                SynergiaPlayer sPlayer = player.GetModPlayer<SynergiaPlayer>();
                if (!sPlayer.IsEquippedUprateLavaCharm) {
                    player.AddBuff(BuffID.Darkness, 2);
                }
            }
        }
    }
}
