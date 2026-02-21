using Synergia.Common.ModConfigs;
using System.Collections.Generic;
using Terraria;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe {
    public abstract class BaseRecipe : ModSystem {
        public sealed override void PostAddRecipes() {
            if (GetInstance<BossConfig>().NewRecipe) {
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
            if (GetInstance<BossConfig>().NewRecipe) {
                Recipes();
            }
        }
        public virtual void DisableRecipe(Recipe recipe) { }
        public abstract void Ingredient(Recipe recipe);
        public virtual void RemoveIngredient(Recipe recipe) { }
        public virtual void Tiles(Recipe recipe) { }
        public virtual void Recipes() { }
        public virtual void PostRecipe() { }
        /// <summary>
        /// Тут и так понятно, но я напишу. Этот метод отключает рецепт, если он создает предмет, который мы передаем в параметре target. 
        /// Второй метод отключает рецепт, если он создает предмет target и имеет в ингредиентах предмет targetIngredient. 
        /// Я сделал эти методы статическими, чтобы не нужно было создавать экземпляр класса для их использования в других классах-наследниках.
        /// </summary>
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
        /// <summary>
        /// Добавляет ингредиент в рецепт, если он создает предмет, который мы передаем в параметре createType. 
        /// по индексу, который мы передаем в параметре indexItem. И добавляет предмет newItem, который мы передаем в параметре newItem.
        /// </summary>
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
        /// <summary>
        /// Добавляет в конец списка ингредиентов предмет, который мы передаем в параметре ingredient, 
        /// и его количество, которое мы передаем в параметре ingredientCount, если рецепт создает предмет, который мы передаем в параметре createType.
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetItem"> для какого предмета </param>
        /// <param name="index">начинается с 0 </param>
        protected static void RemoveIngredient(Recipe recipe, int targetItem, int index) { 
            if (recipe.createItem.type == targetItem) {
                recipe.requiredItem.RemoveAt(index);
            }
        }
        /// <summary>
        /// Добавляет крафт-станцию в рецепт, если он создает предмет, который мы передаем в параметре createType.
        /// </summary>
        protected static void Tiles(Recipe recipe, int createType, int tileType) {
            if (recipe.createItem.type == createType) {
                recipe.requiredTile[^1] = tileType;
            }
        }
        /// Шоркаты для создания рецептов, чтобы не нужно было каждый раз писать одно и то же.
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