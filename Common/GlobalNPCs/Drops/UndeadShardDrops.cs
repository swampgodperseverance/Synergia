using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Avalon.Items.Material.Shards;//if mod item
using static Terraria.ModLoader.ModContent;
using static Terraria.Localization.Language;

namespace Vanilla.Common.GlobalNPCs.Drops
{
public class UndeadShardDrops : GlobalNPC
{	private static Mod avalon = ModLoader.GetMod("Avalon");
public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
{

if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.RadiantBones4.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.RadiantBones4.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("UndeadShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.RadiantBones3.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.RadiantBones3.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("UndeadShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.RadiantBones2.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.RadiantBones2.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("UndeadShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.RadiantBones.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.RadiantBones.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("UndeadShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Radiator2.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Radiator2.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("UndeadShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Radiator.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Radiator.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("UndeadShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.CobaltSkeleton.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.CobaltSkeleton.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("UndeadShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.RadiantBall.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.RadiantBall.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("UndeadShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ChargingSpearbones.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ChargingSpearbones.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("UndeadShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.CenturionShieldless.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.CenturionShieldless.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("UndeadShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Centurion2Shieldless.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Centurion2Shieldless.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("UndeadShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Centurion.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Centurion.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("UndeadShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Centurion2.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Centurion2.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("UndeadShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Geomancer.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Geomancer.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("UndeadShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Geomancer.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Geomancer.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("Peridot").Type,
chanceDenominator: 8,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Geomancer.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Geomancer.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("Zircon").Type,
chanceDenominator: 8,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Geomancer.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Geomancer.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("Tourmaline").Type,
chanceDenominator: 8,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ShaftSentinel.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ShaftSentinel.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("UndeadShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.SkeletonTrapper.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.SkeletonTrapper.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("UndeadShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.DragonSkull.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.DragonSkull.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("UndeadShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.VampireMiner.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.VampireMiner.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("UndeadShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
if (npc.TypeName == GetTextValue("Mods.RoA.NPCs.Archdruid.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.Archdruid.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
avalon.Find<ModItem>("UndeadShard").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 2
)
);
}
}
}
}
