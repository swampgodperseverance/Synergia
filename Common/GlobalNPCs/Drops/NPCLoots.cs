using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Accessories.PreHardmode;
using Avalon.Items.Material;
using Avalon.Items.Material.Ores;
using Avalon.Items.Material.Shards;
using Avalon.Items.Material.TomeMats;
using Avalon.Items.Weapons.Ranged.Hardmode.SunsShadow;
using Avalon.NPCs.Critters;
using Avalon.NPCs.Hardmode;
using Avalon.NPCs.PreHardmode;
using Consolaria.Content.NPCs;
using NewHorizons.Content.Items.Materials;
using NewHorizons.Content.NPCs;
using Synergia.Common.SynergiaCondition;
using Synergia.Content.Items.Accessories;
using Synergia.Content.Items.Weapons.AuraStaff;
using Synergia.Content.Items.Weapons.Throwing;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Accessory.Active;
using ValhallaMod.Items.Accessory.Shield;
using ValhallaMod.Items.Garden;
using ValhallaMod.Items.Material;
using ValhallaMod.NPCs.Corruption;
using ValhallaMod.NPCs.Dungeon;
using ValhallaMod.NPCs.Frost;
using ValhallaMod.NPCs.Goblin;
using ValhallaMod.NPCs.Jungle;
using ValhallaMod.NPCs.Marble;
using ValhallaMod.NPCs.Snowman;
using ValhallaMod.NPCs.Tar;
using ValhallaMod.NPCs.Underground;
using ValhallaMod.NPCs.Underworld;
using ValhallaMod.NPCs.Zombies;
using static Synergia.ModList;
using static Terraria.ModLoader.ModContent;
using Gargoyle = ValhallaMod.NPCs.Granite.Gargoyle;

