using Avalon.Items.Material.Shards;
using Avalon.Items.Placeable.Crafting;
using Avalon.Tiles;
using Bismuth.Content.Items.Materials;
using NewHorizons.Content.Items.Materials;
using PrimeRework;
using Synergia.Content.Items.Weapons.Throwing;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Material;

namespace Synergia.Content.Items.QuestItem {
    public class RustyBook : ModItem {
        public override void SetDefaults() {
            Item.questItem = true;
            Item.rare = ItemRarityID.Quest;
            Item.width = 40;
            Item.height = 25;
        }
        public override void AddRecipes()
        {
            var recipe = CreateRecipe()
                .AddIngredient(ModContent.ItemType<TatteredBook>(), 1)
                                .AddIngredient(ModContent.ItemType<PeatPowder>(), 10)

                                                .AddIngredient(ModContent.ItemType<AncientScrap>(), 8)

 //whar the FUCK

                .AddTile(ModContent.TileType<Avalon.Tiles.TomeForge>());

            recipe.Register();
        }
    }
}