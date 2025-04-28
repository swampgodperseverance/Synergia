using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Linq;
using Vanilla.Content.Projectiles;

namespace Vanilla.Content.Items.Weapons.Throwing
{
	public class Airflow : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 25;
			Item.DamageType = DamageClass.Throwing;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 5;
			Item.value = Item.buyPrice(gold: 1);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<AirflowProjectile>();
			Item.shootSpeed = 12f;
		}

		public override bool CanUseItem(Player player)
		{
			int activeBoomerangs = Main.projectile.Count(p =>
					p.active &&
					p.owner == player.whoAmI &&
					p.type == ModContent.ProjectileType<AirflowProjectile>() &&
					p.aiStyle == 3 && p.timeLeft > 10);

			// Max 2
			return activeBoomerangs < 2;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(
				source,
				position,
				velocity,
				ModContent.ProjectileType<AirflowProjectile>(),
				25,
				knockback,
				player.whoAmI
			);
			return false;
		}
	}
}
