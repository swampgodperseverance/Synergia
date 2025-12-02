using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalTiles {
    public class EditingEvilTiles : GlobalTile {
        readonly static List<int> EvilTiles = [TileID.ShadowOrbs, ModContent.TileType<Avalon.Tiles.Contagion.SnotOrb.SnotOrb>()];
        
        public override bool CanExplode(int i, int j, int type) {
            if (EvilTiles.Contains(type)) {
                return false;
            }
            else {
                return base.CanExplode(i, j, type);
            }
        }
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged) {
            Player player = Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)];
            Item held = player.HeldItem;
            if (EvilTiles.Contains(type)) {
                if (held.hammer >= 55) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return base.CanKillTile(i, j, type, ref blockDamaged);
            }
        }
    }
}
