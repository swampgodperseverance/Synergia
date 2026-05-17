
using Avalon.Items.Material.Shards;
using Bismuth.Content.Items.Armor;
using Bismuth.Content.Items.Placeable;
using Consolaria.Content.Items.Materials;
using NewHorizons.Content.Items.Ammo;
using NewHorizons.Content.Items.Armor.NanotechArmor;
using Terraria;
using Terraria.ID;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.HorizonsChanges
{
    public class BismuthArmor : BaseRecipe
    {
        public override void DisableRecipe(Recipe recipe)
        {
            DisableRecipe(recipe, ItemType<PikemansHelmet>());
            DisableRecipe(recipe, ItemType<PikemansLeggings>());
            DisableRecipe(recipe, ItemType<PikemansBreastplate>());
            DisableRecipe(recipe, ItemType<AdamantiteHat>());
            DisableRecipe(recipe, ItemType<AdamantiteHeadpiece>());
            DisableRecipe(recipe, ItemType<MythrilHeadgear>());
            DisableRecipe(recipe, ItemType<MythrilHeadpiece>());
            DisableRecipe(recipe, ItemType<OrichalcumHat>());
            DisableRecipe(recipe, ItemType<OrichalcumHeadpiece>());
            DisableRecipe(recipe, ItemType<TitaniumHat>());
            DisableRecipe(recipe, ItemType<TitaniumHood>());
            DisableRecipe(recipe, ItemType<PalladiumHat>());
            DisableRecipe(recipe, ItemType<PalladiumHood>());
            DisableRecipe(recipe, ItemType<CobaltHeadgear>());
            DisableRecipe(recipe, ItemType<CobaltHeadpiece>());
            DisableRecipe(recipe, ItemType<ChlorophyteHeadpiece>());
            DisableRecipe(recipe, ItemType<HallowedHeadpiece>());
            DisableRecipe(recipe, ItemType<HallowedFaceShield>());
        }
        public override void Ingredient(Recipe recipe)
        {

            AddIngredient(recipe, ItemType<FeralBreastplate>(), 0, new Item(ItemType<WhiteThread>(), 14));
            AddIngredient(recipe, ItemType<FeralLeggings>(), 0, new Item(ItemType<WhiteThread>(), 12));
            AddIngredient(recipe, ItemType<FeralHelmet>(), 0, new Item(ItemType<WhiteThread>(), 12));
            AddIngredient(recipe, ItemType<WatersBreastplate>(), 1, new Item(ItemType<TorrentShard>(), 8));
            AddIngredient(recipe, ItemType<WatersHelmet>(), 1, new Item(ItemType<TorrentShard>(), 6));
            AddIngredient(recipe, ItemType<WatersLeggings>(), 1, new Item(ItemType<TorrentShard>(), 6));
            AddLotIngredient(recipe, ItemType<WatersBreastplate>(), (ItemID.SoulofMight, 5));
            AddLotIngredient(recipe, ItemType<WatersBreastplate>(), (ItemID.SoulofSight, 5));
            AddLotIngredient(recipe, ItemType<WatersBreastplate>(), (ItemID.SoulofFright, 5));
            AddLotIngredient(recipe, ItemType<WatersHelmet>(), (ItemID.SoulofMight, 5));
            AddLotIngredient(recipe, ItemType<WatersHelmet>(), (ItemID.SoulofSight, 5));
            AddLotIngredient(recipe, ItemType<WatersHelmet>(), (ItemID.SoulofFright, 5));
            AddLotIngredient(recipe, ItemType<WatersLeggings>(), (ItemID.SoulofMight, 5));
            AddLotIngredient(recipe, ItemType<WatersLeggings>(), (ItemID.SoulofSight, 5));
            AddLotIngredient(recipe, ItemType<WatersLeggings>(), (ItemID.SoulofFright, 5));
        }
        public override void PostRecipe()
        {
            CreatePikemansHelmet();
            CreatePikemansBreastplate();
            CreatePikemansLeggings();
            CreateRivetedHood();
            CreateRivetedBreast();
            CreateRivetedLeggings();
        }
        static void CreatePikemansHelmet()
        {
            Recipe recipe = Recipe.Create(ItemType<PikemansHelmet>());
            recipe.AddIngredient(ItemID.Silk, 6);
            recipe.AddIngredient(RoAItem("MercuriumNugget"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreatePikemansBreastplate()
        {
            Recipe recipe = Recipe.Create(ItemType<PikemansBreastplate>());
            recipe.AddIngredient(ItemID.Silk, 10);
            recipe.AddIngredient(RoAItem("MercuriumNugget"), 18);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreatePikemansLeggings()
        {
            Recipe recipe = Recipe.Create(ItemType<PikemansLeggings>());
            recipe.AddIngredient(ItemID.Silk, 8);
            recipe.AddIngredient(RoAItem("MercuriumNugget"), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateRivetedHood()
        {
            Recipe recipe = Recipe.Create(ItemType<RivetedHood>());
            recipe.AddIngredient(RoAItem("AnimalLeather"), 8);
            recipe.AddIngredient(ItemType<AluminiumBar>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateRivetedLeggings()
        {
            Recipe recipe = Recipe.Create(ItemType<RivetedBoots>());
            recipe.AddIngredient(RoAItem("AnimalLeather"), 8);
            recipe.AddIngredient(ItemType<AluminiumBar>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        static void CreateRivetedBreast()
        {
            Recipe recipe = Recipe.Create(ItemType<RivetedJacket>());
            recipe.AddIngredient(RoAItem("AnimalLeather"), 12);
            recipe.AddIngredient(ItemType<AluminiumBar>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

}