using System.Collections.Generic;
using Avalon.Items.Armor.Hardmode;
using Avalon.Items.Armor.PreHardmode;
using Avalon.Items.Material.Bars;
using Avalon.Items.Material.Herbs;
using Avalon.Items.Material.Shards;
using Avalon.Items.Placeable.Tile;
using Consolaria.Content.Items.Weapons.Ranged;
using NewHorizons.Content.Items.Armor.BeastArmor;
using NewHorizons.Content.Items.Armor.NightMageArmor;
using NewHorizons.Content.Items.Armor.RottenArmor;
using NewHorizons.Content.Items.Armor.WyvernHunterArmor;
using NewHorizons.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Weapons.Ranged.Bows;
using ValhallaMod.Items.Weapons.Ranged.Bows.Wood;
using ValhallaMod.Items.Weapons.Summon.Auras;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe {
    public class AllMod : BaseRecipe {
        public override void DisableRecipe(Recipe recipe)
        {
            DisableRecipe(recipe, ItemType<Sharanga>());
        }
        public override void Ingredient(Recipe recipe) {
            AddLotIngredient(recipe, ItemID.VenomBullet, (ModContent.ItemType<VenomShard>(), 1));
            AddLotIngredient(recipe, ItemID.VenomArrow, (ModContent.ItemType<VenomShard>(), 1));
            AddLotIngredient(recipe, ItemID.VenomBullet, (ModContent.ItemType<VenomShard>(), 1));
            AddLotIngredient(recipe, ItemID.FlaskofVenom, (ModContent.ItemType<VenomShard>(), 1));
            AddLotIngredient(recipe, ItemType<VenomSpike>(), (ModContent.ItemType<VenomShard>(), 1));
        }
        public override void PostPostRecipe(Recipe recipe) {
            List<int> potion = [ItemID.ThornsPotion];
            List<int> flower = [ItemID.Deathweed, ItemType<Bloodberry>(), ItemType<Barfbush>()];
            AddRecipeGroup(recipe, new RecipeGroupStruct(potion, flower, RecipeGroups.FLOWER));
        }
        public override void PostRecipe()
        {
            CreateSharanga();

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
    }
}