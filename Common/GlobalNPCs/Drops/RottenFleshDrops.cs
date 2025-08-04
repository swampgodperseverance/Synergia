using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Avalon.Items.Material.Shards;//if mod item
using static Terraria.ModLoader.ModContent;
using static Terraria.Localization.Language;

namespace Vanilla.Common.GlobalNPCs.Drops
{
public class RottenFleshDrops : GlobalNPC
{	private static Mod avalon = ModLoader.GetMod("Avalon");
public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
{

if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieUnicorn.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieUnicorn.DisplayName"))
//Big text after if ( if so, we change it on npc.type == NPCType<MODBOSSNAME>() if npc code not private
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("RottenFlesh").Type,//Item that player will receive
chanceDenominator: 10,//drop chance 100/4=25
minimumDropped: 1,//min amount
maximumDropped: 2//max amount
//if u need exact amount, set identical	numbers
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieUmbrella3.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieUmbrella3.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("RottenFlesh").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieUmbrella2.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieUmbrella2.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("RottenFlesh").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieUmbrella.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieUmbrella.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("RottenFlesh").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieTactical.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieTactical.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("RottenFlesh").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieTactical2.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieTactical2.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("RottenFlesh").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieLibrarian.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieLibrarian.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("RottenFlesh").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieNinja.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieNinja.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("RottenFlesh").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieBucket.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieBucket.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("RottenFlesh").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieBalloon.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieBalloon.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("RottenFlesh").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieBalloon2.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieBalloon2.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("RottenFlesh").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieBalloon3.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieBalloon3.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("RottenFlesh").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.NewHorizons.NPCs.CarrotZombie.DisplayName") || npc.FullName == GetTextValue("Mods.NewHorizons.NPCs.CarrotZombie.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("RottenFlesh").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
}
}
}
