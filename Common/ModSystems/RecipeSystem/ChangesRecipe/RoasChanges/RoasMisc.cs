using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Accessories.PreHardmode;
using Avalon.Items.Material.Shards;
using Avalon.Items.Material.TomeMats;
using Avalon.Items.Potions.Buff;
using Avalon.Items.Tomes.PreHardmode;
using Avalon.Tiles.Contagion.Chunkstone;
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
        public class RoaMisc : BaseRecipe {
            public override void DisableRecipe(Recipe recipe)
            {
                DisableRecipe(recipe, RoAItem("Tapper"));
            }
            public override void Ingredient(Recipe recipe) {
            AddIngredient(recipe, RoAItem("SlipperyGrenade"), 1, new Item(ItemType<Sap>(), 1));
            AddIngredient(recipe, RoAItem("SlipperyBomb"), 1, new Item(ItemType<Sap>(), 1));
            AddIngredient(recipe, RoAItem("SlipperyDynamite"), 1, new Item(ItemType<Sap>(), 1));
            AddIngredient(recipe, RoAItem("SlipperyGlowstick"), 1, new Item(ItemType<Sap>   (), 1));

        }
            public override void PostRecipe()   
            {
                CreateGalipot();
                CreateMercuriumNugget();
        }
            static void CreateGalipot()
            {
                Recipe recipe = Recipe.Create(RoAItem("Galipot"));
                recipe.AddIngredient(ItemID.Bottle);
                recipe.AddIngredient(ItemType<Sap>(), 2);
                recipe.AddTile(TileID.Bottles);
                recipe.Register();
            }
            static void CreateMercuriumNugget()
            {
                Recipe recipe = Recipe.Create(RoAItem("MercuriumNugget"));
                recipe.AddIngredient(RoAItem("MercuriumOre"));
                recipe.AddIngredient(ItemType<ChunkstoneBlock>(), 2);
                recipe.AddTile(TileID.DemonAltar);
                recipe.Register();
            }

    }
    
}