using Synergia.Common.GlobalPlayer;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Synergia.Reassures.Reassures;

namespace Synergia.Content.Items
{
	public class OldTales2 : ModItem
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
				var bookPlayer = player.GetModPlayer<BookPlayerHardMode>();
				bookPlayer.BookVisible = !bookPlayer.BookVisible;
				
				SoundEngine.PlaySound(bookPlayer.BookVisible ? GetSongByName("BookOpen") : GetSongByName("BookClose"));
			}
			return true;
		}
	}
}