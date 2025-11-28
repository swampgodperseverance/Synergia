using Avalon.NPCs.TownNPCs;
using NewHorizons.Content.NPCs.Town;
using StramsSurvival.NPCs;
using Synergia.Common.ModSystems.Hooks;
using Synergia.Dataset;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.NPCs.TownNPCs;
using static Synergia.Common.QuestSystem.QuestConst;
using static Synergia.ModList;
using Carrot = StramsSurvival.Items.Foods.Carrot;
using Carrot1 = NewHorizons.Content.Items.Materials.Carrot;

namespace Synergia.Common;

public partial class QuestSystem {
    public class RegisterQuestDictionary : ModPlayer {
        public static List<int> CarrotID { get; private set; } = [ModContent.ItemType<Carrot>(), ModContent.ItemType<Carrot1>()];
        public static List<int> FoodID { get; private set; } = [];

        public override void OnEnterWorld() {
            if (!HookForQuest.NpcQuestKeys.ContainsKey(ModContent.NPCType<Dwarf>())) {
                HookForQuest.NpcQuestKeys[ModContent.NPCType<Dwarf>()] = new QuestData(DWARF, 0, 1, true);
            }
            if (!HookForQuest.NpcQuestKeys.ContainsKey(NPCID.TaxCollector)) {
                HookForQuest.NpcQuestKeys[NPCID.TaxCollector] = new QuestData(TAXC, 0, 1, true);
            }
            if (!HookForQuest.NpcQuestKeys.ContainsKey(Roa.Find<ModNPC>("Hunter").Type)) {
                HookForQuest.NpcQuestKeys[Roa.Find<ModNPC>("Hunter").Type] = new QuestData(HUNTER, 0, 1, true, 100);
            }
            if (!HookForQuest.NpcQuestKeys.ContainsKey(ModContent.NPCType<Artist>())) {
                HookForQuest.NpcQuestKeys[ModContent.NPCType<Artist>()] = new QuestData(ARTIST, 0, 1, true);
            }
            if (!HookForQuest.NpcQuestKeys.ContainsKey(ModContent.NPCType<Ninja>())) {
                HookForQuest.NpcQuestKeys[ModContent.NPCType<Ninja>()] = new QuestData(NINJA, 0, 1, true, CarrotID);
            }
            if (!HookForQuest.NpcQuestKeys.ContainsKey(ModContent.NPCType<Farmer>())) {
                HookForQuest.NpcQuestKeys[ModContent.NPCType<Farmer>()] = new QuestData(FARMER, 0, 1, true, FoodID);
            }
            if (!HookForQuest.NpcQuestKeys.ContainsKey(ModContent.NPCType<Librarian>())) {
                HookForQuest.NpcQuestKeys[ModContent.NPCType<Librarian>()] = new QuestData(LIBRARIAN, 0, 1, true);
            }





            for (int i = 0; i < ItemLoader.ItemCount; i++) {
                if (ItemID.Sets.IsFood[i]) {
                    FoodID.Add(i);
                }
            }
        }
    }
}