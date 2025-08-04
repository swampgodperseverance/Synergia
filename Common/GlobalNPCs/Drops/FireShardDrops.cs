using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Avalon.Items.Material.Shards;//if mod item
using static Terraria.ModLoader.ModContent;
using static Terraria.Localization.Language;

namespace Vanilla.Common.GlobalNPCs.Drops
{
public class FireShardDrops : GlobalNPC
{	private static Mod avalon = ModLoader.GetMod("Avalon");
public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
{

if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Cacodemon.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Cacodemon.DisplayName"))
//Big text after if ( if so, we change it on npc.type == NPCType<MODBOSSNAME>() if npc code not private
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("FireShard").Type,//Item that player will receive
chanceDenominator: 2,//drop chance 100/4=25
minimumDropped: 2,//min amount
maximumDropped: 4//max amount
//if u need exact amount, set identical	numbers
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.ArchDemon.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.ArchDemon.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("FireShard").Type,
chanceDenominator: 4,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ArchImp.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ArchImp.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("FireShard").Type,
chanceDenominator: 4,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Heater.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Heater.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("FireShard").Type,
chanceDenominator: 4,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Ifrit.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Ifrit.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("FireShard").Type,
chanceDenominator: 2,
minimumDropped: 2,
maximumDropped: 4
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.HeaterWinged.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.HeaterWinged.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("FireShard").Type,
chanceDenominator: 4,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.SinSerpantHead.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.SinSerpantHead.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("FireShard").Type,
chanceDenominator: 4,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.TRAEUnderworld.NPCs.BeholderNPC.DisplayName") || npc.FullName == GetTextValue("Mods.TRAEUnderworld.NPCs.BeholderNPC.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("FireShard").Type,
chanceDenominator: 1,
minimumDropped: 3,
maximumDropped: 7
)
);
}
if (npc.TypeName == GetTextValue("Mods.TRAEUnderworld.NPCs.LittleBoomxie.DisplayName") || npc.FullName == GetTextValue("Mods.TRAEUnderworld.NPCs.LittleBoomxie.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("FireShard").Type,
chanceDenominator: 4,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.TRAEUnderworld.NPCs.Froggabomba.DisplayName") || npc.FullName == GetTextValue("Mods.TRAEUnderworld.NPCs.Froggabomba.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("FireShard").Type,
chanceDenominator: 4,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.TRAEUnderworld.NPCs.LavamanderNPC.DisplayName") || npc.FullName == GetTextValue("Mods.TRAEUnderworld.NPCs.LavamanderNPC.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("FireShard").Type,
chanceDenominator: 4,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.TRAEUnderworld.NPCs.OniRoninNPC.DisplayName") || npc.FullName == GetTextValue("Mods.TRAEUnderworld.NPCs.OniRoninNPC.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("FireShard").Type,
chanceDenominator: 2,
minimumDropped: 2,
maximumDropped: 4
)
);
}
if (npc.TypeName == GetTextValue("Mods.TRAEUnderworld.NPCs.PhoenixNPC.DisplayName") || npc.FullName == GetTextValue("Mods.TRAEUnderworld.NPCs.PhoenixNPC.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("FireShard").Type,
chanceDenominator: 4,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.TRAEUnderworld.NPCs.SalalavaNPC.DisplayName") || npc.FullName == GetTextValue("Mods.TRAEUnderworld.NPCs.SalalavaNPC.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("FireShard").Type,
chanceDenominator: 2,
minimumDropped: 1,
maximumDropped: 2
)
);
}
}
}
}
