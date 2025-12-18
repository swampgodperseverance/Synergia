using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Material.TomeMats;
using Bismuth.Content.Items.Accessories;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Synergia.Helpers.ItemHelper;
using static Synergia.Lists.Items;
using Microsoft.Xna.Framework;
namespace Synergia.Common.GlobalItems.Set {
    public class EditTooltips : GlobalItem {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            BaseAccTooltips(item, tooltips, ModContent.ItemType<BacchusBoots>(), 5, "BacchusBoots");
            BaseAccTooltips(item, tooltips, ModContent.ItemType<BerserksRing>(), 2, "BerserksRing");

            BaseAddTooltips(item, tooltips, WeaponActiveBlood, "BWeapon", "BloodTooltips", color: Color.Red);    

            DeleteLine(item, tooltips, SixToolTipsLin, 6);
            DeleteLine(item, tooltips, SevenToolTipsLin, 7);

            // когда ни будь я это удалю
            if (item.type == ModContent.ItemType<CarbonSteel>()) {
                tooltips.RemoveAll(line => line.Mod == "Terraria" && line.Name == "Tooltip0");
            }
        }
    }
}