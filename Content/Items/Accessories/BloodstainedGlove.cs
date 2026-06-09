using Avalon.Buffs.Debuffs;
using Avalon.Items.Accessories.Hardmode;
using Synergia.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Accessories
{[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
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
        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient(ModContent.ItemType<FrostGauntlet>(), 1)
                .AddIngredient(ModContent.ItemType<SlitthroatNecklace>(), 1)
                .AddIngredient(ItemID.FragmentSolar, 10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
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