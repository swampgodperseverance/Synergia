using Synergia.Content.NPCs;

namespace Synergia.Content.Items.Placeable.Banners {
    public class SnowykazeBannerItem : BaseBanners {
        public override int BannerType => 2;
        public override int BannerNPC => NPCType<Snowykaze>();
    }
}
