using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Material.Bars;
using Avalon.Items.Material.Herbs;
using Avalon.Items.Material.Shards;
using Avalon.Items.Material.TomeMats;
using Avalon.Items.Potions.Buff;
using Avalon.Items.Tomes.PreHardmode;
using Avalon.Items.Tools.Hardmode;
using Avalon.Items.Weapons.Magic.Hardmode.CrystalUnity;
using Avalon.Items.Weapons.Magic.Hardmode.FreezeBolt;
using Avalon.Items.Weapons.Magic.Hardmode.MagicGrenade;
using Avalon.Items.Weapons.Magic.PreHardmode.GlassEye;
using Avalon.Items.Weapons.Magic.Superhardmode;
using Avalon.Items.Weapons.Melee.Hardmode.CraniumCrusher;
using Avalon.Items.Weapons.Melee.Hardmode.DarklightLance;
using Avalon.Items.Weapons.Melee.Hardmode.FeroziumIceSword;
using Avalon.Items.Weapons.Melee.Hardmode.HallowedClaymore;
using Avalon.Items.Weapons.Melee.Hardmode.QuantumClaymore;
using Avalon.Items.Weapons.Melee.Hardmode.TrueAeonsEternity;
using Avalon.Items.Weapons.Melee.Hardmode.VertexOfExcalibur;
using Avalon.Items.Weapons.Melee.PreHardmode.AeonsEternity;
using Avalon.Items.Weapons.Melee.PreHardmode.UrchinMace;
using Avalon.Items.Weapons.Ranged.Hardmode.CrystalTomahawk;
using Avalon.Items.Weapons.Ranged.PreHardmode.Boompipe;
using Avalon.Items.Weapons.Ranged.PreHardmode.Icicle;
using Avalon.Items.Weapons.Ranged.PreHardmode.Longbone;
using Avalon.Items.Weapons.Ranged.PreHardmode.Moonforce;
using Avalon.Items.Weapons.Ranged.PreHardmode.OsmiumTierLongbows;
using Avalon.Items.Weapons.Summon.Hardmode.ReflectorStaff;
using Bismuth.Content.Items.Materials;
using Bismuth.Content.Items.Placeable;
using Consolaria.Content.Items.Materials;
using Microsoft.Build.Tasks;
using NewHorizons.Content.Items.Materials;
using NewHorizons.Content.Items.Weapons.Throwing;
using Synergia.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Material;
using ValhallaMod.Items.Weapons.Hybrid.Swords;
using ValhallaMod.Items.Weapons.Melee.ChannelMelee;
using ValhallaMod.Items.Weapons.Melee.Swords;
using ValhallaMod.Items.Weapons.Ranged.Longbows;
using ValhallaMod.Items.Weapons.Summon.Auras;
using static Synergia.Common.ModSystems.RecipeSystem.RecipeGroups;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.AvalonsChanges {
    public partial class Avalons;
    public class Avalon_Weapons : BaseRecipe {
        public override void DisableRecipe(Recipe recipe) {
            DisableRecipe(recipe, ItemType<FeroziumIceSword>());
            DisableRecipe(recipe, ItemType<MagicGrenade>());
            DisableRecipe(recipe, ItemType<Boompipe>());
            DisableRecipe(recipe, ItemType<CrystalUnity>());
            DisableRecipe(recipe, ItemType<CrystalTomahawk>());
        }
        public override void Ingredient(Recipe recipe) {

            AddIngredient(recipe, ItemType<GlassEye>(), 1, new Item(ItemType<Cinnabar>(), 1));
            ForeachIngredient(recipe, ItemType<FeroziumPickaxe>(), new Item(ItemType<SoulofBlight>(), 10));
            ForeachIngredient(recipe, ItemType<FeroziumWaraxe>(), new Item(ItemType<SoulofBlight>(), 10));
            ForeachIngredient(recipe, ItemType<Moonforce>(), new Item(ItemType<ValhalliteLongbow>(), 1));
            AddLotIngredient(recipe, ItemType<RhodiumLongbow>(), (ModContent.ItemType<RopeLongbow>(), 1));
            AddLotIngredient(recipe, ItemType<IridiumLongbow>(), (ModContent.ItemType<RopeLongbow>(), 1));
            AddLotIngredient(recipe, ItemType<OsmiumLongbow>(), (ModContent.ItemType<RopeLongbow>(), 1));
            AddIngredient(recipe, ItemType<FreezeBolt>(), 1, new Item(ItemType<FrigidShard>(), 3));
            AddIngredient(recipe, ItemType<Icicle>(), 0, new Item(ItemType<IceCrystal>(), 1));
            AddLotIngredient(recipe, ItemType<Longbone>(), (ModContent.ItemType<ElasticCord>(), 1));
            ForeachIngredient(recipe, ItemType<AeonsEternity>(), new Item(RoAItem("StarFusion"), 1));
            AddIngredient(recipe, ItemType<UrchinMace>(), 1, new Item(ItemType<FishTooth>(), 3));
            AddIngredient(recipe, ItemType<TrueAeonsEternity>(), 1, new Item(ItemID.SoulofSight, 10));
            AddIngredient(recipe, ItemType<TrueAeonsEternity>(), 2, new Item(ItemID.SoulofMight, 10));
            AddLotIngredient(recipe, ItemType<TrueAeonsEternity>(), (ItemID.SoulofFright, 10));
            AddIngredient(recipe, ItemType<QuantumClaymore>(), 1, new Item(ItemType<DarkEssence>(), 10));
            AddIngredient(recipe, ItemType<VertexOfExcalibur>(), 1, new Item(ItemType<BladeEvil>(), 1));

        }
        public override void Recipes() {
        }
        public override void PostRecipe() {
            CreateFeroziumIceSword();
            CreateFeroziumIceSword2();
            CreateFeroziumIceSword3();
            CreateCrystalUnity();
            CreateCrystalTomahawk();
            CreateMagmafrostBolt();
            CreateReflectorStaff();
            CreateDarklance();
            CreateCranium();
            CreateeXC();
        }
        public override void RemoveIngredient(Recipe recipe) {
        }
        public override void Tiles(Recipe recipe) {
        }
        static void CreateFeroziumIceSword()
        {
            Recipe recipe = Recipe.Create(ItemType<FeroziumIceSword>());
            recipe.AddIngredient(ItemType<BlueSlice>());
            recipe.AddIngredient(ItemType<SkadisWrath>());
            recipe.AddIngredient(ItemID.AdamantiteBar, 18);
            recipe.AddIngredient(ItemID.FrostCore, 1);
            recipe.AddIngredient(ItemType<FrigidShard>());
            recipe.AddIngredient(ItemType<SoulofBlight>(), 5);
            recipe.AddTile(TileType<CaesiumHeavyAnvilTile>());
            recipe.Register();
        }
        static void CreateFeroziumIceSword2()
        {
            Recipe recipe = Recipe.Create(ItemType<FeroziumIceSword>());
            recipe.AddIngredient(ItemType<BlueSlice>());
            recipe.AddIngredient(ItemType<SkadisWrath>());
            recipe.AddIngredient(ItemID.TitaniumBar, 18);
            recipe.AddIngredient(ItemID.FrostCore, 1);
            recipe.AddIngredient(ItemType<FrigidShard>());
            recipe.AddIngredient(ItemType<SoulofBlight>(), 5);
            recipe.AddTile(TileType<CaesiumHeavyAnvilTile>());
            recipe.Register();
        }
        static void CreateFeroziumIceSword3()
        {
            Recipe recipe = Recipe.Create(ItemType<FeroziumIceSword>());
            recipe.AddIngredient(ItemType<BlueSlice>());
            recipe.AddIngredient(ItemType<SkadisWrath>());
            recipe.AddIngredient(ItemType<TroxiniumBar>(), 18);
            recipe.AddIngredient(ItemID.FrostCore, 1);
            recipe.AddIngredient(ItemType<FrigidShard>());
            recipe.AddIngredient(ItemType<SoulofBlight>(), 5);
            recipe.AddTile(TileType<CaesiumHeavyAnvilTile>());
            recipe.Register();
        }
        static void CreateCrystalUnity()
        {
            Recipe recipe = Recipe.Create(ItemType<CrystalUnity>());
            recipe.AddIngredient(RoAItem("RodOfTheShock"), 1);
            recipe.AddIngredient(RoAItem("RodOfTheDragonfire"), 1);
            recipe.AddIngredient(RoAItem("RodOfTheTerra"), 1);
            recipe.AddIngredient(RoAItem("RodOfTheStream"), 1);
            recipe.AddIngredient(ItemType<ElementDiamond>());
            recipe.AddTile(TileType<CaesiumHeavyAnvilTile>());
            recipe.Register();
        }
        static void CreateCrystalTomahawk()
        {
            Recipe recipe = Recipe.Create(ItemType<CrystalTomahawk>());
            recipe.AddIngredient(ItemType<RustyAxe>(), 55);
            recipe.AddIngredient(ItemID.CrystalShard, 5);
            recipe.AddIngredient(ItemID.SoulofLight, 1);
            recipe.AddTile(TileType<CaesiumHeavyAnvilTile>());
            recipe.Register();
        }
        static void CreateMagmafrostBolt()
        {
            Recipe recipe = Recipe.Create(ItemType<MagmafrostBolt>());
            recipe.AddIngredient(ItemType<FreezeBolt>(), 1);
            recipe.AddIngredient(RoAItem("FireLighter"), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateReflectorStaff()
        {
            Recipe recipe = Recipe.Create(ItemType<ReflectorStaff>());
            recipe.AddIngredient(ItemType<AluminiumBar>(), 10);
            recipe.AddIngredient(ItemType<Quicksilver>(), 10);
            recipe.AddIngredient(ItemType<BigLens>(), 1);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateDarklance()
        {
            Recipe recipe = Recipe.Create(ItemType<DarklightLance>());
            recipe.AddIngredient(ItemID.Spear, 1);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddIngredient(ItemType<DarkEssence>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateCranium()
        {
            Recipe recipe = Recipe.Create(ItemType<CraniumCrusher>());
            recipe.AddIngredient(ItemType<CorrodeClub>(), 1);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateeXC()
        {
            Recipe recipe = Recipe.Create(ItemType<HallowedClaymore>());
            recipe.AddIngredient(ItemID.HallowedBar, 20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}