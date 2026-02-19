using Synergia.Common.GlobalItems;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    public class HookForEditHookItems : ModSystem {
        readonly HashSet<int> VanillaGrapplingHooks = [
            ItemID.AmethystHook, ItemID.Hook, ItemID.AmberHook, ItemID.AncientHallowedHood, ItemID.AntiGravityHook, ItemID.BatHook, ItemID.CandyCaneHook, ItemID.ChristmasHook,
            ItemID.DiamondHook, ItemID.DualHook, ItemID.EmeraldHook, ItemID.FishHook, ItemID.GrapplingHook, ItemID.HotlineFishingHook, ItemID.IlluminantHook, ItemID.LavaFishingHook,
            ItemID.LunarHook, ItemID.QueenSlimeHook, ItemID.RubyHook, ItemID.SapphireHook, ItemID.SlimeHook, ItemID.SpookyHook, ItemID.SquirrelHook, ItemID.StaticHook, ItemID.TendonHook,
            ItemID.ThornHook, ItemID.TopazHook, ItemID.WormHook
        ];
        public override void Load() {
            On_Item.CloneDefaults += On_Item_CloneDefaults;
        }
        void On_Item_CloneDefaults(On_Item.orig_CloneDefaults orig, Item self, int TypeToClone) {
            if (VanillaGrapplingHooks.Contains(TypeToClone)) {
                self.GetGlobalItem<SynergiaGI>().isGrapplingHooks = true;
                orig(self, TypeToClone);
            }
            else { orig(self, TypeToClone); }
        }
        public override void Unload() {
            On_Item.CloneDefaults -= On_Item_CloneDefaults;
        }
    }
}