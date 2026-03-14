using Avalon.Items.Armor.Hardmode;
using Avalon.Items.Armor.PreHardmode;
using Avalon.Items.Material.Herbs;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe {
    public class AllMod : BaseRecipe {
        public override void Ingredient(Recipe recipe) {
        }
        public override void PostPostRecipe(Recipe recipe) {
            List<int> potion = [ItemID.ThornsPotion];
            List<int> flower = [ItemID.Deathweed, ItemType<Bloodberry>(), ItemType<Barfbush>()];
            AddRecipeGroup(recipe, new RecipeGroupStruct(potion, flower, RecipeGroups.FLOWER));
        }
    }
}