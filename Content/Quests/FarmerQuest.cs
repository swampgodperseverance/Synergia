using Bismuth.Utilities.ModSupport;
using StramsSurvival.Items.Placeable.Furniture;
using Terraria;
using Terraria.ModLoader;
using static Synergia.Common.QuestSystem;
using static Synergia.Common.QuestSystem.QuestConst;
using static Synergia.Common.SUtils.LocUtil;
using static Synergia.Lists.Items;

namespace Synergia.Content.Quests {
    public class FarmerQuest : BaseQuestLogic {
        public override string Key => "FarmerQuest";
        public override string DisplayName => LocQuestKey("Farmer", "QuestName");
        public override string DisplayDescription => LocQuestKey("Farmer", "QuestDescription");
        public override string DisplayStage => LocQuestKey("Farmer", "QuestStage");
        public override string NpcKey => FARMER;
        public override int Priority => 10;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override PostBossQuest PostBossRequirement => PostBossQuest.Null;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "Farmer", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "Farmer", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            foreach (int itemId in FoodID) {
                CheckItem(player, ref player.GetModPlayer<QuestBoolean>().FarmerQuest, itemId, 1, 1, LocQuestKey("Farmer", "QuestCompleted"), LocQuestKey("Farmer", "QuestCompletedFalse"), ModContent.ItemType<Oven>());
                if (player.GetModPlayer<QuestBoolean>().FarmerQuest) {
                    break;
                }
            }
        }
        public override bool IsAvailable(Player player) => BaseIsAvailable(player);
        public override bool IsActive(Player player) => BaseIsActive(player);
    }
}
