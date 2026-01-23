using Synergia.Common.ModSystems;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ObjectData;

namespace Synergia.Content.Tiles {
    public class GlobalBanner : ModTile {
        public override void SetStaticDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = [16, 16, 16];
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            TileID.Sets.DisableSmartCursor[Type] = true;
            DustType = -1;
            AddMapEntry(new Color(13, 88, 130), Language.GetText("MapObject.Banner"));
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            int style = frameX / 18;
            SynergiaWorld.BannerType.TryGetValue(style, out (int, int) value);
            Item.NewItem(Main.LocalPlayer.GetSource_FromThis(), i * 16, j * 16, 16, 48, value.Item1);
        }
        public override void NearbyEffects(int i, int j, bool closer) {
            if (closer) {
                int style = Main.tile[i, j].TileFrameX / 18;
                SynergiaWorld.BannerType.TryGetValue(style, out (int, int) value);
                Main.SceneMetrics.NPCBannerBuff[value.Item2] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) {
            if (i % 2 == 1) {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }
    }
}