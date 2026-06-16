using System.Collections.Generic;
using Avalon.Items.Armor.Hardmode;
using Avalon.Items.Armor.PreHardmode;
using Avalon.Items.Material;
using Avalon.Items.Material.Bars;
using Avalon.Items.Material.Herbs;
using Avalon.Items.Material.Shards;
using Avalon.Items.Placeable.Tile;
using Avalon.Items.Weapons.Melee.Hardmode.MasterSword;
using Avalon.Items.Weapons.Melee.Hardmode.QuantumClaymore;
using Avalon.Items.Weapons.Melee.Hardmode.TrueAeonsEternity;
using Bismuth.Content.Items.Materials;
using Bismuth.Content.Items.Weapons.Melee;
using Consolaria.Content.Items.Materials;
using Consolaria.Content.Items.Summons;
using Consolaria.Content.Items.Weapons.Melee;
using Consolaria.Content.Items.Weapons.Ranged;
using Consolaria.Content.Items.Weapons.Throwing;
using NewHorizons.Content.Items.Armor.BeastArmor;
using NewHorizons.Content.Items.Armor.NanotechArmor;
using NewHorizons.Content.Items.Armor.NightMageArmor;
using NewHorizons.Content.Items.Armor.RottenArmor;
using NewHorizons.Content.Items.Armor.WyvernHunterArmor;
using NewHorizons.Content.Items.Materials;
using Synergia.Content.Items.Accessories;
using Synergia.Content.Items.Consumables;
using Synergia.Content.Items.Weapons.AuraStaff;
using Synergia.Content.Items.Weapons.Summon;
using Synergia.Content.Items.Weapons.Throwing;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Material;
using ValhallaMod.Items.Weapons.Hybrid.Swords;
using ValhallaMod.Items.Weapons.Melee.Spears;
using ValhallaMod.Items.Weapons.Melee.Swords;
using ValhallaMod.Items.Weapons.Ranged.Bows;
using ValhallaMod.Items.Weapons.Ranged.Bows.Wood;
using ValhallaMod.Items.Weapons.Summon.Auras;
using ValhallaMod.Items.Weapons.Summon.Whips;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe {
    public class AllMod : BaseRecipe {
        public override void DisableRecipe(Recipe recipe)
        {
            DisableRecipe(recipe, ItemType<Sharanga>());
            DisableRecipe(recipe, ItemID.PumpkinMoonMedallion);
        }
        public override void Ingredient(Recipe recipe) {
            AddLotIngredient(recipe, ItemID.VenomBullet, (ModContent.ItemType<VenomShard>(), 1));
            AddLotIngredient(recipe, ItemID.VenomArrow, (ModContent.ItemType<VenomShard>(), 1));
            AddLotIngredient(recipe, ItemID.VenomBullet, (ModContent.ItemType<VenomShard>(), 1));
            AddLotIngredient(recipe, ItemID.FlaskofVenom, (ModContent.ItemType<VenomShard>(), 1));
            AddLotIngredient(recipe, ItemType<VenomSpike>(), (ModContent.ItemType<VenomShard>(), 1));
            AddIngredient(recipe, ItemType<WhiteThread>(), 0, new Item(ItemID.Cobweb, 2));
            AddIngredient(recipe, ItemType<SuspiciousLookingSkull>(), 0, new Item(ItemID.LunarTabletFragment, 5));
            AddLotIngredient(recipe, ItemID.Zenith, (ModContent.ItemType<SolarWind>(), 1));
            AddLotIngredient(recipe, ItemID.Zenith, (ModContent.ItemType<QuantumClaymore>(), 1));
            AddLotIngredient(recipe, ItemID.Zenith, (ModContent.ItemType<TrueAeonsEternity>(), 1));
            AddLotIngredient(recipe, ItemID.Zenith, (ModContent.ItemType<Radiance>(), 1));
            AddLotIngredient(recipe, ItemID.Zenith, (ModContent.ItemType<BladeEvil>(), 1));
            AddIngredient(recipe, ItemType<Tonbogiri>(), 0, new Item(ModContent.ItemType<TerraSpear>(), 1));
        }
        public override void PostPostRecipe(Recipe recipe) {
            List<int> potion = [ItemID.ThornsPotion];
            List<int> flower = [ItemID.Deathweed, ItemType<Bloodberry>(), ItemType<Barfbush>()];
            AddRecipeGroup(recipe, new RecipeGroupStruct(potion, flower, RecipeGroups.FLOWER));
        }
        public override void PostRecipe()
        {
            CreateSharanga();
            CreateUnderwaterAuraStaff();
            CreateAngryParasite();
            CreateHolyHandGrenade();
            CreateTriwhip();
            CreateEverNecklace();
            CreateMercAura();
            CreateAirflow();
            CreateEarthCollapse();
            CreateUnderScythe();
            CreatePumpkinMedallion();
            CreateShardflingPotion();
        }
        static void CreatePumpkinMedallion()
        {
            Recipe recipe = Recipe.Create(ItemID.PumpkinMoonMedallion);
            recipe.AddIngredient(ItemID.Pumpkin, 30);
            recipe.AddIngredient(ItemType<ThunderShard>(), 5);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateUnderScythe()
        {
            Recipe recipe = Recipe.Create(ItemType<UnderwaterAuraScythe>());
            recipe.AddIngredient(ItemType<UnderwaterAuraStaff>(), 1);
            recipe.AddIngredient(ItemType<TorrentShard>(), 10);
            recipe.AddIngredient(ItemID.SoulofMight, 3);
            recipe.AddIngredient(ItemID.SoulofFright, 3);
            recipe.AddIngredient(ItemID.SoulofSight, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
        static void CreateEarthCollapse()
        {
            Recipe recipe = Recipe.Create(ItemType<EarthCollapse>());
            recipe.AddIngredient(ItemType<EnchantedRing>(), 1);
            recipe.AddIngredient(ItemType<CoreShard>(), 10);
            recipe.AddIngredient(RoAItem("NaturesHeart"), 1);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
        static void CreateAirflow()
        {
            Recipe recipe = Recipe.Create(ItemType<Airflow>());
            recipe.AddIngredient(ModList.StarforgedClassic.Find<ModItem>("AzuriteBarItem").Type, 10);
            recipe.AddTile(ModList.StarforgedClassic.Find<ModTile>("StarForgeTile").Type);
            recipe.Register();
        }
        static void CreateMercAura()
        {
            Recipe recipe = Recipe.Create(ItemType<MercuriumAuraScythe>());
            recipe.AddIngredient(RoAItem("MercuriumNugget"), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateEverNecklace()
        {
            Recipe recipe = Recipe.Create(ItemType<EverwoodNecklace>());
            recipe.AddIngredient(RoAItem("Elderwood"), 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
        static void CreateTriwhip()
        {
            Recipe recipe = Recipe.Create(ItemType<Triwhip>());
            recipe.AddIngredient(ItemType<Ripple>(), 1);
            recipe.AddIngredient(RoAItem("MercuriumZipper"), 1);
            recipe.AddIngredient(ItemType<RopeWhip>(), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
        static void CreateSharanga()
        {
            Recipe recipe = Recipe.Create(ItemType<Sharanga>());
            recipe.AddIngredient(ItemType<HuntressBow>(), 1);
            recipe.AddIngredient(ItemType<WildRootBow>(), 1);
            recipe.AddIngredient(ItemID.Bone, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateUnderwaterAuraStaff()
        {
            Recipe recipe = Recipe.Create(ItemType<UnderwaterAuraStaff>());
            recipe.AddIngredient(ItemID.Seashell, 10);
            recipe.AddIngredient(ItemType<WaterEssence>(), 3);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateAngryParasite()
        {
            Recipe recipe = Recipe.Create(ItemType<AngryParasite>());
            recipe.AddIngredient(ItemType<BacciliteBar>(), 2);
            recipe.AddIngredient(ItemType<BambooWhip>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateHolyHandGrenade()
        {
            Recipe recipe = Recipe.Create(ItemType<HolyHandgrenade>());
            recipe.AddIngredient(ItemID.Dynamite, 5);
            recipe.AddIngredient(ItemType<BismuthBar>(), 2);
            recipe.AddIngredient(ItemID.BottledWater, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateShardflingPotion()
        {
            Recipe recipe = Recipe.Create(ItemType<ShardflingPotion>());
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddIngredient(ItemType<WhiteThread>(), 2);
            recipe.AddIngredient(RoAItem("MiracleMint"), 1);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }
    }
}