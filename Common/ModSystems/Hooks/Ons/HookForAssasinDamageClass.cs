using Bismuth.Utilities;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    public class HookForAssasinDamageClass : ModSystem {
        Hook newDamageClass;

        delegate void origSetDefaults(AssassinItem item);
        delegate void setDefaultsDetour(origSetDefaults orig, AssassinItem item);

        public override void Load() {
            MethodInfo info = typeof(AssassinItem).GetMethod(nameof(AssassinItem.SetDefaults));
            newDamageClass = new(info, (setDefaultsDetour)EditSetDefaults);
        }
        void EditSetDefaults(origSetDefaults orig, AssassinItem item) {
            item.Item.DamageType = DamageClass.Melee;
            item.Item.crit = 7;
        }
        public override void Unload() {
            newDamageClass?.Dispose();
            newDamageClass = null;
        }
        class ClearTooltips : HookForTooltips {
            public override Type ItemType => typeof(AssassinItem);
            public override void Init(origTooltips orig, ModItem item, List<TooltipLine> tooltips) {
                if (item is AssassinItem) { EditTooltips(orig, item, tooltips); }
                else { orig(item, tooltips); }
            }
            public override void EditTooltips(origTooltips orig, ModItem item, List<TooltipLine> tooltips) { }
        }
    }
}