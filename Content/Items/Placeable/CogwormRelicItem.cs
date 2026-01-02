using Synergia.Content.Tiles.Relic;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Placeable
{
	public class CogwormRelicItem : BaseRelicItem
    {
        public override int RelicType => ModContent.TileType<CogwormRelic>();
	}
}