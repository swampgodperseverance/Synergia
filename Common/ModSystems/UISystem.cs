using Microsoft.Xna.Framework;
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
        if (inventoryIndex != -1) {
            layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer("Synergia: Dwarf UI", delegate { mod.DwarfUserInterface.Draw(Main.spriteBatch, new GameTime()); return true; }, InterfaceScaleType.UI));
        }
    }
    public override void UpdateUI(GameTime gameTime) {
        mod.DwarfUserInterface?.Update(gameTime);
    }
}