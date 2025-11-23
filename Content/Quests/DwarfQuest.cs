using Bismuth.Utilities.ModSupport;
using Terraria;
using Terraria.ModLoader;
using ValhallaMod.Items.Material.Bar;
using ValhallaMod.Items.Placeable;
using static Synergia.Common.QuestSystem;
using static Synergia.Common.QuestSystem.QuestConst;
using static Synergia.Common.SUtils.LocUtil;

namespace Synergia.Content.Quests
{
    public class DwarfQuest : BaseQuest
    {
        public override string DisplayName => LocQuestKey("Dwarf", "QuestName"); // Название в книге 
        public override string DisplayDescription => LocQuestKey("Dwarf", "QuestDescription"); // Описание в книге
        public override string DisplayStage => LocQuestKey("Dwarf", "QuestStage"); // Этап в книге
        public override string UniqueKey => "PreSkeletronAnvilQuest";
        public override string NpcKey => DWARF;
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
                return LocQuestKey("Dwarf", "QuestProgress0");
            }
            if (q.ActiveQuests.Contains(UniqueKey) && defeated)
            {
                Progress = 2;
                return LocQuestKey("Dwarf", "QuestProgress2"); // Текст, когда квест активен
            }
            Progress = 1;
            return LocQuestKey("Dwarf", "QuestProgress1"); ;
        }
        public override string GetButtonText(Player player, ref bool Isfristclicked)
        {
            bool defeated = HasDefeated(PostBossRequirement);
            var q = player.GetModPlayer<QuestPlayer>();
            if (q.CompletedQuests.Contains(UniqueKey) && defeated) return "";
            if (Isfristclicked) {
                return LocQuestKey("Dwarf", "QuestButton");
            }
            else return LocQuestKey("Dwarf", "QuestButtonGive");
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

            CheckItem(
                player, // Необходимый параметр
                ref player.GetModPlayer<QuestBoolean>().DwarfQuest, // Особый флаг
                ModContent.ItemType<DwarvenAnvil>(), 1, 1, // Нужный предмет, количество и сколько npc заберет
                LocQuestKey("Dwarf", "QuestCompleted"), // Локализация если квест завершен
                LocQuestKey("Dwarf", "QuestCompletedFalse"), // И если не завершен
                ModContent.ItemType<ValhalliteBar>(), Main.rand.Next(10, 21)); // Награда, количество
                /* а также есть доп параметры
                * IsNotification = true;
                * IsQuestcompleted = true;
                * progres = 0;
                */

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