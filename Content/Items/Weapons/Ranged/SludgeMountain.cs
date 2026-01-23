using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.RangedProjectiles;
using Avalon.Common;
using Avalon.Common.Extensions;

namespace Synergia.Content.Items.Weapons.Ranged
{
    public class SludgeMountain : ModItem
    {
        public override void SetDefaults()
        {
	        Item.width = 44;
			Item.height = 50;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 5f;
			Item.value = Item.sellPrice(0, 1, 50);
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.Item36;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 50f;
			Item.useAmmo = AmmoID.Bullet;
			
		    Item.value = Item.buyPrice(gold: 15);
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 38;
		}
			public override bool Shoot(
				Player player,
				EntitySource_ItemUse_WithAmmo source,
				Vector2 position,
				Vector2 velocity,
				int type,
				int damage,
				float knockback)
			{
				int bulletAmount = Main.rand.Next(3, 6);

				for (int i = 0; i < bulletAmount; i++)
				{
					Vector2 vel = AvalonUtils.GetShootSpread(
						velocity,
						position,
						Type,
						0.18f,
						Main.rand.NextFloat(-4f, 4f),
						type,
						true
					);

					Projectile.NewProjectile(
						source,
						position,
						vel,
						type,
						damage,
						knockback,
						player.whoAmI
					);
				}

				int sludgeType = ModContent.ProjectileType<Sludge>();

				for (int i = 0; i < 2; i++)
				{
					Vector2 vel = AvalonUtils.GetShootSpread(
						velocity,
						position,
						Type,
						0.12f,
						Main.rand.NextFloat(-2f, 2f),
						sludgeType,
						true
					);

					Projectile.NewProjectile(
						source,
						position,
						vel,
						sludgeType,
						damage,
						knockback,
						player.whoAmI
					);
				}

				return false;
			}


		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, -1);
		}
    }
}