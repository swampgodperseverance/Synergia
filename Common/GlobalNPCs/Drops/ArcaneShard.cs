using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Avalon.Items.Material.Shards;//if mod item
using static Terraria.ModLoader.ModContent;
using static Terraria.Localization.Language;

namespace Vanilla.Common.GlobalNPCs.Drops
{
public class ArcaneShard : GlobalNPC
{	private static Mod avalon = ModLoader.GetMod("Avalon");
	private static Mod horizons = ModLoader.GetMod("NewHorizons");
public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
{

if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.Spectropod.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.Spectropod.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("ArcaneShard").Type,
chanceDenominator: 8,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.SpectralMummy.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.SpectralMummy.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("ArcaneShard").Type,
chanceDenominator: 8,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.SpectralElemental.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.SpectralElemental.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("ArcaneShard").Type,
chanceDenominator: 8,
minimumDropped: 1,
maximumDropped: 1
)
);
}
}
}
}
