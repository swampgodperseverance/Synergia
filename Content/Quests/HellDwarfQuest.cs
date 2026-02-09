using Avalon.Items.Material;
using Bismuth.Utilities.ModSupport;
using Synergia.Common.ModSystems.Hooks.Ons;
using Synergia.Content.NPCs;
using Terraria;
using ValhallaMod.Items.Placeable.Blocks;

namespace Synergia.Content.Quests {
    public class HellDwarfQuest : BaseQuestLogic {
        public override int QuestNPC => NPCType<HellDwarf>();
        public override bool IsEndQuest => Main.LocalPlayer.GetModPlayer<QuestBoolean>().HellDwarfQuest;
        public override string DisplayName => LocQuestKey("HellDwarf", "QuestName");
        public override string DisplayDescription => LocQuestKey("HellDwarf", "QuestDescription");
        public override string DisplayStage => LocQuestKey("HellDwarf", "QuestStage");
        public override string Key => "TestUIQuest";
        public override string NpcKey => HELLDWARF;
        public override int Priority => 10;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override int CornerItem => ItemType<BottledLava>();
        public override PostBossQuest PostBossRequirement => PostBossQuest.PostPlantera;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "HellDwarf", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "HellDwarf", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().HellDwarfQuest, ItemType<BottledLava>(), 1, 1, LocQuestKey("HellDwarf", "QuestCompleted"), LocQuestKey("HellDwarf", "QuestCompletedFalse"), ItemType<SinstoneMagma>(), 25);
            if (Progress == 0) { HookForQuest.NpcQuestKeys.Remove(QuestNPC); }
        }
        public override bool IsAvailable(Player player) => BaseIsAvailable(player);
        public override bool IsActive(Player player) => BaseIsActive(player);
    }
}