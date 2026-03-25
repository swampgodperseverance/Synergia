using Avalon.Items.Material.Shards;
using Avalon.Items.Potions.Buff;
using Bismuth.Content.Items.Armor;
using Consolaria.Content.Items.Materials;
using NewHorizons.Content.Items.Armor.AshenArmor;
using NewHorizons.Content.Items.Armor.PyroArmor;
using NewHorizons.Content.Items.Armor.SkyArmor;
using Terraria;
using Terraria.ID;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.HorizonsChanges
{
    public class BismuthArmor : BaseRecipe
    {
        public override void DisableRecipe(Recipe recipe)
        {
            DisableRecipe(recipe, ItemType<PikemansHelmet>());
            DisableRecipe(recipe, ItemType<PikemansLeggings>());
            DisableRecipe(recipe, ItemType<PikemansBreastplate>());
        }
        public override void Ingredient(Recipe recipe)
        {



        }
        public override void PostRecipe()
        {
            CreatePikemansHelmet();
            CreatePikemansBreastplate();
            CreatePikemansLeggings();

        }
        static void CreatePikemansHelmet()
        {
            Recipe recipe = Recipe.Create(ItemType<PikemansHelmet>());
            recipe.AddIngredient(ItemID.Silk, 6);
            recipe.AddIngredient(RoAItem("MercuriumNugget"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreatePikemansBreastplate()
        {
            Recipe recipe = Recipe.Create(ItemType<PikemansBreastplate>());
            recipe.AddIngredient(ItemID.Silk, 10);
            recipe.AddIngredient(RoAItem("MercuriumNugget"), 18);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreatePikemansLeggings()
        {
            Recipe recipe = Recipe.Create(ItemType<PikemansLeggings>());
            recipe.AddIngredient(ItemID.Silk, 8);
            recipe.AddIngredient(RoAItem("MercuriumNugget"), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

}