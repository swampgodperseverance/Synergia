using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Avalon.Items.Material.Shards;//if mod item
using static Terraria.ModLoader.ModContent;
using static Terraria.Localization.Language;

namespace Synergia.Common.GlobalNPCs.Drops
{
public class FrostShard : GlobalNPC
{	private static Mod avalon = ModLoader.GetMod("Avalon");
	private static Mod horizons = ModLoader.GetMod("NewHorizons");
public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
{

if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Draug.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Draug.DisplayName"))
//Big text after if ( if so, we change it on npc.type == NPCType<MODBOSSNAME>() if npc code not private
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("FrostShard").Type,//Item that player will receive
chanceDenominator: 10,//drop chance 100/4=25
minimumDropped: 1,//min amount
maximumDropped: 2//max amount
//if u need exact amount, set identical	numbers
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.DraugAtArms.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.DraugAtArms.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("FrostShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.DraugAtArmsAlt.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.DraugAtArmsAlt.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("FrostShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.DraugSpearman.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.DraugSpearman.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("FrostShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.FrozenEye.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.FrozenEye.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("FrostShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.FrozenSoul.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.FrozenSoul.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("FrostShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ColdFather.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ColdFather.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("SoulofIce").Type,
chanceDenominator: 1,
minimumDropped: 3,
maximumDropped: 8
)
);
}
}
}
}
