using Avalon;
using Avalon.Common;
using Avalon.Common.Extensions;
using Avalon.Common.Templates;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Weapons.Ranged
{
	public class Blunderbussin : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 44;
			Item.height = 50;
			Item.useTime = 30;
			Item.useAnimation = 30;
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
			
		    Item.value = Item.buyPrice(gold: 25);
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 56;
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (ModContent.GetInstance<AvalonClientConfig>().AdditionalScreenshakes)
			{
				UseStyles.gunStyle(player, 0.05f, 5f, 3f);
			}
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			var mp = player.GetModPlayer<BlunderPlayer>();

			if (player.altFunctionUse == 2)
			{
				if (mp.RightClickCooldown > 0)
					return false;

				Item.useTime = 8;
				Item.useAnimation = 8;
				Item.UseSound = SoundID.Item38;
				Item.reuseDelay = 0;
				Item.autoReuse = true;
			}
			else
			{
				Item.useTime = 30;
				Item.useAnimation = 30;
				Item.UseSound = SoundID.Item36;
				Item.reuseDelay = 0;
				Item.autoReuse = true;
			}

			return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
			Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			var mp = player.GetModPlayer<BlunderPlayer>();

			if (player.altFunctionUse == 2)
			{
				if (mp.RightClickCooldown == 0)
				{
					mp.RightClickCooldown = 60 * 10;
					mp.DustTime = 60 * 2;
				}

				for (int i = 0; i < 10; i++)
				{
					float angle = MathHelper.ToRadians(-40 + (i * 8));
					Vector2 v = velocity.RotatedBy(angle) * (0.9f + Main.rand.NextFloat(0.2f));
					Projectile.NewProjectile(source, position, v, type, damage, knockback, player.whoAmI);
				}

				for (int i = 0; i < 3; i++)
				{
					Vector2 dustVel = velocity.RotatedByRandom(MathHelper.ToRadians(30)) * 0.4f;
					int d = Dust.NewDust(position, 8, 8, DustID.Smoke, dustVel.X, dustVel.Y);
					Main.dust[d].scale = 1.2f;
					Main.dust[d].noGravity = true;
				}

				return false;
			}

			int amount = Main.rand.Next(4, 7);
			for (int i = 0; i < amount; i++)
			{
				Vector2 vel = AvalonUtils.GetShootSpread(velocity, position, Type, 0.168f,
					Main.rand.NextFloat(-2.7f, 0f), ItemID.MusketBall, true);

				Projectile.NewProjectile(source, position, vel, type, damage, knockback, player.whoAmI);
			}

			return false;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, -1);
		}

		public override void HoldItem(Player player)
		{
			if (player.altFunctionUse != 2 && Item.useTime == 8)
			{
				Item.useTime = 30;
				Item.useAnimation = 30;
				Item.UseSound = SoundID.Item36;
			}
		}
	}

	public class BlunderPlayer : ModPlayer
	{
		public int RightClickCooldown;
		public int DustTime;

		public override void ResetEffects()
		{
			if (RightClickCooldown > 0)
				RightClickCooldown--;

			if (DustTime > 0)
			{
				DustTime--;

				if (Main.rand.NextBool(4))
				{
					int d = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Smoke);
					Main.dust[d].scale = 0.8f;
					Main.dust[d].velocity *= 0.4f;
					Main.dust[d].noGravity = true;
					Main.dust[d].alpha = 150;
				}
			}
		}
		
		public override void PostUpdate()
		{
			if (DustTime > 0 && Main.rand.NextBool(8))
			{
				Dust dust = Dust.NewDustDirect(
					Player.position + new Vector2(Main.rand.Next(Player.width), Player.height - 5), 
					4, 4, DustID.Smoke, 
					0, -0.3f);
				dust.scale = 0.6f;
				dust.velocity *= 0.15f;
				dust.alpha = 180;
			}
		}
	}
}