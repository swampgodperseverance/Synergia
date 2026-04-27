using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace Synergia.Common.ModSystems.Hooks.ILs {
    public class BossCheakListHook : ModSystem {
        public ILHook iLHook;

        public override bool IsLoadingEnabled(Mod mod) => ModLoader.TryGetMod("BossChecklist", out Mod _) is not false;
        public override void Load() {
            MethodInfo EditBossCheckList = typeof(Avalon.ModSupport.BossChecklistSystem).GetMethod(nameof(Avalon.ModSupport.BossChecklistSystem.PostSetupContent));
            iLHook = new ILHook(EditBossCheckList, ILEditBossCheckList);
        }
        void ILEditBossCheckList(ILContext il) {
            ILCursor c = new(il);
            c.GotoNext(i => i.MatchLdcR4(6f));
            c.Remove();
            c.Emit(OpCodes.Ldc_R4, 6.6f);
        }
    }
}