using Avalon.NPCs.TownNPCs;
using NewHorizons.Content.NPCs.Town;
using StramsSurvival.NPCs;
using Synergia.Common.ModSystems.Hooks.Ons;
using Synergia.Content.NPCs;
using Synergia.Dataset;
using Terraria;
using Terraria.ID;
using ValhallaMod.NPCs.TownNPCs;
using static Synergia.Lists.Items;
using static Synergia.ModList;

namespace Synergia.Common;

public partial class QuestSystem {
    public class RegisterQuestDictionary : ModPlayer {
        public override void OnEnterWorld() {
            //Tir 0
            QuestBoolean quest = Player.GetModPlayer<QuestBoolean>();
            HookForQuest.NpcQuestKeys.Clear();
            if (!quest.HunterQuest) {
                if (!HookForQuest.NpcQuestKeys.ContainsKey(Roa.Find<ModNPC>("Hunter").Type)) {
                    HookForQuest.NpcQuestKeys[Roa.Find<ModNPC>("Hunter").Type] = new QuestData(HUNTER, 0, 1, true, 100);
                }
            }
            if (!quest.DwarfQuest) {
                if (!HookForQuest.NpcQuestKeys.ContainsKey(NPCType<Dwarf>())) {
                    HookForQuest.NpcQuestKeys[NPCType<Dwarf>()] = new QuestData(DWARF, 0, 1, true);
                }
            }
            if (!quest.ArtistQuest) {
                if (!HookForQuest.NpcQuestKeys.ContainsKey(NPCType<Artist>())) {
                    HookForQuest.NpcQuestKeys[NPCType<Artist>()] = new QuestData(ARTIST, 0, 1, true);
                }
            }
            if (!quest.NinjaQuest) {
                if (!HookForQuest.NpcQuestKeys.ContainsKey(ModContent.NPCType<Ninja>())) {
                    HookForQuest.NpcQuestKeys[ModContent.NPCType<Ninja>()] = new QuestData(NINJA, 0, 1, true, CarrotID);
                }
            }
            if (!HookForQuest.NpcQuestKeys.ContainsKey(ModContent.NPCType<Farmer>())) {
                HookForQuest.NpcQuestKeys[ModContent.NPCType<Farmer>()] = new QuestData(FARMER, 0, 1, true, FoodID);
            }
            if (!HookForQuest.NpcQuestKeys.ContainsKey(ModContent.NPCType<Librarian>())) {
                HookForQuest.NpcQuestKeys[ModContent.NPCType<Librarian>()] = new QuestData(LIBRARIAN, 0, 1, true);
            }
            if (!HookForQuest.NpcQuestKeys.ContainsKey(NPCType<HellDwarf>())) {
                HookForQuest.NpcQuestKeys[ModContent.NPCType<HellDwarf>()] = new QuestData(HELLDWARF, 0, 1, true);
            }
            for (int i = 0; i < ItemLoader.ItemCount; i++) {
                if (ItemID.Sets.IsFood[i]) {
                    FoodID.Add(i);
                }
            }
        }
        public override void PostUpdate() {
            QuestBoolean quest = Player.GetModPlayer<QuestBoolean>();
            if (quest.HunterQuest) {
                if (Player.talkNPC == -1 || Main.npc[Player.talkNPC].type != Roa.Find<ModNPC>("Hunter").Type) {
                    if (!HookForQuest.NpcQuestKeys.ContainsKey(Roa.Find<ModNPC>("Hunter").Type)) {
                        HookForQuest.NpcQuestKeys[Roa.Find<ModNPC>("Hunter").Type] = new QuestData(HUNTER, 0, 1, true, 100);
                    }
                }
            }
            if (quest.DwarfQuest) {
                AddQuest(NPCType<Dwarf>(), DWARF);
            }
            if (quest.ArtistQuest) {
                AddQuest(NPCType<Artist>(), ARTIST);
            }
            if (quest.NinjaQuest) {
                AddQuest(NPCType<Ninja>(), NINJA);
            }
        }
        void AddQuest(int npcType, string questKey) {
            if (Player.talkNPC == -1 || Main.npc[Player.talkNPC].type != npcType) {
                if (!HookForQuest.NpcQuestKeys.ContainsKey(npcType)) {
                    HookForQuest.NpcQuestKeys[npcType] = new QuestData(questKey, 0, 1, true);
                }
            }
        }
    }
}