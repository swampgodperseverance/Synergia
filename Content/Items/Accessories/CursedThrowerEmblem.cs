using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace Synergia.Content.Items.Accessories
{
	public class CursedThrowerEmblem : ModItem
	{
		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(this.Item.type, (DrawAnimation) new DrawAnimationVertical(5, 5, false));
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.accessory = true;
			Item.value = Item.sellPrice(gold: 17);
			Item.rare = ItemRarityID.Red;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Throwing) += 0.75f;
			player.GetDamage(DamageClass.Generic) -= 0.5f;
		}
	}
}
