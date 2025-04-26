using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Vanilla.Common.GlobalPlayer;
using Terraria;

namespace Vanilla.Common.ModSystems
{
	public class VanillaSystem : ModSystem
	{
		public override void PostDrawInterface(SpriteBatch spriteBatch)
		{
			Main.LocalPlayer?.GetModPlayer<BookPlayer>()?.DrawBook(spriteBatch);
		}
	}
}