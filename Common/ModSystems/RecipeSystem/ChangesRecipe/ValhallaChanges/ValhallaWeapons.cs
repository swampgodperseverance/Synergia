using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Accessories.PreHardmode;
using Avalon.Items.Material;
using Avalon.Items.Material.Bars;
using Avalon.Items.Material.Ores;
using Avalon.Items.Material.Shards;
using Avalon.Items.Material.TomeMats;
using Avalon.Items.Potions.Buff;
using Avalon.Items.Tomes.PreHardmode;
using Avalon.Items.Tools.Hardmode;
using Bismuth.Content.Items.Materials;
using Consolaria.Content.Items.Materials;
using NewHorizons.Content.Items.Materials;
using NewHorizons.Content.Items.Weapons.Ranged;
using starforgedclassic.Content.Placeables.AzuriteBar;
using Starforgedclassic.Content.Accessories.SkyShield;
using Synergia.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Accessory;
using ValhallaMod.Items.Accessory.Shield;
using ValhallaMod.Items.Garden;
using ValhallaMod.Items.Material;
using ValhallaMod.Items.Material.Bar;
using ValhallaMod.Items.Tools;
using ValhallaMod.Items.Weapons.Magic.Gloves;
using ValhallaMod.Items.Weapons.Melee.ChannelMelee;
using ValhallaMod.Items.Weapons.Melee.Knives;
using ValhallaMod.Items.Weapons.Melee.Swords;
using ValhallaMod.Items.Weapons.Ranged.Bows;
using ValhallaMod.Items.Weapons.Ranged.Guns;
using ValhallaMod.Items.Weapons.Ranged.ProjectileGuns;
using ValhallaMod.Items.Weapons.Summon.Auras;
using ValhallaMod.Items.Weapons.Summon.Whips;
using ValhallaMod.Projectiles.Summon.Sentries;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.AvalonsChanges {
        public class ValhallaWeapons : BaseRecipe {

            public override void Ingredient(Recipe recipe) {
            AddLotIngredient(recipe, ItemType<BlueSlice>(), (ModContent.ItemType<FrigidShard>(), 3));
            // AddIngredient(recipe, ItemType<StarAuraStaff>(), 0, starforgedclassic.Find<ModItem>("AzuriteBarItem"), 10);

            AddLotIngredient(recipe, ItemType<ValhallaMod.Items.Weapons.Summon.Sentries.SnowPeashooterSentryStaff>(), (ModContent.ItemType<IceCrystal>(), 5 ));
            AddLotIngredient(recipe, ItemType<ValhallaMod.Items.Weapons.Summon.Sentries.SnowPeashooterSentryStaff>(), (ModContent.ItemType<FrostShard>(), 4));
            AddLotIngredient(recipe, ItemType<HellAuraStaff>(), (ModContent.ItemType<FireShard>(), 4));
            AddLotIngredient(recipe, ItemType<ValhallaMod.Items.Weapons.Summon.Sentries.FirePeashooterSentryStaff>(), (ModContent.ItemType<FireShard>(), 4));
            ForeachIngredient(recipe, ItemType<FrostDaggers>(), new Item(ItemType<FrigidShard>(), 3));
        }
        public override void PostRecipe()
        {
            CreateEverIce();
            CreateSkadisWrath();
            CreateForbiddenCrescent();
            CreateFrostDaggers();
            CreatePaneOfPain();
            CreateGlassCannon();
            CreateGlassCrossbow();
            CreateGlassPickaxe();
            CreateGlassHamaxe();
            CreateGlassCrossbow();
            CreateWallnut();
            CreateClocklock();
        }
        static void CreateEverIce()
        {
            Recipe recipe = Recipe.Create(ItemType<EverlivingIce>());
            recipe.AddIngredient(ItemType<ValhalliteSword>(), 1);
            recipe.AddIngredient(ItemType<OsmiumBar>(), 10);
            recipe.AddIngredient(ItemType<FrostShard>(), 5);
            recipe.AddIngredient(ItemType<IceCrystal>(), 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateSkadisWrath()
        {
            Recipe recipe = Recipe.Create(ItemType<SkadisWrath>());
            recipe.AddIngredient(ItemType<EverlivingIce>(), 1);
            recipe.AddIngredient(ItemType<SoulofIce>(), 5);
            recipe.AddIngredient(ItemID.SoulofSight, 1);
            recipe.AddIngredient(ItemID.SoulofFright, 1);
            recipe.AddIngredient(ItemID.SoulofMight, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateForbiddenCrescent()
        {
            Recipe recipe = Recipe.Create(ItemType<ForbiddenCrescent>());
            recipe.AddIngredient(ItemType<TroxiniumBar>(), 10);
            recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateFrostDaggers()
        {
            Recipe recipe = Recipe.Create(ItemType<FrostDaggers>());
            recipe.AddIngredient(ItemType<TroxiniumBar>(), 10);
            recipe.AddIngredient(ItemID.FrostCore, 1);
            recipe.AddIngredient(ItemType<FrigidShard>(), 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreatePaneOfPain()
        {
            Recipe recipe = Recipe.Create(ItemType<PaneOfPain>());
            recipe.AddIngredient(ItemType<DurataniumBar>(), 5);
            recipe.AddIngredient(ItemType<HardenedGlass>(), 20);
            recipe.AddTile(TileID.GlassKiln);
            recipe.Register();
        }
        static void CreateGlassCannon()
        {
            Recipe recipe = Recipe.Create(ItemType<GlassCannon>());
            recipe.AddIngredient(ItemType<HardenedGlass>(), 9);
            recipe.AddIngredient(ItemType<Booger>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateGlassCrossbow()
        {
            Recipe recipe = Recipe.Create(ItemType<GlassCrossbow>());
            recipe.AddIngredient(ItemType<HardenedGlass>(), 13);
            recipe.AddIngredient(ItemType<LegalGunParts>(), 1);
            recipe.AddIngredient(ItemType<DurataniumBar>(), 5);
            recipe.AddTile(TileID.GlassKiln);
            recipe.Register();
        }
        static void CreateGlassPickaxe()
        {
            Recipe recipe = Recipe.Create(ItemType<GlassPickaxe>());
            recipe.AddIngredient(ItemType<HardenedGlass>(), 15);
            recipe.AddIngredient(ItemType<Booger>(), 6);
            recipe.AddTile(TileID.GlassKiln);
            recipe.Register();
        }
        static void CreateGlassHamaxe()
        {
            Recipe recipe = Recipe.Create(ItemType<GlassPickaxe>());
            recipe.AddIngredient(ItemType<HardenedGlass>(), 12);
            recipe.AddIngredient(ItemType<Booger>(), 5);
            recipe.AddTile(TileID.GlassKiln);
            recipe.Register();
        }

        static void CreateWallnut()
        {
            Recipe recipe = Recipe.Create(ItemType<GiantWallNutBowler>());
            recipe.AddIngredient(ItemType<WallNutBowler>(), 1);
            recipe.AddIngredient(ItemType<DurataniumBar>(), 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateClocklock()
        {
            Recipe recipe = Recipe.Create(ItemType<Clocklock>());
            recipe.AddIngredient(ItemType<CogBronze>(), 2);
            recipe.AddIngredient(ItemID.FlintlockPistol, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

    }
    
}