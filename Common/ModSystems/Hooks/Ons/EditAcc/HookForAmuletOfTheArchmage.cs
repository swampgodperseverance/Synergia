using Bismuth.Content.Buffs;
using Bismuth.Content.Items.Accessories;
using System;
using System.Collections.Generic;
using Terraria;

namespace Synergia.Common.ModSystems.Hooks.Ons.EditAcc {
    public class HookForAmuletOfTheArchmage {
        readonly static Type type = typeof(ArchmagesAmulet);

        readonly static int itemT = ItemType<ArchmagesAmulet>();
        static int level;

        public class Bonus : HookForEditAcc {
            public override Type HookType => type;
            public override int HookItem => itemT;

            public override void EditAcc(Orig_UpdateAccessory orig, ModItem item, Player player, bool hideVisual) {
                level = GetInstance<BismuthArtifactsLevels>().GetLevel();
                if (level == 1) {
                    player.statManaMax2 += 40;
                    player.manaCost -= 0.05f;
                    GetDamage(player, 0.08f);
                }
                if (level == 2) {
                    player.statManaMax2 += 60;
                    player.manaCost -= 0.07f;
                    GetDamage(player, 0.1f);
                }
                if (level == 3) {
                    player.statManaMax2 += 60;
                    player.manaCost -= 0.07f;
                    GetDamage(player, 0.1f);
                    SonJinWy(player, 3);
                }
                if (level == 4) {
                    player.statManaMax2 += 60;
                    player.manaCost -= 0.1f;
                    GetDamage(player, 0.12f, 0.1f);
                    player.manaRegen += 1; // я хз как из int сделать float. manaRegen это int
                    SonJinWy(player);
                }
                if (level == 5) {
                    player.statManaMax2 += 60;
                    player.manaCost -= 0.1f;
                    GetDamage(player, 0.12f, 0.1f);
                    player.manaRegen += 1; // я хз как из int сделать float. manaRegen это int
                    SonJinWy(player, 1);
                }
            }
            static void GetDamage(Player player, float bonus, float bonus2 = 0) {
                player.GetDamage(DamageClass.Magic) += bonus;
                player.GetCritChance(DamageClass.Magic) += bonus2 == 0 ? bonus : bonus2;
            }
            static void SonJinWy(Player player, float scale = 2) {
                if (scale == 1) { player.AddBuff(BuffType<MagiciansAura>(), 2); }
                else if (player.statLife < player.statLifeMax2 / scale) { player.AddBuff(BuffType<MagiciansAura>(), 2); }
            }
        }
        public class Tooltips : HookForTooltips {
            public override Type ItemType => type;
            public override int Target => itemT;
            public override void EditTooltips(origTooltips orig, ModItem item, List<TooltipLine> tooltips) {
                level = GetInstance<BismuthArtifactsLevels>().GetLevel();
                tooltips.Add(new TooltipLine(Mod, "ItemName", ItemTooltip(ACC + ".BalanceBismuth", "DivineEquipment")) { OverrideColor = new Color(0, 239, 239) });
                TooltipLine tooltip = new(Mod, "Info", ItemTooltip(ACC + ".BalanceBismuth." + "ArchmagesAmulet", "Level" + level.ToString()));
                tooltips.Add(tooltip);
            }
        }
    }
}