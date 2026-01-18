using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using Terraria;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    public abstract class HookForEditAcc : ModSystem {
        Hook origAccBonus;

        public delegate void Orig_UpdateAccessory(ModItem item, Player player, bool hideVisual);
        public delegate void UpdateAccessoryDetour(Orig_UpdateAccessory orig, ModItem item, Player player, bool hideVisual);

        public abstract Type HookType { get; }
        public abstract int HookItem { get; }
        public abstract void EditAcc(Orig_UpdateAccessory orig, ModItem item, Player player, bool hideVisual);
        public override void Load() {
            MethodInfo info = HookType.GetMethod("UpdateAccessory", BindingFlags.Public | BindingFlags.Instance);
            origAccBonus = new(info, (UpdateAccessoryDetour)HookEditAcc);
        }
        void HookEditAcc(Orig_UpdateAccessory orig, ModItem item, Player player, bool hideVisual) {
            if (item.Type == HookItem) {
                EditAcc(orig, item, player, hideVisual);
            }
        }
        public override void Unload() {
            origAccBonus?.Dispose();
            origAccBonus = null;
        }
    }
}
