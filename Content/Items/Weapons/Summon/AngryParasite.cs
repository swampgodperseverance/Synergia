using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Vanilla.Content.Projectiles;
using static Terraria.ModLoader.ModContent;

namespace Vanilla.Content.Items.Weapons.Summon
{
	public class AngryParasite : ModItem
	{
		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<AngryParasiteProjectile>(), 15, 2f, 4f);

			Item.width = 40;
			Item.height = 38;
			
			Item.rare = 2;
			Item.value = Item.sellPrice(0, 5, 0, 0);
		}
	}
}