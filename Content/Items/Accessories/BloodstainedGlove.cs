using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Avalon.Buffs.Debuffs;

namespace Vanilla.Content.Items.Accessories
{
	public class BloodstainedGlove : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Bloodstained Glove");
			// Tooltip.SetDefault("Increases melee damage, speed, and size");
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.accessory = true;
			Item.value = Item.sellPrice(gold: 17);
			Item.rare = ItemRarityID.Red;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Melee) += 0.15f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.15f;
			player.GetArmorPenetration(DamageClass.Melee) += 15;
			player.autoReuseGlove = true;
			player.meleeScaleGlove = true;
			player.GetModPlayer<BloodstainedGlovePlayer>().bloodstainedGlove = true;
		}
	}

	public class BloodstainedGlovePlayer : ModPlayer
	{
		public bool bloodstainedGlove;

		public override void ResetEffects() => bloodstainedGlove = false;

		public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
		{
			if (bloodstainedGlove && item.DamageType == DamageClass.Melee)
			{
				modifiers.Knockback += 2f;
				target.AddBuff(ModContent.BuffType<Lacerated>(), 60 * 5);
			}
		}
	}
}