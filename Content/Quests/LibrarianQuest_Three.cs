using Avalon.NPCs.TownNPCs;
using Bismuth.Utilities.ModSupport;
using Synergia.Content.Items.QuestItem;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Weapons.Magic.Lanterns;

namespace Synergia.Content.Quests {
    internal class LibrarianQuest_Three : BaseQuestLogic {
        public override int QuestNPC => NPCType<Librarian>();
        public override bool IsEndQuest => Main.LocalPlayer.GetModPlayer<QuestBoolean>().LibrarianQuest3;
        public override string Key => "LibrarianQuest_Three";
        public override string DisplayName => LocQuestKey("LibrarianQuest_Three", "QuestName");
        public override string DisplayDescription => LocQuestKey("LibrarianQuest_Three", "QuestDescription");
        public override string DisplayStage => LocQuestKey("LibrarianQuest_Three", "QuestStage");
        public override string NpcKey => LIBRARIAN;
        public override int Priority => 13;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override int CornerItem => ItemType<TimeContinuum>();
        public override PostBossQuest PostBossRequirement => PostBossQuest.PostGolem;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "LibrarianQuest_Three", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "LibrarianQuest_Three", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().LibrarianQuest3, ItemType<TimeContinuum>(), 1, 1, LocQuestKey("LibrarianQuest_Three", "QuestCompleted"), LocQuestKey("LibrariLibrarianQuest_ThreeanQuest_First", "QuestCompletedFalse"), ItemType<Unlighter>());
            if (Progress == 0) {
                CompletedQuickSpawnItem(player, ItemID.GreaterManaPotion, 50);
            }
        }
        public override bool IsAvailable(Player player) => player.GetModPlayer<QuestBoolean>().LibrarianQuest2 && BaseIsAvailable(player);
        public override bool IsActive(Player player) => player.GetModPlayer<QuestBoolean>().LibrarianQuest2 && BaseIsActive(player);
    }
}
