using ReLogic.Content;
using Terraria.GameContent.UI.Elements;

namespace Synergia.UIs {
    public class DrawChat(Asset<Texture2D> customBackground, Asset<Texture2D> customborder, int customCornerSize = 12, int customBarSize = 4) : UIPanel(customBackground, customborder, customCornerSize, customBarSize) {
        public override void OnInitialize() {
            BackgroundColor = Color.White;
            BorderColor = Color.White;
        }
    }
}