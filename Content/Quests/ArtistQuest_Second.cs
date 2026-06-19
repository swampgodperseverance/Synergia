using Avalon.Items.Dyes;
using Bismuth.Utilities.ModSupport;
using Synergia.Common.ModSystems.Hooks.Ons;
using Synergia.Content.Items.QuestItem;
using Synergia.Content.Items.Weapons.Melee;
using Terraria;
using Terraria.ID;
using ValhallaMod.NPCs.TownNPCs;

namespace Synergia.Content.Quests {
    internal class ArtistQuest_Second : BaseQuestLogic {
        public override int QuestNPC => NPCType<Artist>();
        public override bool IsEndQuest => Main.LocalPlayer.GetModPlayer<QuestBoolean>().ArtistQuest2;
        public override string Key => "ArtistQuest";
        public override string DisplayName => LocQuestKey("ArtistQuest_Second", "QuestName");
        public override string DisplayDescription => LocQuestKey("ArtistQuest_Second", "QuestDescription");
        public override string DisplayStage => LocQuestKey("ArtistQuest_Second", "QuestStage");
        public override string NpcKey => ARTIST;
        public override int Priority => 12;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override int CornerItem => ItemType<RoyalInk>();
        public override PostBossQuest PostBossRequirement => PostBossQuest.PostWoF;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "ArtistQuest_Second", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "ArtistQuest_Second", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().ArtistQuest2, ItemType<RoyalInk>(), 1, 1, LocQuestKey("ArtistQuest_Second", "QuestCompleted"), LocQuestKey("ArtistQuest_Second", "QuestCompletedFalse"), ItemType<Seaborn>());
            if (Progress == 0) { CompletedQuickSpawnItem(player, ItemID.GoldCoin, 50); HookForQuest.NpcQuestKeys.Remove(QuestNPC); }
        }
        public override bool IsAvailable(Player player) => player.GetModPlayer<QuestBoolean>().ArtistQuest1 && BaseIsAvailable(player);
        public override bool IsActive(Player player) => player.GetModPlayer<QuestBoolean>().ArtistQuest1 && BaseIsActive(player);
    }
}
