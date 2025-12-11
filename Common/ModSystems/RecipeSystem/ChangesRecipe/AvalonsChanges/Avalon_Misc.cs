using Avalon.Items.Material.Bars;
using Avalon.Items.Material.OreChunks;
using Avalon.Items.Material.Ores;
using Avalon.Items.Material.Shards;
using Avalon.Tiles;
using Bismuth.Content.Items.Materials;
using Synergia.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using static Synergia.Common.ModSystems.RecipeSystem.RecipeGroups;
using static Terraria.ModLoader.ModContent;
using VenomSpike2 = Avalon.Items.Placeable.Tile.VenomSpike;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.AvalonsChanges {
    public partial class Avalons {
        public class Avalons_Misc : BaseRecipe {
            public override void DisableRecipe(Recipe recipe) {
                DisableRecipe(recipe, CaesiumForge);
            }
            public override void Ingredient(Recipe recipe) {
                AddIngredientNotIndex(recipe, ItemType<FrigidShard>(), ItemType<IceCrystal>());
                AddIngredient(recipe, ItemType<VenomSpike2>(), 1, new Item(ItemID.VialofVenom, 5));
            }
            public override void Recipes() {
                Recipes2(ItemType<CaesiumHeavyAnvil>(), HARDMODEANVIL, CaesiumBar, CaesiumForgeTile, 1, 10);
                OreAndBar(ItemType<IridiumBar>(), ItemType<OsmiumBar>(), 10, 10);
                OreAndBar(ItemType<IridiumOre>(), ItemType<OsmiumOre>(), 40, 40);
                OreAndBar(ItemType<OsmiumBar>(), ItemType<RhodiumBar>(), 10, 10);
                OreAndBar(ItemType<OsmiumOre>(), ItemType<RhodiumOre>(), 40, 40);
                OreAndBar(ItemType<RhodiumBar>(), ItemType<IridiumBar>(), 10, 10);
                OreAndBar(ItemType<RhodiumOre>(), ItemType<IridiumOre>(), 40, 40);
            }
            public override void PostRecipe() {
                Recipes2(CaesiumForge, HARDMODEFORGE, ItemType<CaesiumChunk>(), TileID.MythrilAnvil, 1, 30);
            }
            public override void RemoveIngredient(Recipe recipe) {

            }
            public override void Tiles(Recipe recipe) {

            }
            static void OreAndBar(int target, int ingredient, byte count, byte count2) {
                Recipes2(target, ingredient, ItemType<Sulphur>(), TileType<Catalyzer>(), count, 1, count2);
            }
        }
    }
}