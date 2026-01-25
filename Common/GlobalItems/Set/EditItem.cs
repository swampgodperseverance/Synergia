using Avalon.Items.Accessories.Hardmode;
using Bismuth.Content.Items.Accessories;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Weapons.Javelin;

namespace Synergia.Common.GlobalItems.Set {
    public class EditItem : GlobalItem {
        public override void SetDefaults(Item entity) {
            if (entity.type == ItemType<CarrotDagger>()) {
                entity.damage = 15;
            }
        }
        public override void UpdateAccessory(Item item, Player player, bool hideVisual) {
            if (item.type == ItemType<BacchusBoots>()) {
                player.GetDamage(DamageClass.Summon) += 0.10f;
            }
            if (item.type == ItemType<BerserksRing>()) {
                player.GetDamage(DamageClass.Generic) -= 0.20f;
            }
        }
    }
}
