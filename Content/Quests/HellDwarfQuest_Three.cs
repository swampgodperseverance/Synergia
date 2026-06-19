using Bismuth.Utilities.ModSupport;
using Synergia.Content.Items.Misc;
using Synergia.Content.Items.QuestItem;
using Synergia.Content.NPCs;
using Terraria;
using ValhallaMod.Items.Placeable.Blocks;

namespace Synergia.Content.Quests {
    internal class HellDwarfQuest_Three : BaseQuestLogic {
        public override int QuestNPC => NPCType<HellDwarf>();
        public override bool IsEndQuest => Main.LocalPlayer.GetModPlayer<QuestBoolean>().HellDwarfQuest3;
        public override string Key => "HellDwarfQuest_Three";
        public override string DisplayName => LocQuestKey("HellDwarfQuest_Three", "QuestName");
        public override string DisplayDescription => LocQuestKey("HellDwarfQuest_Three", "QuestDescription");
        public override string DisplayStage => LocQuestKey("HellDwarfQuest_Three", "QuestStage");
        public override string NpcKey => HELLDWARF;
        public override int Priority => 13;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override int CornerItem => ItemType<HellwormScale>();
        public override PostBossQuest PostBossRequirement => PostBossQuest.PostPlantera;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "HellDwarfQuest_Three", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "HellDwarfQuest_Three", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().HellDwarfQuest3, ItemType<HellwormScale>(), 1, 1, LocQuestKey("HellDwarfQuest_Three", "QuestCompleted"), LocQuestKey("HellDwarfQuest_Three", "QuestCompletedFalse"), ItemType<SinstoneMagma>(), 45);
            if (Progress == 0) { 
                player.GetModPlayer<QuestBoolean>().finelText = LocQuestKey("HellDwarfQuest_Three", "QuestCompleted");
            }
        }
        public override bool IsAvailable(Player player) => player.GetModPlayer<QuestBoolean>().HellDwarfQuest2 && BaseIsAvailable(player);
        public override bool IsActive(Player player) => player.GetModPlayer<QuestBoolean>().HellDwarfQuest2 && BaseIsActive(player);
    }
}
