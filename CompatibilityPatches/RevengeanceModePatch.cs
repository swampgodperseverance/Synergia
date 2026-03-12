using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Common.GlobalNPCs.AI;

namespace Synergia.CompatibilityPatches
{
	public class RevengeanceModePatch : ModSystem
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CalamityMod");
		public override void PreUpdateNPCs() {
			bool mustDisable = ModLoader.TryGetMod("CalamityMod", out Mod Calamity) && ((bool)Calamity.Call("GetDifficultyActive", "Revengeance") || (bool)Calamity.Call("GetDifficultyActive", "Death") || (bool)Calamity.Call("GetDifficultyActive", "BossRush"));
			GolemExtraAttack.Disabled = mustDisable;
			PlanteraFlowerSpawner.Disabled = mustDisable;
			KingSlimeAI.Disabled = mustDisable;
			EyeAI.Disabled = mustDisable;
			SkeletronPrimeAI.Disabled = mustDisable;
			DestroyerAI.Disabled = mustDisable;
			BrainDashAI.Disabled = mustDisable;
			CultistAI.Disabled = mustDisable;
		}
		public override void Unload() {
			GolemExtraAttack.Disabled = false;
			PlanteraFlowerSpawner.Disabled = false;
			KingSlimeAI.Disabled = false;
			EyeAI.Disabled = false;
			SkeletronPrimeAI.Disabled = false;
			DestroyerAI.Disabled = false;
			BrainDashAI.Disabled = false;
			CultistAI.Disabled = false;
		}
	}
}