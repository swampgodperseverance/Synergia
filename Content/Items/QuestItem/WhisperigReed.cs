using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.QuestItem {
    public class WhisperigReed : ModItem {
        public override void SetDefaults() {
            Item.questItem = true;
            Item.rare = ItemRarityID.Quest;
            Item.width = 40;
            Item.height = 25;
        }
    }
}