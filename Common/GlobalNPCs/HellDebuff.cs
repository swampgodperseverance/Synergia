using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Buffs;

namespace Synergia.Common.GlobalNPCs
{
    public class HellDebuffGlobalNPC : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.SkeletronHead)
            {
                NPC.downedBoss3 = true;
            }
        }
    }
}