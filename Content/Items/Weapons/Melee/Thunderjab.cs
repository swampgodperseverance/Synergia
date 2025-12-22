using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ValhallaMod.Buffs.Cooldown;
using ValhallaMod.Projectiles.Swing;

namespace Synergia.Content.Items.Weapons.Melee
{
	// Token: 0x0200034F RID: 847
	public class Thunderjab : ModItem
	{
		// Token: 0x06000F5C RID: 3932 RVA: 0x00089768 File Offset: 0x00087968
		public override void SetDefaults()
		{
			base.Item.width = 60;
			base.Item.height = 60;
			base.Item.value = Item.sellPrice(0, 2, 50, 0);
			base.Item.rare = 8;
			base.Item.useTime = 40;
			base.Item.useAnimation = 40;
			base.Item.useStyle = 5;
			base.Item.knockBack = 10f;
			base.Item.autoReuse = false;
			base.Item.damage = 84;
			base.Item.DamageType = DamageClass.Melee;
			base.Item.noMelee = true;
			base.Item.noUseGraphic = true;
			base.Item.reuseDelay = 5;
			base.Item.shoot = ModContent.ProjectileType<ThunderjabProj>();
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x00003673 File Offset: 0x00001873
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x000896CC File Offset: 0x000878CC
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				if (player.HasBuff(ModContent.BuffType<SwordCooldown>()))
				{
					return false;
				}
				player.AddBuff(ModContent.BuffType<SwordCooldown>(), 300, true, false);
			}
			return base.CanUseItem(player);
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x00089844 File Offset: 0x00087A44
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int attackType = 0;
			if (player.altFunctionUse == 2)
			{
				attackType = 1;
			}
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer, (float)attackType, 0f, 0f);
			return false;
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x00003673 File Offset: 0x00001873
		public override bool MeleePrefix()
		{
			return true;
		}
	}
}
