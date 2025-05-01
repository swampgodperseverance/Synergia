using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using static Terraria.ModLoader.ModContent;
using static Terraria.Localization.Language;

namespace Vanilla.Common.GlobalNPCs
{
	public class MiniGolemDrops : GlobalNPC
	{
		private static Mod avalon = ModLoader.GetMod("Avalon");
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.TempleGolem.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.TempleGolem.DisplayName"))
			{
				npcLoot.Add(
				ItemDropRule.Common(
				avalon.Find<ModItem>("SunsShadow").Type,
				chanceDenominator: 10,
				minimumDropped: 1,
				maximumDropped: 1
				));
			}
		}
	}
}