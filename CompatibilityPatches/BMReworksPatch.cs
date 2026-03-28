using Terraria.ModLoader;
using Synergia.Common.GlobalNPCs.AI;

namespace Synergia.CompatibilityPatches
{
	[ExtendsFromMod("BloodMoonEnemiesRework")]
	[JITWhenModsEnabled("BloodMoonEnemiesRework")]
	public class BMReworksPatch : ILoadable
	{
		public void Load(Mod mod) => BloodEel.Disabled = ModContent.GetInstance<BloodMoonEnemiesRework.ReworkConfig>().eel;
		public void Unload() => BloodEel.Disabled = false;
	}
}
