using Terraria;
using Terraria.ModLoader;

namespace Vanilla.Common.GlobalNPCs
{
    public class UnderwaterAuraGlobalNPC : GlobalNPC
    {
        public int AuraTimer;

        public override bool InstancePerEntity => true;

        public override void ResetEffects(NPC npc)
        {
            AuraTimer = 0;
        }
    }
}
