using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using Vanilla.Content.Projectiles;

namespace Vanilla.Content.Items.Weapons.Throwing
{
	public class NaturalSelection : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 20;
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
			Item.shoot = ModContent.ProjectileType<NaturalSelectionProj>();
			Item.shootSpeed = 12f;
		}

		public override bool CanUseItem(Player player)
		{
			int activeBoomerangs = Main.projectile.Count(p =>
					p.active &&
					p.owner == player.whoAmI &&
					p.type == ModContent.ProjectileType<NaturalSelectionProj>() &&
					p.aiStyle == 3 && p.timeLeft > 10);

			// Max 5
			return activeBoomerangs < 5;
		}
	}
}
