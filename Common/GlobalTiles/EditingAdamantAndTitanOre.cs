using Avalon.Tiles.Ores;
using Microsoft.Xna.Framework;
using Synergia.Helpers;
using Synergia.Lists;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Tools;

namespace Synergia.Common.GlobalTiles {
    public class EditingAdamantAndTitanOre : GlobalTile {
        int hitToDestroy = 0;
        // TODO: Mr Swamp pls fix bug one value for all tile!
        // Dictionary<Tile, int> hitToDestroy = [];

        public override void Load() {
            // Vanilla code
            On_WorldGen.KillTile_DropItems += (orig, x, y, tileCache, includeLargeObjectDrops, includeAllModdedLargeObjectDrops) => {
                orig(x, y, tileCache, includeLargeObjectDrops, includeAllModdedLargeObjectDrops);
                if (Tiles.VanillaTile.Contains(tileCache.TileType)) {
                    hitToDestroy = 0;
                }
            };
            On_Player.PickTile += (orig, player, x, y, pickPower) => {
                orig(player, x, y, pickPower);
                Tile tile = Framing.GetTileSafely(x, y);
                if (Tiles.VanillaTile.Contains(tile.TileType)) {
                    int id = player.hitTile.HitObject(x, y, 1);
                    if (id >= 0) {
                        // smart cursor fix :) 
                        TileID.Sets.SmartCursorPickaxePriorityOverride[tile.TileType] = 1;
                        hitToDestroy++;
                    }
                }
            };
        }
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged) {
            Main.NewText(hitToDestroy);
            Player player = Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)]; // ?? Maine.LocalPlayer 
            if (type == ModContent.TileType<TroxiniumOre>()) {
                if (PlayerHelpers.GetLocalItem(player).type == ModContent.ItemType<JadePickaxe>()) {
                    return true;
                }
                else {
                    return false;
                }
            }
            if (Tiles.VanillaTile.Contains(type)) {
                if (PlayerHelpers.GetLocalItem(player).type == ModContent.ItemType<JadePickaxe>()) {
                    if (hitToDestroy == 3) {
                        WorldGen.KillTile(i, j);
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    return false;
                }
            }
            else {
                return base.CanKillTile(i, j, type, ref blockDamaged);
            }
        }
        public override bool CanReplace(int i, int j, int type, int tileTypeBeingPlaced) {
            Player player = Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)];
            if (type == ModContent.TileType<TroxiniumOre>()) {
                if (PlayerHelpers.GetLocalItem(player).type == ModContent.ItemType<JadePickaxe>()) {
                    return true;
                }
                else {
                    return false;
                }
            }
            if (Tiles.VanillaTile.Contains(type)) {
                if (PlayerHelpers.GetLocalItem(player).type == ModContent.ItemType<JadePickaxe>()) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return base.CanReplace(i, j, type, tileTypeBeingPlaced);
            }
        }
    }
}
