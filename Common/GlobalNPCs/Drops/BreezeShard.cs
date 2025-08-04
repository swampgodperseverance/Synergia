using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Avalon.Items.Material.Shards;//if mod item
using static Terraria.ModLoader.ModContent;
using static Terraria.Localization.Language;

namespace Vanilla.Common.GlobalNPCs.Drops
{
public class BreezeShard : GlobalNPC
{	private static Mod avalon = ModLoader.GetMod("Avalon");
	private static Mod horizons = ModLoader.GetMod("NewHorizons");
public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
{

if (npc.TypeName == GetTextValue("Mods.RoA.NPCs.BackwoodsRaven.DisplayName") || npc.FullName == GetTextValue("Mods.RoA.NPCs.BackwoodsRaven.DisplayName"))
//Big text after if ( if so, we change it on npc.type == NPCType<MODBOSSNAME>() if npc code not private
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("BreezeShard").Type,//Item that player will receive
chanceDenominator: 10,//drop chance 100/4=25
minimumDropped: 1,//min amount
maximumDropped: 2//max amount
//if u need exact amount, set identical	numbers
)
);
}
if (npc.TypeName == GetTextValue("Mods.RoA.NPCs.Fleder.DisplayName") || npc.FullName == GetTextValue("Mods.RoA.NPCs.Fleder.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("BreezeShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.ArchWyvernHead.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.ArchWyvernHead.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("BreezeShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.ArchWyvernHead.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.ArchWyvernHead.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
horizons.Find<ModItem>("WyvernFur").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.MythicalWyvernHead.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.ArchWyvernHead.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("BreezeShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.MythicalWyvernHead.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.MythicalWyvernHead.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
horizons.Find<ModItem>("WyvernFur").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.SkeletonBat.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.SkeletonBat.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("BreezeShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Gargoyle.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Gargoyle.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("BreezeShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Valkyrie.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Valkyrie.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("BreezeShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.FrozenNimbus.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.FrozenNimbus.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("BreezeShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
}
}
}
