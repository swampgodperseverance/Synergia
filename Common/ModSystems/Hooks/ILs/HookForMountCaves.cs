using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;

namespace Synergia.Common.ModSystems.Hooks.ILs {
    public class HookForMountCaves : ModSystem {
        ILHook ExtraMountCavesGeneratorILHook;

        public override void Load() {
            //LoadRoAHook();
        }
        void LoadRoAHook() {
            if (ModLoader.TryGetMod("RoA", out Mod RoAMod)) {
                Type DryadEntranceClass = RoAMod.GetType().Assembly.GetType("RoA.Content.World.Generations.DryadEntrance");

                MethodInfo ExtraMountCavesGeneratorInfo = DryadEntranceClass.GetMethod("ExtraMountCavesGenerator", BindingFlags.NonPublic | BindingFlags.Instance);
                ExtraMountCavesGeneratorILHook = new ILHook(ExtraMountCavesGeneratorInfo, ILExtraMountCavesGenerator);
            }
        }
        void ILExtraMountCavesGenerator(ILContext il) {
            var ilCursor = new ILCursor(il);
            ilCursor.GotoNext(i => i.MatchLdcI4(120));
            ilCursor.Remove();
            ilCursor.Emit(OpCodes.Ldc_I4, 230);
            ilCursor.GotoNext(i => i.MatchLdcI4(90));
            ilCursor.Remove();
            ilCursor.Emit(OpCodes.Ldc_I4, 200);
            ilCursor.GotoNext(i => i.MatchLdcI4(90));
            ilCursor.Remove();
            ilCursor.Emit(OpCodes.Ldc_I4, 200);
        }
        public override void Unload() {
            UnloadRoAHook();
        }
        void UnloadRoAHook() {
            ExtraMountCavesGeneratorILHook?.Dispose();
            ExtraMountCavesGeneratorILHook = null;
        }
    }
}
