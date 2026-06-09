using Avalon.Items.Tools.Hardmode;
using Avalon.Tiles.Ores;
using Bismuth.Content.Items.Tools;
using Microsoft.Xna.Framework;
using NewHorizons.Content.Items.Tools;
using Synergia.Common.GlobalPlayer;
using Synergia.Content.Items.Tools;
using Synergia.Helpers;
using Synergia.Lists;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Tools;

namespace Synergia.Common.GlobalTiles {
    public class EditingAdamantAndTitanOre : GlobalTile {
        readonly Dictionary<Point, int> hitToDestroy = [];

        public override void Load() {
            // Vanilla code
            On_WorldGen.KillTile_DropItems += (orig, x, y, tileCache, includeLargeObjectDrops, includeAllModdedLargeObjectDrops) => {
                orig(x, y, tileCache, includeLargeObjectDrops, includeAllModdedLargeObjectDrops);
                if (Tiles.VanillaTile.Contains(tileCache.TileType)) {
                    hitToDestroy.Remove(new Point(x, y));
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
                        Point key = new(x, y);
                        if (!hitToDestroy.TryGetValue(key, out int cnt)) {
                            cnt = 0;
                        }
                        hitToDestroy[key] = cnt + 1;
                    }
                }
            };
        }
        private static HashSet<int> GetPowerfulPickaxes()
        {
            HashSet<int> powerfulPickaxes = new HashSet<int>
    {
        ItemID.TitaniumPickaxe,
        ItemID.AdamantitePickaxe,
        ItemID.ChlorophytePickaxe,
        ItemID.SpectrePickaxe,
        ItemID.Picksaw,
        ItemID.ShroomiteDiggingClaw,
        ItemID.NebulaPickaxe,
        ItemID.SolarFlarePickaxe,
        ItemID.StardustPickaxe,
        ItemID.VortexPickaxe,
        
        ItemID.PickaxeAxe,
        ItemID.Drax,
        
        ModContent.ItemType<TroxiniumPickaxe>(),
        ModContent.ItemType<NightPickaxe>(),
        ModContent.ItemType<JadePickaxe>(),
        ModContent.ItemType<FeroziumPickaxe>(),
        ModContent.ItemType<CoreburnedPickaxe>(),
        ModContent.ItemType<ThundernitePickaxe>(),
        ModContent.ItemType<NeutronPickaxe>(),
        ModContent.ItemType<BismuthumPickaxe>(),
        ModContent.ItemType<BismuthumDrill>(),
        ModContent.ItemType<NeutronDrill>()
    };

            return powerfulPickaxes;
        }

        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            hitToDestroy.TryGetValue(new Point(i, j), out int currentHits);
            bool fixGen = !WorldGen.gen && !Main.dedServ && Main.netMode != NetmodeID.Server;

            if (fixGen)
            {
                Player player = Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)];
                DebugPlayer.DebugText(currentHits);

                bool hasPowerfulPickaxe = GetPowerfulPickaxes().Contains(PlayerHelpers.GetLocalItem(player).type);

                if (hasPowerfulPickaxe)
                {
                    if (type == ModContent.TileType<TroxiniumOre>())
                    {
                        return true;
                    }
                    else
                    {
                        if (Tiles.VanillaTile.Contains(type))
                        {
                            if (currentHits >= 3)
                            {
                                WorldGen.KillTile(i, j);
                                return true;
                            }
                            return false;
                        }
                        return true;
                    }
                }
                else if (type == ModContent.TileType<TroxiniumOre>() || Tiles.VanillaTile.Contains(type))
                {
                    return false;
                }
                else
                {
                    return base.CanKillTile(i, j, type, ref blockDamaged);
                }
            }
            else
            {
                return base.CanKillTile(i, j, type, ref blockDamaged);
            }
        }

        public override bool CanReplace(int i, int j, int type, int tileTypeBeingPlaced)
        {
            Player player = Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)];

            if (type == ModContent.TileType<TroxiniumOre>() || Tiles.VanillaTile.Contains(type))
            {
                return GetPowerfulPickaxes().Contains(PlayerHelpers.GetLocalItem(player).type);
            }
            else
            {
                return base.CanReplace(i, j, type, tileTypeBeingPlaced);
            }
        }
    }
}