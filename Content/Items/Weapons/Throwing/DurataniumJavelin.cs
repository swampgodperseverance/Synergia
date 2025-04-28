using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Vanilla.Content.Projectiles;

namespace Vanilla.Content.Items.Weapons.Throwing
{
	public class DurataniumJavelin : ModItem
	{
		public override void SetDefaults()
		{
			var item = Item;

			item.damage = 35;
			item.DamageType = DamageClass.Throwing;
			item.width = 24;
			item.height = 25;
			item.useTime = 15;
			item.useAnimation = 15;
			item.knockBack = 2;
			item.value = Item.buyPrice(silver: 50);
			item.shootSpeed = 14f;
			item.maxStack = 1;
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item1;
			item.useStyle = ItemUseStyleID.Swing;
			item.shoot = ModContent.ProjectileType<DurataniumJavelinProjectile>();
			item.consumable = false;
			item.autoReuse = true;
			item.noUseGraphic = true;
			item.noMelee = true;
		}
	}
}
