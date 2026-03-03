using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Accessories.PreHardmode;
using Avalon.Items.Material;
using Avalon.Items.Material.Shards;
using Avalon.Items.Material.TomeMats;
using Avalon.Items.Tomes.PreHardmode;
using Bismuth.Content.Items.Armor;
using Bismuth.Content.Items.Materials;
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
        public class RoasArmor : BaseRecipe {
        public override void DisableRecipe(Recipe recipe)
        {
            DisableRecipe(recipe, RoAItem("SentinelHelmet"));
            DisableRecipe(recipe, RoAItem("SentinelBreastplate"));
            DisableRecipe(recipe, RoAItem("SentinelLeggings"));
            DisableRecipe(recipe, RoAItem("AshwalkerHood"));
            DisableRecipe(recipe, RoAItem("AshwalkerRobe"));
            DisableRecipe(recipe, RoAItem("AshwalkerLeggings"));
        }
        public override void Ingredient(Recipe recipe) {
            AddIngredient(recipe, RoAItem("WorshipperBonehelm"), 1, new Item(ItemType<AncientScrap>(), 10));
            AddIngredient(recipe, RoAItem("WorshipperMantle"), 1, new Item(ItemType<AncientScrap>(), 15));
            AddIngredient(recipe, RoAItem("WorshipperGarb"), 1, new Item(ItemType<AncientScrap>(), 12));
            AddLotIngredient(recipe, RoAItem("FlametrackerHat"), (ModContent.ItemType<PeatPowder>(), 6));
            AddLotIngredient(recipe, RoAItem("FlametrackerJacket"), (ModContent.ItemType<PeatPowder>(), 8));
            AddLotIngredient(recipe, RoAItem("FlametrackerPants"), (ModContent.ItemType<PeatPowder>(), 7));

        }
        public override void PostRecipe()
        {
            CreateSentinelHelmet();
            CreateSentinelBreastplate();
            CreateSentinelLeggings();
            CreateAshwalkerHood();
            CreateAshwalkerRobe();
            CreateAshwalkerLeggings();
        }
        static void CreateSentinelHelmet()
        {
            Recipe recipe = Recipe.Create(RoAItem("SentinelHelmet"), 1);
            recipe.AddIngredient(ItemType<FeralHelmet>(), 1);
            recipe.AddIngredient(RoAItem("MercuriumNugget"), 10);
            recipe.AddIngredient(ItemType<AncientScrap>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateSentinelBreastplate()
        {
            Recipe recipe = Recipe.Create(RoAItem("SentinelBreastplate"), 1);
            recipe.AddIngredient(ItemType<FeralBreastplate>(), 1);
            recipe.AddIngredient(RoAItem("MercuriumNugget"), 12);
            recipe.AddIngredient(ItemType<AncientScrap>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateSentinelLeggings()
        {
            Recipe recipe = Recipe.Create(RoAItem("SentinelLeggings"), 1);
            recipe.AddIngredient(ItemType<FeralLeggings>(), 1);
            recipe.AddIngredient(RoAItem("MercuriumNugget"), 10);
            recipe.AddIngredient(ItemType<AncientScrap>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateAshwalkerHood()
        {
            Recipe recipe = Recipe.Create(RoAItem("AshwalkerHood"), 1);
            recipe.AddIngredient(ItemType<AncientScrap>(), 16);
            recipe.AddIngredient(ItemID.AshBlock, 8);
            recipe.AddIngredient(RoAItem("FlamingFabric"), 15);
            recipe.AddIngredient(ItemType<FireShard>(), 4);
            recipe.AddTile(TileID.Loom);
            recipe.Register();
        }
        static void CreateAshwalkerRobe()
        {
            Recipe recipe = Recipe.Create(RoAItem("AshwalkerRobe"), 1);
            recipe.AddIngredient(ItemType<AncientScrap>(), 24);
            recipe.AddIngredient(ItemID.AshBlock, 12);
            recipe.AddIngredient(RoAItem("FlamingFabric"), 25);
            recipe.AddIngredient(ItemType<FireShard>(), 6);
            recipe.AddTile(TileID.Loom);
            recipe.Register();
        }
        static void CreateAshwalkerLeggings()
        {
            Recipe recipe = Recipe.Create(RoAItem("AshwalkerLeggings"), 1);
            recipe.AddIngredient(ItemType<AncientScrap>(), 18);
            recipe.AddIngredient(ItemID.AshBlock, 10);
            recipe.AddIngredient(RoAItem("FlamingFabric"), 20);
            recipe.AddIngredient(ItemType<FireShard>(), 5);
            recipe.AddTile(TileID.Loom);
            recipe.Register();
        }
    }
    
}