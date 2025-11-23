using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using System.Collections.Generic;
using Avalon.Items.Material.TomeMats;

namespace Synergia.Common.GlobalItems
{
    public class RemoveCarbonSteelTooltip : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!ModLoader.TryGetMod("Avalon", out Mod avalonMod))
                return;
            if (avalonMod.TryFind("CarbonSteel", out ModItem carbonSteel) &&
                item.type == carbonSteel.Type)
            {
                tooltips.RemoveAll(line => line.Mod == "Terraria" && line.Name == "Tooltip0");
            }
        }
    }
}
