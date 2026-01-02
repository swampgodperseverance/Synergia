using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace Synergia.Content.Items.Accessories {
    [AutoloadEquip(EquipType.Wings)]
    public class PostMechWings : ModItem {
        public override void SetDefaults() {
            Item.height = 20;
            Item.width = 20;
            Item.accessory = true;
        }
        public override void SetStaticDefaults() {
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(170, 7.5f, 1f);
        }
    }
}