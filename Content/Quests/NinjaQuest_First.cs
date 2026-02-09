using Avalon.Items.Material;
using Bismuth.Utilities.ModSupport;
using NewHorizons.Content.NPCs.Town;
using Terraria;
using Terraria.ID;

namespace Synergia.Content.Quests {
    public class NinjaQuest_First : BaseQuestLogic {
        public override int QuestNPC => NPCType<Ninja>();
        public override bool IsEndQuest => Main.LocalPlayer.GetModPlayer<QuestBoolean>().NinjaQuest1;
        public override string Key => "NinjaQuest_1";
        public override string DisplayName => LocQuestKey("NinjaQuest_Frist", "QuestName");
        public override string DisplayDescription => LocQuestKey("NinjaQuest_Frist", "QuestDescription");
        public override string DisplayStage => LocQuestKey("NinjaQuest_Frist", "QuestStage");
        public override string NpcKey => NINJA;
        public override int Priority => 11;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override int CornerItem => ItemID.LuckPotion;
        public override PostBossQuest PostBossRequirement => PostBossQuest.PostSkeletron;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "NinjaQuest_Frist", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "NinjaQuest_Frist", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().NinjaQuest1, ItemID.LuckPotion, 1, 1, LocQuestKey("NinjaQuest_Frist", "QuestCompleted"), LocQuestKey("NinjaQuest_Frist", "QuestCompletedFalse"), ItemType<FakeFourLeafClover>(), 3);
            if (Progress == 0) { CompletedQuickSpawnItem(player, ItemID.GoldCoin, 6); }
        }
        public override bool IsAvailable(Player player) => player.GetModPlayer<QuestBoolean>().NinjaQuest && BaseIsAvailable(player);
        public override bool IsActive(Player player) => player.GetModPlayer<QuestBoolean>().NinjaQuest && BaseIsActive(player);
    }
}
