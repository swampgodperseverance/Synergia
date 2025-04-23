using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.Localization;

namespace Vanilla.Common.GlobalItems.Set
{
    public class GreediestEdit : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.ModItem != null && item.ModItem.Mod.Name == "ValhallaMod")
            {
                switch (item.Name)
                {
                    case "Greediest Mask":
                        ReplaceTooltip(tooltips, "Tooltip0", "14% more damage resistance and 25% throwing damage");
                        break;

                    case "Greediest Tunic":
                        ReplaceTooltip(tooltips, "Tooltip0", "13% more damage resistance and +8 crit");
                        break;

                    case "Greediest Greaves":
                        ReplaceTooltip(tooltips, "Tooltip0", "+9% throwing speed ");
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