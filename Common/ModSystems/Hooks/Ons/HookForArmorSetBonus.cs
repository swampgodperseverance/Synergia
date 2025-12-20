using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using Terraria;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    public abstract class HookForArmorSetBonus : ModSystem {
        Hook ArmorSetBonus;
        public string oldText;

        public abstract Type Armor { get; }
        public abstract int ArmorType { get; }
        public override void Load() {
            MethodInfo target = Armor.GetMethod("UpdateArmorSet", BindingFlags.Public | BindingFlags.Instance);
            ArmorSetBonus = new Hook(target, (New_SetBonus)SetNewSetBonus);
        }
        public delegate void Orig_SetBonus(ModItem item, Player player);
        public delegate void New_SetBonus(Orig_SetBonus orig, ModItem item, Player player);
        void SetNewSetBonus(Orig_SetBonus orig, ModItem item, Player player) {
            if (item.Type == ArmorType) {
                NewLogicForSetBonus(orig, item, player);
            }
        }
        public abstract void NewLogicForSetBonus(Orig_SetBonus orig, ModItem item, Player player);
        public string EditSetBonusText(ref string setBonusText, string oldText, string newText) {
            setBonusText = this.oldText.Replace(oldText, newText);
            return setBonusText;
        }
        public override void Unload() {
            ArmorSetBonus?.Dispose();
            ArmorSetBonus = null;
        }
    }
}