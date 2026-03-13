using System;
using System.Reflection;
using Bismuth;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using Terraria.ModLoader;

namespace YourModName // замени на имя твоего мода
{
    public class ValhallaBossChecklistFix : ModSystem
    {
        private ILHook ilHook;

        public override void Load()
        {
            Mod valhalla;
            if (!ModLoader.TryGetMod("ValhallaMod", out valhalla))
                return;

            var integrationType = valhalla.Code.GetType("ValhallaMod.Systems.ModIntegrationsSystem");
            if (integrationType == null)
                return;

            var method = integrationType.GetMethod("DoBossChecklistIntegration",
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null)
                return;

            ilHook = new ILHook(method, EditBossWeights);
        }

        public override void Unload()
        {
            ilHook?.Dispose();
            ilHook = null;
        }

        private void EditBossWeights(ILContext il)
        {
            var c = new ILCursor(il);

            // old emperor's weight
            while (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(8.96f)))
            {
                c.Remove();                // removing the old one
                c.EmitLdcR4(9.5f);         // new
            }
        }
    }
    public class BusmuthBossChecklistFix : ModSystem
    {
        private ILHook ilHook;

        public override void Load()
        {
            Mod bismuth;
            if (!ModLoader.TryGetMod("Bismuth", out bismuth))
                return;

            var integrationType = bismuth.Code.GetType("Bismuth.Utilities.BossChecklistSupport");
            if (integrationType == null)
                return;

            var method = integrationType.GetMethod("DoBossChecklistIntegration",
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null)
                return;

            ilHook = new ILHook(method, EditBossWeights);
        }

        public override void Unload()
        {
            ilHook?.Dispose();
            ilHook = null;
        }

        private void EditBossWeights(ILContext il)
        {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(MoveType.Before, i => i.MatchLdstr("RhinoOrcBoss")))
            {
                Mod.Logger?.Warn("Bismuth hook: не нашли ldstr \"RhinoOrcBoss\"");
                return;
            }

            int skips = 0;
            bool found = false;
            float oldValue = 0f;

            while (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(out oldValue)))
            {
                skips++;
                if (skips == 1)   
                {
                    c.Remove();
                    c.Emit(OpCodes.Ldc_R4, 6.7f);
                    found = true;
                    break;
                }
            }

            if (found)
            {
                Mod.Logger.Info($"Bismuth: заменили weight RhinoOrcBoss с {oldValue} → 6.7 (после {skips} ldc.r4)");
            }
            else
            {
                Mod.Logger.Warn("Bismuth: нашли имя, но не нашли ни одного ldc.r4 после него");
            }
        }

    }
    public class AvalonBossChecklistFix : ModSystem
    {
        private ILHook ilHook;

        public override void Load()
        {
            Mod avalon;
            if (!ModLoader.TryGetMod("Avalon", out avalon))
                return;

            var integrationType = avalon.Code.GetType("Avalon.ModSupport.BossChecklistSystem");
            if (integrationType == null)
                return;

            var method = integrationType.GetMethod("DoBossChecklistIntegration",
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null)
                return;

            ilHook = new ILHook(method, EditBossWeights);
        }

        public override void Unload()
        {
            ilHook?.Dispose();
            ilHook = null;
        }

        private void EditBossWeights(ILContext il)
        {
            var c = new ILCursor(il);

           
            while (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(6f)))
            {
                c.Remove();                // remove the old one
                c.EmitLdcR4(6.6f);         // New weight
            }

        }

    }
    public class ConsBossChecklistFix : ModSystem
    {
        private ILHook ilHook;

        public override void Load()
        {
            Mod consolaria;
            if (!ModLoader.TryGetMod("Consolaria", out consolaria))
                return;

            var integrationType = consolaria.Code.GetType("Consolaria.Common.CrossContentIntegration");
            if (integrationType == null)
                return;

            var method = integrationType.GetMethod("DoBossChecklistIntegration",
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null)
                return;

            ilHook = new ILHook(method, EditBossWeights);
        }

        public override void Unload()
        {
            ilHook?.Dispose();
            ilHook = null;
        }

        private void EditBossWeights(ILContext il)
        {
            var c = new ILCursor(il);

            while (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(13f)))
            {
                c.Remove();            
                c.EmitLdcR4(13.8f);         
            }

        }

    }
}