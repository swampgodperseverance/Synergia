using Bismuth.Utilities.ModSupport;
using Microsoft.Xna.Framework;
using Synergia.Content.Items.QuestItem;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Synergia.Common.QuestSystem.QuestConst;

namespace Synergia.Content.Tiles.WorldGen {
    public class Reed3 : ModTile {
        public override void SetStaticDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 32;
            TileObjectData.newTile.CoordinateHeights = [16, 16];
            TileObjectData.addTile(Type);
            AddMapEntry(Color.DarkOliveGreen, CreateMapEntryName());
        }
        public override void DropCritterChance(int i, int j, ref int wormChance, ref int grassHopperChance, ref int jungleGrubChance) {
            wormChance = 1;
        }
        public override bool CanDrop(int i, int j) {
            Player player = Main.LocalPlayer;
            IEnumerable<IQuest> quests = QuestRegistry.GetAvailableQuests(player, HUNTER);
            foreach (IQuest quest in quests) {
                if (quest.IsActive(player)) {
                    if (Main.rand.NextBool(3)) {
                        if (player.HasItem(ModContent.ItemType<WhisperigReed>())) {
                            return false;
                        }
                        else { return true; }
                    }
                    else { return false; }
                }
            }
            return false;
        }
        public override IEnumerable<Item> GetItemDrops(int i, int j) {
            yield return new Item(ModContent.ItemType<WhisperigReed>());
        }
    }
}
