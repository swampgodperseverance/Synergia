using Avalon.Items.Material.Bars;
using Avalon.Items.Material.Shards;
using Avalon.Items.Potions.Buff;
using Bismuth.Content.Items.Armor;
using Bismuth.Content.Items.Materials;
using Bismuth.Content.Items.Weapons.Magical;
using Bismuth.Content.Items.Weapons.Melee;
using Bismuth.Content.Tiles;
using Consolaria.Content.Items.Materials;
using NewHorizons.Content.Items.Armor.AshenArmor;
using NewHorizons.Content.Items.Armor.PyroArmor;
using NewHorizons.Content.Items.Armor.SkyArmor;
using Synergia.Content.Items.Misc;
using Terraria;
using Terraria.ID;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.HorizonsChanges
{
    public class BismuthWeapons : BaseRecipe
    {
        public override void DisableRecipe(Recipe recipe)
        {

        }
        public override void Ingredient(Recipe recipe)
        {



        }
        public override void PostRecipe()
        {
            CreateMoonStaff();
            CreateRuneSword();

        }
        static void CreateRuneSword()
        {
            Recipe recipe = Recipe.Create(ItemType<MoonlightStaff>());
            recipe.AddIngredient(ItemType<RuneEssence>(), 12);
            recipe.AddIngredient(ItemType<DurataniumBar>(), 8);
            recipe.AddTile(TileType<RuneTable>());
            recipe.Register();
        }
        static void CreateMoonStaff()
        {
            Recipe recipe = Recipe.Create(ItemType<RunicBlade>());
            recipe.AddIngredient(ItemType<RuneEssence>(), 10);
            recipe.AddIngredient(ItemType<DurataniumBar>(), 8);
            recipe.AddTile(TileType<RuneTable>());
            recipe.Register();
        }

    }

}