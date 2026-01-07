using Mono.Cecil.Cil;
using MonoMod.Cil;
using Synergia.Common.ModSystems.Hooks.Ons;
using Terraria;

namespace Synergia.Common.ModSystems.Hooks.ILs {
    public class HookForFrostLegion : ModSystem {
        public override void Load() {
            On_Main.StartInvasion += HokForNewInvasion.NewInvasion;

            IL_Main.UpdateInvasion_Inner += GiveAchieveIfCompletedNewIvent;
            IL_Player.ItemCheck_UseEventItems += AddNewEvent;
            IL_NPC.checkDead += NoPointForFrostLegion;
        }
        void GiveAchieveIfCompletedNewIvent(ILContext il) {
            ILCursor c = new(il);
            c.GotoNext(i => i.MatchLdcI4(2));
            c.GotoNext(i => i.MatchLdcI4(2));
            c.Remove();
            c.EmitDelegate(() => DownedBossSystem.CompleteNewFrostEvent ? 1 : -1);
        }
        void AddNewEvent(ILContext il) {
            ILCursor c = new(il);
            c.GotoNext(i => i.MatchLdcI4(2));
            c.GotoNext(i => i.MatchLdcI4(2));
            c.Remove();
            c.Emit(OpCodes.Ldc_I4, 5);
        }
        void NoPointForFrostLegion(ILContext il) {
            ILCursor c = new(il);
            c.GotoNext(i => i.MatchLdcI4(387));
            c.GotoNext(i => i.MatchLdcI4(0));
            c.GotoNext(i => i.MatchLdcI4(0));
            c.GotoNext(i => i.MatchLdcI4(0));
            c.Remove();
            c.EmitDelegate(() => {
                if (Main.invasionType != 2) {
                    return 0;
                }
                return 1000;
            });
        }
    }
}