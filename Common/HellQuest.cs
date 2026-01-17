using Bismuth.Utilities.ModSupport;
using Terraria;
using static Synergia.Common.SUtils.LocUtil;

namespace Synergia.Common {
    public class HellQuest {
        public string QuestChat { get; private set; }

        public HellQuest(IQuest quest, Player player) {
            GetTextForActiveQuest(quest, player);
        }
        void GetTextForActiveQuest(IQuest quest, Player player) {
            if (quest == null || player == null) {
                return;
            }
            if (quest.UniqueKey == "TestUIQuest") {
                if (quest.IsCompleted(player)) {
                    QuestChat = LocQuestKey("Dwarf", "QuestProgress0");
                }
                else {
                    QuestChat = LocQuestKey("Dwarf", "QuestCompletedFalse");
                }
            }
        }
    }
}