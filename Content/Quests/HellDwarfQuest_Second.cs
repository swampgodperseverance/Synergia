using Avalon.Items.Weapons.Melee.Hardmode.HellboundHalberd;
using Bismuth.Utilities.ModSupport;
using Synergia.Common.ModSystems.Hooks.Ons;
using Synergia.Content.Items.QuestItem;
using Synergia.Content.NPCs;
using Terraria;
using ValhallaMod.Items.Placeable.Blocks;

namespace Synergia.Content.Quests {
    internal class HellDwarfQuest_Second : BaseQuestLogic {
        public override int QuestNPC => NPCType<HellDwarf>();
        public override bool IsEndQuest => Main.LocalPlayer.GetModPlayer<QuestBoolean>().HellDwarfQuest2;
        public override string Key => "HellDwarfQuest_Second";
        public override string DisplayName => LocQuestKey("HellDwarfQuest_Second", "QuestName");
        public override string DisplayDescription => LocQuestKey("HellDwarfQuest_Second", "QuestDescription");
        public override string DisplayStage => LocQuestKey("HellDwarfQuest_Second", "QuestStage");
        public override string NpcKey => HELLDWARF;
        public override int Priority => 12;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override int CornerItem => ItemType<BoneyRidge>();
        public override PostBossQuest PostBossRequirement => PostBossQuest.PostPlantera;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "HellDwarfQuest_Second", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "HellDwarfQuest_Second", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().HellDwarfQuest2, ItemType<BoneyRidge>(), 1, 1, LocQuestKey("HellDwarfQuest_Second", "QuestCompleted"), LocQuestKey("HellDwarfQuest_Second", "QuestCompletedFalse"), ItemType<SinstoneMagma>(), 25);
            if (Progress == 0) {
                CompletedQuickSpawnItem(player, ItemType<HellboundHalberd>(), 1); 
                player.GetModPlayer<QuestBoolean>().needResset = true;
                player.GetModPlayer<QuestBoolean>().finelText = LocQuestKey("HellDwarfQuest_Second", "QuestCompleted");
                HookForQuest.NpcQuestKeys.Remove(QuestNPC);
            }
        }
        public override bool IsAvailable(Player player) => player.GetModPlayer<QuestBoolean>().HellDwarfQuest1 && BaseIsAvailable(player);
        public override bool IsActive(Player player) => player.GetModPlayer<QuestBoolean>().HellDwarfQuest1 && BaseIsActive(player);
    }
}
