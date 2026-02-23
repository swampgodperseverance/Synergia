using Avalon.NPCs.TownNPCs;
using Bismuth.Utilities.ModSupport;
using Synergia.Common.ModSystems.Hooks.Ons;
using Terraria;
using ValhallaMod.Items.Weapons.Magic.Lanterns;

namespace Synergia.Content.Quests {
    public class LibrarianQuest : BaseQuestLogic {
        public override int QuestNPC => NPCType<Librarian>();
        public override bool IsEndQuest => Main.LocalPlayer.GetModPlayer<QuestBoolean>().LibrarianQuest;
        public override string Key => "LibrarianQuest";
        public override string DisplayName => LocQuestKey("Librarian", "QuestName");
        public override string DisplayDescription => LocQuestKey("Librarian", "QuestDescription");
        public override string DisplayStage => LocQuestKey("Librarian", "QuestStage");
        public override string NpcKey => LIBRARIAN;
        public override int Priority => 10;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override int CornerItem => ModList.Valhalla.Find<ModItem>("DamagedBook").Type;
        public override PostBossQuest PostBossRequirement => PostBossQuest.Null;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "Librarian", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "Librarian", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().LibrarianQuest, ModList.Valhalla.Find<ModItem>("DamagedBook").Type, 1, 1, LocQuestKey("Librarian", "QuestCompleted"), LocQuestKey("Librarian", "QuestCompletedFalse"), ItemType<WaterCandelabra>());
            if (Progress == 0) { HookForQuest.NpcQuestKeys.Remove(QuestNPC); }
        }
        public override bool IsAvailable(Player player) => BaseIsAvailable(player);
        public override bool IsActive(Player player) => BaseIsActive(player);
    }
}
