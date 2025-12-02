using Terraria.ModLoader;

namespace Synergia.Common.ModSystems
{
    public class VanillaKeybinds : ModSystem
    {
        public static ModKeybind ToggleAuraModeKeybind { get; private set; }
        public static ModKeybind ArmorSetBonusActivation { get; private set; }

        public override void Load()
        {
            ToggleAuraModeKeybind = KeybindLoader.RegisterKeybind(Mod, "Toggle Aura Mode", "J");
            ArmorSetBonusActivation = KeybindLoader.RegisterKeybind(Mod, "Armor Set Bonus Activate", "K");
        }

        public override void Unload()
        {
            ToggleAuraModeKeybind = null;
            ArmorSetBonusActivation = null;
        }
    }
}