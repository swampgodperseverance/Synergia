using Synergia.Common.ModSystems;
using Synergia.Content.Tiles;
using Terraria;
using Terraria.ID;

namespace Synergia.Content.Items.Placeable.Banners {
    public abstract class BaseBanners : ModItem {
        public abstract int BannerType { get; }
        public abstract int BannerNPC { get; }
        public override void SetDefaults() {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 0, 10, 0);
            Item.createTile = TileType<GlobalBanner>();
            Item.placeStyle = BannerType;
        }
        public override void SetStaticDefaults() {
            SynergiaWorld.BannerType.Add(BannerType, (Type, BannerNPC));
        }
    }
}