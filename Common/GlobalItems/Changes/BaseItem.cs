using Terraria;

namespace Synergia.Common.GlobalItems.Changes {
    public abstract class BaseItem : GlobalItem {
        public sealed override void SetDefaults(Item entity) {
            EditArmor(entity);
            EditWeapon(entity);
        }
        public virtual void EditArmor(Item entity) { }
        public virtual void EditWeapon(Item entity) { }
        protected static void EditArmor(Item item, int armorType, int defense = 0) {
            if (item.type == armorType) {
                if (defense != 0) {
                    item.defense = defense;
                }
            }
        }
        protected static void EditWeapon(Item item, int weaponType, int damage = 0, int speed = 0, int minePower = 0, int hammerPower = 0, int axePower = 0, bool autoUse = false) {
            if (item.type == weaponType) {
                if (damage != 0) {
                    item.damage = damage;
                }
                if (speed != 0) {
                    item.useTime = speed;
                }
                if (minePower != 0) {
                    item.pick = minePower;
                }
                if (hammerPower != 0) {
                    item.hammer = hammerPower;
                }
                if (axePower != 0) {
                    item.axe = axePower;
                }
                item.autoReuse = autoUse;
            }
        }
        protected static bool IsArmorSet(Player player, int head, int body, int legs) {
            if (player.armor[0].type == head && player.armor[1].type == body && player.armor[2].type == legs) {
                return true;
            }
            else { 
                return false;
            }
        }
    }
}