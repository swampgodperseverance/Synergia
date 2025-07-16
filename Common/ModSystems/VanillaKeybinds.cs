using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Input;

namespace Vanilla.Common.ModSystems
{
    public class VanillaKeybinds : ModSystem
    {
        public static ModKeybind ToggleAuraModeKeybind;

        public override void Load()
        {
            ToggleAuraModeKeybind = KeybindLoader.RegisterKeybind(Mod, "Toggle Aura Mode", "J");
        }

        public override void Unload()
        {
            ToggleAuraModeKeybind = null;
        }
    }
}