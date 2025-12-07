using System.Collections.Generic;
using static Terraria.ID.TileID;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Lists {
    public class Tiles {
        public static List<int> EvilTiles { get; private set; } = [ShadowOrbs, TileType<Avalon.Tiles.Contagion.SnotOrb.SnotOrb>()];
    }
}