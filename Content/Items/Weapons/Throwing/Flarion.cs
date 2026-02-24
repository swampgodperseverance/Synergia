using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Synergia.Content.Projectiles.Thrower;
using Synergia.Common.Rarities;

namespace Synergia.Content.Items.Weapons.Throwing
{
	public class Flarion : ModItem
	{
		public override void SetDefaults()
		{
			base.Item.DamageType = DamageClass.Throwing;
			base.Item.autoReuse = true;
			base.Item.noMelee = true;
			base.Item.useStyle = 1;
			base.Item.shootSpeed = 12f;
			base.Item.damage = 58;
			base.Item.knockBack = 4f;
			base.Item.width = 24;
			base.Item.height = 24;
			base.Item.UseSound = new SoundStyle?(SoundID.Item1);
			base.Item.useAnimation = 44;
			base.Item.useTime = 44;
			base.Item.noUseGraphic = true;
			base.Item.rare = 4;
			base.Item.shoot = ModContent.ProjectileType<FlarionProj>();
			base.Item.value = Item.sellPrice(0, 1, 88, 0);
		
		}
	}
}
