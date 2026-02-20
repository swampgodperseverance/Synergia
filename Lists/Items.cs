using Avalon.Items.Tools.Hardmode;
using Bismuth.Content.Items.Weapons.Throwing;
using Consolaria.Content.Items.Weapons.Melee;
using NewHorizons.Content.Items.Weapons.Ranged;
using NewHorizons.Content.Items.Weapons.Throwing;
using Starforgedclassic.Content.Weapons.Solarang;
using Synergia.Content.Items.Weapons.Throwing;
using System.Collections.Generic;
using ValhallaMod.Items.Garden;
using ValhallaMod.Items.Weapons.Blood;
using ValhallaMod.Items.Weapons.Boomerang;
using ValhallaMod.Items.Weapons.Glaive;
using ValhallaMod.Items.Weapons.Javelin;
using ValhallaMod.Items.Weapons.Thrown;
using static Synergia.ModList;
using static Terraria.ID.ItemID;

namespace Synergia.Lists {
    public class Items {
        public static List<int> FoodID { get; private set; } = [];
        public static List<int> CorruptionItem { get; private set; } = [Musket, BandofStarpower, ShadowOrb, BallOHurt, Roa.Find<ModItem>("PlanetomaStaff").Type, Roa.Find<ModItem>("Bookworms").Type, Roa.Find<ModItem>("Vilethorn").Type];
        public static List<int> SkyChest { get; private set; } = [Starfury, LuckyHorseshoe, ShinyRedBalloon, CelestialMagnet];
        public static List<int> SixToolTipsLin { get; private set; } = [MythrilPickaxe, OrichalcumPickaxe, ItemType<NaquadahDrill>(), ItemType<NaquadahPickaxe>()];
        public static List<int> SevenToolTipsLin { get; private set; } = [MythrilDrill, OrichalcumDrill];
        public static List<int> WeaponActiveBlood { get; private set; } = [Shuriken, BloodyMachete, StarAnise, ShadowFlameKnife, FlyingKnife, Javelin, FrostDaggerfish, BoneJavelin, BoneDagger, Bananarang, PossessedHatchet, ScourgeoftheCorruptor, LightDisc, ThrowingKnife, Horizons.Find<ModItem>("GammaBlade").Type, Horizons.Find<ModItem>("BoneKama").Type, Horizons.Find<ModItem>("PalladiumWaraxe").Type, Horizons.Find<ModItem>("MoltenDagger").Type, Horizons.Find<ModItem>("Kunai").Type, Horizons.Find<ModItem>("RustyAxe").Type, Horizons.Find<ModItem>("BloodSpiller").Type, Horizons.Find<ModItem>("CobaltKunai").Type, Bis.Find<ModItem>("JaguarsChakram").Type, Bis.Find<ModItem>("OrcishJavelin").Type, Bis.Find<ModItem>("ThrowingAxe").Type, Bis.Find<ModItem>("Typhoon").Type, Bis.Find<ModItem>("FuryOfWaters").Type, Bis.Find<ModItem>("SharkJavelin").Type, Bis.Find<ModItem>("SharkKnife").Type, Bis.Find<ModItem>("OrichalcumJavelin").Type, Bis.Find<ModItem>("MythrilJavelin").Type, Bis.Find<ModItem>("ChlorophyteJavelin").Type, Bis.Find<ModItem>("TitaniumJavelin").Type, Bis.Find<ModItem>("CobaltJavelin").Type, Bis.Find<ModItem>("StarOfTheDunedain").Type, Bis.Find<ModItem>("AdamantiteJavelin").Type, Valhalla.Find<ModItem>("TarBlade").Type, Bis.Find<ModItem>("SolarDisk").Type, Bis.Find<ModItem>("JaguarsDagger").Type, Valhalla.Find<ModItem>("DemoniteGlaive").Type, Valhalla.Find<ModItem>("DemoniteGlaive").Type, Valhalla.Find<ModItem>("TrueNightGlaive").Type, Valhalla.Find<ModItem>("NightGlaive").Type, Valhalla.Find<ModItem>("ChlorophyleGlaive").Type, Valhalla.Find<ModItem>("DungeonGlaive").Type, Valhalla.Find<ModItem>("BigBeeNade").Type, Valhalla.Find<ModItem>("Sufferang").Type, Valhalla.Find<ModItem>("CarrotDagger").Type, Valhalla.Find<ModItem>("CactusKnife").Type, Valhalla.Find<ModItem>("SpiderKnife").Type, Valhalla.Find<ModItem>("TrueLightDisc").Type, Valhalla.Find<ModItem>("CorrodeShuriken").Type, Valhalla.Find<ModItem>("TerraGlaive").Type, Valhalla.Find<ModItem>("ToothKnife").Type, Valhalla.Find<ModItem>("ClusterGrenade").Type, Ava.Find<ModItem>("VirulentScythe").Type, Ava.Find<ModItem>("EnchantedShuriken").Type, Ava.Find<ModItem>("CrystalTomahawk").Type, Ava.Find<ModItem>("Icicle").Type, Ava.Find<ModItem>("TetanusChakram").Type, Ava.Find<ModItem>("Shurikerang").Type, ItemType<ValhalliteJavelin>(), ItemType<Selection>(), ItemType<DurataniumJavelin>(), ItemType<BloodyScythe>(), ItemType<DecayedScythe>(), ItemType<GoldGlove>(), ItemType<OculithShard>(), ItemType<Blazes>(), ItemType<BlasphemousHeavens>(), ItemType<ThunderChakram>(), ItemType<JadeGlaive>(), ItemType<Solarang>(), ItemType<InfamousFlame>(), ItemType<NaquadahJavelin>()];
        public static HashSet<int> IsComboWeapons { get; private set; } = [Trimarang, Flamarang, Bone, Shroomerang, ThornChakram, Anchor, WoodenBoomerang, Snowball, FruitcakeChakram, ItemType<OmegaDisc>(), ItemType<Flarion>(), ItemType<EverwoodJavelin>(), ItemType<Pulsar>(), ItemType<SunJavelin>(), ItemType<NaturalSelection>(), ItemType<AlbinoMandible>(), ItemType<StalloyScrew>(), ItemType<Airflow>(), ItemType<TitaniumWarhammer>(), ItemType<AdamantiteDagger>(), ItemType<TeethBreaker>(), ItemType<CactusStar>(), ItemType<AzraelsHeartstopper>(), ItemType<NewHorizons.Content.Items.Weapons.Throwing.ChlorophyteJavelin>(), ItemType<NewHorizons.Content.Items.Weapons.Throwing.NightGlaive>(), ItemType<NewHorizons.Content.Items.Weapons.Throwing.MythrilJavelin>(), ItemType<CrystalDagger>(), ItemType<CrimiteGlaive>(), ItemType<SnowGlaive>(), ItemType<OrichalcumKama>(), ItemType<Carnwennan>(), ItemType<Aeglos>(), ItemType<Lancea>(), ItemType<Garlic>(), ItemType<ScarletGungnir>(), ItemType<BlazingSaws>(), ItemType<NanoStar>(), ItemType<CrystalGrenade>()];
        public static HashSet<int> VanillaGrapplingHooks { get; private set; } = [AmethystHook, Hook, AmberHook, AncientHallowedHood, AntiGravityHook, BatHook, CandyCaneHook, ChristmasHook, DiamondHook, DualHook, EmeraldHook, FishHook, GrapplingHook, HotlineFishingHook, IlluminantHook, LavaFishingHook, LunarHook, QueenSlimeHook, RubyHook, SapphireHook, SlimeHook, SpookyHook, SquirrelHook, StaticHook, TendonHook, ThornHook, TopazHook, WormHook];
        //public static List<int> DisableItem { get; private set; } = [ItemType<AdamantiteHat>(), ItemType<BronzeMask>(), ItemType<CobaltHeadgear>(), ItemType<MythrilHeadgear>(), ItemType<OrichalcumHat>(), ItemType<PalladiumHat>(), ItemType<TitaniumHat>(), ItemType<HallowedFaceShield>(), ItemType<HallowedHeadpiece>()];                                                       
    }
}

    