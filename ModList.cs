namespace Synergia;

// Мы постоянно делали этот код.
// Вместо создания новой переменой бери от сюда 
// using static Synergia.ModList <- что бы не указывать от куда переменная.
// Пример npc.type == consolaria.Find<ModNPC>("SpectralGastropod").Type.
// Или в ArcaneShard.cs.
public static class ModList {
    public static Mod Ava { get; private set; }
    public static Mod Bis { get; private set; }
    public static Mod Cons { get; private set; }
    public static Mod Horizons { get; private set; }
    public static Mod Roa { get; private set; }
    public static Mod StarforgedClassic { get; private set; }
    public static Mod Survival { get; private set; }
    public static Mod Valhalla { get; private set; }
    public static Mod TRAEProjectLoaded { get; private set; }
    public static Mod TRAEProjectRework { get; private set; }
    public static Mod PackBuilderLoaded { get; private set; }

    public static void LoadMod() {
        Ava = ModLoader.GetMod("Avalon");
        Bis = ModLoader.GetMod("Bismuth");
        Cons = ModLoader.GetMod("Consolaria");
        Horizons = ModLoader.GetMod("NewHorizons");
        Roa = ModLoader.GetMod("RoA");
        StarforgedClassic = ModLoader.GetMod("starforgedclassic");
        Survival = ModLoader.GetMod("StramsSurvival");
        Valhalla = ModLoader.GetMod("ValhallaMod");
        TRAEProjectRework = ModLoader.GetMod("TRAEBossRework");
        ModLoader.TryGetMod("TRAEProject", out Mod TRAEProject);
        ModLoader.TryGetMod("PackBuilder", out Mod PackBuilder);
        if (TRAEProject != null) {
            TRAEProjectLoaded = TRAEProject;
        }
        if (PackBuilder != null) {
            PackBuilderLoaded = PackBuilder;
        }
    }
}