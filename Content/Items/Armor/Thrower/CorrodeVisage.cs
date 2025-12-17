using Microsoft.Xna.Framework;
using Terraria;
using RoA.Content.Items.Equipables.Armor.Magic;
using Terraria.ID;
using Terraria.Localization;
using ValhallaMod;
using ValhallaMod.Items.Armor;
using ValhallaMod.Items.Material.Bar;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Armor.Thrower
{
        [AutoloadEquip(EquipType.Head)]
        public class CorrodeVisage : ModItem {
        public override void SetStaticDefaults() {
            // DisplayName.SetDefault("Acolyte Hat");
            // Tooltip.SetDefault("6% reduced mana usage");
            Item.ResearchUnlockCount = 1;
        }

		public override void SetDefaults()
		{
			base.Item.width = 18;
			base.Item.height = 18;
			base.Item.value = Item.sellPrice(0, 2, 20, 0);
			base.Item.rare = 4;
			base.Item.defense = 6;
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x0009F060 File Offset: 0x0009D260
		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Throwing) += 0.15f;
			player.GetAttackSpeed(DamageClass.Throwing) += 0.1f;
			player.GetCritChance(DamageClass.Throwing) += 15;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (body.type == ModContent.ItemType<CorrodeBody>()) && legs.type == ModContent.ItemType<CorrodeLegs>();
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x0009EF95 File Offset: 0x0009D195
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Your attacks inflict enemies with corrosion decreasing their defense";
			player.GetModPlayer<ValhallaPlayer>().onHitCorrode = true;
		}
		public override void AddRecipes()
		{
			base.CreateRecipe(1).AddIngredient<CorrodeBar>(12).AddTile(16).Register();
		}
	}
}