using System.Collections.Generic;
using Terraria;

namespace Synergia.Common.GlobalPlayer {
    public class DebugPlayer : ModPlayer {
        public bool DebugMod;
        public List<string> DeveloperName { get; private set; } = ["1", "Aeris"];
        public override void PreUpdateBuffs() {
            if (DeveloperName.Contains(Player.name)) {
                DebugMod = true;
            }
        }
        public static void DebugText(object text) {
            Player player = Main.LocalPlayer;
            if (player.GetModPlayer<DebugPlayer>().DebugMod) {
                Main.NewText(text.ToString());
            }
        }
    }
}
