using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Avalon.Items.Accessories.Hardmode;
using Terraria.Localization;
using Avalon.Items.Material.Shards;//if mod item
using static Terraria.ModLoader.ModContent;
using static Terraria.Localization.Language;

namespace Vanilla.Common.GlobalNPCs.Drops
{
public class ValhallaAccessories : GlobalNPC
{	private static Mod valhalla = ModLoader.GetMod("ValhallaMod");
public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
{

if (npc.TypeName == GetTextValue("Mods.RoA.NPCs.DeerSkull.DisplayName") || npc.FullName == GetTextValue("Mods.RoA.NPCs.DeerSkull.DisplayName"))
//Big text after if ( if so, we change it on npc.type == NPCType<MODBOSSNAME>() if npc code not private
{
npcLoot.Add(
ItemDropRule.Common(	
valhalla.Find<ModItem>("NecroBuckler").Type,//Item that player will receive
chanceDenominator: 10,//drop chance 100/4=25
minimumDropped: 1,//min amount
maximumDropped: 1//max amount
//if u need exact amount, set identical	numbers
)
);
}
if (npc.TypeName == GetTextValue("Mods.Avalon.NPCs.BoneFish.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.BoneFish.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
valhalla.Find<ModItem>("NecroBuckler").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.DragonSkull.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.DragonSkull.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
valhalla.Find<ModItem>("NecroBuckler").Type,
chanceDenominator: 10,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.SpectralElemental.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.SpectralElemental.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
valhalla.Find<ModItem>("ChaosFlicker").Type,
chanceDenominator: 25,
minimumDropped: 1,
maximumDropped: 1
)
);
}
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.LihzahrdTrickster.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.LihzahrdTrickster.DisplayName"))
{
npcLoot.Add(
ItemDropRule.Common(	
valhalla.Find<ModItem>("ChaosFlicker").Type,
chanceDenominator: 50,
minimumDropped: 1,
maximumDropped: 1
)
);
}
}
}
}
