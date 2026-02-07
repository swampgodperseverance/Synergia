using Avalon.Items.Dyes;
using Bismuth.Utilities.ModSupport;
using Synergia.Common.ModSystems.Hooks.Ons;
using Terraria;
using Terraria.ID;
using ValhallaMod.NPCs.TownNPCs;

namespace Synergia.Content.Quests {
    public class ArtistQuest : BaseQuestLogic {
        public override int QuestNPC => NPCType<Artist>();
        public override bool IsEndQuest => Main.LocalPlayer.GetModPlayer<QuestBoolean>().ArtistQuest;
        public override string Key => "ArtistQuest";
        public override string DisplayName => LocQuestKey("Artist", "QuestName");
        public override string DisplayDescription => LocQuestKey("Artist", "QuestDescription");
        public override string DisplayStage => LocQuestKey("Artist", "QuestStage");
        public override string NpcKey => ARTIST;
        public override int Priority => 10;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override int CornerItem => ItemID.RedHusk;
        public override PostBossQuest PostBossRequirement => PostBossQuest.Null;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "Artist", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "Artist", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().ArtistQuest, ItemID.RedHusk, 1, 1, LocQuestKey("Artist", "QuestCompleted"), LocQuestKey("Artist", "QuestCompletedFalse"), ItemType<CrimstoneDye>());
            if (Progress == 0) { HookForQuest.NpcQuestKeys.Remove(QuestNPC); }
        }
        public override bool IsAvailable(Player player) => BaseIsAvailable(player);
        public override bool IsActive(Player player) => BaseIsActive(player);
    }
}