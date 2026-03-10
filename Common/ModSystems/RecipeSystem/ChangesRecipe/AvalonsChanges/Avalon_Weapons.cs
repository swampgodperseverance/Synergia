using Avalon.Items.Material.Herbs;
using Avalon.Items.Material.Shards;
using Avalon.Items.Potions.Buff;
using Avalon.Items.Weapons.Magic.PreHardmode.GlassEye;
using Bismuth.Content.Items.Materials;
using Consolaria.Content.Items.Materials;
using Synergia.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using static Synergia.Common.ModSystems.RecipeSystem.RecipeGroups;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.AvalonsChanges {
    public partial class Avalons;
    public class Avalon_Weapons : BaseRecipe {
        public override void DisableRecipe(Recipe recipe) {

        }
        public override void Ingredient(Recipe recipe) {

            AddIngredient(recipe, ItemType<GlassEye>(), 1, new Item(ItemType<Cinnabar>(), 1));
        }
        public override void Recipes() {
        }
        public override void PostRecipe() {

        }
        public override void RemoveIngredient(Recipe recipe) {

        }
        public override void Tiles(Recipe recipe) {

        }
       
    }
}