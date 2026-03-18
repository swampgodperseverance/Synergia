using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Accessories.PreHardmode;
using Avalon.Items.Material;
using Avalon.Items.Material.Bars;
using Avalon.Items.Material.Ores;
using Avalon.Items.Material.Shards;
using Avalon.Items.Material.TomeMats;
using Avalon.Items.Potions.Buff;
using Avalon.Items.Tomes.PreHardmode;
using Bismuth.Content.Items.Materials;
using Consolaria.Content.Items.Materials;
using NewHorizons.Content.Items.Materials;
using NewHorizons.Content.Items.Weapons.Ranged;
using Starforgedclassic.Content.Accessories.SkyShield;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Accessory;
using ValhallaMod.Items.Accessory.Shield;
using ValhallaMod.Items.Garden;
using ValhallaMod.Items.Material;
using ValhallaMod.Items.Material.Bar;
using ValhallaMod.Items.Weapons.Melee.Swords;
using ValhallaMod.Items.Weapons.Ranged.ProjectileGuns;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.AvalonsChanges {
        public class ValhallaWeapons : BaseRecipe {

            public override void Ingredient(Recipe recipe) {
            AddLotIngredient(recipe, ItemType<BlueSlice>(), (ModContent.ItemType<FrigidShard>(), 3));



        }
        public override void PostRecipe()
        {
            CreateEverIce();
            CreateSkadisWrath();
        }
        static void CreateEverIce()
        {
            Recipe recipe = Recipe.Create(ItemType<EverlivingIce>());
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

    }
    
}