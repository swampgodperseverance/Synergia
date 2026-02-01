using Avalon.Items.Tools.Hardmode;
using Bismuth.Content.Items.Weapons.Throwing;
using NewHorizons.Content.Items.Weapons.Ranged;
using NewHorizons.Content.Items.Weapons.Throwing;
using Synergia.Content.Items.Weapons.Throwing;
using System.Collections.Generic;
using Consolaria.Content.Items.Weapons.Melee;
using ValhallaMod.Items.Garden;
using ValhallaMod.Items.Weapons.Boomerang;
using ValhallaMod.Items.Weapons.Javelin;
using ValhallaMod.Items.Weapons.Thrown;
using ValhallaMod.Items.Weapons.Glaive;
using static Synergia.ModList;
using static Terraria.ID.ItemID;
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
        public static List<int> WeaponActiveBlood { get; private set; } = [Shuriken, StarAnise, ShadowFlameKnife, FlyingKnife, Javelin, FrostDaggerfish, BoneJavelin, BoneDagger, Bananarang, PossessedHatchet, ScourgeoftheCorruptor, LightDisc, ThrowingKnife, Horizons.Find<ModItem>("GammaBlade").Type, Horizons.Find<ModItem>("BoneKama").Type, Horizons.Find<ModItem>("MoltenDagger").Type, Horizons.Find<ModItem>("Kunai").Type, Horizons.Find<ModItem>("RustyAxe").Type, Horizons.Find<ModItem>("BloodSpiller").Type, Horizons.Find<ModItem>("CobaltKunai").Type, Bis.Find<ModItem>("JaguarsChakram").Type, Bis.Find<ModItem>("OrcishJavelin").Type, Bis.Find<ModItem>("ThrowingAxe").Type, Bis.Find<ModItem>("Typhoon").Type, Bis.Find<ModItem>("FuryOfWaters").Type, Bis.Find<ModItem>("SharkJavelin").Type, Bis.Find<ModItem>("SharkKnife").Type, Bis.Find<ModItem>("OrichalcumJavelin").Type, Bis.Find<ModItem>("MythrilJavelin").Type, Bis.Find<ModItem>("ChlorophyteJavelin").Type, Bis.Find<ModItem>("TitaniumJavelin").Type, Bis.Find<ModItem>("AdamantiteJavelin").Type, Bis.Find<ModItem>("PalladiumJavelin").Type, Bis.Find<ModItem>("SolarDisk").Type, Bis.Find<ModItem>("JaguarsDagger").Type, Valhalla.Find<ModItem>("CarrotDagger").Type, Valhalla.Find<ModItem>("CactusKnife").Type, Valhalla.Find<ModItem>("SpiderKnife").Type, Valhalla.Find<ModItem>("TrueLightDisc").Type, Valhalla.Find<ModItem>("CorrodeShuriken").Type, Valhalla.Find<ModItem>("TerraGlaive").Type, Valhalla.Find<ModItem>("ToothKnife").Type, Valhalla.Find<ModItem>("ClusterGrenade").Type, Ava.Find<ModItem>("VirulentScythe").Type, Ava.Find<ModItem>("CrystalTomahawk").Type, Ava.Find<ModItem>("Shurikerang").Type, ItemType<ValhalliteJavelin>(), ItemType<Selection>(), ItemType<DurataniumJavelin>(), ItemType<BloodyScythe>(), ItemType<DecayedScythe>(), ItemType<GoldGlove>(), ItemType<OculithShard>(), ItemType<Blazes>(), ItemType<BlasphemousHeavens>(), ItemType<JadeGlaive>(), ItemType<InfamousFlame>(), ItemType<NaquadahJavelin>()];
        public static HashSet<int> IsComboWeapons { get; private set; } = [Trimarang, Flamarang, Anchor, WoodenBoomerang, Snowball, FruitcakeChakram, ItemType<SunJavelin>(), ItemType<NaturalSelection>(), ItemType<AlbinoMandible>(), ItemType<StalloyScrew>(), ItemType<TitaniumWarhammer>(), ItemType<AdamantiteDagger>(), ItemType<TeethBreaker>(), ItemType<NewHorizons.Content.Items.Weapons.Throwing.ChlorophyteJavelin>(), ItemType<NewHorizons.Content.Items.Weapons.Throwing.NightGlaive>(), ItemType<NewHorizons.Content.Items.Weapons.Throwing.MythrilJavelin>(), ItemType<CrystalDagger>(), ItemType<CrimiteGlaive>(), ItemType<SnowGlaive>(), ItemType<OrichalcumKama>(), ItemType<Carnwennan>(), ItemType<Aeglos>(), ItemType<Lancea>(), ItemType<Garlic>(), ItemType<ScarletGungnir>(), ItemType<BlazingSaws>(), ItemType<NanoStar>(), ItemType<CrystalGrenade>()];
        //public static List<int> DisableItem { get; private set; } = [ItemType<AdamantiteHat>(), ItemType<BronzeMask>(), ItemType<CobaltHeadgear>(), ItemType<MythrilHeadgear>(), ItemType<OrichalcumHat>(), ItemType<PalladiumHat>(), ItemType<TitaniumHat>(), ItemType<HallowedFaceShield>(), ItemType<HallowedHeadpiece>()];                                                       
    }
}

