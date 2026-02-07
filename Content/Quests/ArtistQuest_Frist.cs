using Bismuth.Utilities.ModSupport;
using Synergia.Common.ModSystems.Hooks.Ons;
using Terraria;
using Terraria.ID;
using ValhallaMod.NPCs.TownNPCs;
using static Synergia.Helpers.ItemHelper;

namespace Synergia.Content.Quests {
    public class ArtistQuest_Frist: BaseQuestLogic {
        public override int QuestNPC => NPCType<Artist>();
        public override bool IsEndQuest => Main.LocalPlayer.GetModPlayer<QuestBoolean>().ArtistQuest1;
        public override string Key => "ArtistQuest_1";
        public override string DisplayName => LocQuestKey("ArtistQuest_Frist", "QuestName");
        public override string DisplayDescription => LocQuestKey("ArtistQuest_Frist", "QuestDescription");
        public override string DisplayStage => LocQuestKey("ArtistQuest_Frist", "QuestStage");
        public override string NpcKey => ARTIST;
        public override int Priority => 11;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override int CornerItem => GetRoAItem("FlamingFabric");
        public override PostBossQuest PostBossRequirement => PostBossQuest.PostSkeletron;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "ArtistQuest_Frist", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "ArtistQuest_Frist", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().ArtistQuest1, GetRoAItem("FlamingFabric"), 1, 1, LocQuestKey("ArtistQuest_Frist", "QuestCompleted"), LocQuestKey("ArtistQuest_Frist", "QuestCompletedFalse"), ItemID.GoldCoin, 3);
            if (Progress == 0) { CompletedQuickSpawnItem(player, GetRoAItem("DevilHunterPants"), 1); CompletedQuickSpawnItem(player, GetRoAItem("DevilHunterCloak"), 1); }
        }
        public override bool IsAvailable(Player player) => player.GetModPlayer<QuestBoolean>().ArtistQuest && BaseIsAvailable(player);
        public override bool IsActive(Player player) => player.GetModPlayer<QuestBoolean>().ArtistQuest && BaseIsActive(player);
    }
}
