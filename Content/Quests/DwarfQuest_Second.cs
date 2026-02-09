using Avalon.Items.Material;
using Bismuth.Utilities.ModSupport;
using Terraria;
using Terraria.ID;
using ValhallaMod.NPCs.TownNPCs;

namespace Synergia.Content.Quests {
    public class DwarfQuest_Second : BaseQuestLogic {
        public override int QuestNPC => NPCType<Dwarf>();
        public override bool IsEndQuest => Main.LocalPlayer.GetModPlayer<QuestBoolean>().DwarfQuest2;
        public override string DisplayName => LocQuestKey("DwarfQuest_Second", "QuestName");
        public override string DisplayDescription => LocQuestKey("DwarfQuest_Second", "QuestDescription");
        public override string DisplayStage => LocQuestKey("DwarfQuest_Second", "QuestStage");
        public override string Key => "DwarfQuest_Second";
        public override string NpcKey => DWARF;
        public override int Priority => 12;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override int CornerItem => ItemType<SoulofIce>();
        public override PostBossQuest PostBossRequirement => PostBossQuest.PostMechBosses;
        public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "DwarfQuest_Second", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "DwarfQuest_Second", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().DwarfQuest2, ItemType<SoulofIce>(), 1, 1, LocQuestKey("DwarfQuest_Second", "QuestCompleted"), LocQuestKey("DwarfQuest_Second", "QuestCompletedFalse"), ItemID.GoldCoin, 5);
            if (Progress == 0) { CompletedQuickSpawnItem(player, Main.rand.Next(Lists.Items.FoodID), 1); }
        }
        public override bool IsAvailable(Player player) => player.GetModPlayer<QuestBoolean>().DwarfQuest1 && BaseIsAvailable(player);
        public override bool IsActive(Player player) => player.GetModPlayer<QuestBoolean>().DwarfQuest1 && BaseIsActive(player);
    }
}
