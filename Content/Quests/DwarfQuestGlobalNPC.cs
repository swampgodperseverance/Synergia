using Bismuth.Utilities.ModSupport;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Common.ModSystems.Hooks;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ValhallaMod.Items.Placeable;
using ValhallaMod.NPCs.TownNPCs;
using static Synergia.Common.QuestSystem;
using static Synergia.Common.QuestSystem.QuestConst;
using static Synergia.Common.QuestSystem.QuestDrawSystem;

namespace Synergia.Content.Quests
{
    public class DwarfQuestGlobalNPC : GlobalNPC
    {
        public override void OnChatButtonClicked(NPC npc, bool firstButton)
        {
            if (npc.type == ModContent.NPCType<Dwarf>())
            {
                if (firstButton) { }
                else
                {
                    Player player = Main.LocalPlayer;
                    var quest = QuestRegistry.GetAvailableQuests(player, DWARF).FirstOrDefault();
                    quest?.OnChatButtonClicked(player);
                }
            }
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => DrawQuestIcon(npc, spriteBatch, DWARF);
    }
    public class DwarfQuestSystem : HookForQuest
    {
        public override Type CompileTime() => typeof(Dwarf);

        public override void NewSetChatButtons(Orig_SetChatButtons orig, Dwarf npc, ref string button, ref string button2)
        {
            orig(npc, ref button, ref button2);
            if (button2 == "Help")
            {
                Player player = Main.LocalPlayer;
                var quest = QuestRegistry.GetAvailableQuests(player, DWARF).FirstOrDefault();
                if (quest != null)
                {
                    button2 = quest.GetButtonText(player);
                }
            }
        }
        public override string NewGetChat(Orig_GetChat orig, Dwarf npc)
        {
            string originalResult = orig(npc);
            Player player = Main.LocalPlayer;
            var quest = QuestRegistry.GetAvailableQuests(player, DWARF).FirstOrDefault();
            foreach (NPC nPC in Main.npc)
            {
                if (quest != null)
                {
                    return quest.GetChat(nPC, player, quest.CornerItem);
                }
            }
            return originalResult;
        }
    }
    public class DwarfQuest : BaseQuest
    {
        public override string DisplayName => Language.GetTextValue("Mods.Synergia.Quest.Dwarf.QuestName"); // Название в книге 
        public override string DisplayDescription => Language.GetTextValue("Mods.Synergia.Quest.Dwarf.QuestDescription"); // Описание в книге
        public override string DisplayStage => Language.GetTextValue("Mods.Synergia.Quest.Dwarf.QuestStage"); // Этап в книге
        public override string UniqueKey => "PreSkeletronAnvilQuest";
        public override string NpcKey => DWARF;
        public override int Priority => 10;
        public override bool ISManyEndings => false;
        public override QuestPhase Phase => QuestPhase.PreSkeletron;
        public override int CornerItem => ModContent.ItemType<DwarvenAnvil>();
        public override PostBossQuest PostBossRequirement => PostBossQuest.Null;
        public override string GetChat(NPC npc, Player player, int corneritem)
        {
            corneritem = CornerItem;
            Main.npcChatCornerItem = corneritem;
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
        public override string GetButtonText(Player player)
        {
            bool defeated = HasDefeated(PostBossRequirement);
            var q = player.GetModPlayer<QuestPlayer>();
            if (q.CompletedQuests.Contains(UniqueKey) && defeated) return "";
            return Language.GetTextValue("Mods.Synergia.Quest.Dwarf.QuestButton");
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
                Language.GetTextValue("Mods.Synergia.Quest.Dwarf.QuestCompleted"), // Локализация если квест завершен
                Language.GetTextValue("Mods.Synergia.Quest.Dwarf.QuestCompletedFalse"), // И если не завершен
                ItemID.ZapinatorGray, 10); // Награда, количество
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