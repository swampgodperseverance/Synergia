using Microsoft.Xna.Framework;
using Synergia.Common.GlobalPlayer;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.ModSystems;

public class UISystem : ModSystem {
    readonly Synergia mod = GetInstance<Synergia>();
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
        int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
        int resourceBarsIndex = layers.FindIndex(layers => layers.Name.Equals("Vanilla: Resource Bars"));

        if (inventoryIndex != -1) {
            layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer("Synergia: Dwarf UI", delegate { mod.DwarfUserInterface.Draw(Main.spriteBatch, new GameTime()); return true; }, InterfaceScaleType.UI));
        }
        if (resourceBarsIndex != -1) {
            layers.Insert(resourceBarsIndex, new LegacyGameInterfaceLayer("Synergia: Throwing UI", delegate { if (Main.myPlayer >= 0) { Main.LocalPlayer.GetModPlayer<ThrowingUI>().DrawThrowingUI(); } return true; }, InterfaceScaleType.UI));
        }
    }
    public override void UpdateUI(GameTime gameTime) {
        mod.DwarfUserInterface?.Update(gameTime);
    }
}