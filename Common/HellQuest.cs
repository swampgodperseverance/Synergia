using Bismuth.Utilities.ModSupport;
using System.Collections.Generic;
using Terraria;
using ValhallaMod.Items.Placeable;

namespace Synergia.Common {
    public class HellQuest {
        public string QuestChat { get; private set; }
        public Dictionary<int, string> FinalText { get; private set; } = [];

        public HellQuest(IQuest quest, Player player) {
            GetTextForActiveQuest(quest, player);
            FinalText.Add(ItemType<DwarvenAnvil>(), LocQuestKey("Dwarf", "QuestProgress0"));
        }
        void GetTextForActiveQuest(IQuest quest, Player player) {
            if (quest == null || player == null) {
                return;
            }

            AddQuestChat(quest, player, "TestUIQuest", "HellDwarf", !player.GetModPlayer<QuestBoolean>().HellDwarfQuest);
            if (!player.GetModPlayer<QuestBoolean>().needResset) {
                AddQuestChat(quest, player, "HellDwarfQuest_First", "HellDwarfQuest_First", player.GetModPlayer<QuestBoolean>().HellDwarfQuest);
                AddQuestChat(quest, player, "HellDwarfQuest_Second", "HellDwarfQuest_Second", player.GetModPlayer<QuestBoolean>().HellDwarfQuest1);
                AddQuestChat(quest, player, "HellDwarfQuest_Three", "HellDwarfQuest_Three", player.GetModPlayer<QuestBoolean>().HellDwarfQuest2);
            }
        }
        void AddQuestChat(IQuest quest, Player player, string questKey, string locKey, bool questBolean) {
            if (quest.UniqueKey == questKey) {
                if (!quest.IsCompleted(player)) {
                    QuestChat = LocQuestKey(locKey, "QuestProgress0");
                }
                if (questBolean) {
                    QuestChat = LocQuestKey(locKey, "QuestCompletedFalse");
                }
            }
        }
        public string GetName(int item) {
            FinalText.TryGetValue(item, out string name);
            return name;
        }
    }
}