using Bismuth.Utilities.ModSupport;
using StramsSurvival.NPCs;
using Terraria;
using Terraria.ID;
using static Synergia.Lists.Items;

namespace Synergia.Content.Quests {
    public class FarmerQuest_First : BaseQuestLogic {
        public override int QuestNPC => NPCType<Farmer>();
        public override bool IsEndQuest => Main.LocalPlayer.GetModPlayer<QuestBoolean>().FarmerQuest1;
        public override string Key => "FarmerQuest_First";
        public override string DisplayName => LocQuestKey("FarmerQuest_First", "QuestName");
        public override string DisplayDescription => LocQuestKey("FarmerQuest_First", "QuestDescription");
        public override string DisplayStage => LocQuestKey("FarmerQuest_First", "QuestStage");
        public override string NpcKey => FARMER;
        public override int Priority => 11;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override PostBossQuest PostBossRequirement => PostBossQuest.PostSkeletron;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "FarmerQuest_First", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "FarmerQuest_First", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            foreach (int itemId in FoodID) {
                CheckItem(player, ref player.GetModPlayer<QuestBoolean>().FarmerQuest1, itemId, 1, 1, LocQuestKey("FarmerQuest_First", "QuestCompleted"), LocQuestKey("FarmerQuest_First", "QuestCompletedFalse"), FoodID[Main.rand.Next(FoodID)]);
                if (Progress == 0) { CompletedQuickSpawnItem(player, ItemID.GoldCoin, Main.rand.Next(1, 6)); }
                if (player.GetModPlayer<QuestBoolean>().FarmerQuest1) {
                    break;
                }
            }
        }
        public override bool IsAvailable(Player player) => player.GetModPlayer<QuestBoolean>().FarmerQuest && BaseIsAvailable(player);
        public override bool IsActive(Player player) => player.GetModPlayer<QuestBoolean>().FarmerQuest && BaseIsActive(player);
    }
}
