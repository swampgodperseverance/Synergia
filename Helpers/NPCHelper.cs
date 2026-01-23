namespace Synergia.Helpers {
    public static partial class NPCHelper {
        public static void AddBanner(this ModNPC npc, int item) {
            npc.Banner = npc.Type;
            npc.BannerItem = item;
        }
    }
}
