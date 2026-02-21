using Avalon.Items.Material.Herbs;
using Avalon.Items.Potions.Buff;
using Consolaria.Content.Items.Materials;
using Synergia.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using static Synergia.Common.ModSystems.RecipeSystem.RecipeGroups;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.AvalonsChanges {
    public partial class Avalons;
    public class Avalon_Potions : BaseRecipe {
        public override void DisableRecipe(Recipe recipe) {
            DisableRecipe(recipe, ItemType<GauntletPotion>());
        }
        public override void Ingredient(Recipe recipe) {
            AddIngredientNotIndex(recipe, ItemType<AuraPotion>(), RoAItem("FlamingFabric"));
            AddIngredientNotIndex(recipe, ItemType<CloverPotion>(), RoAItem("MiracleMint"));
            AddIngredient(recipe, ItemType<FortunePotion>(), 3, new Item(RoAItem("MiracleMint"), 2));
            AddIngredientNotIndex(recipe, ItemType<FuryPotion>(), RoAItem("MiracleMint"));
            AddIngredient(recipe, ItemType<GamblerPotion>(), 3, new Item(ItemType<SoulofBlight>(), 2));
        }
        public override void Recipes() {
        }
        public override void PostRecipe() {
            CreatePotion();
        }
        public override void RemoveIngredient(Recipe recipe) {

        }
        public override void Tiles(Recipe recipe) {

        }
        static void CreatePotion() {
            Recipe recipe = Recipe.Create(ItemType<GauntletPotion>());
            recipe.AddIngredient(ItemType<Sweetstem>());
            recipe.AddRecipeGroup(FLOWER);
            recipe.AddRecipeGroup(IRON, 3);
            recipe.AddIngredient(RoAItem("MercuriumNugget"));
            recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }
    }
}