using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Accessories.PreHardmode;
using Avalon.Items.Material.Bars;
using Avalon.Items.Material.Shards;
using Avalon.Items.Material.TomeMats;
using Avalon.Items.Tomes.PreHardmode;
using Consolaria.Content.Items.Materials;
using NewHorizons.Content.Items.Materials;
using Starforgedclassic.Content.Accessories.SkyShield;
using Starforgedclassic.Content.Weapons.MeteoricEdge;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Accessory;
using ValhallaMod.Items.Accessory.Shield;
using ValhallaMod.Items.Garden;
using ValhallaMod.Items.Material;
using ValhallaMod.Items.Material.Bar;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.AvalonsChanges {
        public class RoasWeapons : BaseRecipe {

            public override void Ingredient(Recipe recipe) {
            AddIngredient(recipe, RoAItem("StarFusion"), 0, new Item(ItemType<MeteoricEdge>(), 1));
            AddIngredient(recipe, RoAItem("PastoralRod"), 1, new Item(ItemType<ElasticCord>(), 1));
            AddLotIngredient(recipe, RoAItem("TectonicCane"), (ModContent.ItemType<FireShard>(), 3));
            AddLotIngredient(recipe, RoAItem("ThornyClaws"), (ModContent.ItemType<ToxinShard>(), 3));
            AddLotIngredient(recipe, RoAItem("HellfireClaws"), (ModContent.ItemType<FireShard>(), 3));
        }
            public override void PostRecipe()
            {
                CreateRodPyre();
                CreateRodStream();
                CreateRodCondor();
                CreateRodQuake();
                CreateRodShock();
            }
            static void CreateRodPyre()
            {
                Recipe recipe = Recipe.Create(RoAItem("RodOfTheDragonfire"));
                recipe.AddIngredient(RoAItem("MercuriumNugget"), 15);
                recipe.AddIngredient(ItemType<BismuthBar>(), 10);
                recipe.AddIngredient(RoAItem("SphereOfPyre"), 1);
                recipe.AddTile(TileID.Anvils);
                recipe.Register();
            }
            static void CreateRodStream()
            {
                Recipe recipe = Recipe.Create(RoAItem("RodOfTheStream"));
                recipe.AddIngredient(RoAItem("MercuriumNugget"), 15);
                recipe.AddIngredient(ItemType<BismuthBar>(), 10);
                recipe.AddIngredient(RoAItem("SphereOfStream"), 1);
                recipe.AddTile(TileID.Anvils);
                recipe.Register();
            }
            static void CreateRodCondor()
            {
                Recipe recipe = Recipe.Create(RoAItem("RodOfTheCondor"));
                recipe.AddIngredient(RoAItem("MercuriumNugget"), 15);
                recipe.AddIngredient(ItemType<BismuthBar>(), 10);
                recipe.AddIngredient(RoAItem("SphereOfCondor"), 1);
                recipe.AddTile(TileID.Anvils);
                recipe.Register();
            }
            static void CreateRodQuake()
            {
                Recipe recipe = Recipe.Create(RoAItem("RodOfTheTerra"));
                recipe.AddIngredient(RoAItem("MercuriumNugget"), 15);
                recipe.AddIngredient(ItemType<BismuthBar>(), 10);
                recipe.AddIngredient(RoAItem("SphereOfQuake"), 1);
                recipe.AddTile(TileID.Anvils);
                recipe.Register();
            }
            static void CreateRodShock()
            {
                Recipe recipe = Recipe.Create(RoAItem("RodOfTheShock"));
                recipe.AddIngredient(RoAItem("MercuriumNugget"), 15);
                recipe.AddIngredient(ItemType<BismuthBar>(), 10);
                recipe.AddIngredient(RoAItem("SphereOfShock"), 1);
                recipe.AddTile(TileID.Anvils);
                recipe.Register();
            }

    }
    
}