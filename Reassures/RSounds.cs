using Terraria.Audio;

namespace Synergia.Reassures {
    public partial class Reassures {
        public static SoundStyle GetSongByName(string name) => new($"Synergia/Assets/Sounds/{name}");
        public static string GetSongByName2(string name) => $"Synergia/Assets/Sounds/{name}";
        public class RSounds {
            public static readonly SoundStyle BookOpenSound = new("Synergia/Assets/Sounds/BookOpen");
            public static readonly SoundStyle BookCloseSound = new("Synergia/Assets/Sounds/BookClose");
            public static readonly SoundStyle CragwormHit = new("Synergia/Assets/Sounds/CragwormHit");
            public static readonly SoundStyle CragwormHit2 = new("Synergia/Assets/Sounds/CragwormHit2");
            public static readonly SoundStyle WormRoar = new("Synergia/Assets/Sounds/WormRoar");
            public static readonly SoundStyle BrokenBone = new("Synergia/Assets/Sounds/BrokenBone");
            public static readonly SoundStyle SpearSound = new("Synergia/Assets/Sounds/SpearSound");
            public static readonly SoundStyle WormBoom = new("Synergia/Assets/Sounds/WormBoom");
            public static readonly SoundStyle LavaBone = new("Synergia/Assets/Sounds/LavaBone");
            public static readonly SoundStyle FeatherFlow = new("Synergia/Assets/Sounds/FeatherFlow");
            public static readonly SoundStyle WeakSword = new("Synergia/Assets/Sounds/WeakSword");
            public static readonly SoundStyle PowerSword = new("Synergia/Assets/Sounds/PowerSword");
            public static readonly SoundStyle SandSphere = new("Synergia/Assets/Sounds/SandSphere");
            public static readonly SoundStyle Impulse = new("Synergia/Assets/Sounds/Impulse");
            public static readonly SoundStyle Watersound = new("Synergia/Assets/Sounds/Watersound");
            public static readonly SoundStyle SilentPunch = new("Synergia/Assets/Sounds/SilentPunch");
            public static readonly SoundStyle BigBoom = new("Synergia/Assets/Sounds/BigBoom");
            public static readonly SoundStyle SwamplingRoar = new("Synergia/Assets/Sounds/SwamplingRoar");
        }
    }
    public class RegisterNewSongPatch : ILoadable {
        public void Load(Mod mod) {
            MusicLoader.AddMusic(mod, "Assets/Sounds/PeacefulTownV2");
            MusicLoader.AddMusic(mod, "Assets/Sounds/InfernoFrontierSoundtrack");
        }
        public void Unload() { }
    }
}