using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace Synergia.Content.Items.Armor.Magic;

[AutoloadEquip(EquipType.Body)]
sealed class BronzeAcolyteJacket : ModItem {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
    }
    public override void SetDefaults() {
        int width = 26; int height = 20;
        Item.Size = new Vector2(width, height);
        Item.rare = ItemRarityID.Blue;
        Item.defense = 3;
        Item.value = Item.sellPrice(0, 0, 7, 50);
    }
    public override void UpdateEquip(Player player) => player.statManaMax2 += 40;
}