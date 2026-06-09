using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Common.GlobalPlayer;
using Consolaria.Content.Items.Materials;

namespace Synergia.Content.Items.Accessories
{
    public class MirrorOfTheLost : ModItem
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MagicMirror, 1)
                .AddIngredient(ModContent.ItemType<SoulofBlight>(), 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MirrorOfTheLostPlayer>().mirrorEquipped = true;
        }
    }
}
