using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Material.TomeMats;
using Bismuth.Content.Items.Accessories;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Synergia.Helpers.ItemHelper;
using static Synergia.Lists.Items;

namespace Synergia.Common.GlobalItems.Set {
    public class EditTooltips : GlobalItem {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            BaseAccTooltips(item, tooltips, ModContent.ItemType<BacchusBoots>(), 5, "BacchusBoots");
            BaseAccTooltips(item, tooltips, ModContent.ItemType<BerserksRing>(), 2, "BerserksRing");
            if (item.type == ModContent.ItemType<CarbonSteel>()) {
                tooltips.RemoveAll(line => line.Mod == "Terraria" && line.Name == "Tooltip0");
            }
            foreach (int type in SixToolTipsLin) {
                DeleteLine(item, tooltips, type, 6);
            }
            foreach (int type in SevenToolTipsLin) {
                DeleteLine(item, tooltips, type, 7);
            }
        }
    }
}