using Bismuth.Content.Tiles;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    public class EditOrcishChest : ModSystem {
        Hook orchChest;

        delegate bool orig_OnRightClick(OrcishChest chest, int x, int y);
        delegate bool get_OnRightClicDetour(orig_OnRightClick orig, OrcishChest chest, int x, int y);

        public override void Load() {
            MethodInfo info = typeof(OrcishChest).GetMethod(nameof(OrcishChest.RightClick), BindingFlags.Public | BindingFlags.Instance);
            orchChest = new Hook(info, (get_OnRightClicDetour)Set_RightClic);
        }

        bool Set_RightClic(orig_OnRightClick orig, OrcishChest chest, int x, int y) {
            if (Biome.NewHell.LothorDeadSystem.LothorDead) { return orig(chest, x, y); }
            else {
                if (Main.netMode != NetmodeID.Server) { Main.NewText(Language.GetTextValue($"Mods.Synergia.UI.OrcishChestInfo"), 180, 18, 180); }
                else { ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(Language.GetTextValue($"Mods.Synergia.UI.OrcishChestInfo")), new Color(180, 18, 180)); }
                return false; 
            }
        }

        public override void Unload() {
            base.Unload();
        }
    }
}