using Avalon.Items.Armor.Hardmode;
using Avalon.Items.Armor.Superhardmode;
using Avalon.Items.Material;
using Avalon.Tiles;
using NewHorizons.Content.Items.Armor.NeutronArmor;
using NewHorizons.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using static Synergia.Common.ModSystems.RecipeSystem.RecipeGroups;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.AvalonsChanges {
    public partial class Avalons {
        public class Avalon_Armor : BaseRecipe {
            public override void DisableRecipe(Recipe recipe) {

            }
            public override void Ingredient(Recipe recipe) {
                AddIngredient(recipe, ItemType<AeroforceGuardia>(), 3, new Item(ItemID.Nanites, 10));
                AddIngredient(recipe, ItemType<AeroforceLeggings>(), 3, new Item(ItemID.Nanites, 15));
                AddIngredient(recipe, ItemType<AeroforceProtector>(), 3, new Item(ItemID.Nanites, 15));
                AddIngredient(recipe, ItemType<CaesiumGalea>(), 0, new Item(CaesiumBar, 10));
                AddIngredient(recipe, ItemType<CaesiumGreaves>(), 0, new Item(CaesiumBar, 10));
                AddIngredient(recipe, ItemType<CaesiumPlateMail>(), 0, new Item(CaesiumBar, 20));
                AddIngredientNotIndex(recipe, EarthsplitterChestpiece, CorrodeBar, 10);
                AddIngredientNotIndex(recipe, EarthsplitterHelm, CorrodeBar, 4);
                AddIngredientNotIndex(recipe, EarthsplitterLeggings, CorrodeBar, 8);
            }
            public override void Recipes() {
                AncientArmor(ItemType<AncientBodyplate>(), ItemType<NeutronMantle>());
                AncientArmor(ItemType<AncientLeggings>(), ItemType<NeutronLeggings>());
                AncientArmor(ItemType<AncientHeadpiece>(), ItemType<NeutronHood>());
            }
            public override void RemoveIngredient(Recipe recipe) {

            }
            public override void Tiles(Recipe recipe) {
                Tiles(recipe, CaesiumGalea, CaesiumHeavyAnvilTile);
                Tiles(recipe, CaesiumGreaves, CaesiumHeavyAnvilTile);
                Tiles(recipe, CaesiumPlateMail, CaesiumHeavyAnvilTile);
                Tiles(recipe, EarthsplitterChestpiece, CaesiumHeavyAnvilTile);
                Tiles(recipe, EarthsplitterHelm, CaesiumHeavyAnvilTile);
                Tiles(recipe, EarthsplitterLeggings, CaesiumHeavyAnvilTile);
            }
            static void AncientArmor(int createType, int ingredientType) {
                Recipe recipe = Recipe.Create(createType);
                recipe.AddIngredient(ingredientType);
                recipe.AddIngredient(ItemType<DarkCore>(), 10);
                recipe.AddRecipeGroup(FRAGMENT, 10);
                recipe.AddRecipeGroup(FRAGMENT2, 10);
                recipe.AddIngredient(ItemType<LifeDew>(), 5);
                recipe.AddIngredient(ItemType<GhostintheMachine>());
                recipe.AddTile(TileType<CaesiumForge>());
                recipe.Register();
            }
        }
    }
}