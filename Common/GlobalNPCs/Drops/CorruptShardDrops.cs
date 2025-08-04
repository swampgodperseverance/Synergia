using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Avalon.Items.Material.Shards;//if mod item
using static Terraria.ModLoader.ModContent;
using static Terraria.Localization.Language;

namespace Vanilla.Common.GlobalNPCs.Drops
{
public class CorruptShardDrops : GlobalNPC
{	private static Mod avalon = ModLoader.GetMod("Avalon");
public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
{

if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.OrbOfCorruption.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.OrbOfCorruption.DisplayName"))
//Big text after if ( if so, we change it on npc.type == NPCType<MODBOSSNAME>() if npc code not private
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("CorruptShard").Type,//Item that player will receive
chanceDenominator: 4,//drop chance 100/4=25
minimumDropped: 1,//min amount
maximumDropped: 2//max amount
//if u need exact amount, set identical	numbers
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.FleshSlime.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.FleshSlime.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("CorruptShard").Type,
chanceDenominator: 4,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.ShadowSlime.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.ShadowSlime.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("CorruptShard").Type,
chanceDenominator: 4,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.ShadowMummy.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.ShadowMummy.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("CorruptShard").Type,
chanceDenominator: 4,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.ShadowHammer.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.ShadowHammer.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("CorruptShard").Type,
chanceDenominator: 4,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.FleshMummy.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.FleshMummy.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("CorruptShard").Type,
chanceDenominator: 4,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.FleshAxe.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.FleshAxe.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("CorruptShard").Type,
chanceDenominator: 4,
minimumDropped: 1,
maximumDropped: 2
)
);
}
}
}
}
