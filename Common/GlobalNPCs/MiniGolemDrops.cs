using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Avalon.Items.Weapons.Ranged.Hardmode;// если предмет выпадает модовый
using static Terraria.ModLoader.ModContent;
using static Terraria.Localization.Language;

namespace Vanilla.Common.GlobalNPCs
{
public class MiniGolemDrops : GlobalNPC
{
public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
{
if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.TempleGolem.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.TempleGolem.DisplayName"))
//Большой текст после if ( мы заменям на npc.type == NPCType<MODBOSSNAME>() если код босса не приватный
{
npcLoot.Add(
ItemDropRule.Common(
ModContent.ItemType<SunsShadow>(),//предмет который выпадает
chanceDenominator: 10,//шанс выпадения, 100 / 2 = 50%
minimumDropped: 1,//количество выпадения минимум
maximumDropped: 1//количество выпадения максимум
//если нужно чтобы выпадало четкое количество указываем одинаковые числа
)
);
}
}
}
}
