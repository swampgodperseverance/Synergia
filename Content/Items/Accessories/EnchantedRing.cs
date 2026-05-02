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

            auraPlayer.bonusAuraRadius += 0.08f; 

            auraPlayer.auraBuffTime += 180; 
        }
    }
}
