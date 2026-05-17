using Avalon.Items.Material.Bars;
using Synergia.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Armor;
using ValhallaMod.Items.Material;
using ValhallaMod.Items.Weapons.Hybrid.Swords;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.AvalonsChanges {
   public class ValhallaArmor : BaseRecipe {
        public override void DisableRecipe(Recipe recipe)
        {
            DisableRecipe(recipe, ItemType<EvilLegs>());
            DisableRecipe(recipe, ItemType<EvilHead>());
            DisableRecipe(recipe, ItemType<EvilBody>());
        }
        public override void Ingredient(Recipe recipe) {



        }
        public override void PostRecipe()
        {
            CreateEvilLegs();
            CreateEvilHead();
            CreateEvilBody();
            CreateFlinxBolero();
        }
        static void CreateEvilLegs()
        {
            Recipe recipe = Recipe.Create(ItemType<EvilLegs>());
            recipe.AddIngredient(ItemType<CaesiumBar>(), 15);
            recipe.AddIngredient(ItemType<EvilIngot>(), 1);
            recipe.AddTile(ModContent.TileType<CaesiumHeavyAnvilTile>());
            recipe.Register();
        }
        static void CreateEvilBody()
        {
            Recipe recipe = Recipe.Create(ItemType<EvilBody>());
            recipe.AddIngredient(ItemType<CaesiumBar>(), 20);
            recipe.AddIngredient(ItemType<EvilIngot>(), 2);
            recipe.AddTile(ModContent.TileType<CaesiumHeavyAnvilTile>());
            recipe.Register();
        }
        static void CreateEvilHead()
        {
            Recipe recipe = Recipe.Create(ItemType<EvilHead>());
            recipe.AddIngredient(ItemType<CaesiumBar>(), 15);
            recipe.AddIngredient(ItemType<EvilIngot>(), 1);
            recipe.AddTile(ModContent.TileType<CaesiumHeavyAnvilTile>());
            recipe.Register();
        }
        static void CreateFlinxBolero()
        {
            Recipe recipe = Recipe.Create(ItemType<FlinxFurBolero>());
            recipe.AddIngredient(ItemID.Silk, 10);
            recipe.AddIngredient(ItemID.FlinxFur, 4);
            recipe.AddIngredient(ItemType<BismuthBar>(), 4);
            recipe.AddTile(TileID.Loom);
            recipe.Register();
        }
    }
}