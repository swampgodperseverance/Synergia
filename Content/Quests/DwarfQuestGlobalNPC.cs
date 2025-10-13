using Bismuth.Utilities.ModSupport;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour;
using ValhallaMod.Items.Placeable;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.NPCs.TownNPCs;

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
                    Player player = Main.player[Main.myPlayer];
                    var quest = QuestRegistry.GetAvailableQuests(player, BaseQuest.DwarfBlacksmith).FirstOrDefault();
                    quest?.OnChatButtonClicked(player);
                    var quests = QuestRegistry.GetAvailableQuests(player, BaseQuest.DwarfBlacksmith).ToList();
                    Main.NewText($"Quests found: {quests.Count} for key={BaseQuest.DwarfBlacksmith}");
                    foreach (var q in quests)
                    {
                        Main.NewText($"Quest: {q.DisplayName}, NpcKey={q.NpcKey}");
                    }
                }
            }
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.type == ModContent.NPCType<Dwarf>())
            {
                var quests = QuestRegistry.GetAvailableQuests(Main.LocalPlayer, BaseQuest.DwarfBlacksmith);
                foreach (var quest in quests)
                {
                    bool showAvailable = quest.IsAvailable(Main.LocalPlayer) && !quest.IsActive(Main.LocalPlayer);
                    bool showActive = quest.IsActive(Main.LocalPlayer);
                    quest?.IsActiveQuestUIIcon(showAvailable, showActive, spriteBatch, npc, Main.LocalPlayer);
                }
            }
        }
    }
    public class HookClass : ModSystem
    {
        private Hook _GetSetChatButtons;
        private Hook _GetGetChats;
        public override void Load()
        {
            MethodInfo target = typeof(Dwarf).GetMethod(nameof(Dwarf.SetChatButtons), BindingFlags.Instance | BindingFlags.Public);
            _GetSetChatButtons = new Hook(target, (GetSetChatButtons)NewSetChatButtons);
            MethodInfo _GetGetChat = typeof(Dwarf).GetMethod(nameof(Dwarf.GetChat), BindingFlags.Instance | BindingFlags.Public);
            _GetGetChats = new Hook(_GetGetChat, (GetChat)NewGetChat);
        }
        public override void Unload()
        {
            _GetSetChatButtons?.Dispose();
            _GetSetChatButtons = null;
            _GetGetChats?.Dispose();
            _GetGetChats = null;
        }
        private delegate void Orig_SetChatButtons(Dwarf npc, ref string button, ref string button2);
        private delegate void GetSetChatButtons(Orig_SetChatButtons orig, Dwarf npc, ref string button, ref string button2);
        private delegate string Orig_GetChat(Dwarf npc);
        private delegate string GetChat(Orig_GetChat orig, Dwarf npc);
        private void NewSetChatButtons(Orig_SetChatButtons orig, Dwarf npc, ref string button, ref string button2)
        {
            orig(npc, ref button, ref button2);
            if (button2 == "Help")
            {
                Player player = Main.player[Main.myPlayer];
                var quest = QuestRegistry.GetAvailableQuests(player, BaseQuest.DwarfBlacksmith).FirstOrDefault();
                if (quest != null)
                {
                    button2 = quest.GetButtonText(player);
                }
            }
        }
        private string NewGetChat(Orig_GetChat orig, Dwarf npc)
        {
            string originalResult = orig(npc);
            Player player = Main.player[Main.myPlayer];
            var quest = QuestRegistry.GetAvailableQuests(player, BaseQuest.DwarfBlacksmith).FirstOrDefault();
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
        //Progress // 0 - not started, 1 - available, 2 - active
        public override string DisplayName => "Dwarven Anvil"; // Название в книге
        public override string DisplayDescription => "Somewhere deep within the winter biome lies an ancient Dwarven Anvil."; // Описание в книге
        public override string DisplayStage => "Collect Anvil"; // Этап в книге
        public override string UniqueKey => "PreSkeletronAnvilQuest"; // Уникальный ключ
        public override string NpcKey => DwarfBlacksmith; // Ключ NPC, который дает квест
        public override int Priority => 10; // Приоритет квеста
        public override bool ISManyEndings => false; // Если у квеста несколько стадий
        public override QuestPhase Phase => QuestPhase.PreSkeletron; // Для алхимика
        public override int CornerItem => ModContent.ItemType<DwarvenAnvil>(); // Иконка в углу чата
        public override PostBossQuest PostBossRequirement => PostBossQuest.PostEoC; // После убийства босса квест активен

        public override string GetChat(NPC npc, Player player, int corneritem)
        {
            corneritem = CornerItem;
            Main.npcChatCornerItem = corneritem;
            var q = player.GetModPlayer<QuestPlayer>();
            bool defeated = HasDefeated(PostBossRequirement);

            if (q.CompletedQuests.Contains(UniqueKey) && defeated)
            {
                Progress = 0; // Сброс прогресса после завершения
                return "Thanks for the Anvil!";
            }
            if (q.ActiveQuests.Contains(UniqueKey) && defeated)
            {
                Progress = 2; // Квест активен
                return "Have you gathered an Anvil yet?";
            }
            Progress = 1; // Квест доступен
            return "Please bring me an Anvil before you defeat Skeletron.";
        }

        public override string GetButtonText(Player player)
        {
            bool defeated = HasDefeated(PostBossRequirement);
            var q = player.GetModPlayer<QuestPlayer>();
            if (q.CompletedQuests.Contains(UniqueKey) && defeated) 
                return ""; // Если квест завершен, кнопка не показывается
            return "Accept";
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

            // Проверяем наличие предмета у игрока
            for (int i = 0; i < player.inventory.Length; i++)
            {
                if ((player.inventory[i].type == ModContent.ItemType<DwarvenAnvil>()) && player.inventory[i].stack >= 1)
                {
                    player.inventory[i].stack -= 1;
                    q.CompletedQuests.Add(UniqueKey);
                    Main.npcChatText = "Quest completed! Thanks for the Dwarven Anvil!";
                    Notification(player, true, false); // Уведомление о завершении
                    Progress = 0;
                    return;
                }
            }

            // Если предмета нет, но квест ещё не активен
            if (!q.ActiveQuests.Contains(UniqueKey))
            {
                q.ActiveQuests.Add(UniqueKey);
                Progress = 2;
                Notification(player, false, true); // Уведомление о старте
            }

            Main.npcChatText = "You don't have an anvil yet.";
        }

        public override bool IsAvailable(Player player)
        {
            var q = player.GetModPlayer<QuestPlayer>();
            bool defeated = HasDefeated(PostBossRequirement);
            bool isAvailable = !q.CompletedQuests.Contains(UniqueKey);
            if (isAvailable && defeated)
                Progress = 1;
            else
                Progress = 0;
            return isAvailable;
        }

        public override bool IsActive(Player player)
        {
            var q = player.GetModPlayer<QuestPlayer>();
            bool isActive = q.ActiveQuests.Contains(UniqueKey);
            if (isActive)
                Progress = 2;
            return isActive;
        }

        public override void IsActiveQuestUIIcon(bool isAvailableQuest, bool isActiveQuest, SpriteBatch spriteBatch, NPC npc, Player player)
        {
            isAvailableQuest = Progress == 1;
            isActiveQuest = Progress == 2;
            base.IsActiveQuestUIIcon(isAvailableQuest, isActiveQuest, spriteBatch, npc, player);
        }
    }
}