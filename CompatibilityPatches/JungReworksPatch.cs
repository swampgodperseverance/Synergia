using Terraria.ModLoader;
using Synergia.Common.GlobalNPCs.AI;

namespace Synergia.CompatibilityPatches
{
	[ExtendsFromMod("GolemRework")]
	[JITWhenModsEnabled("GolemRework")]
	public class JungReworksPatch : ILoadable
	{
		public void Load(Mod mod) {
			var c = ModContent.GetInstance<GolemRework.ReworkConfig>();
			PlanteraFlowerSpawner.Disabled = c.plantera;
			GolemExtraAttack.Disabled = c.golem;
		}
		public void Unload() {
			PlanteraFlowerSpawner.Disabled = false;
			GolemExtraAttack.Disabled = false;
		}
	}
}