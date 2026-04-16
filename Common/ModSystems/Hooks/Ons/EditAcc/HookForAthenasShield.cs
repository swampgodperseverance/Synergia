using System;
using System.Collections.Generic;
using Bismuth.Content.Buffs;
using Bismuth.Content.Items.Accessories;
using Synergia.Common.GlobalPlayer;
using Terraria;
namespace Synergia.Common.ModSystems.Hooks.Ons.EditAcc
{
    public class HookForAthenasShield
    {
        readonly static Type type = typeof(AthenasShield);
        readonly static int itemT = ItemType<AthenasShield>();
        static int level;
        public class BonusAthena : HookForEditAcc
        {
            public override Type HookType => type;
            public override int HookItem => itemT;
            public override void EditAcc(Orig_UpdateAccessory orig, ModItem item, Player player, bool hideVisual)
            {
                level = GetInstance<BismuthArtifactsLevels>().GetLevel();
                if (level == 1)
                {
                    player.GetModPlayer<SynergiaPlayer>().potionHealMultiplier += 0.10f;
                    player.GetModPlayer<SynergiaPlayer>().potionCooldownReduction += 5;
                    LowHPDefense(player, 100, 10);
                }
                if (level == 2)
                {
                    player.GetModPlayer<SynergiaPlayer>().potionHealMultiplier += 0.15f;
                    player.GetModPlayer<SynergiaPlayer>().potionCooldownReduction += 8;
                    LowHPDefense(player, 150, 10);
                }
                if (level == 3)
                {
                    player.GetModPlayer<SynergiaPlayer>().potionHealMultiplier += 0.15f;
                    player.GetModPlayer<SynergiaPlayer>().potionCooldownReduction += 8;
                    LowHPDefense(player, 150, 10);
                    ProjectileReflect(player, 0.03f);
                }
                if (level == 4)
                {
                    player.GetModPlayer<SynergiaPlayer>().potionHealMultiplier += 0.15f;
                    player.GetModPlayer<SynergiaPlayer>().potionCooldownReduction += 8;
                    LowHPDefense(player, 150, 18);
                    ProjectileReflect(player, 0.08f);
                }
                if (level == 5)
                {
                    player.GetModPlayer<SynergiaPlayer>().potionHealMultiplier += 0.15f;
                    player.GetModPlayer<SynergiaPlayer>().potionCooldownReduction += 10;
                    LowHPDefense(player, 150, 25);
                    ProjectileReflect(player, 0.10f);
                    player.endurance += 0.15f;
                }
            }
            static void LowHPDefense(Player player, int threshold, int defense)
            {
                if (player.statLife < threshold)
                {
                    player.statDefense += defense;
                }
            }
            static void ProjectileReflect(Player player, float chance)
            {
                player.GetModPlayer<SynergiaPlayer>().projectileReflectChance += chance;
            }
        }
        public class AthenaTooltip : HookForTooltips
        {
            public override Type ItemType => type;
            public override int Target => itemT;
            public override void EditTooltips(origTooltips orig, ModItem item, List<TooltipLine> tooltips)
            {
                level = GetInstance<BismuthArtifactsLevels>().GetLevel();
                tooltips.Add(new TooltipLine(Mod, "ItemName", ItemTooltip(ACC + ".BalanceBismuth", "DivineEquipment")) { OverrideColor = new Color(0, 239, 239) });
                TooltipLine tooltip = new(Mod, "Info", ItemTooltip(ACC + ".BalanceBismuth." + "AthenasShield", "Level" + level.ToString()));
                tooltips.Add(tooltip);
            }
        }
    }
}