using Avalon.NPCs.TownNPCs;
using Bismuth.Utilities.ModSupport;
using NewHorizons.Content.NPCs.Town;
using Synergia.Content.NPCs;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using ValhallaMod.NPCs.TownNPCs;
using static Synergia.ModList;

namespace Synergia.Common {
    public partial class QuestSystem {
        public class QuestDrawSystem : GlobalNPC {
            public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == NPCType<Dwarf>() || npc.type == NPCID.TaxCollector || npc.type == Roa.Find<ModNPC>("Hunter").Type || npc.type == NPCType<Artist>() || npc.type == NPCType<Ninja>() || npc.type == NPCType<Librarian>() || npc.type == NPCType<HellDwarf>();
            public static void DrawQuestIcon(NPC npc, SpriteBatch spriteBatch, int npcType, string name) {
                if (npc.type != npcType) return;
                Player player = Main.LocalPlayer;
                IEnumerable<IQuest> quests = QuestRegistry.GetAvailableQuests(player, name);
                foreach (IQuest quest in quests) {
                    bool showAvailable = quest.IsAvailable(player) && !quest.IsActive(player);
                    bool showActive = quest.IsActive(player);
                    quest?.IsActiveQuestUIIcon(showAvailable, showActive, spriteBatch, npc, player);
                }
            }
            public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
                DrawQuestIcon(npc, spriteBatch, NPCType<Dwarf>(), DWARF);
                DrawQuestIcon(npc, spriteBatch, NPCID.TaxCollector, TAXC);
                DrawQuestIcon(npc, spriteBatch, Roa.Find<ModNPC>("Hunter").Type, HUNTER);
                DrawQuestIcon(npc, spriteBatch, NPCType<Artist>(), ARTIST);
                DrawQuestIcon(npc, spriteBatch, NPCType<Ninja>(), NINJA);
                DrawQuestIcon(npc, spriteBatch, NPCType<Librarian>(), LIBRARIAN);
                DrawQuestIcon(npc, spriteBatch, NPCType<HellDwarf>(), HELLDWARF);
            }
        }
    }
}
