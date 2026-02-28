using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalNPCs.AI
{
	public class GolemExtraAttacks : GlobalNPC
	{
		public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type >= NPCID.Golem && npc.type <= NPCID.GolemHeadFree;
		public override void SetDefaults(NPC npc) => npc.trapImmune = true;
		public override void AI(NPC npc) {
            if(NPC.golemBoss < 0) return;
			if(npc.type == NPCID.GolemHead && npc.whoAmI < NPC.golemBoss) npc.position += Main.npc[NPC.golemBoss].velocity;
			if(npc.type > NPCID.GolemHead && npc.type < NPCID.GolemHeadFree) npc.dontTakeDamage = Main.npc[NPC.golemBoss].dontTakeDamage;
			if(npc.type > NPCID.GolemHead && npc.type < NPCID.GolemHeadFree && npc.ai[0] == 0f && Main.npc[NPC.golemBoss].ai[0] == 0f && Main.npc[NPC.golemBoss].ai[1] < -20f) npc.ai[1] = 0f;
		}
	}
}
