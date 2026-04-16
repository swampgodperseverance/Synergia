using System;
using System.Collections.Generic;
using Bismuth.Content.Buffs;
using Bismuth.Content.Items.Accessories;
using Synergia.Common.GlobalPlayer;
using Terraria;
namespace Synergia.Common.ModSystems.Hooks.Ons.EditAcc
{
    public class HookForRingOfOmnipotence
    {
        readonly static Type type = typeof(RingOfOmnipotence);
        readonly static int itemT = ItemType<RingOfOmnipotence>();
        static int level;
        public class BonusRingOfOmnipotence : HookForEditAcc
        {
            public override Type HookType => type;
            public override int HookItem => itemT;
            public override void EditAcc(Orig_UpdateAccessory orig, ModItem item, Player player, bool hideVisual)
            {
                level = GetInstance<BismuthArtifactsLevels>().GetLevel();
                if (level == 1)
                {
                    GetDamage(player, 0.08f);
                    player.endurance += 0.05f;
                }
                if (level == 2)
                {
                    GetDamage(player, 0.10f);
                    player.statLifeMax2 += 25;
                    player.endurance += 0.08f;
                }
                if (level == 3)
                {
                    GetDamage(player, 0.10f);
                    player.statLifeMax2 += 25;
                    player.endurance += 0.08f;
                    player.GetModPlayer<SynergiaPlayer>().omnipotenceDodgeChance += 0.005f; 
                }
                if (level == 4)
                {
                    GetDamage(player, 0.10f);
                    player.statLifeMax2 += 40;
                    player.endurance += 0.10f;
                    player.GetModPlayer<SynergiaPlayer>().omnipotenceDodgeChance += 0.015f;
                    player.GetModPlayer<SynergiaPlayer>().omnipotenceThornsOnDodge = true;
                    player.GetModPlayer<SynergiaPlayer>().omnipotenceThornsMultiplier = 1f;
                }
                if (level == 5)
                {
                    GetDamage(player, 0.12f);
                    player.statLifeMax2 += 60;
                    player.endurance += 0.15f;
                    player.GetModPlayer<SynergiaPlayer>().omnipotenceDodgeChance += 0.025f;
                    player.GetModPlayer<SynergiaPlayer>().omnipotenceThornsOnDodge = true;
                    player.GetModPlayer<SynergiaPlayer>().omnipotenceThornsMultiplier = 3f;
                }
            }
            static void GetDamage(Player player, float bonus, float crit = 0f)
            {
                player.GetDamage(DamageClass.Generic) += bonus;
                player.GetCritChance(DamageClass.Generic) += crit;
            }
        }
        public class TooltipsRingOfOmnipotence : HookForTooltips
        {
            public override Type ItemType => type;
            public override int Target => itemT;
            public override void EditTooltips(origTooltips orig, ModItem item, List<TooltipLine> tooltips)
            {
                level = GetInstance<BismuthArtifactsLevels>().GetLevel();
                tooltips.Add(new TooltipLine(Mod, "ItemName", ItemTooltip(ACC + ".BalanceBismuth", "DivineEquipment")) { OverrideColor = new Color(0, 239, 239) });
                TooltipLine tooltip = new(Mod, "Info", ItemTooltip(ACC + ".BalanceBismuth." + "RingOfOmnipotence", "Level" + level.ToString()));
                tooltips.Add(tooltip);
            }
        }
    }
}