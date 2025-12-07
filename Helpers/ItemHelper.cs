using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using static Synergia.Common.SUtils.LocUtil;

namespace Synergia.Helpers {
    public static class ItemHelper {
        public static void BaseAccTooltips(Item item, List<TooltipLine> tooltips, int type, byte line, string key) {
            if (item.type == type) {
                tooltips[line].Text = ItemTooltip(ACC, key);
            }
        }
        public static void AddLoot(Item item, ItemLoot itemLoot, int type, int drop, byte chance, byte min = 1, byte max = 1) {
            if (item.type == type) {
                itemLoot.Add(ItemDropRule.Common(drop, chance, min, max));
            }
        }
    }
}
