using Avalon.Items.Material.Bars;
using Avalon.Items.Material.Shards;
using Avalon.Items.Weapons.Melee.Hardmode.FeroziumIceSword;
using Consolaria.Content.Items.Materials;
using NewHorizons.Content.Items.Armor.AshenArmor;
using NewHorizons.Content.Items.Armor.LightMageArmor;
using NewHorizons.Content.Items.Armor.NightMageArmor;
using NewHorizons.Content.Items.Armor.PyroArmor;
using NewHorizons.Content.Items.Armor.SkyArmor;
using NewHorizons.Content.Items.Armor.WyvernHunterArmor;
using NewHorizons.Content.Items.Materials;
using Synergia.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Weapons.Melee.Swords;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.HorizonsChanges
{
    public class HorizonsArmor : BaseRecipe
    {
        //MAKE RECIPE FOR THESE
        public override void DisableRecipe(Recipe recipe)
        {
            DisableRecipe(recipe, ItemType<NightMagePants>());
            DisableRecipe(recipe, ItemType<NightMageHat>());
            DisableRecipe(recipe, ItemType<NightMageCape>());
        }

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
            AddIngredient(recipe, ItemType<WyvernHunterBreastplate>(), 2, new Item(ItemType<TornadoShard>(), 4));
            AddIngredient(recipe, ItemType<WyvernHunterHeadgear>(), 2, new Item(ItemType<TornadoShard>(), 6));
            AddIngredient(recipe, ItemType<WyvernHunterPants>(), 2, new Item(ItemType<TornadoShard>(), 4));
            AddIngredient(recipe, ItemType<LightMageHat>(), 1, new Item(ItemType<WhiteThread>(), 14));
            AddIngredient(recipe, ItemType<LightMagePants>(), 1, new Item(ItemType<WhiteThread>(), 18));
            AddIngredient(recipe, ItemType<LightMageRobe>(), 1, new Item(ItemType<WhiteThread>(), 24));
            AddIngredient(recipe, ItemType<LightMageHat>(), 2, new Item(ItemType<ArcaneShard>(), 6));
            AddIngredient(recipe, ItemType<LightMagePants>(),2, new Item(ItemType<ArcaneShard>(), 8));
            AddIngredient(recipe, ItemType<LightMageRobe>(), 2, new Item(ItemType<ArcaneShard>(), 10));


        }
        public override void PostRecipe()
        {
            CreateWywernHead();
            CreateWywernBody();
            CreateWywernLegs();
            CreateLightHead();
            CreateLightBody();
            CreateLightLegs();
        }
        static void CreateWywernHead()
        {
            Recipe recipe = Recipe.Create(ItemType<WyvernHunterHeadgear>());
            recipe.AddIngredient(ItemType<TroxiniumBar>(), 10);
            recipe.AddIngredient(ItemType<WyvernFur>(), 1);
            recipe.AddIngredient(ItemType<TornadoShard>(), 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateWywernBody()
        {
            Recipe recipe = Recipe.Create(ItemType<WyvernHunterBreastplate>());
            recipe.AddIngredient(ItemType<TroxiniumBar>(), 15);
            recipe.AddIngredient(ItemType<WyvernFur>(), 2);
            recipe.AddIngredient(ItemType<TornadoShard>(), 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateWywernLegs()
        {
            Recipe recipe = Recipe.Create(ItemType<WyvernHunterPants>());
            recipe.AddIngredient(ItemType<TroxiniumBar>(), 10);
            recipe.AddIngredient(ItemType<WyvernFur>(), 1);
            recipe.AddIngredient(ItemType<TornadoShard>(), 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateLightLegs()
        {
            Recipe recipe = Recipe.Create(ItemType<LightMagePants>());
            recipe.AddIngredient(ItemType<DurataniumBar>(), 5);
            recipe.AddIngredient(ItemType<WhiteThread>(), 18);
            recipe.AddIngredient(ItemType<ArcaneShard>(), 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateLightBody()
        {
            Recipe recipe = Recipe.Create(ItemType<LightMageRobe>());
            recipe.AddIngredient(ItemType<DurataniumBar>(), 10);
            recipe.AddIngredient(ItemType<WhiteThread>(), 24);
            recipe.AddIngredient(ItemType<ArcaneShard>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateLightHead()
        {
            Recipe recipe = Recipe.Create(ItemType<LightMageHat>());
            recipe.AddIngredient(ItemType<DurataniumBar>(), 4);
            recipe.AddIngredient(ItemType<WhiteThread>(), 14);
            recipe.AddIngredient(ItemType<ArcaneShard>(), 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }

}