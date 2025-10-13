using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Common.GlobalPlayer;

namespace Synergia.Content.Items.Accessories
{
    public class MirrorOfTheLost : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mirror of the Lost");
            // Tooltip.SetDefault("+25% damage inside Valhalla aura\n-25% damage outside");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MirrorOfTheLostPlayer>().mirrorEquipped = true;
        }
    }
}
