using Bismuth.Utilities.ModSupport;
using StramsSurvival.Items.Foods;
using Synergia.Content.Items.QuestItem;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Synergia.Common.QuestSystem;
using static Synergia.Common.QuestSystem.QuestConst;
using static Synergia.Common.SUtils.LocUtil;

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
        public override int CornerItem => ModContent.ItemType<WhisperigReed>();
        public override PostBossQuest PostBossRequirement => PostBossQuest.PostBoss2;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "Hunter", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "Hunter", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().HunterQuest, ModContent.ItemType<WhisperigReed>(), 1, 1, LocQuestKey("Hunter", "QuestCompleted"), LocQuestKey("Hunter", "QuestCompletedFalse"), ModContent.ItemType<SurfandTurf>());
            if (Progress == 0) {
                CompletedQuickSpawnItem(player, ItemID.GoldCoin, 5);
                //ModifyNotificationText(player, "IsWoork", Colors.CoinCopper);
            }
        }
        public override bool IsAvailable(Player player) => BaseIsAvailable(player);
        public override bool IsActive(Player player) => BaseIsActive(player);
    }
}