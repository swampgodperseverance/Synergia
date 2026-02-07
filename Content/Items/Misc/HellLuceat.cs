using Synergia.UIs;
using Terraria;
using Terraria.ID;

namespace Synergia.Content.Items.Misc {
    public class HellLuceat : ModItem {
        public override void SetDefaults() {
            Item.width = 20;
            Item.height = 20;
            Item.rare = RarityType<Common.Rarities.LavaGradientRarity>();
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 1;
            Item.useAnimation = 15;
        }
        public override bool? UseItem(Player player) {
            GetInstance<Synergia>().LuceatInterface.SetState(new LuceatUI());
            return false;
        }
    }
}
