using Synergia.Common.SUtils;
using Terraria;
using Terraria.ID;
using ValhallaMod;
using ValhallaMod.Items.Armor;
using ValhallaMod.Items.Material.Bar;

namespace Synergia.Content.Items.Armor.Thrower
{
	[AutoloadEquip(EquipType.Head)]
	public class CorrodeVisage : ModItem {
		public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;
		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 2, 20, 0);
			Item.rare = 4;
			Item.defense = 6;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(DamageClass.Throwing) += 0.15f;
			player.GetAttackSpeed(DamageClass.Throwing) += 0.1f;
			player.GetCritChance(DamageClass.Throwing) += 15;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ItemType<CorrodeBody>() && legs.type == ItemType<CorrodeLegs>() && head.type == Type;
		}
		public override void UpdateArmorSet(Player player) {
			player.setBonus = LocUtil.ItemTooltip(LocUtil.ARM, "CorrodeSetBonus");
			player.GetModPlayer<ValhallaPlayer>().onHitCorrode = true;
		}
		public override void AddRecipes() => CreateRecipe().AddIngredient<CorrodeBar>(12).AddTile(TileID.Anvils).Register();
	}
}