using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Accessories.PreHardmode;
using Avalon.Items.Material;
using Avalon.Items.Material.Bars;
using Avalon.Items.Material.Ores;
using Avalon.Items.Material.Shards;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Accessory;
using ValhallaMod.Items.Accessory.Active;
using ValhallaMod.Items.Accessory.Shield;
using ValhallaMod.Items.Material.Bar;
using static Synergia.Common.ModSystems.RecipeSystem.RecipeGroups;
using static Terraria.ModLoader.ModContent;


namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.AvalonsChanges {

        public class ValhallaAccessories : BaseRecipe {

            public override void Ingredient(Recipe recipe) {
            AddIngredient(recipe, ItemType<BlessedHeroShield>(), 2, new Item(ItemType<ForsakenCross>(), 1));
            AddIngredient(recipe, ItemType<MagnetHorseshoe>(), 2, new Item(ItemType<Starstone>(), 10));
            AddIngredient(recipe, ItemType<MagnetHorseshoe>(), 3, new Item(ItemType<Heartstone>(), 10)); 
            AddLotIngredient(recipe, ItemType<RestorationRose>(), (ModContent.ItemType<RestorationBand>(), 1));
            AddIngredient(recipe, ItemType<SquareRoot>(), 2, new Item(ItemType<CoreShard>(), 3));
            AddLotIngredient(recipe, ItemType<TrueSight>(), (ModContent.ItemType<Tourmaline>(), 1));
            AddLotIngredient(recipe, ItemType<TrueSight>(), (ModContent.ItemType<Peridot>(), 1));
            AddLotIngredient(recipe, ItemType<TrueSight>(), (ModContent.ItemType<Zircon>(), 1));
        }
        public override void PostRecipe()
        {
            CreateGlassShield();
            CreatePotionBeltSmall();
            CreateLesserPotionBelt();
            CreateCondensedKnowledge();
        }
        static void CreateGlassShield()
        {
            Recipe recipe = Recipe.Create(ItemType<GlassShield>());
            recipe.AddIngredient(ItemType<HardenedGlass>(), 12);
            recipe.AddIngredient(ItemType<Booger>(), 8);
            recipe.AddTile(TileID.GlassKiln);
            recipe.Register();
        }
        static void CreatePotionBeltSmall()
        {
            Recipe recipe = Recipe.Create(ItemType<PotionBelt>());
            recipe.AddIngredient(ItemType<LesserPotionBelt>(), 1);
            recipe.AddIngredient(ItemID.Deathweed, 3);
            recipe.AddIngredient(ItemID.StrangeBrew, 5);
            recipe.AddIngredient(ItemType<BacciliteBar>(), 5);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }
        static void CreateLesserPotionBelt()
        {
            Recipe recipe = Recipe.Create(ItemType<LesserPotionBelt>());
            recipe.AddIngredient(ItemID.Daybloom, 2);
            recipe.AddIngredient(ItemType<BronzeBar>(), 5);
            recipe.AddIngredient(ItemID.StrangeBrew, 5);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }
        static void CreateCondensedKnowledge()
        {
            Recipe recipe = Recipe.Create(ItemType<LesserPotionBelt>());
            recipe.AddIngredient(ItemID.Book, 3);
            recipe.AddIngredient(ItemType<Starstone>(), 5);
            recipe.AddIngredient(ItemID.ManaFlower, 1);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }

    }
    
}