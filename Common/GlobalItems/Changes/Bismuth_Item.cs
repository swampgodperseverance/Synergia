// Code by SerNik
using Bismuth.Content.Items.Armor;
using Bismuth.Content.Items.Tools;
using Bismuth.Content.Items.Weapons.Melee;
using Bismuth.Content.Items.Weapons.Ranged;
using System.Collections.Generic;
using Terraria;
using Synergia.Helpers;

namespace Synergia.Common.GlobalItems.Changes {
    public class Bismuth_Item : BaseItem {
        public override void EditArmor(Item entity) {
            EditArmor(entity, ItemType<BronzeMask>(), 9);
            EditArmor(entity, ItemType<BronzeBreastplate>(), 10);
            EditArmor(entity, ItemType<BronzeLeggings>(), 9);
        }
        public override void EditWeapon(Item entity) {
            EditWeapon(entity, ItemType<BronzeBow>(), 30, autoUse: true);
            EditWeapon(entity, ItemType<BronzeSword>(), 35, autoUse: true);
            EditWeapon(entity, ItemType<BronzePickaxe>(), 10, minePower: 80, autoUse: true);
            EditWeapon(entity, ItemType<BronzeHammer>(), 10, hammerPower: 90, autoUse: true);
            EditWeapon(entity, ItemType<BronzeAxe>(), 10, axePower: 90, autoUse: true);
        }
        public override void UpdateEquip(Item item, Player player) {
            if (item.type == ItemType<BronzeMask>()) {
                player.GetDamage(DamageClass.Throwing) += 0.05f;
                player.ThrownVelocity += 0.3f;
                player.GetCritChance(DamageClass.Throwing) += 1f;
            }
            if (item.type == ItemType<BronzeBreastplate>()) {
                player.GetDamage(DamageClass.Throwing) += 0.04f;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            ItemHelper.EditTooltips(item, tooltips, ItemType<BronzeMask>(), "5", "10");
            ItemHelper.EditTooltips(item, tooltips, ItemType<BronzeMask>(), "12", "15");
            ItemHelper.EditTooltips(item, tooltips, ItemType<BronzeMask>(), "4", "5");
            ItemHelper.EditTooltips(item, tooltips, ItemType<BronzeBreastplate>(), "6", "10");
        }
    }
}