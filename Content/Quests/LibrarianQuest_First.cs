using Avalon.Items.Weapons.Magic.PreHardmode.LotusLeech;
using Avalon.NPCs.TownNPCs;
using Bismuth.Utilities.ModSupport;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Material;

namespace Synergia.Content.Quests {
    public class LibrarianQuest_First : BaseQuestLogic {
        public override int QuestNPC => NPCType<Librarian>();
        public override bool IsEndQuest => Main.LocalPlayer.GetModPlayer<QuestBoolean>().LibrarianQuest1;
        public override string Key => "LibrarianQuest_First";
        public override string DisplayName => LocQuestKey("LibrarianQuest_First", "QuestName");
        public override string DisplayDescription => LocQuestKey("LibrarianQuest_First", "QuestDescription");
        public override string DisplayStage => LocQuestKey("LibrarianQuest_First", "QuestStage");
        public override string NpcKey => LIBRARIAN;
        public override int Priority => 11;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override int CornerItem => ItemType<DamagedBook>();
        public override PostBossQuest PostBossRequirement => PostBossQuest.PostSkeletron;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "LibrarianQuest_First", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "LibrarianQuest_First", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().LibrarianQuest1, ItemType<DamagedBook>(), 1, 1, LocQuestKey("LibrarianQuest_First", "QuestCompleted"), LocQuestKey("LibrarianQuest_First", "QuestCompletedFalse"), ItemType<LotusLeech>());
            if (Progress == 0) {
                CompletedQuickSpawnItem(player, ItemID.ManaPotion, 15);
            }
        }
        public override bool IsAvailable(Player player) => player.GetModPlayer<QuestBoolean>().LibrarianQuest && BaseIsAvailable(player);
        public override bool IsActive(Player player) => player.GetModPlayer<QuestBoolean>().LibrarianQuest && BaseIsActive(player);
    }
}
