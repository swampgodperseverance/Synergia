using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace Synergia.Content.Items.Armor.Melee.Thunder;

[AutoloadEquip(EquipType.Legs)]
public class ThunderLegs : ModItem {
    public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;
    public override void SetDefaults() {
        Item.Size = new Vector2(26, 20);
        Item.rare = ItemRarityID.Yellow;
        Item.defense = 20;
        Item.value = Item.sellPrice(0, 3, 8, 50);
    }
    public override void UpdateEquip(Player player) {
        player.GetAttackSpeed(DamageClass.Melee) += 0.08f;
        player.moveSpeed += 0.12f;
    }
}