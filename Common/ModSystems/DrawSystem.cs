using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Synergia.Common.GlobalPlayer;
using Terraria;

namespace Synergia.Common.ModSystems;

public class DrawSystem : ModSystem {
	public override void PostDrawInterface(SpriteBatch spriteBatch) {
        Player player = Main.LocalPlayer;
        player?.GetModPlayer<BookPlayer>()?.DrawBook(spriteBatch);
        player?.GetModPlayer<BookPlayerHardMode>()?.DrawBook(spriteBatch);
    }
}