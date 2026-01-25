using NewHorizons.Content.Items.Accessories;
using Synergia.Common.GlobalPlayer;
using System;
using Terraria;

namespace Synergia.Common.ModSystems.Hooks.Ons.EditAcc {
    public class HookForScrollOfJonin : HookForEditAcc {
        public override Type HookType => typeof(ScrollOfJonin);
        public override int HookItem => ItemType<ScrollOfJonin>();
        public override void EditAcc(Orig_UpdateAccessory orig, ModItem item, Player player, bool hideVisual) {
            player.GetAttackSpeed(DamageClass.Throwing) += 0.12f;
            ThrowingPlayer throwing = player.GetModPlayer<ThrowingPlayer>();
            throwing.ModifyMaxComboTime += 60;
            throwing.ModifyMaxTimeForReset += 30;
        }
    }
}