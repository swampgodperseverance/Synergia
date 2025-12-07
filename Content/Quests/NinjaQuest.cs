using Bismuth.Utilities.ModSupport;
using NewHorizons.Content.Items.Accessories;
using Synergia.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Synergia.Common.QuestSystem;
using static Synergia.Common.QuestSystem.QuestConst;
using static Synergia.Common.SUtils.LocUtil;
using static Synergia.Lists.Items;

namespace Synergia.Content.Quests {
    public class NinjaQuest : BaseQuestLogic {
        readonly UIHelper helper = new();

        public override string Key => "NinjaQuestPostKingSlime";
        public override string DisplayName => LocQuestKey("Ninja", "QuestName");
        public override string DisplayDescription => LocQuestKey("Ninja", "QuestDescription");
        public override string DisplayStage => LocQuestKey("Ninja", "QuestStage");
        public override string NpcKey => NINJA;
        public override int Priority => 10;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override PostBossQuest PostBossRequirement => PostBossQuest.Null;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "Ninja", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "Ninja", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            int itemId = helper.NoStaticGetNextItemType(CarrotID);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().NinjaQuest, itemId, 1, 1, LocQuestKey("Ninja", "QuestCompleted"), LocQuestKey("Ninja", "QuestCompletedFalse"), ModContent.ItemType<ShurikenWheel>());
            if (Progress == 0) {
                CompletedQuickSpawnItem(player, ItemID.GoldCoin, 4);
            }
        }
        public override bool IsAvailable(Player player) => BaseIsAvailable(player);
        public override bool IsActive(Player player) => BaseIsActive(player);
    }
}