using Avalon.Items.Accessories.Hardmode;
using System;
using Terraria;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    public class TestAcc : HookForEditAcc {
        public override Type HookType => typeof(AlchemicalSkull);
        public override int HookItem => ItemType<AlchemicalSkull>();
        public override void EditAcc(Orig_UpdateAccessory orig, ModItem item, Player player, bool hideVisual) {
            // не используй  orig(item, player, hideVisual); что бы полностю переписать 
            item.Item.defense = 100;
        }
    }
}
