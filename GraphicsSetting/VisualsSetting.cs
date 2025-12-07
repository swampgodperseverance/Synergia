using Synergia.Content.NPCs;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace Synergia.GraphicsSetting;

public class VisualsSetting : ModSystem {
    public override void PostUpdateEverything() {
        Player player = Main.LocalPlayer;
        if (player == null || !player.active) { // <-- без этого порой игра зависала при запуске мира.
            return;
        }
        else {
            GetFilter(GetNpcType(ModContent.NPCType<Cogworm>()), SynegiyGraphics.COGWORMSHADER);
            base.PostUpdateEverything();
        }
    }
    static bool GetNpcType(int npcType) {
        foreach (NPC npc in Main.npc) {
            if (npc.active && npc.type == npcType) {
                return true;
            }
        }
        return false;
    }
    static void GetFilter(bool active, string name) {
        if (active) {
            if (!Filters.Scene[name].IsActive()) {
                Filters.Scene.Activate(name, default);
            }
        }
        else {
            if (Filters.Scene[name].IsActive()) {
                Filters.Scene.Deactivate(name);
            }
        }
    }
}