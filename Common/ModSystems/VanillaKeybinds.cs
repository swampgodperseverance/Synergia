using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Input;

namespace Synergia.Common.ModSystems
{
    public class VanillaKeybinds : ModSystem
    {
        public static ModKeybind ToggleAuraModeKeybind;
        public static ModKeybind ArmorSetBonusActivation;

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