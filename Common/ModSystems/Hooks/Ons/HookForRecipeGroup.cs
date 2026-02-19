using Avalon.Items.Material.Bars;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Terraria;
using Terraria.ID;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    public class HookForRecipeGroup : ModSystem {
        Hook consolariaRecipeGroupHook;

        delegate void orig_AddRecipeGroups(Consolaria.RecipeGroups system);
        delegate void AddRecipeGroupsDetour(orig_AddRecipeGroups orig, Consolaria.RecipeGroups system);

        public override void Load() {
            MethodInfo recipeGroup = typeof(Consolaria.RecipeGroups).GetMethod(nameof(Consolaria.RecipeGroups.AddRecipeGroups));
            consolariaRecipeGroupHook = new Hook(recipeGroup, (AddRecipeGroupsDetour)EditRecipeGroups);
        }
        void EditRecipeGroups(orig_AddRecipeGroups orig, Consolaria.RecipeGroups system) {
            Consolaria.RecipeGroups.Titanium = new RecipeGroup(() => "Adamantite or Titanium or Troxinium bar", ItemID.AdamantiteBar, ItemID.TitaniumBar, ItemType<TroxiniumBar>());
            RecipeGroup.RegisterGroup("Consolaria:TitaniumRecipeGroup", Consolaria.RecipeGroups.Titanium);
        }
        public override void Unload() {
            consolariaRecipeGroupHook?.Dispose();
            consolariaRecipeGroupHook = null;
        }
    }
}
