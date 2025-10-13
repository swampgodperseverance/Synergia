using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace Synergia.Content.Items.Histories
{
	public class FlashHistory : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.rare = ItemRarityID.White;
		}
	}
}