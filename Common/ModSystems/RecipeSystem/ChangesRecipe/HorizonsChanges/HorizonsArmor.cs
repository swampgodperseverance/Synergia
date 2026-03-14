using Avalon.Items.Material.Shards;
using Consolaria.Content.Items.Materials;
using NewHorizons.Content.Items.Armor.AshenArmor;
using NewHorizons.Content.Items.Armor.PyroArmor;
using NewHorizons.Content.Items.Armor.SkyArmor;
using Terraria;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.HorizonsChanges
{
    public class HorizonsArmor : BaseRecipe
    {

        public override void Ingredient(Recipe recipe)
        {
            AddLotIngredient(recipe, ItemType<AshenBreastplate>(), (ModContent.ItemType<FireShard>(), 6));
            AddLotIngredient(recipe, ItemType<AshenLeggins>(), (ModContent.ItemType<FireShard>(), 4));
            AddLotIngredient(recipe, ItemType<AshenShroud>(), (ModContent.ItemType<FireShard>(), 4));
            AddLotIngredient(recipe, ItemType<PyroChest>(), (ModContent.ItemType<FireShard>(), 6));
            AddLotIngredient(recipe, ItemType<PyroHat>(), (ModContent.ItemType<FireShard>(), 4));
            AddLotIngredient(recipe, ItemType<PyroLegwear>(), (ModContent.ItemType<FireShard>(), 4));
            AddIngredient(recipe, ItemType<SkyHood>(), 2, new Item(ItemType<WhiteThread>(), 6));
            AddIngredient(recipe, ItemType<SkyCape>(), 2, new Item(ItemType<WhiteThread>(), 12));
            AddIngredient(recipe, ItemType<SkyPants>(), 2, new Item(ItemType<WhiteThread>(), 8));
            AddIngredient(recipe, ItemType<SkyHood>(), 3, new Item(ItemType<BreezeShard>(), 2));
            AddIngredient(recipe, ItemType<SkyCape>(), 3, new Item(ItemType<BreezeShard>(), 6));
            AddIngredient(recipe, ItemType<SkyPants>(), 3, new Item(ItemType<BreezeShard>(), 4));


        }

    }

}