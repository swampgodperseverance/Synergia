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
using NewHorizons.Content.Items.Armor.SkyArmor;
using NewHorizons.Content.Items.Materials;
using NewHorizons.Content.Items.Weapons.Magic;
using NewHorizons.Content.Items.Weapons.Ranged;
using starforgedclassic.Content.Placeables.AzuriteBar;
using Starforgedclassic.Content.Accessories.SkyShield;
using Synergia.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Accessory;
using ValhallaMod.Items.Accessory.Active;
using ValhallaMod.Items.Accessory.Shield;
using ValhallaMod.Items.Consumable;
using ValhallaMod.Items.Garden;
using ValhallaMod.Items.Material;
using ValhallaMod.Items.Material.Bar;
using ValhallaMod.Items.Weapons.Magic.Gloves;
using ValhallaMod.Items.Weapons.Melee.ChannelMelee;
using ValhallaMod.Items.Weapons.Melee.Knives;
using ValhallaMod.Items.Weapons.Melee.Swords;
using ValhallaMod.Items.Weapons.Ranged.ProjectileGuns;
using ValhallaMod.Items.Weapons.Summon.Auras;
using ValhallaMod.Items.Weapons.Summon.Whips;
using ValhallaMod.Projectiles.Summon.Sentries;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.AvalonsChanges {
        public class ValhallaAccessories : BaseRecipe {

            public override void Ingredient(Recipe recipe) {
            AddIngredient(recipe, ItemType<BlessedHeroShield>(), 2, new Item(ItemType<ForsakenCross>(), 1));
        }
        public override void PostRecipe()
        {
            CreateGlassShield();
        }
        static void CreateGlassShield()
        {
            Recipe recipe = Recipe.Create(ItemType<GlassShield>());
            recipe.AddIngredient(ItemType<HardenedGlass>(), 12);
            recipe.AddIngredient(ItemType<Booger>(), 8);
            recipe.AddTile(TileID.GlassKiln);
            recipe.Register();
        }

    }
    
}