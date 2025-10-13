using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Common.GlobalPlayer;

namespace Synergia.Content.Items.Accessories
{
    public class BloodyNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bloody Necklace");
            // Tooltip.SetDefault("Blood for blood. Entering a Valhalla aura grants a deadly counter, but makes you bleed.");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BloodyNecklacePlayer>().bloodyNecklaceEquipped = true;
        }
    }
}