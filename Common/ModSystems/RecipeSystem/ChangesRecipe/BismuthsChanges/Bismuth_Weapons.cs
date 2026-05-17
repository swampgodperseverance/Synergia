using Avalon.Items.Material.Bars;
using Avalon.Items.Material.Ores;
using Avalon.Items.Material.Shards;
using Avalon.Items.Potions.Buff;
using Bismuth.Content.Items.Armor;
using Bismuth.Content.Items.Materials;
using Bismuth.Content.Items.Other;
using Bismuth.Content.Items.Placeable;
using Bismuth.Content.Items.Weapons.Magical;
using Bismuth.Content.Items.Weapons.Melee;
using Bismuth.Content.Tiles;
using Consolaria.Content.Items.Materials;
using NewHorizons.Content.Items.Ammo;
using NewHorizons.Content.Items.Armor.AshenArmor;
using NewHorizons.Content.Items.Armor.PyroArmor;
using NewHorizons.Content.Items.Armor.SkyArmor;
using NewHorizons.Content.Items.Weapons;
using NewHorizons.Content.Items.Weapons.Magic;
using NewHorizons.Content.Items.Weapons.Ranged;
using NewHorizons.Content.Items.Weapons.Throwing;
using Synergia.Content.Items.Misc;
using Synergia.Content.Items.Weapons.AuraStaff;
using Synergia.Content.Items.Weapons.Melee;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Material;
using ValhallaMod.Items.Material.Bar;
using ValhallaMod.Items.Weapons.Ranged.Bows;
using ValhallaMod.Items.Weapons.Ranged.Guns;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.HorizonsChanges
{
    public class BismuthWeapons : BaseRecipe
    {
        public override void DisableRecipe(Recipe recipe)
        {
            DisableRecipe(recipe, ItemType<GalvornStaff>());
            DisableRecipe(recipe, ItemType<Bismuth.Content.Items.Weapons.Melee.OrcishSword>());
            DisableRecipe(recipe, ItemType<Bismuth.Content.Items.Weapons.Melee.BismuthumSword>());
            DisableRecipe(recipe, ItemType<Bismuth.Content.Items.Weapons.Melee.GalvornBlade>());
        }
        public override void Ingredient(Recipe recipe)
        {

            AddIngredient(recipe, ItemType<KeyOfTheSun>(), 1, new Item(ItemType<ThunderBar>(), 1));
            AddLotIngredient(recipe, ItemType<TruePhilosopherStone>(), (ModContent.ItemType<ZincOre>(), 30));
            AddLotIngredient(recipe, ItemType<TruePhilosopherStone>(), (ModContent.ItemType<BismuthOre>(), 30));
            AddLotIngredient(recipe, ItemType<TruePhilosopherStone>(), (ModContent.ItemType<BronzeOre>(), 30));
            AddLotIngredient(recipe, ItemType<TruePhilosopherStone>(), (ModContent.ItemType<NickelOre>(), 30));
            AddLotIngredient(recipe, ItemType<ChlorophyteJavelin>(), (ModContent.ItemType<VenomShard>(), 1));
        }
        public override void PostRecipe()
        {
            CreateMoonStaff();
            CreateRuneSword();
            CreateGalvornStaff();
            CreateOrcishSword();
            CreateBismuthumSword();
            CreateGalvornBlade();

        }
        static void CreateRuneSword()
        {
            Recipe recipe = Recipe.Create(ItemType<MoonlightStaff>());
            recipe.AddIngredient(ItemType<RuneEssence>(), 12);
            recipe.AddIngredient(ItemType<DurataniumBar>(), 8);
            recipe.AddTile(TileType<Bismuth.Content.Tiles.RuneTable>());
            recipe.Register();
        }
        static void CreateMoonStaff()
        {
            Recipe recipe = Recipe.Create(ItemType<RunicBlade>());
            recipe.AddIngredient(ItemType<RuneEssence>(), 10);
            recipe.AddIngredient(ItemType<DurataniumBar>(), 8);
            recipe.AddTile(TileType<Bismuth.Content.Tiles.RuneTable>());
            recipe.Register();
        }
        static void CreateGalvornStaff()
        {
            Recipe recipe = Recipe.Create(ItemType<GalvornAuraStaff>());
            recipe.AddIngredient(ItemType<GalvornBar>(), 10);
            recipe.AddIngredient(ItemType<EarthEssence>(), 3);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateGalvornBlade()
        {
            Recipe recipe = Recipe.Create(ItemType<Content.Items.Weapons.Melee.GalvornBlade>());
            recipe.AddIngredient(ItemType<GalvornBar>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateBismuthumSword()
        {
            Recipe recipe = Recipe.Create(ItemType<Content.Items.Weapons.Melee.BismuthumSword>());
            recipe.AddIngredient(ItemType<Bismuth.Content.Items.Placeable.BismuthumBar>(), 10);
            recipe.AddTile(TileType<CaesiumHeavyAnvilTile>());
            recipe.Register();
        }
        static void CreateOrcishSword()
        {
            Recipe recipe = Recipe.Create(ItemType<Content.Items.Weapons.Melee.OrcishSword>());
            recipe.AddIngredient(ItemType<Bismuth.Content.Items.Materials.OrcishBar>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

}