using Bismuth.Utilities.ModSupport;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using Synergia.Common;
using Synergia.Content.Quests;
using System;
using System.Reflection;
using Terraria.Audio;
using Terraria.ModLoader;

namespace Synergia
{
    public class Synergia : Mod
    {
        private Synergia instruktion; 
        private ILHook ExtraMountCavesGeneratorILHook;
        
        public override void Load()
        {
            QuestRegistry.Register(new DwarfQuest());
            instruktion = this;
            LoadRoAHook();
        }

        public override void Unload()
        {
            UnloadRoAHook();
        }
        
        public void LoadRoAHook()
        {
            if (ModLoader.TryGetMod("RoA", out Mod RoAMod))
            {
                Type DryadEntranceClass = RoAMod.GetType().Assembly.GetType("RoA.Content.World.Generations.DryadEntrance");

                MethodInfo ExtraMountCavesGeneratorInfo = DryadEntranceClass.GetMethod("ExtraMountCavesGenerator", BindingFlags.NonPublic | BindingFlags.Instance);
                ExtraMountCavesGeneratorILHook = new ILHook(ExtraMountCavesGeneratorInfo, ILExtraMountCavesGenerator);
            }
        }

        public void UnloadRoAHook()
        {
            if (ExtraMountCavesGeneratorILHook != null)
            {
                ExtraMountCavesGeneratorILHook.Dispose();
                ExtraMountCavesGeneratorILHook = null;
            }
        }

        private void ILExtraMountCavesGenerator(ILContext il)
        {
            try
            {
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
            catch (Exception e)
            {
                MonoModHooks.DumpIL(ModContent.GetInstance<Synergia>(), il);
                throw new ILPatchFailureException(ModContent.GetInstance<Synergia>(), il, e);
            }
        }
    }

    public class Sounds : ModSystem
    {
        public static SoundStyle Type(string name)
        {
            SoundStyle soundStyle = new($"Synergia/Assets/Sounds/{name}");
            return soundStyle;
        }

        [Obsolete("устарел. Используйте Sounds.Type(\"Name\") вместо этого.")] public static readonly SoundStyle BookOpen = new ("Synergia/Assets/Sounds/BookOpen");
        [Obsolete("устарел. Используйте Sounds.Type(\"Name\") вместо этого.")] public static readonly SoundStyle BookClose = new ("Synergia/Assets/Sounds/BookClose");
        [Obsolete("устарел. Используйте Sounds.Type(\"Name\") вместо этого.")] public static readonly SoundStyle CragwormHit = new("Synergia/Assets/Sounds/CragwormHit");
        [Obsolete("устарел. Используйте Sounds.Type(\"Name\") вместо этого.")] public static readonly SoundStyle CragwormHit2 = new("Synergia/Assets/Sounds/CragwormHit2");
        [Obsolete("устарел. Используйте Sounds.Type(\"Name\") вместо этого.")] public static readonly SoundStyle WormRoar = new("Synergia/Assets/Sounds/WormRoar");
        [Obsolete("устарел. Используйте Sounds.Type(\"Name\") вместо этого.")] public static readonly SoundStyle BrokenBone = new("Synergia/Assets/Sounds/BrokenBone");
        [Obsolete("устарел. Используйте Sounds.Type(\"Name\") вместо этого.")] public static readonly SoundStyle SpearSound = new("Synergia/Assets/Sounds/SpearSound");
        [Obsolete("устарел. Используйте Sounds.Type(\"Name\") вместо этого.")] public static readonly SoundStyle WormBoom = new("Synergia/Assets/Sounds/WormBoom");
        [Obsolete("устарел. Используйте Sounds.Type(\"Name\") вместо этого.")] public static readonly SoundStyle LavaBone = new("Synergia/Assets/Sounds/LavaBone");
        [Obsolete("устарел. Используйте Sounds.Type(\"Name\") вместо этого.")] public static readonly SoundStyle FeatherFlow = new("Synergia/Assets/Sounds/FeatherFlow");
        [Obsolete("устарел. Используйте Sounds.Type(\"Name\") вместо этого.")] public static readonly SoundStyle WeakSword = new("Synergia/Assets/Sounds/WeakSword");
        [Obsolete("устарел. Используйте Sounds.Type(\"Name\") вместо этого.")] public static readonly SoundStyle PowerSword = new("Synergia/Assets/Sounds/PowerSword");
        [Obsolete("устарел. Используйте Sounds.Type(\"Name\") вместо этого.")] public static readonly SoundStyle SandSphere = new("Synergia/Assets/Sounds/SandSphere");
        [Obsolete("устарел. Используйте Sounds.Type(\"Name\") вместо этого.")] public static readonly SoundStyle Impulse = new("Synergia/Assets/Sounds/Impulse");
    }

    public class SynergiaSystems : ModSystem
    {
        public override void PostUpdateEverything()
        {
            TimerSystem.Update();
        }
    }
}