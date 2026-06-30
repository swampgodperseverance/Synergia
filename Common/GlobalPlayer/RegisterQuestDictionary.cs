using Avalon.NPCs.TownNPCs;
using Bismuth;
using Bismuth.Content.NPCs;
using NewHorizons.Content.NPCs.Town;
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
                if (!HookForQuest.NpcQuestKeys.ContainsKey(NPCType<Ninja>())) {
                    HookForQuest.NpcQuestKeys[NPCType<Ninja>()] = new QuestData(NINJA, 0, 1, true);
                }
            }
            if (!quest.LibrarianQuest) {
                if (!HookForQuest.NpcQuestKeys.ContainsKey(NPCType<Librarian>())) {
                    HookForQuest.NpcQuestKeys[NPCType<Librarian>()] = new QuestData(LIBRARIAN, 0, 1, true);
                }
            }
            if (!quest.HellDwarfQuest) {
                if (!HookForQuest.NpcQuestKeys.ContainsKey(NPCType<HellDwarf>())) {
                    HookForQuest.NpcQuestKeys[NPCType<HellDwarf>()] = new QuestData(HELLDWARF, 0, 1, true);
                }
            }
            if (!quest.ImperianConsulQuest) {
                if (!HookForQuest.NpcQuestKeys.ContainsKey(NPCType<ImperianConsul>())) {
                    HookForQuest.NpcQuestKeys[NPCType<ImperianConsul>()] = new QuestData("ImperianConsul", 0, 1, true);
                }
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

            MultiQuest(quest.DwarfQuest, quest.DwarfQuest1, NPCType<Dwarf>(), DWARF);
            OmegaQuest(quest.ArtistQuest, quest.ArtistQuest1, quest.ArtistQuest2, NPCType<Artist>(), ARTIST);
            OmegaQuest(quest.LibrarianQuest, quest.LibrarianQuest1, quest.LibrarianQuest2, NPCType<Librarian>(), LIBRARIAN);

            if (!quest.needResset) { OmegaQuest(quest.HellDwarfQuest, quest.HellDwarfQuest1, quest.HellDwarfQuest2, NPCType<HellDwarf>(), HELLDWARF); }

            if (quest.NinjaQuest) {
                AddQuest(NPCType<Ninja>(), NINJA);
            }
            if (quest.ImperianConsulQuest) {
                AddQuest(NPCType<ImperianConsul>(), "ImperianConsul");
            }
            if (Player.GetModPlayer<Quests>().EquipmentQuest == 0) { Player.GetModPlayer<Quests>().EquipmentQuest = -1; }
        }
        void OmegaQuest(bool quest1, bool quest2, bool quest3, int npcType, string questKey) {
            if (quest1) {
                if (quest2) {
                    if (quest3) { AddQuest(npcType, questKey); }
                    else { AddQuest(npcType, questKey); }
                }
                else AddQuest(npcType, questKey);
            }
        }
        void MultiQuest(bool quest1, bool quest2, int npcType, string questKey) {
            if (quest1) {
                if (quest2) { AddQuest(npcType, questKey); }
                else AddQuest(npcType, questKey);
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