using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Synergia.Lists.Tiles;

namespace Synergia.Common.GlobalTiles {
    public class EditingEvilTiles : GlobalTile {
        public override bool CanExplode(int i, int j, int type) {
            if (EvilTiles.Contains(type)) { return false; }
            else { return base.CanExplode(i, j, type); }
        }
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged) {
            Player player = Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)];
            Item held = player.HeldItem;
            if (EvilTiles.Contains(type)) {
                if (held.hammer >= 55) { return true; }
                else { return false; }
            }
            else { return base.CanKillTile(i, j, type, ref blockDamaged); }
        }
    }
}