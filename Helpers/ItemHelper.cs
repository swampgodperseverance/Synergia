using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using static Synergia.Common.SUtils.LocUtil;

namespace Synergia.Helpers {
    public static class ItemHelper {
        public static void BaseAccTooltips(Item item, List<TooltipLine> tooltips, int type, byte line, string key, bool custom = false) {
            if (item.type == type) {
                tooltips[line].Text = custom ? key : ItemTooltip(ACC, key);
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
        public static void CustomColor(Item item, List<TooltipLine> tooltips, int type, byte line, Color color) {
            if (item.type == type) {
                tooltips[line].OverrideColor = color;
            }
        }
        public static void BaseAddTooltips(Item item, List<TooltipLine> tooltips, List<int> types, string name, string key, Color? color = null, bool custom = false) {
            foreach (int type in types) {
                if (item.type == type) {
                    TooltipLine tooltipLine = new(Synergia.Instance, name, custom ? key : ItemTooltip(WEP, key));
                    if (color != null) {
                        tooltipLine.OverrideColor = color;
                    }
                    tooltips.Add(tooltipLine);
                }
            }
        }
        public static void BaseAddTooltips(Item item, List<TooltipLine> tooltips, int type, string name, string key, Color? color = null, bool custom = false) {
            if (item.type == type) {
                TooltipLine tooltipLine = new(Synergia.Instance, name, custom ? key : ItemTooltip(WEP, key));
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
        public static int GetRoAItem(string name) => ModList.Roa.Find<ModItem>(name).Type;
        public static int GetSelectItem(Mod modName, string itemName) => modName.Find<ModItem>(itemName).Type;
    }
}
