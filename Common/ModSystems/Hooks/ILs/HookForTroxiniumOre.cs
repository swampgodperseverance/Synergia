using Avalon.Tiles.Ores;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Terraria.ModLoader;

namespace Synergia.Common.ModSystems.Hooks.ILs {
    public class HookForOreThirdLevel : ModSystem {
        ILHook newMinPick;
        public override void Load() {
            MethodInfo methodInfo = typeof(TroxiniumOre).GetMethod(nameof(TroxiniumOre.SetStaticDefaults), BindingFlags.Public | BindingFlags.Instance);
            newMinPick = new ILHook(methodInfo, EditMinPickForTroxiniumOre);
        }
        void EditMinPickForTroxiniumOre(ILContext il) {
            ILCursor ilCursor = new(il);
            ilCursor.GotoNext(i => i.MatchLdcI4(150));
            ilCursor.Remove();
            ilCursor.Emit(OpCodes.Ldc_I4, 145);
        }
        public override void Unload() {
            newMinPick?.Dispose();
            newMinPick = null;
        }
    }
}