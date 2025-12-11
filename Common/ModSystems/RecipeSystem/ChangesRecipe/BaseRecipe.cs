using Synergia.Common.ModConfigs;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe {
    public abstract class BaseRecipe : ModSystem {
        public sealed override void PostAddRecipes() {
            if (ModContent.GetInstance<BossConfig>().NewRecipe) {
                List<Recipe> recipesCopy = [.. Main.recipe];
                foreach (Recipe recipe in recipesCopy) {
                    DisableRecipe(recipe);
                    Ingredient(recipe);
                    RemoveIngredient(recipe);
                    Tiles(recipe);
                }
                PostRecipe();
            }
        }
        public sealed override void AddRecipes() {
            if (ModContent.GetInstance<BossConfig>().NewRecipe) {
                Recipes();
            }
        }
        public virtual void DisableRecipe(Recipe recipe) { }
        public abstract void Ingredient(Recipe recipe);
        public virtual void RemoveIngredient(Recipe recipe) { }
        public virtual void Tiles(Recipe recipe) { }
        public virtual void Recipes() { }
        public virtual void PostRecipe() { }
        protected static void DisableRecipe(Recipe recipe, int target) {
            if (recipe.createItem.type == target) {
                recipe.DisableRecipe();
            }
        }
        protected static void DisableRecipe(Recipe recipe, int target, int targetIngredient) {
            if (recipe.createItem.type == target) {
                if (recipe.HasIngredient(targetIngredient)) {
                    recipe.DisableRecipe();
                }
            }
        }
        protected static void AddIngredient(Recipe recipe, int createType, int indexItem, Item newItem) {
            if (recipe.createItem.type == createType) {
                recipe.requiredItem[indexItem] = newItem;
            }
        }
        protected static void AddIngredient2(Recipe recipe, int createType, int ingredient, int ingredient2, int ingredientCount = 1, int ingredientCount2 = 1) {
            if (recipe.createItem.type == createType) {
                recipe.AddIngredient(ingredient, ingredientCount);
                recipe.AddIngredient(ingredient2, ingredientCount2);
            }
        }
        protected static void AddIngredientNotIndex(Recipe recipe, int createType, int ingredient, int ingredientCount = 1) {
            if (recipe.createItem.type == createType) {
                recipe.AddIngredient(ingredient, ingredientCount);
            }
        }
        protected static void ForeachIngredient(Recipe recipe, int createType, Item newItem) {
            if (recipe.createItem.type == createType) {
                recipe.requiredItem[^1] = newItem;
            }
        }
        protected static void RemoveIngredient(Recipe recipe, int targetItem, int index) {
            if (recipe.createItem.type == targetItem) {
                recipe.requiredItem.RemoveAt(index);
            }
        }
        protected static void Tiles(Recipe recipe, int createType, int tileType) {
            if (recipe.createItem.type == createType) {
                recipe.requiredTile[^1] = tileType;
            }
        }
        protected static void Recipes4(int target, int ingredient, int ingredient2, int ingredient3, int ingredient4, int tileType, byte count = 1, byte count2 = 1, byte count3 = 1, byte count4 = 1, byte targetCount = 1) {
            Recipe recipe = Recipe.Create(target, targetCount);
            recipe.AddIngredient(ingredient, count);
            recipe.AddIngredient(ingredient2, count2);
            recipe.AddIngredient(ingredient3, count3);
            recipe.AddIngredient(ingredient4, count4);
            recipe.AddTile(tileType);
            recipe.Register();
        }
        protected static void Recipes3(int target, int ingredient, int ingredient2, int ingredient3, int tileType, byte count = 1, byte count2 = 1, byte count3 = 1, byte targetCount = 1) {
            Recipe recipe = Recipe.Create(target, targetCount);
            recipe.AddIngredient(ingredient, count);
            recipe.AddIngredient(ingredient2, count2);
            recipe.AddIngredient(ingredient3, count3);
            recipe.AddTile(tileType);
            recipe.Register();
        }
        protected static void Recipes2(int target, int ingredient, int ingredient2, int tileType, byte count = 1, byte count2 = 1, byte targetCount = 1) {
            Recipe recipe = Recipe.Create(target, targetCount);
            recipe.AddIngredient(ingredient, count);
            recipe.AddIngredient(ingredient2, count2);
            recipe.AddTile(tileType);
            recipe.Register();
        }
        protected static void Recipes2(int target, string group, int ingredient2, int tileType, byte count = 1, byte count2 = 1, byte targetCount = 1) {
            Recipe recipe = Recipe.Create(target, targetCount);
            recipe.AddRecipeGroup(group, count);
            recipe.AddIngredient(ingredient2, count2);
            recipe.AddTile(tileType);
            recipe.Register();
        }
        protected static void Recipes(int target, int ingredient, int tileType, byte count = 1, byte targetCount = 1) {
            Recipe recipe = Recipe.Create(target, targetCount);
            recipe.AddIngredient(ingredient, count);
            recipe.AddTile(tileType);
            recipe.Register();
        }
        protected static int RoAItem(string itemName) {
            return ModList.Roa.Find<ModItem>(itemName).Type;
        }
    }
}