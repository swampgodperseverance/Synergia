using Avalon.Tiles.Ores;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using Terraria.ModLoader;

namespace Synergia.Common.ModSystems.Hooks.Ils {
    public class HookForOreThirdLevel : ModSystem {
        ILHook newMinPick;
        public override void Load() {
            MethodInfo methodInfo = typeof(TroxiniumOre).GetMethod(nameof(TroxiniumOre.SetStaticDefaults), BindingFlags.Public | BindingFlags.Instance);
            newMinPick = new ILHook(methodInfo, EditMinPickForTroxiniumOre);
        }
        void EditMinPickForTroxiniumOre(ILContext il) {
            try {
                ILCursor ilCursor = new(il);
                ilCursor.GotoNext(i => i.MatchLdcI4(150));
                ilCursor.Remove();
                ilCursor.Emit(OpCodes.Ldc_I4, 145);
            }
            catch (Exception e) {
                MonoModHooks.DumpIL(ModContent.GetInstance<Synergia>(), il);
                throw new ILPatchFailureException(ModContent.GetInstance<Synergia>(), il, e);
            }
        }
        public override void Unload() {
            newMinPick?.Dispose();
            newMinPick = null;
        }
    }
}