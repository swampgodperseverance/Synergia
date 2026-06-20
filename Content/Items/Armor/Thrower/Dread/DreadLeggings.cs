using Synergia.Common.Rarities;
using Synergia.Common.SUtils;
using Terraria;
using Terraria.ID;
using ValhallaMod;
using ValhallaMod.Items.Armor;
using ValhallaMod.Items.Material.Bar;

namespace Synergia.Content.Items.Armor.Thrower.Dread
{
	[AutoloadEquip(EquipType.Legs)]
	public class DreadLeggings : ModItem {
		public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;
		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 3, 90, 0);
			Item.rare = ModContent.RarityType<CoreburnedRarity>();
			Item.defense = 6;
		}
		public override void UpdateEquip(Player player) {
			player.GetAttackSpeed(DamageClass.Throwing) += 0.12f;
			player.moveSpeed += 0.1f;
        }
		//public override bool IsArmorSet(Item head, Item body, Item legs) {
		//	return body.type == ItemType<CorrodeBody>() && legs.type == ItemType<CorrodeLegs>() && head.type == Type;
		//}
		//public override void UpdateArmorSet(Player player) {

		//}
	}
}