using Synergia.Content.NPCs;

namespace Synergia.Content.Items.Placeable.Banners {
    public class ElderSnowmanBannerItem : BaseBanners {
        public override int BannerType => 1;
        public override int BannerNPC => NPCType<ElderSnowman>();
    }
}