using Bismuth.Utilities.ModSupport;
using Terraria;
using Terraria.ModLoader;
using ValhallaMod.Items.Material;
using ValhallaMod.Items.Weapons.Magic;
using static Synergia.Common.QuestSystem;
using static Synergia.Common.QuestSystem.QuestConst;
using static Synergia.Common.SUtils.LocUtil;

namespace Synergia.Content.Quests {
    public class LibrarianQuest : BaseQuestLogic {
        public override string Key => "LibrarianQuest";
        public override string DisplayName => LocQuestKey("Librarian", "QuestName");
        public override string DisplayDescription => LocQuestKey("Librarian", "QuestDescription");
        public override string DisplayStage => LocQuestKey("Librarian", "QuestStage");
        public override string NpcKey => LIBRARIAN;
        public override int Priority => 10;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override PostBossQuest PostBossRequirement => PostBossQuest.Null;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "Librarian", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "Librarian", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().LibrarianQuest, ModContent.ItemType<DamagedBook>(), 1, 1, LocQuestKey("Librarian", "QuestCompleted"), LocQuestKey("Librarian", "QuestCompletedFalse"), ModContent.ItemType<WaterCandelabra>());
        }
        public override bool IsAvailable(Player player) => BaseIsAvailable(player);
        public override bool IsActive(Player player) => BaseIsActive(player);
    }
}
