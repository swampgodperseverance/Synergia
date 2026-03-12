using Terraria.ModLoader;
using Synergia.Common.GlobalNPCs.AI;

namespace Synergia.CompatibilityPatches
{
	[ExtendsFromMod("PrimeRework")]
	[JITWhenModsEnabled("PrimeRework")]
	public class MechReworksPatch : ILoadable
	{
		public void Load(Mod mod) {
			var c = ModContent.GetInstance<PrimeRework.ReworkConfig>();
			DestroyerAI.Disabled = c.destroyer;
			SkeletronPrimeAI.Disabled = c.prime;
		}
		public void Unload() {
			DestroyerAI.Disabled = false;
			SkeletronPrimeAI.Disabled = false;
		}
	}
}