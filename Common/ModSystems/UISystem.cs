using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Common.GlobalPlayer;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using static Synergia.Helpers.UIHelper;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.ModSystems;

public class UISystem : ModSystem {
    static readonly Synergia mod = GetInstance<Synergia>();
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
        SpriteBatch spriteBatch = Main.spriteBatch;

        int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
        int resourceBarsIndex = layers.FindIndex(layers => layers.Name.Equals("Vanilla: Resource Bars"));

        AddLayer(layers, inventoryIndex, "Synergia: Dwarf UI", () => { mod.DwarfUserInterface.Draw(Main.spriteBatch, new GameTime()); return true; });
        AddLayer(layers, resourceBarsIndex, "Synergia: Throwing UI", () => { if (Main.myPlayer >= 0) { Main.LocalPlayer.GetModPlayer<ThrowingUI>().DrawThrowingUI(); } return true; });
        AddLayer(layers, inventoryIndex, "Synergia: Summon UI", () => { if (Main.myPlayer >= 0) { Main.LocalPlayer.GetModPlayer<SummonUI>().DrawSummonUI(spriteBatch); } return true; });
    }
    public override void UpdateUI(GameTime gameTime) {
        mod.DwarfUserInterface?.Update(gameTime);
    }
}