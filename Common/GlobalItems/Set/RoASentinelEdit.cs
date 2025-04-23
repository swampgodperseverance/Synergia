using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.Localization;

namespace Vanilla.Common.GlobalItems.Set
{
    public class RoASentinelEdit : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.ModItem != null && item.ModItem.Mod.Name == "RoA")
            {
                switch (item.Name)
                {
                    case "Sentinel Helmet":
                        ReplaceTooltip(tooltips, "Tooltip0", "15% Increased throwing damage and crit chance by 5%");
                        break;

                    case "Sentinel Breastplate":
                        ReplaceTooltip(tooltips, "Tooltip0", "Throwing speed increased by 10% ");
                        break;

                    case "Sentinel Leggins":
                        ReplaceTooltip(tooltips, "Tooltip0", "Running speed increased by 20%");
                        break;
                }
            }
        }

        private void ReplaceTooltip(List<TooltipLine> tooltips, string tooltipName, string newText)
        {
            for (int i = 0; i < tooltips.Count; i++)
            {
                if (tooltips[i].Name == tooltipName && tooltips[i].Mod == "Terraria")
                {
                    tooltips[i].Text = newText;
                    break;
                }
            }
        }
    }
}