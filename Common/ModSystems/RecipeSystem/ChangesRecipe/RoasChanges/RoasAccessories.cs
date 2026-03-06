using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Accessories.PreHardmode;
using Avalon.Items.Material.Ores;
using Avalon.Items.Material.Shards;
using Avalon.Items.Material.TomeMats;
using Avalon.Items.Potions.Buff;
using Avalon.Items.Tomes.PreHardmode;
using Consolaria.Content.Items.Materials;
using NewHorizons.Content.Items.Materials;
using Starforgedclassic.Content.Accessories.SkyShield;
using Synergia.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Accessory;
using ValhallaMod.Items.Accessory.Shield;
using ValhallaMod.Items.Garden;
using ValhallaMod.Items.Material;
using ValhallaMod.Items.Material.Bar;
using static Synergia.Content.Items.Misc.NULLItem;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.AvalonsChanges {
        public class RoasAccessories : BaseRecipe {

            public override void Ingredient(Recipe recipe) {
                AddLotIngredient(recipe, RoAItem("FenethsBlazingWreath"), (ModContent.ItemType<FireShard>(), 1));
                AddLotIngredient(recipe, RoAItem("FeathersInABottle"), (ModContent.ItemType<BreezeShard>(), 3));
                AddLotIngredient(recipe, RoAItem("SnowWreath"), (ModContent.ItemType<FrostShard>(), 1));
                AddLotIngredient(recipe, RoAItem("ForestWreath"), (ModContent.ItemType<EarthShard>(), 1));
                AddLotIngredient(recipe, RoAItem("JungleWreath"), (ModContent.ItemType<ToxinShard>(), 1));
                AddLotIngredient(recipe, RoAItem("BeachWreath"), (ModContent.ItemType<WaterShard>(), 1));
                AddIngredient(recipe, RoAItem("JungleWreath2"), 1, new Item(ItemType<ToxinShard>(), 3));
                AddIngredient(recipe, RoAItem("SnowWreath2"), 1, new Item(ItemType<FrostShard>(), 3));
                AddIngredient(recipe, RoAItem("ForestWreath2"), 1, new Item(ItemType<EarthShard>(), 3));
                AddIngredient(recipe, RoAItem("BeachWreath2"), 1, new Item(ItemType<WaterShard>(), 3));
                AddIngredient(recipe, RoAItem("CosmicHat"), 2, new Item(ItemType<Starstone>(), 5));
                AddIngredient(recipe, RoAItem("RoyalQualityHoney"), 1, new Item(ItemType<Heartstone>(), 5));


        }

    }
    
}