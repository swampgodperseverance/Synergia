using Avalon.Common.Templates; // Legenda
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
// LaminatedTable, LaminatedBed, RiseOfTheOldGod
namespace Synergia.Content.Tiles.WorldGen;

public class SynergiaEditTiles {
    public class LaminatedTable : TableTemplate {
        public override bool CanDrop(int i, int j) => true;
        public override IEnumerable<Item> GetItemDrops(int i, int j) { yield return new Item(ModContent.ItemType<ValhallaMod.Items.Placeable.Table.LaminatedTable>()); }
    }
    public class LaminatedBed : BedTemplate {
        public override bool CanDrop(int i, int j) => true;
        public override IEnumerable<Item> GetItemDrops(int i, int j) { yield return new Item(ModContent.ItemType<ValhallaMod.Items.Placeable.Bed.LaminatedBed>()); }
    }
    public class RiseOfTheOldGod : ModTile { 
        public override void SetStaticDefaults() { 
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.Width = 6;
            TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 16];
            Main.tileFrameImportant[Type] = true;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(100, 50, 30), CreateMapEntryName());
        }
        public override bool CanDrop(int i, int j) => true;
        public override IEnumerable<Item> GetItemDrops(int i, int j) { yield return new Item(ModContent.ItemType<ValhallaMod.Items.Placeable.Painting.RiseOfTheOldGod>()); }
    }
}