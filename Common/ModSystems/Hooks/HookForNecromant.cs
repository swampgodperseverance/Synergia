using Bismuth.Content.NPCs;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.ModSystems.Hooks
{
    public class HookForNecromant : ModSystem
    {
        private Hook newAiForNecromant;
        private FieldInfo currentPhaseField;
        #region Info
        /*
         * Вы правы, рефлексия в программировании действительно требует много времени и ресурсов, 
         * поскольку она связана с анализом и изменением программы во время ее выполнения. 
         * Это связано с тем, что во время рефлексии программа вынуждена динамически работать с метаданными, 
         * такими как информация о типах, свойствах и методах, вместо использования заранее скомпилированного кода, 
         * что приводит к увеличению накладных расходов.
        */
        #endregion
        public override void Load()
        {
            Type type = typeof(EvilNecromancer);

            // Не только хук ну и рефлексию что бы достать приватное поле
            currentPhaseField = type.GetField("currentphase", BindingFlags.NonPublic | BindingFlags.Instance);

            MethodInfo target = type.GetMethod(nameof(EvilNecromancer.AI), BindingFlags.Instance | BindingFlags.Public);
            newAiForNecromant = new Hook(target, (GetSetAI)NewNecromancerAI);
        }

        public override void Unload()
        {
            newAiForNecromant?.Dispose();
            newAiForNecromant = null;
            currentPhaseField = null;
        }

        private delegate void Orig_SetAI(EvilNecromancer npc);
        private delegate void GetSetAI(Orig_SetAI orig, EvilNecromancer npc);

        private void NewNecromancerAI(Orig_SetAI orig, EvilNecromancer npc)
        {
            orig(npc);

            if (!npc.NPC.active) return;
               
            int currentphase = (int)currentPhaseField.GetValue(npc);

            if (currentphase == 2)
            {
                Main.NewText("Некромант в фазе 2 — метание орбов!");
            }
            else if (currentphase == 3)
            {
                Main.NewText("Некромант в фазе 3 — призыв миньонов!");
            }
        }
    }
}