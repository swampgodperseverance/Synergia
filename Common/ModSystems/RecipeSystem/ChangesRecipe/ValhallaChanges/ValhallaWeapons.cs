
using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Material;
using Avalon.Items.Material.Bars;
using Avalon.Items.Material.Ores;
using Avalon.Items.Material.Shards;
using Avalon.Items.Tomes.Hardmode;
using Avalon.Items.Tomes.PreHardmode;
using Avalon.Items.Tools.Hardmode;
using Avalon.Items.Weapons.Magic.Hardmode.EnergyRevolver;
using Avalon.Items.Weapons.Magic.Hardmode.MagicGrenade;
using Avalon.Items.Weapons.Melee.Hardmode.DarklightLance;
using Avalon.Items.Weapons.Melee.PreHardmode.MarrowMasher;
using Avalon.Items.Weapons.Melee.PreHardmode.UrchinMace;
using Avalon.Items.Weapons.Ranged.PreHardmode.Icicle;
using Avalon.Items.Weapons.Ranged.PreHardmode.Thompson;
using Avalon.Prefixes;
using Bismuth.Content.Items.Materials;
using Bismuth.Content.Items.Placeable;
using Bismuth.Content.Items.Weapons.Magical;
using Bismuth.Content.Items.Weapons.Ranged;
using Consolaria.Content.Items.Weapons.Ranged;
using NewHorizons.Content.Items.Materials;
using NewHorizons.Content.Items.Weapons.Magic;
using NewHorizons.Content.Items.Weapons.Throwing;
using starforgedclassic;
using starforgedclassic.Content.Weapons.PlumeShot;
using Synergia.Content.Items.Materials;
using Synergia.Content.Items.Misc;
using Synergia.Content.Items.Weapons.Melee;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Consumable;
using ValhallaMod.Items.Garden.PlantPots;
using ValhallaMod.Items.Material;
using ValhallaMod.Items.Material.Bar;
using ValhallaMod.Items.Tools;
using ValhallaMod.Items.Weapons.Blood;
using ValhallaMod.Items.Weapons.Hybrid.Swords;
using ValhallaMod.Items.Weapons.Magic.Gloves;
using ValhallaMod.Items.Weapons.Magic.Guns;
using ValhallaMod.Items.Weapons.Magic.Staffs;
using ValhallaMod.Items.Weapons.Magic.Thrown;
using ValhallaMod.Items.Weapons.Magic.Tomes;
using ValhallaMod.Items.Weapons.Melee.Boomerangs;
using ValhallaMod.Items.Weapons.Melee.ChannelMelee;
using ValhallaMod.Items.Weapons.Melee.Glaives;
using ValhallaMod.Items.Weapons.Melee.Knives;
using ValhallaMod.Items.Weapons.Melee.Shortswords;
using ValhallaMod.Items.Weapons.Melee.Spears;
using ValhallaMod.Items.Weapons.Melee.Swords;
using ValhallaMod.Items.Weapons.Ranged.Bows;
using ValhallaMod.Items.Weapons.Ranged.DartGuns;
using ValhallaMod.Items.Weapons.Ranged.Darts;
using ValhallaMod.Items.Weapons.Ranged.Guns;
using ValhallaMod.Items.Weapons.Ranged.Javelins;
using ValhallaMod.Items.Weapons.Ranged.RocketLaunchers;
using ValhallaMod.Items.Weapons.Ranged.Thrown;
using ValhallaMod.Items.Weapons.Summon.Auras;
using ValhallaMod.Items.Weapons.Summon.Whips;
using static Synergia.ModList;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.AvalonsChanges {
        public class ValhallaWeapons : BaseRecipe {
        public override void DisableRecipe(Recipe recipe)
        {
            DisableRecipe(recipe, ItemType<HarpyBow>());
            DisableRecipe(recipe, ItemType<HarpyGreatbow>());
            DisableRecipe(recipe, ItemType<PlagueRifleV>());
            DisableRecipe(recipe, ItemType<HeavensSeal>());
            DisableRecipe(recipe, ItemType<TerraSpear>());
            DisableRecipe(recipe, ItemType<ObsidianSeal>());
            DisableRecipe(recipe, ItemType<GhostVenomStaff>());
            DisableRecipe(recipe, ItemType<CarrotDagger>());
        }
        public override void Ingredient(Recipe recipe) {
            AddLotIngredient(recipe, ItemType<BlueSlice>(), (ModContent.ItemType<FrigidShard>(), 3));
            AddIngredient(recipe, ItemType<StarAuraStaff>(), 0, new Item(ModList.StarforgedClassic.Find<ModItem>("AzuriteBarItem").Type)); 
            AddIngredient(recipe, ItemType<SpiderEgg>(), 1, new Item(ItemID.Grenade, 33));
            AddIngredient(recipe, ItemType<Scarabine>(), 0, new Item(ModContent.ItemType<Thompson>(), 1));
            AddIngredient(recipe, ItemType<Forfeiter>(), 0, new Item(ModContent.ItemType<Scarabine>(), 1));
            AddIngredient(recipe, ItemType<Forfeiter>(), 3, new Item(ModContent.ItemType<GalvornBar>(), 10));
            AddLotIngredient(recipe, ItemType<ValhallaMod.Items.Weapons.Summon.Sentries.SnowPeashooterSentryStaff>(), (ModContent.ItemType<IceCrystal>(), 5 ));
            AddLotIngredient(recipe, ItemType<ValhallaMod.Items.Weapons.Summon.Sentries.SnowPeashooterSentryStaff>(), (ModContent.ItemType<FrostShard>(), 4));
            AddLotIngredient(recipe, ItemType<HellAuraStaff>(), (ModContent.ItemType<FireShard>(), 4));
            AddLotIngredient(recipe, ItemType<VenomDart>(), (ModContent.ItemType<VenomShard>(), 1));
            AddLotIngredient(recipe, ItemType<ValhallaMod.Items.Weapons.Summon.Sentries.FirePeashooterSentryStaff>(), (ModContent.ItemType<FireShard>(), 4));
            ForeachIngredient(recipe, ItemType<FrostDaggers>(), new Item(ItemType<FrigidShard>(), 3));
            AddLotIngredient(recipe, ItemType<ChlorophyteShortsword>(), (ModContent.ItemType<VenomShard>(), 1));
            AddLotIngredient(recipe, ItemType<LeafShield>(), (ModContent.ItemType<VenomShard>(), 1));
            AddLotIngredient(recipe, ItemType<ShellStaff>(), (ModContent.ItemType<VenomShard>(), 1));
            AddLotIngredient(recipe, ItemType<ChlorophyleGlaive>(), (ModContent.ItemType<VenomShard>(), 1));
            AddLotIngredient(recipe, ItemType<SpiderSabre2>(), (ModContent.ItemType<VenomShard>(), 1));
            AddLotIngredient(recipe, ItemType<SparkingShortsword>(), (ModContent.ItemType<FireShard>(), 1));
            AddLotIngredient(recipe, ItemType<Cerberus>(), (ModContent.ItemType<FireShard>(), 1));
            AddLotIngredient(recipe, ItemType<SporePlantPot>(), (ModContent.ItemType<ToxinShard>(), 1));
            AddLotIngredient(recipe, ItemType<Bulbasword>(), (ModContent.ItemType<ToxinShard>(), 1));
        }
        public override void RemoveIngredient(Recipe recipe)
        {
            RemoveIngredient(recipe, ItemType<ValhallaMod.Items.Weapons.Melee.Glaives.DemoniteGlaive>(), 1);
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
            CreateWallnut();
            CreateClocklock();
            CreateHarpyBow();
            CreateGreateHarpyBow();
            CreateLittleFriend();
            CreateHuntressBow1();
            CreateHuntressBow2();
            CreateHuntressBow3();
            CreatePlagueRifleV();
            CreateHeavensSeal();
            CreateRifle76();
            CreateMacEleven();
            CreateHuntersHunch1();
            CreateHuntersHunch2();
            CreateHuntersHunch3();
            CreateHurbringerDawn();
            CreateScarabine();
            CreateReptile();
            CreateLaserScissors();
            CreateCrescento();
            CreateBambooWhip();
            CreateHyperthermia();
            CreateTerraSpear1();
            CreateTerraSpear2();
            CreateBladeEvil();
            CreateMarbleBreaker();
            CreateOlympusSlasher();
            CreateGoldenBomb();
            CreateArcaneBarrage();
            CreateObsidianSeal();
            CreateGhostVenomStaff();
            CreateSparkOfSight();
            CreateSnowflake();
            CreateOmegaDisc();
            CreateAzraels();
            CreateJadeNaginata();
        }
        static void CreateJadeNaginata()
        {
            Recipe recipe = Recipe.Create(ItemType<JadeSpear>());
            recipe.AddIngredient(ItemType<JadeFragment>(), 12);
            recipe.AddIngredient(ItemType<JadeCloth>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateAzraels()
        {
            Recipe recipe = Recipe.Create(ItemType<AzraelsHeartstopper>());
            recipe.AddIngredient(ItemType<ScarletGungnir>(), 1);
            recipe.AddIngredient(ItemID.Ectoplasm, 10);
            recipe.AddIngredient(ItemType<Consolaria.Content.Items.Materials.SoulofBlight>(), 8);
            recipe.AddTile(TileType<CaesiumHeavyAnvilTile>());
            recipe.Register();
        }
        static void CreateOmegaDisc()
        {
            Recipe recipe = Recipe.Create(ItemType<OmegaDisc>());
            recipe.AddIngredient(ItemType<AluminiumBar>(), 10);
            recipe.AddIngredient(ItemType<AncientScrap>(), 5);
            recipe.AddIngredient(ItemID.Bone, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateHeavensSeal()
        {
            Recipe recipe = Recipe.Create(ItemType<HeavensSeal>());
            recipe.AddIngredient(ItemID.HallowedBar, 1);
            recipe.AddIngredient(ItemID.FallenStar, 1);
            recipe.AddIngredient(ItemID.SoulofFlight, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
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
            recipe.AddIngredient(ItemType<HardenedGlass>(), 13);
            recipe.AddIngredient(ItemType<LegalGunParts>(), 1);
            recipe.AddIngredient(ItemType<DurataniumBar>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateGlassCrossbow()
        {
            Recipe recipe = Recipe.Create(ItemType<GlassCrossbow>());
            recipe.AddIngredient(ItemType<HardenedGlass>(), 9);
            recipe.AddIngredient(ItemType<Booger>(), 3);
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
            Recipe recipe = Recipe.Create(ItemType<GlassHamaxe>());
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
        static void CreateHarpyBow()
        {
            Recipe recipe = Recipe.Create(ItemType<HarpyBow>());
            recipe.AddIngredient(ItemType<AluminiumBow>(), 1);
            recipe.AddIngredient(ItemID.Feather, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateGreateHarpyBow()
        {
            Recipe recipe = Recipe.Create(ItemType<HarpyGreatbow>());
            recipe.AddIngredient(ItemType<HarpyBow>(), 1);
            recipe.AddIngredient(ItemType<PlumeShot>(), 1);
            recipe.AddIngredient(ItemType<TornadoShard>(), 5);
            recipe.AddIngredient(ItemID.SoulofFright, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateLittleFriend()
        {
            Recipe recipe = Recipe.Create(ItemType<LittleFriend>());
            recipe.AddIngredient(ItemType<AluminiumBar>(), 20);
            recipe.AddIngredient(ItemType<BigLens>(), 1);
            recipe.AddIngredient(ItemType<PeatPowder>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateHuntressBow1()
        {
            Recipe recipe = Recipe.Create(ItemType<HuntressBow>());
            recipe.AddIngredient(ItemType<AluminiumBow>(), 1);
            recipe.AddIngredient(ItemType<Starstone>(), 10);
            recipe.AddIngredient(ItemType<BacciliteBar>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateHuntressBow2()
        {
            Recipe recipe = Recipe.Create(ItemType<HuntressBow>());
            recipe.AddIngredient(ItemType<AluminiumBow>(), 1);
            recipe.AddIngredient(ItemType<Starstone>(), 10);
            recipe.AddIngredient(ItemID.CrimtaneBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateHuntressBow3()
        {
            Recipe recipe = Recipe.Create(ItemType<HuntressBow>());
            recipe.AddIngredient(ItemType<AluminiumBow>(), 1);
            recipe.AddIngredient(ItemType<Starstone>(), 10);
            recipe.AddIngredient(ItemID.DemoniteBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreatePlagueRifleV()
        {
            Recipe recipe = Recipe.Create(ItemType<PlagueRifleV>());
            recipe.AddIngredient(ItemType<PlagueRifle>(), 1);
            recipe.AddIngredient(RoAItem("ChemicalPrisoner"), 1);
            recipe.AddIngredient(ItemID.VialofVenom, 10);
            recipe.AddIngredient(ItemType<VenomShard>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateRifle76()
        {
            Recipe recipe = Recipe.Create(ItemType<Rifle76>());
            recipe.AddIngredient(ItemType<PlagueRifle>(), 1);
            recipe.AddIngredient(ItemType<AluminiumBar>(), 15);
            recipe.AddIngredient(ItemID.SoulofSight, 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateMacEleven()
        {
            Recipe recipe = Recipe.Create(ItemType<MacEleven>());
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddIngredient(ItemID.HallowedBar, 5);
            recipe.AddIngredient(ItemID.HellstoneBar, 10);
            recipe.AddIngredient(ItemType<BlastShard>(), 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateScarabine()
        {
            Recipe recipe = Recipe.Create(ItemType<Scarabine>());
            recipe.AddIngredient(ItemID.Boomstick, 1);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddIngredient(ItemID.FossilOre, 18);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateHuntersHunch1()
        {
            Recipe recipe = Recipe.Create(ItemType<HuntersHunch>());
            recipe.AddIngredient(ItemType<TroxiniumBar>(), 10);
            recipe.AddIngredient(ItemType<GalvornBar>(), 10);
            recipe.AddRecipeGroup(RecipeGroupID.Wood);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateHuntersHunch2()
        {
            Recipe recipe = Recipe.Create(ItemType<HuntersHunch>());
            recipe.AddIngredient(ItemID.AdamantiteBar, 10);
            recipe.AddIngredient(ItemType<GalvornBar>(), 10);
            recipe.AddRecipeGroup(RecipeGroupID.Wood);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateHuntersHunch3()
        {
            Recipe recipe = Recipe.Create(ItemType<HuntersHunch>());
            recipe.AddIngredient(ItemID.TitaniumBar, 10);
            recipe.AddIngredient(ItemType<GalvornBar>(), 10);
            recipe.AddRecipeGroup(RecipeGroupID.Wood);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateHurbringerDawn()
        {
            Recipe recipe = Recipe.Create(ItemType<HarbingerOfDawn>());
            recipe.AddIngredient(ItemType<Sharanga>(), 1);
            recipe.AddIngredient(ItemType<Heat>(), 1);
            recipe.AddIngredient(ItemType<Consolaria.Content.Items.Materials.SoulofBlight>(), 10);
            recipe.AddIngredient(ItemID.LunarTabletFragment, 10);
            recipe.AddTile(TileType<CaesiumHeavyAnvilTile>());
            recipe.Register();
        }
        static void CreateReptile()
        {
            Recipe recipe = Recipe.Create(ItemType<ReptileRavager>());
            recipe.AddIngredient(ItemType<UrchinMace>(), 1);
            recipe.AddIngredient(ItemType<MarrowMasher>(), 1); 
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateLaserScissors()
        {
            Recipe recipe = Recipe.Create(ItemType<ScissorsKnives>());
            recipe.AddIngredient(ItemType<EnchantedBar>(), 10);
            recipe.AddIngredient(ItemType<BrokenHiltPiece>(), 6);
            recipe.AddIngredient(ItemID.Bone, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateCrescento()
        {
            Recipe recipe = Recipe.Create(ItemType<ShortNight>());
            recipe.AddIngredient(ItemType<Snotknife>(), 1);
            recipe.AddIngredient(ItemType<WaveHarvester>(), 1);
            recipe.AddIngredient(ItemType<Bulbasword>(), 1);
            recipe.AddIngredient(ItemType<SparkingShortsword>(), 1);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
        static void CreateBambooWhip()
        {
            Recipe recipe = Recipe.Create(ItemType<BambooWhip>());
            recipe.AddIngredient(ItemID.BambooBlock, 16);
            recipe.AddIngredient(ItemType<NewHorizons.Content.Items.Weapons.Summon.RopeWhip>(), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
        static void CreateHyperthermia()
        {
            Recipe recipe = Recipe.Create(ItemType<Hyperthermia>());
            recipe.AddIngredient(ItemType<GraniteCharger>(), 1);
            recipe.AddIngredient(ItemType<EnergyRevolver>(), 1);
            recipe.AddIngredient(ItemID.HeatRay, 1);
            recipe.AddTile(TileType<CaesiumHeavyAnvilTile>());
            recipe.Register();
        }
        static void CreateTerraSpear1()
        {
            Recipe recipe = Recipe.Create(ItemType<TerraSpear>());
            recipe.AddIngredient(ItemType<TrueDarkLance>(), 1);
            recipe.AddIngredient(ItemType<TrueGungnir>(), 1);
            recipe.AddIngredient(ItemType<BrokenSpear>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateTerraSpear2()
        {
            Recipe recipe = Recipe.Create(ItemType<TerraSpear>());
            recipe.AddIngredient(ItemType<TrueDarkLance>(), 1);
            recipe.AddIngredient(ItemType<DarklightLance>(), 1);
            recipe.AddIngredient(ItemType<BrokenSpear>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateBladeEvil()
        {
            Recipe recipe = Recipe.Create(ItemType<BladeEvil>());
            recipe.AddIngredient(ItemType<TroxiniumBar>(), 10);
            recipe.AddIngredient(ItemType<EvilIngot>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateMarbleBreaker()
        {
            Recipe recipe = Recipe.Create(ItemType<MarbleShieldBreaker>());
            recipe.AddIngredient(ItemID.Marble, 30); 
            recipe.AddIngredient(ItemID.HallowedBar, 10); 
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateOlympusSlasher()
        {
            Recipe recipe = Recipe.Create(ItemType<OlimpusSlasher>());
            recipe.AddIngredient(ItemID.Marble, 30);
            recipe.AddIngredient(ItemID.SoulofLight, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateGoldenBomb()
        {
            Recipe recipe = Recipe.Create(ItemType<GoldenBomb>());
            recipe.AddIngredient(ItemType<MagicGrenade>(), 1);
            recipe.AddIngredient(ItemType<ValhallaMod.Items.Material.PureGoldChunk>(), 10);
            recipe.AddIngredient(ItemType<Consolaria.Content.Items.Materials.SoulofBlight>(), 10);
            recipe.AddTile(ModContent.TileType<CaesiumHeavyAnvilTile>());
            recipe.Register();
        }
        static void CreateArcaneBarrage()
        {
            Recipe recipe = Recipe.Create(ItemType<ArcaneBarrage>());
            recipe.AddIngredient(ItemType<TatteredBook>(), 1);
            recipe.AddIngredient(ItemID.Amethyst, 10);
            recipe.AddIngredient(ItemType<EnchantedBar>(), 6);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
        static void CreateObsidianSeal()
        {
            Recipe recipe = Recipe.Create(ItemType<ObsidianSeal>());
            recipe.AddIngredient(ItemID.SpellTome, 1);
            recipe.AddIngredient(ItemType<ArcaneBarrage>(), 1);
            recipe.AddIngredient(ItemType<RainOfTears>(), 1);
            recipe.AddIngredient(ItemID.Obsidian, 15);
            recipe.AddIngredient(ItemType<EvilIngot>(), 1);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
        static void CreateGhostVenomStaff()
        {
            Recipe recipe = Recipe.Create(ItemType<GhostVenomStaff>());
            recipe.AddIngredient(ItemID.VenomStaff, 1);
            recipe.AddIngredient(RoAItem("MercuriumStaff"), 1);
            recipe.AddIngredient(ItemID.SpectreStaff, 1);
            recipe.AddIngredient(ItemType<VenomShard>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateSparkOfSight()
        {
            Recipe recipe = Recipe.Create(ItemType<SparkOfSight>());
            recipe.AddIngredient(ItemType<GhostVenomStaff>(), 1);
            recipe.AddIngredient(ItemType<ObsidianSeal>(), 1);
            recipe.AddIngredient(ItemType<BismuthumBar>(), 10);
            recipe.AddIngredient(ItemID.Ectoplasm, 10);
            recipe.AddTile(ModContent.TileType<CaesiumHeavyAnvilTile>());
            recipe.Register();
        }
        static void CreateSnowflake()
        {
            Recipe recipe = Recipe.Create(ItemType<SnowGlaive>());
            recipe.AddIngredient(ItemID.IceBlock, 15);
            recipe.AddIngredient(ItemType<Icicle>(), 50);
            recipe.AddIngredient(ItemID.FallenStar, 8);
            recipe.AddIngredient(ItemType<FrostShard>(), 6);
            recipe.AddTile(TileID.IceMachine);
            recipe.Register();
        }
    }
    
}