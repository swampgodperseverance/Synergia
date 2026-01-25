using NewHorizons.Content.Items.Accessories;
using System;
using Terraria;

namespace Synergia.Common.ModSystems.Hooks.Ons.EditAcc {
    public class HookForScrollOfGenin : HookForEditAcc {
        public override Type HookType => typeof(ScrollOfGenin);
        public override int HookItem => ItemType<ScrollOfGenin>();
        public override void EditAcc(Orig_UpdateAccessory orig, ModItem item, Player player, bool hideVisual) {
            player.GetAttackSpeed(DamageClass.Throwing) += 0.08f;
        }
    }
}