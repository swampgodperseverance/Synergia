using Avalon.NPCs.Bosses.Hardmode;
using Avalon.NPCs.Bosses.PreHardmode;
using Avalon.NPCs.Hardmode;
using Synergia.Common.GlobalNPCs.Changes;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.AvalonsChanges {
    public partial class Avalons {
        public class Avalon_NPC : BaseNPC {
            public override void EditNPC(NPC npc) {
                EditNPC(npc, NPCType<BacteriumPrime>(), 2, 750);
                EditNPC(npc, NPCType<Blaze>(), 5, 8050);
                EditNPC(npc, NPCType<DesertBeak>(), 5, 950);
                EditNPC(npc, NPCType<Phantasm>(), 18, 180000);
            }
        }
    }
}