// Code by 𝒜𝑒𝓇𝒾𝓈
using System.Collections.Generic;
using Terraria;

namespace Synergia.Common.GlobalItems {
    public class SynergiaGI : GlobalItem {
        public bool isGrapplingHooks;

        public override bool InstancePerEntity => true;
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (isGrapplingHooks) {
                tooltips.Add(new TooltipLine(Mod, "hook", "Hooks"));
            }
        }
    }
}
