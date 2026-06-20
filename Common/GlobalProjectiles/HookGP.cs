using Avalon.Common.Players;
using Synergia.Common.Biome;
using Synergia.Common.GlobalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalProjectiles
{
    internal class HookGP : GlobalProjectile
    {
        internal static bool Disabled = false;

        public override bool? CanUseGrapple(int type, Player player)
        {
            if (Disabled) return base.CanUseGrapple(type, player);

            int staminaCost = Main.hardMode ? 14 : 8;

            if (player.GetModPlayer<AvalonStaminaPlayer>().StatStam <= staminaCost) { return false; }
            else
            {
                if (player.InModBiome<NewHell>() && !player.GetModPlayer<BiomePlayer>().arenaBiome) { return false; }
                else { return base.CanUseGrapple(type, player); }
            }
        }

        public override void UseGrapple(Player player, ref int type)
        {
            if (Disabled) return;

            int staminaCost = Main.hardMode ? 14 : 8;
            player.GetModPlayer<AvalonStaminaPlayer>().StatStam -= staminaCost;
        }
    }
}