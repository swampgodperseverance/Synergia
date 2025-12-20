using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Material.TomeMats;
using Bismuth.Content.Items.Accessories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using static Synergia.Common.SUtils.LocUtil;
using static Synergia.Helpers.ItemHelper;
using static Synergia.Lists.Items;
using static Synergia.ModList;

namespace Synergia.Common.GlobalItems.Set {
    public class EditTooltips : GlobalItem {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            BaseAccTooltips(item, tooltips, ModContent.ItemType<BacchusBoots>(), 5, "BacchusBoots");
            BaseAccTooltips(item, tooltips, ModContent.ItemType<BerserksRing>(), 2, "BerserksRing");

            BaseAddTooltips(item, tooltips, WeaponActiveBlood, "BWeapon", "BloodTooltips", color: Color.Red);

            if (item.ModItem != null && item.ModItem.Mod == Bis) {
                string itemNameValue = Lang.GetItemNameValue(item.type);
                if (!string.IsNullOrEmpty(itemNameValue) && itemNameValue.Contains("Bronze", StringComparison.OrdinalIgnoreCase)) {
                    tooltips[0].Text = ItemTooltip(WEP, "Cleaned") + " " + item.Name;
                }
            }

            DeleteLine(item, tooltips, SixToolTipsLin, 6);
            DeleteLine(item, tooltips, SevenToolTipsLin, 7);

            // когда ни будь я это удалю
            if (item.type == ModContent.ItemType<CarbonSteel>()) {
                tooltips.RemoveAll(line => line.Mod == "Terraria" && line.Name == "Tooltip0");
            }
        }
    }
}