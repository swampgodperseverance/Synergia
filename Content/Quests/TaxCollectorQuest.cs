using Bismuth.Utilities.ModSupport;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using ValhallaMod.Items.Placeable;
using static Synergia.Common.QuestSystem;
using static Synergia.Common.QuestSystem.QuestConst;

namespace Synergia.Content.Quests
{
    public class TaxCollectorQuest : BaseQuest
    {
        public override string DisplayName => "TTT"; // Название в книге 
        public override string DisplayDescription => "DisPlay"; // Описание в книге
        public override string DisplayStage => "Sttaggeee"; // Этап в книге
        public override string UniqueKey => "TaxQuest";
        public override string NpcKey => TAXC;
        public override int Priority => 10;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override int CornerItem => ModContent.ItemType<DwarvenAnvil>();
        public override PostBossQuest PostBossRequirement => PostBossQuest.Null;
        public override string GetChat(NPC npc, Player player)
        {
            Main.npcChatCornerItem = CornerItem;
            var q = player.GetModPlayer<QuestPlayer>();
            bool defeated = HasDefeated(PostBossRequirement);
            if (q.CompletedQuests.Contains(UniqueKey) && defeated)
            {
                Progress = 0;
                return Language.GetTextValue("Mods.Synergia.Quest.Dwarf.QuestProgress0");
            }
            if (q.ActiveQuests.Contains(UniqueKey) && defeated)
            {
                Progress = 2;
                return Language.GetTextValue("Mods.Synergia.Quest.Dwarf.QuestProgress2"); // Текст, когда квест активен
            }
            Progress = 1;
            return Language.GetTextValue("Mods.Synergia.Quest.Dwarf.QuestProgress1");
        }
        public override string GetButtonText(Player player, ref bool Isfristclicked)
        {
            bool defeated = HasDefeated(PostBossRequirement);
            var q = player.GetModPlayer<QuestPlayer>();
            if (q.CompletedQuests.Contains(UniqueKey) && defeated) return "";
            if (Isfristclicked) {
                return Language.GetTextValue("Mods.Synergia.Quest.Dwarf.QuestButton");
            }
            else return Language.GetTextValue("Mods.Synergia.Quest.Dwarf.QuestButtonGive");
        }
        public override bool IsCompleted(Player player)
        {
            var q = player.GetModPlayer<QuestPlayer>();
            return q.CompletedQuests.Contains(UniqueKey);
        }
        public override void OnChatButtonClicked(Player player)
        {
            var q = player.GetModPlayer<QuestPlayer>();

            if (q.CompletedQuests.Contains(UniqueKey)) return;

            CheckItem(player, ref player.GetModPlayer<QuestBoolean>().DwarfQuest, ModContent.ItemType<DwarvenAnvil>(), 1, 1, Language.GetTextValue("Mods.Synergia.Quest.Dwarf.QuestCompleted"), Language.GetTextValue("Mods.Synergia.Quest.Dwarf.QuestCompletedFalse"));

            if (!q.ActiveQuests.Contains(UniqueKey))
            {
                Notification(player, false, true);
                q.ActiveQuests.Add(UniqueKey);
                Progress = 2;
            }
        }
        public override bool IsAvailable(Player player)
        {
            var q = player.GetModPlayer<QuestPlayer>();
            bool isAvailable = !q.CompletedQuests.Contains(UniqueKey);
            if (isAvailable && HasDefeated(PostBossRequirement)) Progress = 1;
            else Progress = 0;
            return isAvailable;
        }
        public override bool IsActive(Player player)
        {
            var q = player.GetModPlayer<QuestPlayer>();
            bool isActive = q.ActiveQuests.Contains(UniqueKey);
            if (isActive) Progress = 2;
            return isActive;
        }
    }
}