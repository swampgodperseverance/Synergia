using Avalon.Items.Material;
using Bismuth.Utilities.ModSupport;
using Terraria;
using ValhallaMod.Items.Consumable.Bag;
using ValhallaMod.NPCs.TownNPCs;

namespace Synergia.Content.Quests {
    public class DwarfQuest_Frist : BaseQuestLogic {
        public override int QuestNPC => NPCType<Dwarf>();
        public override bool IsEndQuest => Main.LocalPlayer.GetModPlayer<QuestBoolean>().DwarfQuest1;
        public override string DisplayName => LocQuestKey("DwarfQuest_Frist", "QuestName");
        public override string DisplayDescription => LocQuestKey("DwarfQuest_Frist", "QuestDescription");
        public override string DisplayStage => LocQuestKey("DwarfQuest_Frist", "QuestStage");
        public override string Key => "PostWoFDwarfQuest";
        public override string NpcKey => DWARF;
        public override int Priority => 11;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override int CornerItem => ItemType<SoulofIce>();
        public override PostBossQuest PostBossRequirement => PostBossQuest.PostWoF;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "DwarfQuest_Frist", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "DwarfQuest_Frist", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().DwarfQuest1, ItemType<SoulofIce>(), 1, 1, LocQuestKey("DwarfQuest_Frist", "QuestCompleted"), LocQuestKey("DwarfQuest_Frist", "QuestCompletedFalse"), ItemType<GreatGift>(), 1);
        }
        public override bool IsAvailable(Player player) => player.GetModPlayer<QuestBoolean>().DwarfQuest && BaseIsAvailable(player);
        public override bool IsActive(Player player) => player.GetModPlayer<QuestBoolean>().DwarfQuest && BaseIsActive(player);
    }
}
