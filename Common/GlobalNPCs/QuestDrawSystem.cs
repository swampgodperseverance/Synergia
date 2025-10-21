using Bismuth.Utilities.ModSupport;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common
{
    public partial class QuestSystem
    {
        public class QuestDrawSystem : GlobalNPC
        {
            public static void DrawQuestIcon(NPC npc, SpriteBatch spriteBatch, string name)
            {
                Player player = Main.LocalPlayer;
                IEnumerable<IQuest> quests = QuestRegistry.GetAvailableQuests(player, name);
                foreach (var quest in quests)
                {
                    bool showAvailable = quest.IsAvailable(player) && !quest.IsActive(player);
                    bool showActive = quest.IsActive(player);
                    quest?.IsActiveQuestUIIcon(showAvailable, showActive, spriteBatch, npc, player);
                }
            }
        }
    }
}