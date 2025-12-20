using Synergia.Common.GlobalPlayer;
using Synergia.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.ModSystems.RecipeSystem {
    public class OnCraft {
        public static void SulphurConsumeIngredientCallback(Recipe recipe, int type, ref int amount, bool isDecrafting) {
            if (type == ModContent.ItemType<SulfuricAcid>()) {
                SynergiaPlayer data = Main.LocalPlayer.GetModPlayer<SynergiaPlayer>();

                if (data.useSulfuricAcid != 5) {
                    amount = 0;
                    data.useSulfuricAcid++;
                }
                else {
                    amount = 1;
                    data.Player.QuickSpawnItem(data.Player.GetSource_FromThis(), ItemID.Bottle);
                    data.useSulfuricAcid = 0;
                }
            }
        }
    }
}
