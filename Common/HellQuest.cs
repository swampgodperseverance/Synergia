using Bismuth.Utilities.ModSupport;
using System.Collections.Generic;
using Terraria;
using ValhallaMod.Items.Placeable;
using static Synergia.Common.QuestSystem;
using static Synergia.Common.SUtils.LocUtil;

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
            if (quest.UniqueKey == "TestUIQuest") {
                if (!quest.IsCompleted(player)) {
                    QuestChat = LocQuestKey("Dwarf", "QuestCompletedFalse");
                }
                if (player.GetModPlayer<QuestBoolean>().HellDwarfQuest) {
                    QuestChat = LocQuestKey("Dwarf", "QuestProgress0");
                }
            }
        }
        public string GetName(int item) {
            FinalText.TryGetValue(item, out string name);
            return name;
        }
    }
}