using Avalon.Common.Players;
using Synergia.Common.Biome;
using Synergia.Common.GlobalPlayer;
using Terraria;

namespace Synergia.Common.GlobalProjectiles {
    internal class HookGP : GlobalProjectile {
        public override bool? CanUseGrapple(int type, Player player) {
            if (player.GetModPlayer<AvalonStaminaPlayer>().StatStam <= 8) { return false; }
            else {
                if (player.InModBiome<NewHell>() && !player.GetModPlayer<BiomePlayer>().arenaBiome) { return false; }
                else { return base.CanUseGrapple(type, player); }
            }
        }
        public override void UseGrapple(Player player, ref int type) {
            player.GetModPlayer<AvalonStaminaPlayer>().StatStam -= 8;
        }
    }
}