namespace Synergia.Common.GlobalNPCs.Drops {
    public class NPCLoots : GlobalNPC {
        static readonly HashSet<int> AllowedModdedNPCs =
        [
            NPCType<Mime>(),NPCType<ContaminatedGhoul>(), NPCType<ContaminatedBunny>(), NPCType<GiantAlbinoCharger>(), NPCType<AlbinoCharger>(), NPCType<AlbinoAntlion>(),
            NPCType<ShadowMummy>(), NPCType<FleshMummy>(), NPCType<VampireMiner>(), NPCType<ZombieUnicorn>(), NPCType<ZombieUmbrella>(), NPCType<ZombieUmbrella2>(),
            NPCType<ZombieUmbrella3>(), NPCType<ZombieTactical>(), NPCType<ZombieTactical2>(), NPCType<ZombieLibrarian>(), NPCType<ZombieNinja>(), NPCType<ZombieBucket>(),
            NPCType<ZombieBalloon>(), NPCType<ZombieBalloon2>(), NPCType<ZombieBalloon3>(), NPCType<CarrotZombie>(), NPCType<Croc>(), NPCType<InfectedMan>(), NPCType<GoblinBunny>(),
            NPCType<VoodooCultistAlt>(), NPCType<VoodooCultist>(), NPCType<WitchDoctor>(), NPCType<BlueGoblin>(), NPCType<LihzahrdTrickster>()
        ]; // NPCType<GoblinRaider>()

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
            #region AvalonAccessories
            if (npc.type == NPCType<ArchImp>() || npc.type == NPCType<Firebird>() || npc.type == NPCType<Heater>() || npc.type == NPCType<HeaterWinged>() || npc.type == NPCType<LavaJellyfish>() || npc.type == NPCType<SporeSlinger>() || npc.type == NPCType<SporeSmasher>()) {
                npcLoot.Add(ItemDropRule.Common(ItemType<Vortex>(), 100));
            }
            if (npc.type == NPCType<Cacodemon>()) {
                npcLoot.Add(ItemDropRule.Common(ItemType<Vortex>(), 20));
            }
            if (npc.type == NPCType<Ifrit>()) {
                npcLoot.Add(ItemDropRule.Common(ItemType<Vortex>(), 10));
            }
            if (npc.type == NPCType<ChargingSpearbones>() || npc.type == NPCType<CenturionShieldless>() || npc.type == NPCType<Centurion2Shieldless>() || npc.type == NPCType<Centurion>() || npc.type == NPCType<Centurion2>() || npc.type == NPCType<Geomancer>() || npc.type == NPCType<ShaftSentinel>() || npc.type == NPCType<SkeletonTrapper>() || npc.type == NPCType<DragonSkull>() || npc.type == NPCType<VampireMiner>() || npc.type == Roa.Find<ModNPC>("Archdruid").Type) {
                npcLoot.Add(ItemDropRule.Common(ItemType<SoullessLocket>(), 100));
            }
            if (npc.type == Roa.Find<ModNPC>("Hunter").Type) {
                npcLoot.Add(ItemDropRule.Common(ItemType<HiddenBlade>(), 10));
            }
            if (npc.type == NPCType<FleshMummy>() || npc.type == NPCType<ShadowMummy>()) {
                npcLoot.Add(ItemDropRule.Common(ItemType<HiddenBlade>(), 100));
            }
            if (npc.type == NPCType<FleshSlime>() || npc.type == NPCType<FleshAxe>()) {
                npcLoot.Add(ItemDropRule.Common(ItemType<GoldenShield>(), 100));
            }
            if (npc.type == NPCType<ZombieTactical>() || npc.type == NPCType<Coldmando>() || npc.type == NPCType<MisterShotty>() || npc.type == NPCType<SnowmanTrasher>()) {
                npcLoot.Add(ItemDropRule.Common(ItemType<AmmoMagazine>(), 100));
            }
            if (npc.type == NPCType<OrbOfCorruption>() || npc.type == NPCType<ShadowSlime>() || npc.type == NPCType<ShadowHammer>()) {
                npcLoot.Add(ItemDropRule.Common(ItemType<GreekExtinguisher>(), 100));
            }
            if (npc.type == NPCType<Radiator>() || npc.type == NPCType<Radiator2>()) {
                npcLoot.Add(ItemDropRule.Common(ItemType<SixHundredWattLightbulb>(), 100));
            }
            if (npc.type == NPCType<SpectralGastropod>() || npc.type == NPCType<SpectralMummy>()) {
                npcLoot.Add(ItemDropRule.Common(ItemType<ArcaneShard>(), 8)); npcLoot.Add(ItemDropRule.Common(ItemType<RubberBoot>(), 100));
            }
            if (npc.type == NPCType<Cacodemon>()) {
                npcLoot.Add(ItemDropRule.Common(ItemType<BloodyWhetstone>(), 33));
            }
            #endregion
            #region BooksFragmentsDrops
            if (npc.type == NPCType<DragonHornet>()) {
                npcLoot.Add(ItemDropRule.Common(ItemType<StrongVenom>(), 7));
            }
            if (npc.type == NPCType<SkeletonBat>()) {
                npcLoot.Add(ItemDropRule.Common(ItemType<RubybeadHerb>(), 7)); npcLoot.Add(ItemDropRule.Common(ItemType<MysticalClaw>(), 7));
                npcLoot.Add(ItemDropRule.Common(ItemType<BreezeShard>(), 10, 1, 2));
            }
            if (npc.type == Roa.Find<ModNPC>("Fleder").Type || npc.type == NPCType<Gargoyle>() || npc.type == NPCType<DemonicSpirit>() || npc.type == NPCType<Valkyrie>()) {
                npcLoot.Add(ItemDropRule.Common(ItemType<MysticalClaw>(), 7));
            }
            if (npc.type == NPCType<InfectedMan>() || npc.type == NPCType<FrozenNimbus>() || npc.type == NPCType<FrozenSoul>() || npc.type == NPCType<GiantAlbinoCharger>()) {
                npcLoot.Add(ItemDropRule.Common(ItemType<ElementDust>(), 7));
            }
            if (npc.type == NPCType<FleshSlime>() || npc.type == NPCType<ShadowSlime>() || npc.type == NPCType<Sludger>() || npc.type == NPCType<TinyToxicSludge>()) {
                npcLoot.Add(ItemDropRule.Common(ItemType<DewofHerbs>(), 7));
            }
            if (npc.type == NPCType<VileDecomposer>() || npc.type == NPCType<DragonSnatcher>()) {
                npcLoot.Add(ItemDropRule.Common(ItemType<DewOrb>(), 7));
            }
            if (npc.type == NPCType<ArchWyvernHead>() || npc.type == NPCType<MythicalWyvernHead>()) {
                npcLoot.Add(ItemDropRule.Common(ItemType<MysticalTotem>(), 7)); npcLoot.Add(ItemDropRule.Common(ItemType<BreezeShard>(), 10, 1, 2));
            }
            #endregion
            #region BreezeShard
            if (npc.type == Roa.Find<ModNPC>("BackwoodsRaven").Type || npc.type == Roa.Find<ModNPC>("Fleder").Type) {
                npcLoot.Add(ItemDropRule.Common(ItemType<BreezeShard>(), 10, 1, 2));
            }
            if (npc.type == NPCType<ArchWyvernHead>()) {
                npcLoot.Add(ItemDropRule.Common(ItemType<BreezeShard>(), 10, 1, 2));
                npcLoot.Add(ItemDropRule.Common(ItemType<WyvernFur>(), 10, 1, 2));
            }
            if (npc.type == NPCType<MythicalWyvernHead>()) { npcLoot.Add(ItemDropRule.Common(ItemType<WyvernFur>(), 10, 1, 2)); }
            if (npc.type == NPCType<Gargoyle>()) { npcLoot.Add(ItemDropRule.Common(ItemType<BreezeShard>(), 10, 1, 2)); }
            if (npc.type == NPCType<Valkyrie>()) { npcLoot.Add(ItemDropRule.Common(ItemType<BreezeShard>(), 10, 1, 2)); }
            if (npc.type == NPCType<FrozenNimbus>()) { npcLoot.Add(ItemDropRule.Common(ItemType<BreezeShard>(), 10, 1, 2)); }
            #endregion
            #region CorruptShardDrops
            if (npc.type == NPCType<OrbOfCorruption>() || npc.type == NPCType<FleshSlime>() || npc.type == NPCType<ShadowSlime>() || npc.type == NPCType<ShadowMummy>() || npc.type == NPCType<ShadowHammer>() || npc.type == NPCType<FleshMummy>() || npc.type == NPCType<FleshAxe>()) {
                npcLoot.Add(ItemDropRule.Common(ItemType<CorruptShard>(), 4, 1, 2));
            }
            #endregion
            #region FireShardDrops
            if (npc.type == NPCType<Cacodemon>() || npc.type == NPCType<ArchDemon>() || npc.type == NPCType<ArchImp>() || npc.type == NPCType<Heater>() || npc.type == NPCType<Ifrit>() || npc.type == NPCType<HeaterWinged>()) { // || npc.type == Valhalla.Find<ModNPC>("SinSerpantHead").Type
                npcLoot.Add(ItemDropRule.Common(ItemType<FireShard>(), 2, 2, 4));
            }
            if (TRAEProjectLoaded != null) {
                if (npc.type == TRAEProjectLoaded.Find<ModNPC>("BeholderNPC").Type) { npcLoot.Add(ItemDropRule.Common(ItemType<FireShard>(), 1, 3, 7)); }
                if (npc.type == TRAEProjectLoaded.Find<ModNPC>("OniRoninNPC").Type) { npcLoot.Add(ItemDropRule.Common(ItemType<FireShard>(), 2, 2, 4)); }
                if (npc.type == TRAEProjectLoaded.Find<ModNPC>("SalalavaNPC").Type) { npcLoot.Add(ItemDropRule.Common(ItemType<FireShard>(), 2, 1, 2)); }
                if (npc.type == TRAEProjectLoaded.Find<ModNPC>("LittleBoomxie").Type || npc.type == TRAEProjectLoaded.Find<ModNPC>("Froggabomba").Type || npc.type == TRAEProjectLoaded.Find<ModNPC>("LavamanderNPC").Type || npc.type == TRAEProjectLoaded.Find<ModNPC>("PhoenixNPC").Type) { 
                    npcLoot.Add(ItemDropRule.Common(ItemType<FireShard>(), 4, 1, 2)); 
                }
            }
            #endregion
            #region FrostShard
            if (npc.type == NPCType<Draug>() || npc.type == NPCType<DraugAtArms>() || npc.type == NPCType<DraugAtArmsAlt>() || npc.type == NPCType<DraugSpearman>() || npc.type == NPCType<FrozenEye>() || npc.type == NPCType<FrozenSoul>() || npc.type == NPCType<ColdFather>()) {
                npcLoot.Add( ItemDropRule.Common(ItemType<FrostShard>(), 10, 1, 2) );
            }
            #endregion
            #region MiniGolemDrops
            if (npc.type == NPCType<TempleGolem>()){
                npcLoot.Add(ItemDropRule.Common(ItemType<SunsShadow>(), 10, 1, 1));
            }
            #endregion
            #region MiscDrops
            if (npc.type == Roa.Find<ModNPC>("Ent").Type) { npcLoot.Add(ItemDropRule.Common(ItemType<VerdurousStaff>(), 10)); }
            if (npc.type == Roa.Find<ModNPC>("Hog").Type) { npcLoot.Add(ItemDropRule.Common(ItemType<SuspiciousBag>(), 9)); }
            if (npc.type == Roa.Find<ModNPC>("Archdruid").Type) { npcLoot.Add(ItemDropRule.Common(ItemType<BrokenDice>(), 9)); }
            if (npc.type == NPCType<SpectralElemental>()) { npcLoot.Add(ItemDropRule.Common(ItemType<SuspiciousBag>(), 9)); }
            if (npc.type == NPCType<AlbinoAntlion>()) { npcLoot.Add(ItemDropRule.Common(ItemType<SuspiciousBag>(), 15)); }
            if (npc.type == Roa.Find<ModNPC>("SapSlime").Type) { npcLoot.Add(ItemDropRule.Common(ItemType<Sap>(), 5)); }
            if (npc.type == NPCType<ColdFather>()) { npcLoot.Add(ItemDropRule.Common(ItemType<SoulofIce>(), 1, 3, 6)); }
            if (npc.type == NPCType<IrateBones>()) { npcLoot.Add(ItemDropRule.Common(ItemType<AncientScrap>(), 7)); }
            if (npc.type == NPCType<Blaze>()) { npcLoot.Add(ItemDropRule.Common(ItemType<Blazes>(), 4)); }
            if (npc.type == NPCID.Mothron) {
               npcLoot.Add(ItemDropRule.Common(ItemType<BrokenGlaive>(), 4));
               npcLoot.Add(ItemDropRule.Common(ItemType<BrokenSpear>(), 4));
            }
            #endregion
            #region RottenFleshDrops
            if (npc.type == NPCType<ZombieUnicorn>() || npc.type == NPCType<ZombieUmbrella>() || npc.type == NPCType<ZombieUmbrella2>() || npc.type == NPCType<ZombieUmbrella3>() || npc.type == NPCType<ZombieTactical>() || npc.type == NPCType<ZombieTactical2>() || npc.type == NPCType<ZombieLibrarian>() || npc.type == NPCType<ZombieNinja>() || npc.type == NPCType<ZombieBucket>() || npc.type == NPCType<ZombieBalloon>() || npc.type == NPCType<ZombieBalloon2>() || npc.type == NPCType<ZombieBalloon3>() || npc.type == NPCType<CarrotZombie>()) {
                npcLoot.Add(ItemDropRule.Common(ItemType<RottenFlesh>(), 10, 1, 2));
                npcLoot.Add(ItemDropRule.Common(ItemType<SoullessLocket>(), 100)); // эти враги уже были что бы код не повторялся он был разделен
            }
            #endregion
            #region SkinningNPC
            bool isModdedAllowed = AllowedModdedNPCs.Contains(npc.type);
            bool invalidType = npc.SpawnedFromStatue || npc.value == 0 || npc.boss || npc.HitSound == SoundID.NPCHit2 || npc.HitSound == SoundID.NPCHit4 || npc.HitSound == SoundID.NPCHit5 || npc.HitSound == SoundID.NPCHit30 || npc.HitSound == SoundID.NPCHit34 || npc.HitSound == SoundID.NPCHit36 || npc.HitSound == SoundID.NPCHit39 || npc.HitSound == SoundID.NPCHit41 || npc.HitSound == SoundID.NPCHit49 || npc.HitSound == SoundID.NPCHit54;
            invalidType |= npc.FullName.Contains("Slime") || npc.FullName.Contains("Elemental") || npc.FullName.Contains("Golem") || npc.FullName.Contains("Dandelion") || npc.FullName.Contains("Skeleton") || npc.FullName.Contains("Skull");
            if (!isModdedAllowed && invalidType) { return; }
            int roughLeather = Roa.Find<ModItem>("RoughLeather").Type;
            int animalLeather = Roa.Find<ModItem>("AnimalLeather").Type;
            if (roughLeather == 0 || animalLeather == 0) return;
            int itemType = npc.aiStyle == NPCAIStyleID.Fighter ? roughLeather : animalLeather;
            LeadingConditionRule dropCondition = new(new SkinningDropCondition());
            dropCondition.OnSuccess(ItemDropRule.Common(itemType, 8));
            npcLoot.Add(dropCondition);
            #endregion
            #region ToxinShard
            if (npc.type == NPCType<InfectedMan>() || npc.type == NPCType<DragonSnatcher>() || npc.type == NPCType<DragonHornet>()){ 
                npcLoot.Add(ItemDropRule.Common(ItemType<ToxinShard>(), 10, 1, 2)); 
            }
            if (npc.type == NPCType<BeePrincess>()) { npcLoot.Add(ItemDropRule.Common(ItemType<ToxinShard>(), 4, 2, 6)); }
            #endregion
            #region UndeadShardDrops
            if (npc.type == NPCType<RadiantBones>() || npc.type == NPCType<RadiantBones2>() || npc.type == NPCType<RadiantBones3>() || npc.type == NPCType<RadiantBones4>() || npc.type == NPCType<Radiator>() || npc.type == NPCType<Radiator2>() || npc.type == NPCType<CobaltSkeleton>() || npc.type == NPCType<RadiantBall>() || npc.type == NPCType<ChargingSpearbones>() || npc.type == NPCType<CenturionShieldless>() || npc.type == NPCType<Centurion2Shieldless>() || npc.type == NPCType<Centurion>() || npc.type == NPCType<Centurion2>() || npc.type == NPCType<Geomancer>() || npc.type == NPCType<ShaftSentinel>() || npc.type == NPCType<SkeletonTrapper>() || npc.type == NPCType<DragonSkull>() || npc.type == NPCType<VampireMiner>() || npc.type == Roa.Find<ModNPC>("Archdruid").Type) {
                npcLoot.Add(ItemDropRule.Common(ItemType<UndeadShard>(), 10, 1, 2));
            }
            if (npc.type == NPCType<Geomancer>()) { 
                npcLoot.Add(ItemDropRule.Common(ItemType<Peridot>(), 8, 1, 2));
                npcLoot.Add(ItemDropRule.Common(ItemType<Zircon>(), 8, 1, 2));
                npcLoot.Add(ItemDropRule.Common(ItemType<Tourmaline>(), 8, 1, 2));
            }
            #endregion
            #region ValhallaAccessories
            if (npc.type == Roa.Find<ModNPC>("DeerSkullHead").Type || npc.type == NPCType<BoneFish>() || npc.type == NPCType<DragonSkull>()) {
                npcLoot.Add(ItemDropRule.Common(ItemType<NecroBuckler>(), 10));
            }
            if (npc.type == NPCType<SpectralElemental>()) { npcLoot.Add(ItemDropRule.Common(ItemType<ChaosFlicker>(), 25)); }
            if (npc.type == NPCType<LihzahrdTrickster>()) { npcLoot.Add(ItemDropRule.Common(ItemType<ChaosFlicker>(), 50)); }
            #endregion
            #region WaterShardDrops
            if (npc.type == NPCType<Orca>()) { npcLoot.Add(ItemDropRule.Common(ItemType<WaterShard>(), 4, 1, 2)); }
            #endregion
            #region SpectralElemental
            if (npc.type == NPCType<SpectralElemental>()) { npcLoot.Add(ItemDropRule.Common(ItemType<ArcaneShard>(), 8)); }
            #endregion
        }
    }
}