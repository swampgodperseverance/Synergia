using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Synergia;
using Synergia.Common.GlobalPlayer;

namespace Synergia.Content.Items
{
	public class OldTales : ModItem
	{
		public override void SetStaticDefaults()
		{
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.RaiseLamp;
			Item.holdStyle = ItemHoldStyleID.HoldLamp;
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = false;
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool? UseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				var bookPlayer = player.GetModPlayer<BookPlayer>();
				bookPlayer.BookVisible = !bookPlayer.BookVisible;
				
				SoundEngine.PlaySound(bookPlayer.BookVisible ?
					Sounds.BookOpenSound : Sounds.BookCloseSound);
			}
			return true;
		}
	}
}