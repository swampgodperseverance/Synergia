using Avalon.Items.Material.Ores;
using Synergia.Common.GlobalPlayer;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Materials {
    public class SulfuricAcid : ModItem {
        public override void SetDefaults() {
            Item.CloneDefaults(ItemID.BottledWater);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "UseCount", $"использовано {Main.LocalPlayer.GetModPlayer<SynergiaPlayer>().useSulfuricAcid} из {5}"));
        }
        public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<Sulphur>();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddTile(TileID.Furnaces);
            recipe.Register();
        }
    }
}