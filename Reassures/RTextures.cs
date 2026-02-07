using ReLogic.Content;

namespace Synergia.Reassures {
    public partial class Reassures {
        public static string Blank { get; private set; } = "Synergia/Assets/Textures/Blank";
        public static string GetUIElementName(string element) => $"Synergia/Assets/UIs/{element}";
        public static string GetTexturesElementName(string element) => $"Synergia/Assets/Textures/{element}";
        public static void RegisterTextureElement(Asset<Texture2D>[] assets, byte countElement, string nameElement, bool ui = false) {
            for (int i = 0; i < countElement; i++) {
                assets[i] = ui ? Request<Texture2D>(GetUIElementName(nameElement + i.ToString())) : Request<Texture2D>(GetTexturesElementName(nameElement + i.ToString()));
            }
        }
        public class RTextures : ModSystem {
            public static Asset<Texture2D>[] FeatherLeft { get; private set; } = new Asset<Texture2D>[3];
            public static Asset<Texture2D>[] FeatherRight { get; private set; } = new Asset<Texture2D>[3];
            public static Asset<Texture2D>[] Present { get; private set; } = new Asset<Texture2D>[7];
            public static Asset<Texture2D>[] Location { get; private set; } = new Asset<Texture2D>[4]; // 5
            public static Asset<Texture2D>[] GlowLocation { get; private set; } = new Asset<Texture2D>[4];
            public override void Load() {
                RegisterTextureElement(FeatherLeft, 3, "FeatherLeft");
                RegisterTextureElement(FeatherRight, 3, "FeatherRight");
                RegisterTextureElement(Present, 7, "BGPresent_");
                RegisterTextureElement(Location, 4, "TeleportationLocation_", true);
                RegisterTextureElement(GlowLocation, 4, "TeleportationLocation_glow_", true);
            }
        }
    }
}
