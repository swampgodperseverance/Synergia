using Synergia.Common.GlobalPlayer;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using static Synergia.Helpers.UIHelper;

namespace Synergia.Common.ModSystems;

public class UISystem : ModSystem {
    static readonly Synergia mod = GetInstance<Synergia>();
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
        SpriteBatch spriteBatch = Main.spriteBatch;

        int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
        //int resourceBarsIndex = layers.FindIndex(layers => layers.Name.Equals("Vanilla: Resource Bars"));
        //int chat = layers.FindIndex(layers => layers.Name.Equals("Vanilla: Inventory"));

        AddLayer(layers, inventoryIndex, "Synergia: Dwarf UI", () => { mod.DwarfUserInterface.Draw(Main.spriteBatch, new GameTime()); return true; });
        AddLayer(layers, inventoryIndex, "Synergia: Throwing UI", () => { if (Main.myPlayer >= 0) { Main.LocalPlayer.GetModPlayer<ThrowingUI>().DrawThrowingUI(); } return true; });
        AddLayer(layers, inventoryIndex, "Synergia: Summon UI", () => { if (Main.myPlayer >= 0) { Main.LocalPlayer.GetModPlayer<SummonUI>().DrawSummonUI(spriteBatch); } return true; });
        AddLayer(layers, inventoryIndex, "Synergia: Blood UI", () => { if (Main.myPlayer >= 0) { Main.LocalPlayer.GetModPlayer<BloodUI>().DrawBloodUI(spriteBatch); } return true; });
        AddLayer(layers, inventoryIndex, "Synergia: Dwarf Chat UI", () => { mod.DwarfChatInterface.Draw(Main.spriteBatch, new GameTime()); return true; });
    }
    public override void UpdateUI(GameTime gameTime) {
        mod.DwarfUserInterface?.Update(gameTime);
        mod.DwarfChatInterface?.Update(gameTime);
    }
}