using Terraria.ModLoader;
using Synergia.Common.GlobalNPCs.AI;

namespace Synergia.CompatibilityPatches
{
	[ExtendsFromMod("EvilBossesRework")]
	[JITWhenModsEnabled("EvilBossesRework")]
	public class EvilReworksPatch : ILoadable
	{
		public void Load(Mod mod) {
			var c = ModContent.GetInstance<EvilBossesRework.ReworkConfig>();
			BrainDashAI.Disabled = c.boc;
			BacteriumPrimeAI.Disabled = c.bac;
		}
		public void Unload() {
			BrainDashAI.Disabled = false;
			BacteriumPrimeAI.Disabled = false;
		}
	}
}
