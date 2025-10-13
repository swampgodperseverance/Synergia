using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Items.Misc;

namespace Synergia.Content.Items.Misc
{
    public class CaesiumHeavyAnvil : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Caesium Heavy Anvil");
            // Tooltip.SetDefault("An upgraded anvil for Caesium-tier crafting");
        }

        public override void SetDefaults()
        {
            Item.width = 52; 
            Item.height = 22;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.createTile = ModContent.TileType<CaesiumHeavyAnvilTile>();
        }
    }
}
