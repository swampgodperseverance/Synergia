using Avalon.NPCs.TownNPCs;
using Bismuth.Utilities.ModSupport;
using NewHorizons.Content.NPCs.Town;
using StramsSurvival.NPCs;
using Synergia.Content.NPCs;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using ValhallaMod.NPCs.TownNPCs;
using static Synergia.ModList;

namespace Synergia.Common {
    public partial class QuestSystem {
        public class QuestDrawSystem : GlobalNPC {
            /// <summary> обязательно нужно указать npcType </summary>
            /// <param name="npcType"> Кому рисовать </param>
            /// <param name="name">Ключ квеста</param>
            public static void DrawQuestIcon(NPC npc, SpriteBatch spriteBatch, int npcType, string name) {
                Player player = Main.LocalPlayer;
                IEnumerable<IQuest> quests = QuestRegistry.GetAvailableQuests(player, name);
                foreach (IQuest quest in quests) {
                    if (npc.type == npcType) {
                        bool showAvailable = quest.IsAvailable(player) && !quest.IsActive(player);
                        bool showActive = quest.IsActive(player);
                        quest?.IsActiveQuestUIIcon(showAvailable, showActive, spriteBatch, npc, player);
                    }
                }
            }
            //public static void DrawQuestIcon(NPC npc, SpriteBatch spriteBatch) {
            //    BaseQuestLogic.AllQuest.TryGetValue(npc.type, out List<BaseQuestLogic> value2);
            //    Player player = Main.LocalPlayer;
            //    if (value2 != null) {
            //        if (value2[0] != null) {
            //            BaseQuestLogic value = value2[0];
            //            if (!value.IsEndQuest) {
            //                bool showAvailable = value.IsAvailable(player) && !value.IsActive(player);
            //                bool showActive = value.IsActive(player);
            //                value?.IsActiveQuestUIIcon(showAvailable, showActive, spriteBatch, npc, player);
            //            }
            //        }
            //        if (value2[1] != null) {
            //            BaseQuestLogic value3 = value2[1];
            //            if (!value3.IsEndQuest) {
            //                bool showAvailable = value3.IsAvailable(player) && !value3.IsActive(player);
            //                bool showActive = value3.IsActive(player);
            //                value3?.IsActiveQuestUIIcon(showAvailable, showActive, spriteBatch, npc, player);
            //            }
            //        }
            //    }
            //}
            public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
                DrawQuestIcon(npc, spriteBatch, ModContent.NPCType<Dwarf>(), DWARF);
                DrawQuestIcon(npc, spriteBatch, NPCID.TaxCollector, TAXC);
                DrawQuestIcon(npc, spriteBatch, Roa.Find<ModNPC>("Hunter").Type, HUNTER);
                DrawQuestIcon(npc, spriteBatch, ModContent.NPCType<Artist>(), ARTIST);
                DrawQuestIcon(npc, spriteBatch, ModContent.NPCType<Ninja>(), NINJA);
                DrawQuestIcon(npc, spriteBatch, ModContent.NPCType<Farmer>(), FARMER);
                DrawQuestIcon(npc, spriteBatch, ModContent.NPCType<Librarian>(), LIBRARIAN);
                DrawQuestIcon(npc, spriteBatch, ModContent.NPCType<HellDwarf>(), HELLDWARF);
            }
        }
    }
}