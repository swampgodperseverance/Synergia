using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Accessories.PreHardmode;
using Avalon.Items.Material.Bars;
using Avalon.Items.Material.Ores;
using Avalon.Items.Material.Shards;
using Avalon.Items.Placeable.Tile;
using Bismuth.Content.Items.Placeable;
using NewHorizons.Content.Items;
using NewHorizons.Content.Items.Ammo;
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
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.HorizonsChanges
{
    public class HorizonsWeapons : BaseRecipe
    {

        public override void Ingredient(Recipe recipe)
        {

            AddIngredient(recipe, ItemType<HandicraftedBlunderbuss>(), 1, new Item(ItemType<LegalGunParts>(), 1));
            AddIngredient(recipe, ItemType<HandicraftedFlamethrower>(), 1, new Item(ItemType<LegalGunParts>(), 1));
            AddLotIngredient(recipe, ItemType<RopeWhip>(), (ModContent.ItemType<ElasticCord>(), 1));
            AddLotIngredient(recipe, ItemType<FlareCannon>(), (ModContent.ItemType<FireShard>(), 3));
            AddLotIngredient(recipe, ItemType<Scorcher>(), (ModContent.ItemType<FireShard>(), 3));
            AddLotIngredient(recipe, ItemType<MoltenDagger>(), (ModContent.ItemType<FireShard>(), 1));
            AddLotIngredient(recipe, ItemType<FlamingFlare>(), (ModContent.ItemType<FireShard>(), 1));
            AddIngredient(recipe, ItemType<RopeWhip>(), 1, new Item(ItemID.Wood, 12));
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

        }
        public override void PostRecipe()
        {
            CreateSeaBeast();
            CreateSpaceCowboy();
            CreateWandMana1();
            CreateWandMana2();
            CreateGungnir();
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
    }
    
}