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
            AddQuestChat(quest, player, "TestUIQuest", "Dwarf");
            AddQuestChat(quest, player, "HellDwarfQuest_First", "HellDwarfQuest_First");
        }
        void AddQuestChat(IQuest quest, Player player, string questKey, string locKey) {
            if (quest.UniqueKey == questKey) {
                if (!quest.IsCompleted(player)) {
                    QuestChat = LocQuestKey(locKey, "QuestCompletedFalse");
                }
                if (player.GetModPlayer<QuestBoolean>().HellDwarfQuest) {
                    QuestChat = LocQuestKey(locKey, "QuestProgress0");
                }
            }
        }
        public string GetName(int item) {
            FinalText.TryGetValue(item, out string name);
            return name;
        }
    }
}