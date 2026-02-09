using Avalon.Items.Dyes;
using Bismuth.Utilities.ModSupport;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Placeable.Blocks;
using ValhallaMod.NPCs.TownNPCs;

namespace Synergia.Content.Quests {
    public class HellDwarfQuest_First : BaseQuestLogic {
        public override int QuestNPC => NPCType<Artist>();
        public override bool IsEndQuest => Main.LocalPlayer.GetModPlayer<QuestBoolean>().HellDwarfQuest1;
        public override string Key => "HellDwarfQuest_First";
        public override string DisplayName => LocQuestKey("HellDwarfQuest_First", "QuestName");
        public override string DisplayDescription => LocQuestKey("HellDwarfQuest_First", "QuestDescription");
        public override string DisplayStage => LocQuestKey("HellDwarfQuest_First", "QuestStage");
        public override string NpcKey => HELLDWARF;
        public override int Priority => 11;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override int CornerItem => ItemID.RedHusk;
        public override PostBossQuest PostBossRequirement => PostBossQuest.PostPlantera;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "HellDwarfQuest_First", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "HellDwarfQuest_First", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().ArtistQuest, ItemID.RedHusk, 1, 1, LocQuestKey("HellDwarfQuest_First", "QuestCompleted"), LocQuestKey("HellDwarfQuest_First", "QuestCompletedFalse"), ItemType<SinstoneMagma>());
        }
        public override bool IsAvailable(Player player) => player.GetModPlayer<QuestBoolean>().HellDwarfQuest && BaseIsAvailable(player);
        public override bool IsActive(Player player) => player.GetModPlayer<QuestBoolean>().HellDwarfQuest && BaseIsActive(player);
    }
}
