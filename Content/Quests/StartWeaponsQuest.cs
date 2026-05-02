using Bismuth;
using Bismuth.Content.NPCs;
using Bismuth.Utilities.ModSupport;
using Synergia.Common.GlobalPlayer;
using Terraria;

namespace Synergia.Content.Quests {
    public class StartWeaponsQuest : BaseQuestLogic {
        public override int QuestNPC => NPCType<ImperianConsul>();
        public override bool IsEndQuest => Main.LocalPlayer.GetModPlayer<QuestBoolean>().ImperianConsulQuest;
        public override string Key => "ImperianConsulQuest";
        public override string DisplayName => LocQuestKey("ImperianConsul", "QuestName");
        public override string DisplayDescription {
            get {
                if (Main.LocalPlayer.GetModPlayer<StartWeapons>().EndQuest) {
                    string consulName = NPC.FindFirstNPC(NPCType<ImperianConsul>()) != -1 ? Main.npc[NPC.FindFirstNPC(NPCType<ImperianConsul>())].GivenName : Lang.GetNPCNameValue(NPCType<Alchemist>());
                    return string.Format(LocQuestKey("ImperianConsul", "QuestDescription2"), consulName);
                }
                else {
                    string commanderName = NPC.FindFirstNPC(NPCType<ImperianCommander>()) != -1 ? Main.npc[NPC.FindFirstNPC(NPCType<ImperianCommander>())].GivenName : Lang.GetNPCNameValue(NPCType<Alchemist>());
                    return string.Format(LocQuestKey("ImperianConsul", "QuestDescription"), commanderName);
                }
            }
        }
        public override string DisplayStage {
            get {
                if (Main.LocalPlayer.GetModPlayer<StartWeapons>().EndQuest) {
                    string commanderName = NPC.FindFirstNPC(NPCType<ImperianCommander>()) != -1 ? Main.npc[NPC.FindFirstNPC(NPCType<ImperianCommander>())].GivenName : Lang.GetNPCNameValue(NPCType<Alchemist>());
                    return string.Format(LocQuestKey("ImperianConsul", "QuestStage2"), commanderName);
                }
                else { return LocQuestKey("ImperianConsul", "QuestStage"); }
            }
        }
        public override string NpcKey => "ImperianConsul";
        public override int Priority => 10;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override PostBossQuest PostBossRequirement => PostBossQuest.Null;
        //public override string GetChat(NPC npc, Player player) => BaseGetChat(player, "ImperianConsul", "QuestProgress0", "QuestProgress2", "QuestProgress1");
        public override string GetChat(NPC npc, Player player) {
            QuestPlayer q = player.GetModPlayer<QuestPlayer>();
            bool defeated = HasDefeated(PostBossRequirement);
            if (q.CompletedQuests.Contains(UniqueKey) && defeated) {
                Progress = 0;
                return LocQuestKey("ImperianConsul", "QuestProgress0");
            }
            if (q.ActiveQuests.Contains(UniqueKey) && defeated) {
                Progress = 2;
                return LocQuestKey("ImperianConsul", "QuestProgress2");
            }
            Progress = 1;
            string commanderName = NPC.FindFirstNPC(NPCType<ImperianCommander>()) != -1 ? Main.npc[NPC.FindFirstNPC(NPCType<ImperianCommander>())].GivenName : Lang.GetNPCNameValue(ModContent.NPCType<Alchemist>());
            return string.Format(LocQuestKey("ImperianConsul", "QuestProgress1"), commanderName);
        }
        public override string GetButtonText(Player player, ref bool Isfristclicked) => BaseGetButtonText(player, ref Isfristclicked, "ImperianConsul", "QuestButton", "QuestButtonGive");
        public override bool IsCompleted(Player player) => BaseIsCompleted(player);
        public override void OnChatButtonClicked(Player player) {
            BaseOnChatButtonClicked(player);
            if (player.GetModPlayer<StartWeapons>().EndQuest) {
                Main.npcChatText = LocQuestKey("ImperianConsul", "QuestCompleted");
                player.GetModPlayer<Bismuth.Quests>().EquipmentQuest = 100;
                player.GetModPlayer<QuestPlayer>().CompletedQuests.Add(UniqueKey);
                Notification(player, ISCompletedSuccessfully: true, ISQUESTACCEPTED: false);
                Progress = 0;
            }
            else { 
                Main.npcChatText = LocQuestKey("ImperianConsul", "QuestCompletedFalse");
                player.GetModPlayer<StartWeapons>().ActiveQuest = true;
            }
        }
        public override bool IsAvailable(Player player) => BaseIsAvailable(player);
        public override bool IsActive(Player player) => BaseIsActive(player);
    }
}
