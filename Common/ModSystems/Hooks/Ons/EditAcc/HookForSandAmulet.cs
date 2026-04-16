using System;
using System.Collections.Generic;
using Bismuth.Content.Buffs;
using Bismuth.Content.Items.Accessories;
using Synergia.Common.GlobalPlayer;
using Terraria;
namespace Synergia.Common.ModSystems.Hooks.Ons.EditAcc
{
    public class HookForHeartOfDesert
    {
        readonly static Type type = typeof(HeartOfDesert);
        readonly static int itemT = ItemType<HeartOfDesert>();
        static int level;
        public class BonusHeartOfDesert : HookForEditAcc
        {
            public override Type HookType => type;
            public override int HookItem => itemT;
            public override void EditAcc(Orig_UpdateAccessory orig, ModItem item, Player player, bool hideVisual)
            {
                level = GetInstance<BismuthArtifactsLevels>().GetLevel();
                bool inDesert = player.ZoneDesert;
                if (level == 1)
                {
                    if (inDesert)
                    {
                        player.moveSpeed += 0.10f;
                        player.lifeRegen += 1;
                        GetDamage(player, 0.08f);
                        player.statDefense += 5;
                    }
                }
                if (level == 2)
                {
                    if (inDesert)
                    {
                        player.moveSpeed += 0.10f;
                        player.lifeRegen += 2;
                        GetDamage(player, 0.10f);
                        player.statDefense += 8;
                    }
                    player.GetModPlayer<SynergiaPlayer>().desertSandstormImmunity = true;
                }
                if (level == 3)
                {
                    if (inDesert)
                    {
                        player.moveSpeed += 0.15f;
                        player.lifeRegen += 2;
                        GetDamage(player, 0.12f, 0.06f);
                        player.statDefense += 12;
                    }
                    player.GetModPlayer<SynergiaPlayer>().desertSandstormImmunity = true;
                }
                if (level == 4)
                {
                    if (inDesert)
                    {
                        player.moveSpeed += 0.15f;
                        player.lifeRegen += 3;
                        GetDamage(player, 0.15f, 0.08f);
                        player.statDefense += 15;
                        player.endurance += 0.05f;
                    }
                    player.GetModPlayer<SynergiaPlayer>().desertSandstormImmunity = true;
                }
                if (level == 5)
                {
                    if (inDesert)
                    {
                        player.moveSpeed += 0.20f;
                        player.lifeRegen += 4;
                        GetDamage(player, 0.18f, 0.10f);
                        player.statDefense += 20;
                        player.endurance += 0.10f;
                    }
                    player.GetModPlayer<SynergiaPlayer>().desertSandstormImmunity = true;
                }
            }
            static void GetDamage(Player player, float bonus, float crit = 0f)
            {
                player.GetDamage(DamageClass.Generic) += bonus;
                player.GetCritChance(DamageClass.Generic) += crit;
            }
        }
        public class TooltipsHeartOfDesert : HookForTooltips
        {
            public override Type ItemType => type;
            public override int Target => itemT;
            public override void EditTooltips(origTooltips orig, ModItem item, List<TooltipLine> tooltips)
            {
                level = GetInstance<BismuthArtifactsLevels>().GetLevel();
                tooltips.Add(new TooltipLine(Mod, "ItemName", ItemTooltip(ACC + ".BalanceBismuth", "DivineEquipment")) { OverrideColor = new Color(0, 239, 239) });
                TooltipLine tooltip = new(Mod, "Info", ItemTooltip(ACC + ".BalanceBismuth." + "HeartOfDesert", "Level" + level.ToString()));
                tooltips.Add(tooltip);
            }
        }
    }
}