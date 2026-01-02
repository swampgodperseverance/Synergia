using Bismuth.Utilities.ModSupport;
using Terraria;
using ValhallaMod.Items.Material;
using ValhallaMod.Items.Placeable;
using static Synergia.Common.QuestSystem;
using static Synergia.Common.QuestSystem.QuestConst;
using static Synergia.Common.SUtils.LocUtil;

namespace Synergia.Content.Quests
{
    public class DwarfQuest : BaseQuestLogic {
        public override string DisplayName => LocQuestKey("Dwarf", "QuestName");
        public override string DisplayDescription => LocQuestKey("Dwarf", "QuestDescription");
        public override string DisplayStage => LocQuestKey("Dwarf", "QuestStage");
        public override string Key => "PreSkeletronAnvilQuest";
        public override string NpcKey => DWARF;
        public override int Priority => 10;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override int CornerItem => ItemType<DwarvenAnvil>();
        public override PostBossQuest PostBossRequirement => PostBossQuest.Null;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "Dwarf", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "Dwarf", "QuestButton", "QuestButtonGive");     
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().DwarfQuest, ItemType<DwarvenAnvil>(), 1, 1, LocQuestKey("Dwarf", "QuestCompleted"), LocQuestKey("Dwarf", "QuestCompletedFalse"), ItemType<ValhalliteOre>(), RandomValue(45, 60));
        }
        public override bool IsAvailable(Player player) => BaseIsAvailable(player);
        public override bool IsActive(Player player) => BaseIsActive(player);
    }
}