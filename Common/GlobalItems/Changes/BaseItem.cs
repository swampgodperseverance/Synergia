using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalItems.Changes {
    public abstract class BaseItem : GlobalItem {
        public sealed override void SetDefaults(Item entity) {
            EditArmor(entity);
        }
        public abstract void EditArmor(Item entity);
        protected static void EditArmor(Item item, int armorType, int defense) {
            if (item.type == armorType) {
                item.defense = defense;
            }
        }
    }
}
