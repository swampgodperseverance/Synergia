using Bismuth.Content.NPCs;
using Terraria;
using Terraria.ModLoader;
using ValhallaMod.NPCs.TownNPCs;
using static Synergia.Common.SUtils.LocUtil;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.GlobalNPCs;

public class NewDialogForNpc : GlobalNPC
{
    public override void GetChat(NPC npc, ref string chat)
    {
        int DwarfInBismuth = NPC.FindFirstNPC(NPCType<DwarfBlacksmith>());
        int DwarfInValhаla = NPC.FindFirstNPC(NPCType<Dwarf>());

        if (npc.type == NPCType<Dwarf>()) 
        {
            if (Main.rand.NextBool(10)) {
                chat = LocKey(CategoryName.NPC, "Dialog.FrostDwarf");
            }
            if (Main.rand.NextBool(11) && DwarfInBismuth >= 0) {
                chat = string.Format(LocKey(CategoryName.NPC, "Dialog.FrostDwarf2"), Main.npc[DwarfInBismuth].GivenName);
            }
        }
        if (npc.type == NPCType<DwarfBlacksmith>()) 
        {
            if (Main.rand.NextBool(10)) { 
                chat = LocKey(CategoryName.NPC, "Dialog.CityDwarf");
            }
            if (Main.rand.NextBool(11) && DwarfInValhаla >= 0) { 
                chat = string.Format(LocKey(CategoryName.NPC, "Dialog.CityDwarf2"), Main.npc[DwarfInValhаla].GivenName);
            }
        }
    }
}