using Bismuth.Utilities.ModSupport;
using Synergia.Content.Items.QuestItem;
using Synergia.Content.Items.Weapons.Melee;
using Terraria;
using ValhallaMod.NPCs.TownNPCs;

namespace Synergia.Content.Quests {
    internal class ArtistQuest_Three : BaseQuestLogic {
        public override int QuestNPC => NPCType<Artist>();
        public override bool IsEndQuest => Main.LocalPlayer.GetModPlayer<QuestBoolean>().ArtistQuest3;
        public override string Key => "ArtistQuest_Three";
        public override string DisplayName => LocQuestKey("ArtistQuest_Three", "QuestName");
        public override string DisplayDescription => LocQuestKey("ArtistQuest_Three", "QuestDescription");
        public override string DisplayStage => LocQuestKey("ArtistQuest_Three", "QuestStage");
        public override string NpcKey => ARTIST;
        public override int Priority => 13;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override int CornerItem => ItemType<FeneathsBrush>();
        public override PostBossQuest PostBossRequirement => PostBossQuest.PostPlantera;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "ArtistQuest_Three", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "ArtistQuest_Three", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().ArtistQuest3, ItemType<FeneathsBrush>(), 1, 1, LocQuestKey("ArtistQuest_Three", "QuestCompleted"), LocQuestKey("ArtistQuest_Three", "QuestCompletedFalse"), ItemType<Seaborn>());
        }
        public override bool IsAvailable(Player player) => player.GetModPlayer<QuestBoolean>().ArtistQuest2 && BaseIsAvailable(player);
        public override bool IsActive(Player player) => player.GetModPlayer<QuestBoolean>().ArtistQuest2 && BaseIsActive(player);
    }
}
