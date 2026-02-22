// Code by SerNik
using Synergia.Common.GlobalItems;
using Terraria;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    public class HookForEditHookItems : ModSystem {
        public override void Load() => On_Item.CloneDefaults += On_Item_CloneDefaults;
        void On_Item_CloneDefaults(On_Item.orig_CloneDefaults orig, Item self, int TypeToClone) {
            if (Lists.Items.VanillaGrapplingHooks.Contains(TypeToClone)) {
                self.GetGlobalItem<SynergiaGI>().isGrapplingHooks = true;
                orig(self, TypeToClone);
            }
            else { orig(self, TypeToClone); }
        }
        public override void Unload() => On_Item.CloneDefaults -= On_Item_CloneDefaults;
    }
}