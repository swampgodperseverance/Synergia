using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalNPCs.Changes {
    public abstract class BaseNPC : GlobalNPC {
        public abstract void EditNPC(NPC npc);
        public sealed override void SetDefaults(NPC entity) {
            EditNPC(entity);
        }
        protected static void EditNPC(NPC npc, int target, int defenseDelta = 0, int lifeDelta = 0) {
            if (npc.type == target) {
                npc.defense += defenseDelta;
                npc.lifeMax += lifeDelta;
                npc.life = npc.lifeMax;
            }
        }
    }
}