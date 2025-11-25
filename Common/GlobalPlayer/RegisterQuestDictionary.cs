using Synergia.Common.ModSystems.Hooks;
using Synergia.Dataset;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.NPCs.TownNPCs;
using static Synergia.Common.QuestSystem.QuestConst;

namespace Synergia.Common.GlobalPlayer
{
    public partial class QuestSystem
    {
        public class RegisterQuestDictionary : ModPlayer
        {
            public override void OnEnterWorld()
            {
                if (!HookForQuest.NpcQuestKeys.ContainsKey(ModContent.NPCType<Dwarf>())) {
                    HookForQuest.NpcQuestKeys[ModContent.NPCType<Dwarf>()] = new QuestData(DwafPositionX, DWARF, 0, 1, true);
                }
                if (!HookForQuest.NpcQuestKeys.ContainsKey(NPCID.TaxCollector)) {
                    HookForQuest.NpcQuestKeys[NPCID.TaxCollector] = new QuestData(BasePositionX, TAXC, 0, 1, true);
                }
            }
        }
    }
}