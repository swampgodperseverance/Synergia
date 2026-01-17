using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.UI.Chat;
using static Terraria.Main;

namespace Synergia.UIs {
    // Vanilla Code
    public class TextDisplayCache2 {
        private string _originalText;
        private int _lastScreenWidth;
        private int _lastScreenHeight;
        private InputMode _lastInputMode;

        // Added by TML.
        private Color originalColor;

        public List<List<TextSnippet>> TextLines { get; private set; } // Changed from string[]

        public int AmountOfLines { get; private set; }

        // Fix all instances of drawing text to use TextSnippets instead of strings (#FixNPCChat)
        // Added baseColor parameter
        public void PrepareCache(string text, Color baseColor) {
            if (false | (screenWidth != _lastScreenWidth) | (screenHeight != _lastScreenHeight) | (_originalText != text) | (PlayerInput.CurrentInputMode != _lastInputMode) | (originalColor != baseColor)) {
                _lastScreenWidth = screenWidth;
                _lastScreenHeight = screenHeight;
                _originalText = text;
                _lastInputMode = PlayerInput.CurrentInputMode;
                text = Lang.SupportGlyphs(text);
                originalColor = baseColor;

                /*
                TextLines = Utils.WordwrapString(text, FontAssets.MouseText.Value, 460, 10, out var lineAmount);
                AmountOfLines = lineAmount;
                */
                TextLines = Utils.WordwrapStringSmart(text, baseColor, FontAssets.MouseText.Value, 460, 10);
                AmountOfLines = TextLines.Count;
            }
        }
    }
}