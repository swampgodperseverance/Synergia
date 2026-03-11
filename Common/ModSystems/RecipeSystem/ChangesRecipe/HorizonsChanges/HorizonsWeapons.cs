using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Accessories.PreHardmode;
using Avalon.Items.Material.Bars;
using Avalon.Items.Material.Ores;
using Avalon.Items.Material.Shards;
using NewHorizons.Content.Items.Weapons.Magic;
using NewHorizons.Content.Items.Weapons.Ranged;
using NewHorizons.Content.Items.Weapons.Summon;
using NewHorizons.Content.Items.Weapons.Throwing;
using Starforgedclassic.Content.Accessories.SkyShield;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Material;
using ValhallaMod.Items.Material.Bar;
using ValhallaMod.Items.Weapons.Ranged.ProjectileGuns;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.HorizonsChanges
{
        public class HorizonsWeapons : BaseRecipe {

            public override void Ingredient(Recipe recipe) {

            AddIngredient(recipe, ItemType<HandicraftedBlunderbuss>(), 1, new Item(ItemType<LegalGunParts>(), 1));
            AddIngredient(recipe, ItemType<HandicraftedFlamethrower>(), 1, new Item(ItemType<LegalGunParts>(), 1));
            AddLotIngredient(recipe, ItemType<RopeWhip>(), (ModContent.ItemType<ElasticCord>(), 1));
            AddIngredient(recipe, ItemType<RopeWhip>(), 1, new Item(ItemID.Wood, 12));
            AddIngredient(recipe, ItemType<Carnwennan>(), 1, new Item(ItemType<PureGoldChunk>(), 10));
            AddIngredient(recipe, ItemType<DarkVolley>(), 1, new Item(ItemType<WickedShard>(), 5));


        }
        public override void PostRecipe()
        {
            CreateSeaBeast();
            CreateSpaceCowboy();
        }
        static void CreateSeaBeast()
        {
            Recipe recipe = Recipe.Create(ItemType<TheSeaBeast>());
            recipe.AddIngredient(ItemType<LooseCannon>(), 1);
            recipe.AddIngredient(ItemType<TheGalleon>(), 1);
            recipe.AddIngredient(ItemID.Shotgun, 1);
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
            recipe.AddIngredient(ItemType<BronzeBar>(), 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

    }
    
}