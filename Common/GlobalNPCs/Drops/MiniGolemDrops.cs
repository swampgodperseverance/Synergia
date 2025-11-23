using Avalon.Items.Weapons.Ranged.Hardmode.SunsShadow;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using static Terraria.Localization.Language;

namespace Synergia.Common.GlobalNPCs.Drops {
    public class MiniGolemDrops : GlobalNPC {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.TempleGolem.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.TempleGolem.DisplayName"))
            //Большой текст после if ( мы заменям на npc.type == NPCType<MODBOSSNAME>() если код босса не приватный
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunsShadow>(), 10, 1, 1));
            }
        }
    }
}