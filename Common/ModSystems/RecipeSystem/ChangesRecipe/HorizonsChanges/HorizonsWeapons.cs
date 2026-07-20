using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Accessories.PreHardmode;
using Avalon.Items.Material.Bars;
using Avalon.Items.Material.Ores;
using Avalon.Items.Material.Shards;
using Avalon.Items.Placeable.Tile;
using Avalon.Items.Weapons.Melee.PreHardmode.Shurikerang;
using Bismuth.Content.Items.Armor;
using Bismuth.Content.Items.Placeable;
using NewHorizons.Content.Items;
using NewHorizons.Content.Items.Accessories;
using NewHorizons.Content.Items.Ammo;
using NewHorizons.Content.Items.Materials;
using NewHorizons.Content.Items.Weapons;
using NewHorizons.Content.Items.Weapons.Magic;
using NewHorizons.Content.Items.Weapons.Ranged;
using NewHorizons.Content.Items.Weapons.Summon;
using NewHorizons.Content.Items.Weapons.Throwing;
using Starforgedclassic.Content.Accessories.SkyShield;
using Synergia.Content.Items.Weapons.Ranged;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Material;
using ValhallaMod.Items.Material.Bar;
using ValhallaMod.Items.Weapons.Ranged.Guns;
using ValhallaMod.Items.Weapons.Ranged.ProjectileGuns;
using ValhallaMod.Items.Weapons.Summon.Whips;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.HorizonsChanges
{
    public class HorizonsWeapons : BaseRecipe
    {
        public override void DisableRecipe(Recipe recipe)
        {
            DisableRecipe(recipe, ItemType<PalladiumWaraxe>());
            DisableRecipe(recipe, ItemType<OrichalcumKama>());
            DisableRecipe(recipe, ItemType<AdamantiteDagger>());
            DisableRecipe(recipe, ItemType<TitaniumWarhammer>());
            DisableRecipe(recipe, ItemType<CobaltKunai>());
            DisableRecipe(recipe, ItemType<MythrilJavelin>());
            DisableRecipe(recipe, ItemType<MoltenDagger>());
            DisableRecipe(recipe, ItemType<ChlorophyteJavelin>());
        }
        public override void Ingredient(Recipe recipe)
        {

            AddIngredient(recipe, ItemType<HandicraftedBlunderbuss>(), 1, new Item(ItemType<LegalGunParts>(), 1));
            AddIngredient(recipe, ItemType<HandicraftedFlamethrower>(), 1, new Item(ItemType<LegalGunParts>(), 1));
            AddLotIngredient(recipe, ItemType<NewHorizons.Content.Items.Weapons.Summon.RopeWhip>(), (ModContent.ItemType<ElasticCord>(), 1));
            AddLotIngredient(recipe, ItemType<FlareCannon>(), (ModContent.ItemType<FireShard>(), 3));
            AddLotIngredient(recipe, ItemType<Scorcher>(), (ModContent.ItemType<FireShard>(), 3));
            AddLotIngredient(recipe, ItemType<FlamingFlare>(), (ModContent.ItemType<FireShard>(), 1));
            AddIngredient(recipe, ItemType<NewHorizons.Content.Items.Weapons.Summon.RopeWhip>(), 1, new Item(ItemID.Wood, 12));
            AddIngredient(recipe, ItemType<Carnwennan>(), 1, new Item(ItemType<PureGoldChunk>(), 10));
            AddIngredient(recipe, ItemType<DarkVolley>(), 1, new Item(ItemType<WickedShard>(), 5));
            AddIngredient(recipe, ItemType<DarkVolley>(), 1, new Item(ItemType<WickedShard>(), 5));
            AddIngredient(recipe, ItemType<IncendiaryGrenade>(), 0, new Item(RoAItem("FlamingFabric"), 1));
            AddIngredient(recipe, ItemType<Repeater>(), 0, new Item(ItemType<Clocklock>(), 1));
            AddIngredient(recipe, ItemType<Graverobber>(), 1, new Item(ItemType<UndeadShard>(), 5));
            AddIngredient(recipe, ItemType<GelCanister>(), 1, new Item(ItemType<HardenedGlass>(), 1));
            AddIngredient(recipe, ItemType<HymnOfProtection>(), 0, new Item(ItemType<TatteredBook>(), 1));
            AddIngredient(recipe, ItemType<HymnOfProtection>(), 2, new Item(ItemType<AluminiumBar>(), 5));
            AddLotIngredient(recipe, ItemType<ClockworkShotgun>(), (ModContent.ItemType<Clocklock>(), 1));
            AddIngredient(recipe, ItemType<CrimerasTongue>(), 1, new Item(ItemType<BambooWhip>(), 1));
            AddIngredient(recipe, ItemType<HungryWorm>(), 1, new Item(ItemType<BambooWhip>(), 1));
            AddIngredient(recipe, ItemType<NanoStar>(), 0, new Item(ItemType<AluminiumBar>(), 8));
            AddIngredient(recipe, ItemType<ScarletGungnir>(), 1, new Item(ItemType<WickedShard>(), 5));
            AddIngredient(recipe, ItemType<CrystalGrenade>(), 0, new Item(ItemID.CrystalShard, 3));
            AddIngredient(recipe, ItemType<CrystalDagger>(), 0, new Item(ItemID.CrystalShard, 1));
            AddIngredient(recipe, ItemType<RustyAxe>(), 1, new Item(ItemType<AncientScrap>(), 1));
        }
        public override void RemoveIngredient(Recipe recipe)
        {
            RemoveIngredient(recipe, ItemType<BloodSpiller>(), 1);
            RemoveIngredient(recipe, ItemType<CrystalDagger>(), 1);
        }
        public override void PostRecipe()
        {
            CreateSeaBeast();
            CreateSpaceCowboy();
            CreateWandMana1();
            CreateWandMana2();
            CreateGungnir();
            CreateSaws();
            CreateAD();
            CreateTW();
            Createck();
            CreateMJ();
            CreateOK();
            CreatePW();
            CreateMD();
            CreateCJ();
        }
        static void CreateCJ()
        {
            Recipe recipe = Recipe.Create(ItemType<ChlorophyteJavelin>(), 50);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 1);
            recipe.AddIngredient(ItemType<VenomShard>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateMD()
        {
            Recipe recipe = Recipe.Create(ItemType<MoltenDagger>(), 50);
            recipe.AddIngredient(ItemID.HellstoneBar, 1);
            recipe.AddIngredient(ItemID.Obsidian, 1);
            recipe.AddIngredient(ItemType<FireShard>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreatePW()
        {
            Recipe recipe = Recipe.Create(ItemType<PalladiumWaraxe>(), 50);
            recipe.AddIngredient(ItemID.PalladiumBar, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateOK()
        {
            Recipe recipe = Recipe.Create(ItemType<OrichalcumKama>(), 50);
            recipe.AddIngredient(ItemID.OrichalcumBar, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateMJ()
        {
            Recipe recipe = Recipe.Create(ItemType<MythrilJavelin>(), 50);
            recipe.AddIngredient(ItemID.MythrilBar, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void Createck()
        {
            Recipe recipe = Recipe.Create(ItemType<CobaltKunai>(), 50);
            recipe.AddIngredient(ItemID.CobaltBar, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateTW()
        {
            Recipe recipe = Recipe.Create(ItemType<TitaniumWarhammer>(), 50);
            recipe.AddIngredient(ItemID.TitaniumBar, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateAD()
        {
            Recipe recipe = Recipe.Create(ItemType<AdamantiteDagger>(), 50);
            recipe.AddIngredient(ItemID.AdamantiteBar, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateSeaBeast()
        {
            Recipe recipe = Recipe.Create(ItemType<TheSeaBeast>());
            recipe.AddIngredient(ItemType<LooseCannon>(), 1);
            recipe.AddIngredient(ItemType<TheGalleon>(), 1);
            recipe.AddIngredient(ItemType<Blunderbussin>(), 1);
            recipe.AddIngredient(ItemID.SoulofSight, 1);
            recipe.AddIngredient(ItemID.SoulofFright, 1);
            recipe.AddIngredient(ItemID.SoulofMight, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        static void CreateSpaceCowboy()
        {
            Recipe recipe = Recipe.Create(ItemType<SpaceCowboy>());
            recipe.AddIngredient(ItemID.Revolver, 1);
            recipe.AddIngredient(ItemID.SpaceGun, 1);
            recipe.AddIngredient(ItemType<NaquadahBar>(), 2);
            recipe.AddIngredient(ItemType<Avalon.Items.Material.Bars.BronzeBar>(), 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateWandMana1()
        {
            Recipe recipe = Recipe.Create(ItemType<WandOfMana>());
            recipe.AddIngredient(ItemType<DuskplateBlock>(), 14);
            recipe.AddIngredient(ItemID.ManaCrystal, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateWandMana2()
        {
            Recipe recipe = Recipe.Create(ItemType<WandOfMana>());
            recipe.AddIngredient(ItemType<MoonplateBlock>(), 14);
            recipe.AddIngredient(ItemID.ManaCrystal, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateGungnir()
        {
            Recipe recipe = Recipe.Create(ItemType<ScarletGungnir>());
            recipe.AddIngredient(ItemType<TroxiniumBar>(), 12);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddIngredient(ItemID.SoulofSight, 6);
            recipe.AddIngredient(ItemID.SoulofFright, 6);
            recipe.AddIngredient(ItemID.SoulofMight, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();

        }
        static void CreateSaws()
        {
            Recipe recipe = Recipe.Create(ItemType<BlazingSaws>());
            recipe.AddIngredient(ItemType<Shurikerang>(), 1);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();

        }
    }
    
}