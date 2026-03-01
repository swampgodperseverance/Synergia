using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Accessories.PreHardmode;
using Avalon.Items.Material.Shards;
using Avalon.Items.Material.TomeMats;
using Avalon.Items.Tomes.PreHardmode;
using Consolaria.Content.Items.Materials;
using NewHorizons.Content.Items.Materials;
using Starforgedclassic.Content.Accessories.SkyShield;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Accessory;
using ValhallaMod.Items.Accessory.Shield;
using ValhallaMod.Items.Garden;
using ValhallaMod.Items.Material;
using ValhallaMod.Items.Material.Bar;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.AvalonsChanges {
        public class RoasAccessories : BaseRecipe {

            public override void Ingredient(Recipe recipe) {
                AddLotIngredient(recipe, RoAItem("FenethsBlazingWreath"), (ModContent.ItemType<FireShard>(), 1));
                AddLotIngredient(recipe, RoAItem("SnowWreath"), (ModContent.ItemType<FrostShard>(), 1));
                AddLotIngredient(recipe, RoAItem("ForestWreath"), (ModContent.ItemType<EarthShard>(), 1));
                AddLotIngredient(recipe, RoAItem("JungleWreath"), (ModContent.ItemType<ToxinShard>(), 1));
                AddLotIngredient(recipe, RoAItem("BeachWreath"), (ModContent.ItemType<WaterShard>(), 1));

        }

        }
    
}