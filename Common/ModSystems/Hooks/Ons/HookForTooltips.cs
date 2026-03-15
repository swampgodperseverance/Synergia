using Bismuth.Utilities;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    public abstract class HookForTooltips : ModSystem {
        Hook newTooltips;

        public delegate void origTooltips(ModItem item, List<TooltipLine> tooltips);
        public delegate void TooltipsDetour(origTooltips orig, ModItem item, List<TooltipLine> tooltips);

        public abstract Type ItemType { get; }
        public virtual int Target => -1;
        public abstract void EditTooltips(origTooltips orig, ModItem item, List<TooltipLine> tooltips);

        public override void Load() {
            MethodInfo info = ItemType.GetMethod("ModifyTooltips");
            newTooltips = new(info, (TooltipsDetour)Init);
        }
        public virtual void Init(origTooltips orig, ModItem item, List<TooltipLine> tooltips) {
            if (Target == -1) { orig(item, tooltips); }
            if (item.Type == Target) { EditTooltips(orig, item, tooltips); }
            else { orig(item, tooltips); }
        }
        public override void Unload() {
            newTooltips?.Dispose();
            newTooltips = null;
        }
    }
}
