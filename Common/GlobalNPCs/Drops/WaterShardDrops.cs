using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Avalon.Items.Material.Shards;//if mod item
using static Terraria.ModLoader.ModContent;
using static Terraria.Localization.Language;

namespace Vanilla.Common.GlobalNPCs.Drops
{
public class WaterShardDrops : GlobalNPC
{	private static Mod avalon = ModLoader.GetMod("Avalon");
public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
{
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.Orca.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.Orca.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("WaterShard").Type,
chanceDenominator: 4,
minimumDropped: 1,
maximumDropped: 2
)
);
}
}
}
}
