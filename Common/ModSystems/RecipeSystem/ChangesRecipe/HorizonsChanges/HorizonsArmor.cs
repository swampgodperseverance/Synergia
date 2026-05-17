using Avalon.Items.Material.Bars;
using Avalon.Items.Material.Shards;
using Avalon.Items.Placeable.Painting;
using Avalon.Items.Weapons.Melee.Hardmode.FeroziumIceSword;
using Bismuth.Content.Items.Placeable;
using Consolaria.Content.Items.Materials;
using NewHorizons.Content.Items.Accessories;
using NewHorizons.Content.Items.Armor.AshenArmor;
using NewHorizons.Content.Items.Armor.BeastArmor;
using NewHorizons.Content.Items.Armor.LightMageArmor;
using NewHorizons.Content.Items.Armor.NanotechArmor;
using NewHorizons.Content.Items.Armor.NightMageArmor;
using NewHorizons.Content.Items.Armor.PyroArmor;
using NewHorizons.Content.Items.Armor.RottenArmor;
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
            DisableRecipe(recipe, ItemType<RottenCloak>());
            DisableRecipe(recipe, ItemType<RottenLeggings>());
            DisableRecipe(recipe, ItemType<RottenScarf>());
            DisableRecipe(recipe, ItemType<BeastHelmet>());
            DisableRecipe(recipe, ItemType<BeastLeggings>());
            DisableRecipe(recipe, ItemType<BeastBreastplate>());
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
            AddIngredient(recipe, ItemType<NanotechBoots>(), 0, new Item(ItemType<AluminiumBar>(), 10));
            AddIngredient(recipe, ItemType<NanotechBreastplate>(), 0, new Item(ItemType<AluminiumBar>(), 12));
            AddIngredient(recipe, ItemType<NanotechHelmet>(), 0, new Item(ItemType<AluminiumBar>(), 8));
            AddIngredient(recipe, ItemType<NanoJetpack>(), 0, new Item(ItemType<AluminiumBar>(), 12));

        }
        public override void PostRecipe()
        {
            CreateWywernHead();
            CreateWywernBody();
            CreateWywernLegs();
            CreateLightHead();
            CreateLightBody();
            CreateLightLegs();
            CreateBeastHead();
            CreateBeastBody();
            CreateBeastLegs();
            CreateRottenHead();
            CreateRottenBody();
            CreateRottenLegs();
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
            recipe.AddIngredient(ItemType<TroxiniumBar>(), 12);
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
        static void CreateBeastHead()
        {
            Recipe recipe = Recipe.Create(ItemType<BeastHelmet>());
            recipe.AddIngredient(ItemType<AluminiumBar>(), 10);
            recipe.AddIngredient(ItemID.TissueSample, 8);
            recipe.AddIngredient(ItemType<CorruptShard>(), 6);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateBeastBody()
        {
            Recipe recipe = Recipe.Create(ItemType<BeastBreastplate>());
            recipe.AddIngredient(ItemType<AluminiumBar>(), 15);
            recipe.AddIngredient(ItemID.TissueSample, 10);
            recipe.AddIngredient(ItemType<CorruptShard>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateBeastLegs()
        {
            Recipe recipe = Recipe.Create(ItemType<BeastLeggings>());
            recipe.AddIngredient(ItemType<AluminiumBar>(), 10);
            recipe.AddIngredient(ItemID.TissueSample, 8);
            recipe.AddIngredient(ItemType<CorruptShard>(), 6);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateRottenHead()
        {
            Recipe recipe = Recipe.Create(ItemType<RottenScarf>());
            recipe.AddIngredient(ItemType<AluminiumBar>(), 10);
            recipe.AddIngredient(ItemID.ShadowScale, 8);
            recipe.AddIngredient(ItemType<CorruptShard>(), 6);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateRottenBody()
        {
            Recipe recipe = Recipe.Create(ItemType<RottenCloak>());
            recipe.AddIngredient(ItemType<AluminiumBar>(), 15);
            recipe.AddIngredient(ItemID.ShadowScale, 10);
            recipe.AddIngredient(ItemType<CorruptShard>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateRottenLegs()
        {
            Recipe recipe = Recipe.Create(ItemType<RottenLeggings>());
            recipe.AddIngredient(ItemType<AluminiumBar>(), 10);
            recipe.AddIngredient(ItemID.ShadowScale, 8);
            recipe.AddIngredient(ItemType<CorruptShard>(), 6);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

}