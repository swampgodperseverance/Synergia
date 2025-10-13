using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod;

namespace Synergia.Content.Items.Accessories
{
    public class EnchantedRing : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Enchanted Ring");
            // Tooltip.SetDefault("Increases ValhallaMod aura radius by 10%\nIncreases aura buff duration by 3 seconds");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var auraPlayer = player.GetModPlayer<AuraPlayer>();

            // Increase aura radius by 10%
            auraPlayer.bonusAuraRadius += 0.10f; // Green: +10% aura radius

            auraPlayer.auraBuffTime += 180; // Green: +3 seconds to aura buff time
        }
    }
}
