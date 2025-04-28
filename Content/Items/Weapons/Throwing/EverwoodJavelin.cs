using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Vanilla.Content.Projectiles;

namespace Vanilla.Content.Items.Weapons.Throwing
{
	public class EverwoodJavelin : ModItem
	{
		public override void SetDefaults()
		{
			var item = Item;

			item.damage = 40;
			item.DamageType = DamageClass.Throwing;
			item.width = 24;
			item.height = 25;
			item.useTime = 35;
			item.useAnimation = 35;
			item.knockBack = 2;
			item.value = Item.buyPrice(silver: 50);
			item.shootSpeed = 10f;
			item.maxStack = 1;
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item1;
			item.shoot = ModContent.ProjectileType<EverwoodJavelinProjectile>();
			item.useStyle = ItemUseStyleID.Swing;
			item.consumable = false;
			item.autoReuse = true;
			item.noUseGraphic = true;
			item.noMelee = true;
		}
	}
}
