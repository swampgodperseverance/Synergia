using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Avalon.Items.Material.Shards;//if mod item
using static Terraria.ModLoader.ModContent;
using static Terraria.Localization.Language;

namespace Synergia.Common.GlobalNPCs.Drops
{
public class BooksFragmentsDrops : GlobalNPC
{	private static Mod avalon = ModLoader.GetMod("Avalon");
public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
{

if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.DragonHornet.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.DragonHornet.DisplayName"))
//Big text after if ( if so, we change it on npc.type == NPCType<MODBOSSNAME>() if npc code not private
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("StrongVenom").Type,//Item that player will receive
chanceDenominator: 7,//drop chance 100/4=25
minimumDropped: 1,//min amount
maximumDropped: 1//max amount
//if u need exact amount, set identical	numbers
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.SkeletonBat.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.SkeletonBat.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("RubybeadHerb").Type,
chanceDenominator: 7,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.SkeletonBat.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.SkeletonBat.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("MysticalClaw").Type,
chanceDenominator: 7,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.RoA.NPCs.Fleder.DisplayName") || npc.FullName == GetTextValue("Mods.RoA.NPCs.Fleder.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("MysticalClaw").Type,
chanceDenominator: 7,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Gargoyle.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Gargoyle.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("MysticalClaw").Type,
chanceDenominator: 7,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.DemonicSpirit.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.DemonicSpirit.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("MysticalClaw").Type,
chanceDenominator: 7,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Valkyrie.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Valkyrie.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("MysticalClaw").Type,
chanceDenominator: 7,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.InfectedMan.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.InfectedMan.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("ElementDust").Type,
chanceDenominator: 7,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.FrozenNimbus.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.FrozenNimbus.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("ElementDust").Type,
chanceDenominator: 7,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.FrozenSoul.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.FrozenSoul.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("ElementDust").Type,
chanceDenominator: 7,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.GiantAlbinoCharger.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.GiantAlbinoCharger.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("ElementDust").Type,
chanceDenominator: 7,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.SpectralMummy.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.SpectralMummy.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("ElementDust").Type,
chanceDenominator: 7,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.FleshSlime.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.FleshSlime.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("DewofHerbs").Type,
chanceDenominator: 7,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.ShadowSlime.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.ShadowSlime.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("DewofHerbs").Type,
chanceDenominator: 7,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Sludger.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Sludger.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("DewofHerbs").Type,
chanceDenominator: 7,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.TinyToxicSludge.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.TinyToxicSludge.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("DewofHerbs").Type,
chanceDenominator: 7,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.VileDecomposer.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.VileDecomposer.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("DewOrb").Type,
chanceDenominator: 7,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.DragonSnatcher.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.DragonSnatcher.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("DewOrb").Type,
chanceDenominator: 7,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.ArchWyvernHead.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.ArchWyvernHead.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("MysticalTotem").Type,
chanceDenominator: 7,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.MythicalWyvernHead.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.ArchWyvernHead.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("MysticalTotem").Type,
chanceDenominator: 7,
minimumDropped: 1,
maximumDropped: 1
)
);
}
}
}
}
