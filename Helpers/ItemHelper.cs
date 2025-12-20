using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using static Synergia.Common.SUtils.LocUtil;

namespace Synergia.Helpers {
    public static class ItemHelper {
        public static void BaseAccTooltips(Item item, List<TooltipLine> tooltips, int type, byte line, string key) {
            if (item.type == type) {
                tooltips[line].Text = ItemTooltip(ACC, key);
            }
        }
        public static void DeleteLine(Item item, List<TooltipLine> tooltips, int type, byte line) {
            if (item.type == type) {
                tooltips[line].Text = string.Empty;
            }
        }
        public static void DeleteLine(Item item, List<TooltipLine> tooltips, List<int> types, byte line) {
            foreach (int type in types) {
                if (item.type == type) {
                    tooltips[line].Text = string.Empty;
                }
            }
        }
        public static void BaseAddTooltips(Item item, List<TooltipLine> tooltips, List<int> types, string name, string key, Color? color = null) {
            foreach (int type in types) {
                if (item.type == type) {
                    TooltipLine tooltipLine = new(Synergia.Instance, name, ItemTooltip(WEP, key));
                    if (color != null) {
                        tooltipLine.OverrideColor = color;
                    }
                    tooltips.Add(tooltipLine);
                }
            }
        }
        public static void BaseAddTooltips(Item item, List<TooltipLine> tooltips, int type, string name, string key, Color? color = null) {
            if (item.type == type) {
                TooltipLine tooltipLine = new(Synergia.Instance, name, ItemTooltip(WEP, key));
                if (color != null) {
                    tooltipLine.OverrideColor = color;
                }
                tooltips.Add(tooltipLine);
            }
        }
        public static void AddLoot(Item item, ItemLoot itemLoot, int type, int drop, byte chance, byte min = 1, byte max = 1) {
            if (item.type == type) {
                itemLoot.Add(ItemDropRule.Common(drop, chance, min, max));
            }
        }
        public static void EditTooltips(Item item, List<TooltipLine> tooltips, int type, string oldToolTip, string newTooltips) {
            if (item.type == type) {
                foreach (TooltipLine line in tooltips) {
                    line.Text = line.Text.Replace(oldToolTip, newTooltips);
                }
            }
        }
    }
}
