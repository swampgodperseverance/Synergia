using ReLogic.Content;

namespace Synergia.Reassures {
    public partial class Reassures {
        public static string Blank { get; private set; } = "Synergia/Assets/Textures/Blank";
        public static string GetUIElementName(string element) => $"Synergia/Assets/UIs/{element}";
        public static string GetTexturesElementName(string element) => $"Synergia/Assets/Textures/{element}";
        public static void RegisterTextureElement(Asset<Texture2D>[] assets, byte countElement, string nameElement) {
            for (int i = 0; i < countElement; i++) {
                assets[i] = Request<Texture2D>(GetTexturesElementName(nameElement + i.ToString()));
            }
        }
        public class RTextures : ModSystem {
            public static Asset<Texture2D>[] FeatherLeft { get; private set; } = new Asset<Texture2D>[3];
            public static Asset<Texture2D>[] FeatherRight { get; private set; } = new Asset<Texture2D>[3];
            public override void Load() {
                RegisterTextureElement(FeatherLeft, 3, "FeatherLeft");
                RegisterTextureElement(FeatherRight, 3, "FeatherRight");
            }
        }
    }
}
