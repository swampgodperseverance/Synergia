using Bismuth.Content.Items.Armor;
using Synergia.Common.GlobalPlayer;
using System;
using Terraria;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    public class HookForBronzeSetBonus : HookForArmorSetBonus {
        public override Type Armor => typeof(BronzeMask);
        public override int ArmorType => ItemType<BronzeMask>();

        public override void NewLogicForSetBonus(Orig_SetBonus orig, ModItem item, Player player) {
            orig(item, player);
            oldText = player.setBonus;

            player.ThrownVelocity += 0.03f;
            player.statDefense += 2;

            player.setBonus = EditSetBonusText(ref oldText, "7", "10");
            player.setBonus = EditSetBonusText(ref oldText, "+1", "+3") + "\n" + SUtils.LocUtil.ItemTooltip(SUtils.LocUtil.ARM, "BronzeSetBonus");

            player.GetModPlayer<SynergiaPlayer>().equipBronzeSet = true;
        }
    }
}