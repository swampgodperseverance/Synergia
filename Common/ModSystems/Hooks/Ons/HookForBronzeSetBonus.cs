using Bismuth.Content.Items.Armor;
using Synergia.Common.GlobalPlayer;
using System;
using Terraria;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    public class HookForBronzeSetBonus : HookForArmorSetBonus {
        public override Type Armor => typeof(BronzeMask);
        public override int ArmorType => ItemType<BronzeMask>();

        public override void NewLogicForSetBonus(Orig_SetBonus orig, ModItem item, Player player) {
            player.endurance += 10f;
            player.setBonus = "";
            player.setBonus = ItemTooltip(ARM, "BronzeSetBonus");
        }
    }
}