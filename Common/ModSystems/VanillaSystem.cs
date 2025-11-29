using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Synergia.Common.GlobalPlayer;
using Terraria;

namespace Synergia.Common.ModSystems
{
	public class VanillaSystem : ModSystem
	{
		public override void PostDrawInterface(SpriteBatch spriteBatch)
		{
			Main.LocalPlayer?.GetModPlayer<BookPlayer>()?.DrawBook(spriteBatch);
			Main.LocalPlayer?.GetModPlayer<BookPlayerHardMode>()?.DrawBook(spriteBatch);
            Main.LocalPlayer?.GetModPlayer<ThrowingUI>()?.DrawThrowingUI(spriteBatch);
        }
	}
}