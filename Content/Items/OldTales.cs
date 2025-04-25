using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Vanilla.Common.ModSystems;

namespace Vanilla.Content.Items
{
    public class OldTales : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Old Tales");
            // Tooltip.SetDefault("Right-click to reveal the forgotten image...");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.rare = ItemRarityID.Blue;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2 && Main.myPlayer == player.whoAmI)
            {
                if (!ModContent.GetInstance<OldTalesSystem>().UIVisible)
                {
                    ModContent.GetInstance<OldTalesSystem>().ShowUI();
                }
                return false;
            }

            return base.CanUseItem(player);
        }
    }
}