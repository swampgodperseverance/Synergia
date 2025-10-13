using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Avalon.Items.Material.Shards;
using Synergia.Content.Items.Weapons.Throwing;
using Synergia.Content.Items.Weapons.AuraStaff;
using Synergia.Content.Items.Accessories;
using static Terraria.ModLoader.ModContent;
using static Terraria.Localization.Language;

namespace Synergia.Common.GlobalNPCs.Drops
{
    public class MiscDrops : GlobalNPC
    {	private static Mod avalon = ModLoader.GetMod("Avalon");
	    private static Mod horizons = ModLoader.GetMod("NewHorizons");
        private static Mod valhalla = ModLoader.GetMod("ValhallaMod");
         public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.TypeName == GetTextValue("Mods.RoA.NPCs.Ent.DisplayName") ||
                npc.FullName == GetTextValue("Mods.RoA.NPCs.Ent.DisplayName"))
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<VerdurousStaff>(),
                    chanceDenominator: 10,
                    minimumDropped: 1,
                    maximumDropped: 1
                ));
            }

            if (npc.TypeName == GetTextValue("Mods.RoA.NPCs.Hog.DisplayName") ||
                npc.FullName == GetTextValue("Mods.RoA.NPCs.Hog.DisplayName"))
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<SuspiciousBag>(),
                    chanceDenominator: 9,
                    minimumDropped: 1,
                    maximumDropped: 1
                ));
            }

            if (npc.TypeName == GetTextValue("Mods.RoA.NPCs.Archdruid.DisplayName") ||
                npc.FullName == GetTextValue("Mods.RoA.NPCs.Archdruid.DisplayName"))
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<BrokenDice>(),
                    chanceDenominator: 9,
                    minimumDropped: 1,
                    maximumDropped: 1
                ));
            }

            if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.SpectralElemental.DisplayName") ||
                npc.FullName == GetTextValue("Mods.Consolaria.NPCs.SpectralElemental.DisplayName"))
            {
                npcLoot.Add(ItemDropRule.Common(
                    avalon.Find<ModItem>("ChaosDust").Type,
                    chanceDenominator: 9,
                    minimumDropped: 1,
                    maximumDropped: 1
                ));
            }
            if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.AlbinoAntlion.DisplayName") ||
                npc.FullName == GetTextValue("Mods.Consolaria.NPCs.AlbinoAntlion.DisplayName"))
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<SandAlbinoAuraStaff>(),
                    chanceDenominator: 15,
                    minimumDropped: 1,
                    maximumDropped: 1
                ));
            }

            if (npc.TypeName == GetTextValue("Mods.RoA.NPCs.SapSlime.DisplayName") ||
                npc.FullName == GetTextValue("Mods.RoA.NPCs.SapSlime.DisplayName"))
            {
                npcLoot.Add(ItemDropRule.Common(
                    valhalla.Find<ModItem>("Sap").Type,
                    chanceDenominator: 5,
                    minimumDropped: 1,
                    maximumDropped: 1
                ));
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ColdFather.DisplayName") ||
                npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ColdFather.DisplayName"))
            {
                npcLoot.Add(ItemDropRule.Common(
                    avalon.Find<ModItem>("SoulofIce").Type,
                    chanceDenominator: 1,
                    minimumDropped: 3,
                    maximumDropped: 6
                ));
            }
            if (npc.TypeName == GetTextValue("Mods.Avalon.NPCs.IrateBones.DisplayName") ||
                npc.FullName == GetTextValue("Mods.Avalon.NPCs.IrateBones.DisplayName"))
            {
                npcLoot.Add(ItemDropRule.Common(
                    horizons.Find<ModItem>("AncientScrap").Type,
                    chanceDenominator: 7,
                    minimumDropped: 1,
                    maximumDropped: 1
                ));
            }
            if (npc.TypeName == GetTextValue("Mods.Avalon.NPCs.Blaze.DisplayName") ||
                npc.FullName == GetTextValue("Mods.Avalon.NPCs.Blaze.DisplayName"))
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<Blazes>(),
                    chanceDenominator: 4,
                    minimumDropped: 1,
                    maximumDropped: 1
                ));
            }
            if (npc.type == NPCID.Mothron)
            {
                npcLoot.Add(ItemDropRule.Common(
                    valhalla.Find<ModItem>("BrokenGlaive").Type,
                    chanceDenominator: 4, 
                    minimumDropped: 1,
                    maximumDropped: 1
                ));
            }
            if (npc.type == NPCID.Mothron)
            {
                npcLoot.Add(ItemDropRule.Common(
                    valhalla.Find<ModItem>("BrokenSpear").Type,
                    chanceDenominator: 4, 
                    minimumDropped: 1,
                    maximumDropped: 1
                ));
            }
        }
    }
}
