using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Material.TomeMats;
using Bismuth.Content.Items.Accessories;
using NewHorizons.Content.Items.Accessories;
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
            BaseAccTooltips(item, tooltips, ItemType<BacchusBoots>(), 5, "BacchusBoots");
            BaseAccTooltips(item, tooltips, ItemType<BerserksRing>(), 2, "BerserksRing");
            BaseAccTooltips(item, tooltips, ItemType<ScrollOfGenin>(), 2, AddAttackSpeed(DamageClassName("throwing"), 8), true);
            BaseAddTooltips(item, tooltips, WeaponActiveBlood, "BWeapon", "BloodTooltips", color: Color.Red);

            if (item.ModItem != null && item.ModItem.Mod == Bis) {
                string itemNameValue = Lang.GetItemNameValue(item.type);
                if (!string.IsNullOrEmpty(itemNameValue) && itemNameValue.Contains("Bronze", StringComparison.OrdinalIgnoreCase)) {
                    tooltips[0].Text = ItemTooltip(WEP, "Cleaned") + " " + item.Name;
                }
            }

            DeleteLine(item, tooltips, SixToolTipsLin, 6);
            DeleteLine(item, tooltips, SevenToolTipsLin, 7);
            for (byte i = 4; i > 1; i--) {
                DeleteLine(item, tooltips, ItemType<ScrollOfChunin>(), i);
                DeleteLine(item, tooltips, ItemType<ScrollOfJonin>(), i);
            }

            BaseAccTooltips(item, tooltips, ItemType<ScrollOfChunin>(), 2, AddAttackSpeed(DamageClassName("throwing"), 8)  + "\n" + string.Format(AddBaseTooltips("SynergiaThrowing"), 60, 30), true);
            BaseAccTooltips(item, tooltips, ItemType<ScrollOfJonin>(),  2, AddAttackSpeed(DamageClassName("throwing"), 12) + "\n" + string.Format(AddBaseTooltips("SynergiaThrowing"), 30, 60), true);

            // когда ни будь я это удалю
            if (item.type == ItemType<CarbonSteel>()) {
                tooltips.RemoveAll(line => line.Mod == "Terraria" && line.Name == "Tooltip0");
            }
        }
    }
}