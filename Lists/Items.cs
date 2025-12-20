using Avalon.Items.Tools.Hardmode;
using System.Collections.Generic;
using Terraria.ModLoader;
using static Synergia.ModList;
using static Terraria.ID.ItemID;
using static Terraria.ModLoader.ModContent;
using Carrot = StramsSurvival.Items.Foods.Carrot;
using Carrot1 = NewHorizons.Content.Items.Materials.Carrot;

namespace Synergia.Lists {
    public class Items {
        public static List<int> CarrotID { get; private set; } = [ItemType<Carrot>(), ItemType<Carrot1>()];
        public static List<int> FoodID { get; private set; } = [];
        public static List<int> CorruptionItem { get; private set; } = [Musket, BandofStarpower, ShadowOrb, BallOHurt, Roa.Find<ModItem>("PlanetomaStaff").Type, Roa.Find<ModItem>("Bookworms").Type, Roa.Find<ModItem>("Vilethorn").Type];
        public static List<int> SkyChest { get; private set; } = [Starfury, LuckyHorseshoe, ShinyRedBalloon, CelestialMagnet];
        public static List<int> SixToolTipsLin { get; private set; } = [MythrilPickaxe, OrichalcumPickaxe, ItemType<NaquadahDrill>(), ItemType<NaquadahPickaxe>()];
        public static List<int> SevenToolTipsLin { get; private set; } = [MythrilDrill, OrichalcumDrill];
        public static List<int> WeaponActiveBlood { get; private set; } = [Zenith /* Сюда предмет */];
    }
}
