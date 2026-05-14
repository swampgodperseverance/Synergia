using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.ModSystems
{
	public class ProgressionTweaks : GlobalNPC
	{
		public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == 379 || npc.type == 380 || npc.type == 437 || npc.type == 438;
		public override void OnSpawn(NPC npc, Terraria.DataStructures.IEntitySource source) {
			//despawn cultist if Sinlord and Ocram are not yet defeated
			if(SynergiaWorld.SinlordDead && Consolaria.Common.ModSystems.DownedBossSystem.downedOcram) return;
			npc.active = false;
			npc.netUpdate = true;
		}
	}
}