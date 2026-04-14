using Synergia.Content.Items.Weapons.Mage;
using System;
using System.Collections.Generic;
using Terraria;
using ValhallaMod;
using ValhallaMod.DamageClasses;

namespace Synergia.Common.GlobalItems {
    public class AddBloodDamage : GlobalItem {
        public int lifeCost = -1;

        public override bool InstancePerEntity => true;
        public override void SetDefaults(Item entity) {
            if (entity.type == ModList.Roa.Find<ModItem>("ItemName").Type) {
                lifeCost = 2;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (lifeCost > 0) {
                tooltips.Add(new TooltipLine(Mod, "Blood Cost", $"Uses {LifeCostCalculate(item, Main.LocalPlayer)} life"));
                tooltips.Add(new TooltipLine(Mod, "Beta Info", "--Beta item--"));
            }
        }
        int LifeCostCalculate(Item item, Player player) {
            BloodDamagePlayer modPlayer = player.GetModPlayer<BloodDamagePlayer>();
            float num = lifeCost;
            num *= modPlayer.lifeCost;
            if (item.DamageType == GetInstance<WarlockDamageClass>()) {
                num *= player.manaCost;
            }

            return (int)Math.Round(num);
        }
        public override bool CanUseItem(Item item, Player player) {
            BloodDamagePlayer modPlayer = player.GetModPlayer<BloodDamagePlayer>();
            int num = LifeCostCalculate(item, player);
            if (player.statLife > num) {
                if (Main.rand.NextFloat() > modPlayer.lifeConsume) {
                    player.statLife -= num;
                }

                player.lifeRegenTime = 0f;
                return true;
            }

            return false;
        }
    }
}