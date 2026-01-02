using System.Collections.Generic;
using static Terraria.ID.TileID;

namespace Synergia.Lists {
    public class Tiles {
        public static List<int> EvilTiles { get; private set; } = [ShadowOrbs, TileType<Avalon.Tiles.Contagion.SnotOrb.SnotOrb>()];
        public static List<int> VanillaTile { get; private set; } = [Adamantite, Titanium];
    }
}