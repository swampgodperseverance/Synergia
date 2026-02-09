using Bismuth.Utilities.ModSupport;
using Synergia.Helpers;
using Terraria;
using Terraria.ID;

namespace Synergia.Content.Quests {
    public class HunterQuest_First : BaseQuestLogic {
        public override string DisplayName => LocQuestKey("HunterQuest_Frist", "QuestName");
        public override string DisplayDescription => LocQuestKey("HunterQuest_Frist", "QuestDescription");
        public override string DisplayStage => LocQuestKey("HunterQuest_Frist", "QuestStage");
        public override string Key => "HunterQuest_1";
        public override string NpcKey => HUNTER;
        public override int Priority => 11;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override int CornerItem => ItemHelper.GetRoAItem("LothorMask");
        public override PostBossQuest PostBossRequirement => PostBossQuest.PostBoss2;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "HunterQuest_Frist", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "HunterQuest_Frist", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().HunterQuest1, ItemHelper.GetRoAItem("LothorMask"), 1, 1, LocQuestKey("HunterQuest_Frist", "QuestCompleted"), LocQuestKey("HunterQuest_Frist", "QuestCompletedFalse"), 4372, 1);
            if (Progress == 0) { CompletedQuickSpawnItem(player, ItemID.GoldCoin, 15); }
        }
        public override bool IsAvailable(Player player) => player.GetModPlayer<QuestBoolean>().HunterQuest && BaseIsAvailable(player);
        public override bool IsActive(Player player) => player.GetModPlayer<QuestBoolean>().HunterQuest && BaseIsActive(player);
    }
}