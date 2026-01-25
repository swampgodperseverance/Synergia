using NewHorizons.Content.Items.Accessories;
using Synergia.Common.GlobalPlayer;
using System;
using Terraria;

namespace Synergia.Common.ModSystems.Hooks.Ons.EditAcc {
    public class HookForScrollOfChunin : HookForEditAcc {
        public override Type HookType => typeof(ScrollOfChunin);
        public override int HookItem => ItemType<ScrollOfChunin>();
        public override void EditAcc(Orig_UpdateAccessory orig, ModItem item, Player player, bool hideVisual) {
            player.GetAttackSpeed(DamageClass.Throwing) += 0.08f;
            ThrowingPlayer throwing = player.GetModPlayer<ThrowingPlayer>();
            throwing.ModifyMaxComboTime += 30;
            throwing.ModifyMaxTimeForReset += 60;
        }
    }
}
