using Bismuth.Utilities.ModSupport;
using Synergia.Common.ModSystems.Hooks.Ons;
using Synergia.Content.Items.QuestItem;
using Terraria;
using Terraria.ID;

namespace Synergia.Content.Quests {
    public class HunterQuest : BaseQuestLogic {
        public override string Key => "HunterQuestPostBrain";
        public override string DisplayName => LocQuestKey("Hunter", "QuestName");
        public override string DisplayDescription => LocQuestKey("Hunter", "QuestDescription");
        public override string DisplayStage => LocQuestKey("Hunter", "QuestStage");
        public override string NpcKey => HUNTER;
        public override int Priority => 10;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override int CornerItem => ItemType<WhisperigReed>();
        public override PostBossQuest PostBossRequirement => PostBossQuest.PostBoss2;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "Hunter", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "Hunter", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().HunterQuest, ItemType<WhisperigReed>(), 1, 1, LocQuestKey("Hunter", "QuestCompleted"), LocQuestKey("Hunter", "QuestCompletedFalse"), 1);
            if (Progress == 0) {
                CompletedQuickSpawnItem(player, ItemID.GoldCoin, 5);
                HookForQuest.NpcQuestKeys.Remove(ModList.Roa.Find<ModNPC>("Hunter").Type);
            }
        }
        public override bool IsAvailable(Player player) => BaseIsAvailable(player);
        public override bool IsActive(Player player) => BaseIsActive(player);
    }
}