using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Avalon.Items.Material.Shards;//if mod item
using static Terraria.ModLoader.ModContent;
using static Terraria.Localization.Language;

namespace Vanilla.Common.GlobalNPCs.Drops
{
public class ToxinShard : GlobalNPC
{	private static Mod avalon = ModLoader.GetMod("Avalon");
	private static Mod horizons = ModLoader.GetMod("NewHorizons");
public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
{

if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.InfectedMan.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.InfectedMan.DisplayName"))
//Big text after if ( if so, we change it on npc.type == NPCType<MODBOSSNAME>() if npc code not private
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("ToxinShard").Type,//Item that player will receive
chanceDenominator: 10,//drop chance 100/4=25
minimumDropped: 1,//min amount
maximumDropped: 2//max amount
//if u need exact amount, set identical	numbers
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.BeePrincess.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.BeePrincess.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("ToxinShard").Type,
chanceDenominator: 4,
minimumDropped: 2,
maximumDropped: 6
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.DragonSnatcher.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.DragonSnatcher.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("ToxinShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.DragonHornet.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.DragonHornet.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("ToxinShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
}
}
}
