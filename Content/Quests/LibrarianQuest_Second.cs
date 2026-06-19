using Avalon.Items.Weapons.Magic.PreHardmode.LotusLeech;
using Avalon.NPCs.TownNPCs;
using Bismuth.Utilities.ModSupport;
using Synergia.Common.ModSystems.Hooks.Ons;
using Synergia.Content.Items.QuestItem;
using Terraria;
using Terraria.ID;

namespace Synergia.Content.Quests {
    internal class LibrarianQuest_Second : BaseQuestLogic {
        public override int QuestNPC => NPCType<Librarian>();
        public override bool IsEndQuest => Main.LocalPlayer.GetModPlayer<QuestBoolean>().LibrarianQuest2;
        public override string Key => "LibrarianQuest_Second";
        public override string DisplayName => LocQuestKey("LibrarianQuest_Second", "QuestName");
        public override string DisplayDescription => LocQuestKey("LibrarianQuest_Second", "QuestDescription");
        public override string DisplayStage => LocQuestKey("LibrarianQuest_Second", "QuestStage");
        public override string NpcKey => LIBRARIAN;
        public override int Priority => 12;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override int CornerItem => ItemType<RoyalInk>();
        public override PostBossQuest PostBossRequirement => PostBossQuest.PostWoF;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "LibrarianQuest_Second", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "LibrarianQuest_Second", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().LibrarianQuest2, ItemType<RoyalInk>(), 1, 1, LocQuestKey("LibrarianQuest_Second", "QuestCompleted"), LocQuestKey("LibrarianQuest_Second", "QuestCompletedFalse"), ItemType<LotusLeech>());
            if (Progress == 0) {
                CompletedQuickSpawnItem(player, ItemID.GoldCoin, 50);
                HookForQuest.NpcQuestKeys.Remove(QuestNPC);
            }
        }
        public override bool IsAvailable(Player player) => player.GetModPlayer<QuestBoolean>().LibrarianQuest1 && BaseIsAvailable(player);
        public override bool IsActive(Player player) => player.GetModPlayer<QuestBoolean>().LibrarianQuest1 && BaseIsActive(player);
    }
}
