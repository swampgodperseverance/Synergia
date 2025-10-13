using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod;

namespace Synergia.Content.Items.Accessories
{
    public class SuspiciousBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Suspicious Bag");
            // Tooltip.SetDefault("While linked to a Valhalla aura, gain 22% increased aura radius and immunity to slow\nSmells... suspicious");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.sellPrice(gold: 4);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var auraPlayer = player.GetModPlayer<AuraPlayer>();
            var bagPlayer = player.GetModPlayer<SuspiciousBagPlayer>();

            bagPlayer.bagEquipped = true;

            // 🌀 Increase auraSameMax by 1
            auraPlayer.auraSameMax = 1 + 1;
        }
    }
}
