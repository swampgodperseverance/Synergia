using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace Synergia.Content.Items.Armor.Magic
{
    [AutoloadEquip(EquipType.Head)]
    public class BronzeAcolyteHat : ModItem {
        public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;
        public override void SetDefaults() {
            int width = 28; int height = 16;
            Item.Size = new Vector2(width, height);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 2;
            Item.value = Item.sellPrice(0, 0, 4, 50);
        }
        public override void UpdateEquip(Player player) => player.manaCost -= 0.06f;
        public override bool IsArmorSet(Item head, Item body, Item legs) {
            bool bBody; bool bLegs;

            bBody = body.type == ModList.Roa.Find<ModItem>("CopperAcolyteJacket").Type   || body.type == ModList.Roa.Find<ModItem>("TinAcolyteJacket").Type   || body.type == ItemType<BronzeAcolyteJacket>();
            bLegs = legs.type == ModList.Roa.Find<ModItem>("CopperAcolyteLeggings").Type || legs.type == ModList.Roa.Find<ModItem>("TinAcolyteLeggings").Type || legs.type == ItemType<BronzeAcolyteLeggings>();

            return bBody && bLegs && head.type == Type;
        }
        public override void UpdateArmorSet(Player player) {
            player.setBonus = Language.GetText("Mods.RoA.Items.Tooltips.AcolyteSetBonus").Value;

            if (player.statMana <= 40) {
                player.GetDamage(DamageClass.Magic) += 0.4f;
            }
        }
    }
}