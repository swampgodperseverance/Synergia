using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Vanilla.Content.Projectiles;
using Vanilla.Common.Player;

namespace Vanilla.Content.Items.Thrower
{
	public class OculithShard : ModItem
	{
		public override void SetDefaults()
		{
			var item = Item;

			item.damage = 70;
			item.DamageType = DamageClass.Throwing;
			item.width = 24;
			item.height = 25;
			item.useTime = 20;
			item.useAnimation = 20;
			item.knockBack = 2;
			item.value = Item.buyPrice(silver: 50);
			item.shootSpeed = 10f;
			item.maxStack = 1;
			item.rare = ItemRarityID.Lime;
			item.UseSound = SoundID.Item1;
			item.useStyle = ItemUseStyleID.Swing;
			item.shoot = ModContent.ProjectileType<OculithShardProjectile>();
			item.consumable = false;
			item.autoReuse = true;
			item.noUseGraphic = true;
			item.noMelee = true;
		}
		
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			var modPlayer = player.GetModPlayer<OculithSkullPlayer>();
			modPlayer.skullShotCounter++;

			if (modPlayer.skullShotCounter >= 5)
			{
				modPlayer.skullShotCounter = 0;

				for (int i = -1; i <= 1; i++)
				{
					// Чуть поворачиваем основной вектор (например, на ±30 градусов)
					Vector2 perturbedVelocity = velocity.RotatedBy(MathHelper.ToRadians(i * 30f)) * 0.7f;

					// Немного сдвигаем позицию спавна (например, вбок)
					Vector2 spawnOffset = new Vector2(i * 8f, 0); // по горизонтали

					Projectile.NewProjectile(
						source,
						position + spawnOffset,
						perturbedVelocity,
						ModContent.ProjectileType<OculithSkull>(),
						damage + 50,
						knockback,
						player.whoAmI
					);
				}

				Projectile.NewProjectile(
					source,
					position,
					velocity,
					ModContent.ProjectileType<OculithShardProjectile2>(),
					damage + 140,
					knockback,
					player.whoAmI
				);
			}
			else
			{
				// Обычный бросок (например, как нож или камень)
				Projectile.NewProjectile(
					source,
					position,
					velocity,
					ModContent.ProjectileType<OculithShardProjectile>(),
					damage,
					knockback,
					player.whoAmI
				);
			}

			return false;
		}
	}
}
