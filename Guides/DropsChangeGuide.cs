using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Avalon.Items.Material.Shards;//drops for npcs
using Synergia.Content.Items.Weapons.Throwing;//drops for npcs
using Synergia.Content.Items.Weapons.AuraStaff;//drops for npcs
using Synergia.Content.Items.Accessories;
using static Terraria.ModLoader.ModContent;
using static Terraria.Localization.Language;

namespace Synergia.Common.GlobalNPCs.Drops
{
    public class MiscDrops : GlobalNPC
    {	private static Mod avalon = ModLoader.GetMod("Avalon");//mods for npcs
	    private static Mod horizons = ModLoader.GetMod("NewHorizons");//mods for npcs
        private static Mod valhalla = ModLoader.GetMod("ValhallaMod");//mods for npcs
         public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.TypeName == GetTextValue("Mods.RoA.NPCs.Ent.DisplayName") || // here you should write down npc internal name which loot u wanna modificate
                npc.FullName == GetTextValue("Mods.RoA.NPCs.Ent.DisplayName"))
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<VerdurousStaff>(), //modded item from YOUR mod
                    chanceDenominator: 10, // 100/10 = 10% 
                    minimumDropped: 1,
                    maximumDropped: 1
                ));
            }

            if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.SpectralElemental.DisplayName") ||
                npc.FullName == GetTextValue("Mods.Consolaria.NPCs.SpectralElemental.DisplayName"))
            {
                npcLoot.Add(ItemDropRule.Common(
                    avalon.Find<ModItem>("ChaosDust").Type, // loot from other mod
                    chanceDenominator: 9,
                    minimumDropped: 1,
                    maximumDropped: 1
                ));
            }
        }
    }
}
