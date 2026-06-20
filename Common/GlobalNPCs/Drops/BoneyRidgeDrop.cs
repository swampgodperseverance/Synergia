using Synergia.Content.Items.QuestItem;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.NPCs.Zombies;
using static Synergia.ModList;
using static Terraria.ModLoader.ModContent;
using Gargoyle = ValhallaMod.NPCs.Granite.Gargoyle;

namespace Synergia.Common.GlobalNPCs.Drops
{
    public class BoneyRidgeDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.BoneSerpentHead)
            {
                var postPlanteraCondition = new Conditions.DownedPlantera();

                npcLoot.Add(ItemDropRule.ByCondition(
                    postPlanteraCondition,
                    ModContent.ItemType<BoneyRidge>(),
                    20 
                ));
            }
        }
    }
}