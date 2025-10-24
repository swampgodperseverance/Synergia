using Bismuth.Utilities.ModSupport;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.NPCs.TownNPCs;
using static Synergia.Common.QuestSystem.QuestConst;

namespace Synergia.Common
{
    public partial class QuestSystem
    {
        public class QuestDrawSystem : GlobalNPC
        {
            /// <summary> обязательно нужно указать npcType </summary>
            /// <param name="npcType"> Кому рисовать </param>
            /// <param name="name">Ключь квеста</param>
            public static void DrawQuestIcon(NPC npc, SpriteBatch spriteBatch, int npcType, string name)
            {
                Player player = Main.LocalPlayer;
                IEnumerable<IQuest> quests = QuestRegistry.GetAvailableQuests(player, name);
                foreach (var quest in quests)
                {
                    if (npc.type == npcType)
                    {
                        bool showAvailable = quest.IsAvailable(player) && !quest.IsActive(player);
                        bool showActive = quest.IsActive(player);
                        quest?.IsActiveQuestUIIcon(showAvailable, showActive, spriteBatch, npc, player);
                    }
                }
            }
            public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
            {
                DrawQuestIcon(npc, spriteBatch, ModContent.NPCType<Dwarf>(), DWARF);
                DrawQuestIcon(npc, spriteBatch, NPCID.TaxCollector, TAXC);
            }
        }
    }
}