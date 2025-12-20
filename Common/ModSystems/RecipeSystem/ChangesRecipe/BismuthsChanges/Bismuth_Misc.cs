using Synergia.Content.Items.Materials;
using Terraria;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe {
    public partial class Bismuths;
    public class Bismuth_Misc : ModSystem {
        public override void PostAddRecipes() {
            foreach (Recipe recipe in Main.recipe) {
                if (recipe.createItem.type == Bismuths.BBronze) {
                    recipe.DisableRecipe();
                }
            }

            Recipe NewBronzeRecipe = Recipe.Create(Bismuths.BBronze);
            NewBronzeRecipe.AddIngredient(Bismuths.ABronze);
            NewBronzeRecipe.AddIngredient<SulfuricAcid>();
            NewBronzeRecipe.AddConsumeIngredientCallback(OnCraft.SulphurConsumeIngredientCallback);
            NewBronzeRecipe.Register();
        }
    }
}