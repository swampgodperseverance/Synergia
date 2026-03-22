using Avalon.Items.Material.Bars;
using Avalon.Items.Material.Herbs;
using Avalon.Items.Material.Shards;
using Avalon.Items.Potions.Buff;
using Avalon.Items.Tomes.PreHardmode;
using Avalon.Items.Tools.Hardmode;
using Avalon.Items.Weapons.Magic.Hardmode.MagicGrenade;
using Avalon.Items.Weapons.Magic.PreHardmode.GlassEye;
using Avalon.Items.Weapons.Melee.Hardmode.FeroziumIceSword;
using Bismuth.Content.Items.Materials;
using Consolaria.Content.Items.Materials;
using Synergia.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Weapons.Melee.Swords;
using static Synergia.Common.ModSystems.RecipeSystem.RecipeGroups;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.AvalonsChanges {
    public partial class Avalons;
    public class Avalon_Weapons : BaseRecipe {
        public override void DisableRecipe(Recipe recipe) {
            DisableRecipe(recipe, ItemType<FeroziumIceSword>());
            DisableRecipe(recipe, ItemType<MagicGrenade>());
        }
        public override void Ingredient(Recipe recipe) {

            AddIngredient(recipe, ItemType<GlassEye>(), 1, new Item(ItemType<Cinnabar>(), 1));
            ForeachIngredient(recipe, ItemType<FeroziumPickaxe>(), new Item(ItemType<SoulofBlight>(), 10));
            ForeachIngredient(recipe, ItemType<FeroziumWaraxe>(), new Item(ItemType<SoulofBlight>(), 10));
        }
        public override void Recipes() {
        }
        public override void PostRecipe() {
            CreateFeroziumIceSword();
            CreateFeroziumIceSword2();
            CreateFeroziumIceSword3();
        }
        public override void RemoveIngredient(Recipe recipe) {

        }
        public override void Tiles(Recipe recipe) {

        }
        static void CreateFeroziumIceSword()
        {
            Recipe recipe = Recipe.Create(ItemType<FeroziumIceSword>());
            recipe.AddIngredient(ItemType<BlueSlice>());
            recipe.AddIngredient(ItemType<SkadisWrath>());
            recipe.AddIngredient(ItemID.AdamantiteBar, 18);
            recipe.AddIngredient(ItemID.FrostCore, 1);
            recipe.AddIngredient(ItemType<FrigidShard>());
            recipe.AddTile(TileType<CaesiumHeavyAnvilTile>());
            recipe.Register();
        }
        static void CreateFeroziumIceSword2()
        {
            Recipe recipe = Recipe.Create(ItemType<FeroziumIceSword>());
            recipe.AddIngredient(ItemType<BlueSlice>());
            recipe.AddIngredient(ItemType<SkadisWrath>());
            recipe.AddIngredient(ItemID.TitaniumBar, 18);
            recipe.AddIngredient(ItemID.FrostCore, 1);
            recipe.AddIngredient(ItemType<FrigidShard>());
            recipe.AddTile(TileType<CaesiumHeavyAnvilTile>());
            recipe.Register();
        }
        static void CreateFeroziumIceSword3()
        {
            Recipe recipe = Recipe.Create(ItemType<FeroziumIceSword>());
            recipe.AddIngredient(ItemType<BlueSlice>());
            recipe.AddIngredient(ItemType<SkadisWrath>());
            recipe.AddIngredient(ItemType<TroxiniumBar>(), 18);
            recipe.AddIngredient(ItemID.FrostCore, 1);
            recipe.AddIngredient(ItemType<FrigidShard>());
            recipe.AddTile(TileType<CaesiumHeavyAnvilTile>());
            recipe.Register();
        }

    }
}