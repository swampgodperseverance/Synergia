using Terraria;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    public class HokForNewInvasion {
        public static void NewInvasion(On_Main.orig_StartInvasion orig, int type) {
            orig(type);
            if (type == 5) {
                GetInstance<EventManger>().Events.TryGetValue(nameof(FrostLegion), out ModEvent mod);
                mod.ActiveEvent();
                Main.invasionType = 2;
            }
        }
    }
}
