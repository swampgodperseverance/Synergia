using Synergia.Content.Items.QuestItem;
using Terraria;

namespace Synergia.Common.GlobalTiles {
    public class ReedsDrop : GlobalTile {
        public override void Drop(int i, int j, int type) {
            if (type == TileType<Bismuth.Content.Tiles.Reed>() || type == TileType<Bismuth.Content.Tiles.Reed2>()) {
                if (!Main.LocalPlayer.HasItem(ItemType<WhisperigReed>())) {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), new Vector2(i * 16, j * 16), ItemType<WhisperigReed>(), 1, noBroadcast: false, -1);
                }
            }
        }
    }
}
