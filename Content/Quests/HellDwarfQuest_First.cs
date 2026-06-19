using Bismuth.Utilities.ModSupport;
using Synergia.Common.ModSystems.Hooks.Ons;
using Synergia.Content.Items.QuestItem;
using Synergia.Content.NPCs;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Placeable.Blocks;

namespace Synergia.Content.Quests {
    public class HellDwarfQuest_First : BaseQuestLogic {
        public override int QuestNPC => NPCType<HellDwarf>();
        public override bool IsEndQuest => Main.LocalPlayer.GetModPlayer<QuestBoolean>().HellDwarfQuest1;
        public override string Key => "HellDwarfQuest_First";
        public override string DisplayName => LocQuestKey("HellDwarfQuest_First", "QuestName");
        public override string DisplayDescription => LocQuestKey("HellDwarfQuest_First", "QuestDescription");
        public override string DisplayStage => LocQuestKey("HellDwarfQuest_First", "QuestStage");
        public override string NpcKey => HELLDWARF;
        public override int Priority => 11;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override int CornerItem => ItemType<Pandemonium>();
        public override PostBossQuest PostBossRequirement => PostBossQuest.PostPlantera;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "HellDwarfQuest_First", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "HellDwarfQuest_First", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().HellDwarfQuest1, ItemType<Pandemonium>(), 1, 1, LocQuestKey("HellDwarfQuest_First", "QuestCompleted"), LocQuestKey("HellDwarfQuest_First", "QuestCompletedFalse"), ItemType<SinstoneMagma>(), 25);
            if (Progress == 0) { 
                HookForQuest.NpcQuestKeys.Remove(QuestNPC); 
                player.GetModPlayer<QuestBoolean>().needResset = true;
                player.GetModPlayer<QuestBoolean>().finelText = LocQuestKey("HellDwarfQuest_First", "QuestCompleted");
            }
        }
        public override bool IsAvailable(Player player) => player.GetModPlayer<QuestBoolean>().HellDwarfQuest && BaseIsAvailable(player);
        public override bool IsActive(Player player) => player.GetModPlayer<QuestBoolean>().HellDwarfQuest && BaseIsActive(player);
    }
}
