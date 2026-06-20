using System;
using System.Collections.Generic;
using Avalon.Items.Weapons.Magic.Hardmode.AquaImpact;
using Avalon.Items.Weapons.Magic.Hardmode.Boomlash;
using Avalon.Items.Weapons.Magic.Hardmode.DevilsScythe;
using Avalon.Items.Weapons.Magic.Hardmode.MagicGrenade;
using Avalon.Items.Weapons.Magic.Hardmode.Sunstorm;
using Avalon.Items.Weapons.Magic.PreHardmode.ChaosTome;
using Avalon.Items.Weapons.Magic.PreHardmode.Smogscreen;
using Avalon.Items.Weapons.Melee.Hardmode.CraniumCrusher;
using Avalon.Items.Weapons.Melee.Hardmode.DarklightLance;
using Avalon.Items.Weapons.Melee.Hardmode.Starstorm;
using Avalon.Items.Weapons.Melee.PreHardmode.AeonsEternity;
using Avalon.Items.Weapons.Melee.PreHardmode.SanguineKatana;
using Avalon.Items.Weapons.Ranged.Hardmode.CrystalTomahawk;
using Avalon.Items.Weapons.Ranged.PreHardmode.Boompipe;
using Avalon.Items.Weapons.Ranged.PreHardmode.EggCannon;
using Avalon.Items.Weapons.Summon.Hardmode.Gastropod;
using Bismuth.Content.Items.Weapons.Assassin;
using Bismuth.Content.Items.Weapons.Magical;
using Bismuth.Content.Items.Weapons.Melee;
using Bismuth.Content.Items.Weapons.Ranged;
using Bismuth.Content.Items.Weapons.Throwing;
using Consolaria.Content.Items.Weapons.Ammo;
using Consolaria.Content.Items.Weapons.Melee;
using Consolaria.Content.Items.Weapons.Ranged;
using NewHorizons.Content.Items.Weapons.Magic;
using NewHorizons.Content.Items.Weapons.Melee;
using NewHorizons.Content.Items.Weapons.Ranged;
using NewHorizons.Content.Items.Weapons.Throwing;
using NewHorizons.Content.Projectiles.Melee;
using Synergia.Content.Projectiles.Reworks.Reworks2;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Weapons.Magic.Arcana;
using ValhallaMod.Items.Weapons.Magic.Lanterns;
using ValhallaMod.Items.Weapons.Magic.Music;
using ValhallaMod.Items.Weapons.Magic.Staffs;
using ValhallaMod.Items.Weapons.Magic.Thrown;
using ValhallaMod.Items.Weapons.Magic.Tomes;
using ValhallaMod.Items.Weapons.Melee.ChannelMelee;
using ValhallaMod.Items.Weapons.Melee.Glaives;
using ValhallaMod.Items.Weapons.Melee.Knives;
using ValhallaMod.Items.Weapons.Melee.Shortswords;
using ValhallaMod.Items.Weapons.Melee.Spears;
using ValhallaMod.Items.Weapons.Melee.Swords;
using ValhallaMod.Items.Weapons.Ranged.Bows;
using ValhallaMod.Items.Weapons.Ranged.DartGuns;
using ValhallaMod.Items.Weapons.Ranged.Guns;
using ValhallaMod.Items.Weapons.Ranged.Javelins;
using ValhallaMod.Items.Weapons.Ranged.Launchers;
using ValhallaMod.Items.Weapons.Ranged.Longbows;
using ValhallaMod.Items.Weapons.Ranged.RocketLaunchers;
using ValhallaMod.Items.Weapons.Ranged.Thrown;
using ValhallaMod.Items.Weapons.Summon.Minions;
using ValhallaMod.Items.Weapons.Summon.Sentries;
using ValhallaMod.Items.Weapons.Summon.Whips;

namespace Synergia.Common.GlobalItems.Changes
{
    public class BalanceEditor : GlobalItem
    {
        internal static bool Disabled = false;
        private static Dictionary<int, Action<Item>> Changes;

        public override void Load()
        {
            int chemicalPrisonerType =
                ModLoader.GetMod("RoA")?.Find<ModItem>("ChemicalPrisoner")?.Type ?? 0;

            int ZapperType =
                ModLoader.GetMod("RoA")?.Find<ModItem>("MercuriumZipper")?.Type ?? 0;
            int kamotype =
                 ModLoader.GetMod("NewHorizons")?.Find<ModItem>("BoneKama")?.Type ?? 0;
            int arterialSptayType =
                ModLoader.GetMod("RoA")?.Find<ModItem>("ArterialSpray")?.Type ?? 0;
            int ravensEyeType =
                ModLoader.GetMod("RoA")?.Find<ModItem>("RavensEye")?.Type ?? 0;
            int RodStreamType =
                ModLoader.GetMod("RoA")?.Find<ModItem>("RodOfTheStream")?.Type ?? 0;
            int RodShockType =
                ModLoader.GetMod("RoA")?.Find<ModItem>("RodOfTheShock")?.Type ?? 0;
            int StarFusionType =
               ModLoader.GetMod("RoA")?.Find<ModItem>("StarFusion")?.Type ?? 0;


            Changes = new()
            {
                [ItemID.TerraBlade] = item =>
                {
                    item.damage -= 20;
                    item.useAnimation -= 3;
                },
                [ItemID.Flamarang] = item =>
                {
                    item.damage -= 12;
                },

                [ModContent.ItemType<Radiance>()] = item =>
                {
                    item.damage += 21;
                },

                [ModContent.ItemType<NanoStar>()] = item =>
                {
                    item.damage -= 24;
                    item.useAnimation += 10;
                    item.useTime += 10;
                },
                [ModContent.ItemType<TerraGlaive>()] = item =>
                {
                    item.damage -= 10;
                    item.crit -= 8;
                    item.useAnimation += 20;
                    item.useTime += 20;
                },
                [ModContent.ItemType<TerraSpear>()] = item =>
                {
                    item.damage -= 40;
                    item.useAnimation += 15;
                    item.useTime += 15;
                },
                [ModContent.ItemType<CraniumCrusher>()] = item =>
                {
                    item.damage -= 34;
                    item.useAnimation += 10;
                    item.useTime += 10;
                },
                [ModContent.ItemType<Beesting>()] = item =>
                {
                    item.damage -= 28;
                    item.useAnimation += 5;
                    item.useTime += 5;

                },
                [chemicalPrisonerType] = item =>
                {
                    item.damage -= 5;
                    item.useAnimation += 4;
                    item.useTime += 4;
                },
                [ModContent.ItemType<Avalon.Items.Weapons.Ranged.PreHardmode.EggCannon.EggCannon>()] = item =>
                {
                    item.damage -= 6;
                    item.useAnimation += 2;
                    item.useTime += 2;
                },
                [ModContent.ItemType<Smogscreen>()] = item =>
                {
                    item.useAnimation += 8;
                    item.useTime += 8;
                    item.mana += 6;
                },
                [ModContent.ItemType<StarWand>()] = item =>
                {
                    item.useAnimation += 10;
                    item.useTime += 10;
                    item.mana += 1;
                },
                [ModContent.ItemType<WoodenStaff>()] = item =>
                {
                    item.mana += 2;
                    item.damage -= 2;
                },
                [ModContent.ItemType<SporeStaff>()] = item =>
                {
                    item.mana += 4;
                    item.damage -= 6;
                },
                [ModContent.ItemType<IluminantBatbow>()] = item =>
                {
                    item.damage += 20;
                },
                [ModContent.ItemType<Tarrifier>()] = item =>
                {
                    item.mana += 4;
                    item.damage += 4;
                },
                [ModContent.ItemType<GraniteCharger>()] = item =>
                {
                    item.mana += 4;
                    item.damage += 10;
                },
                [ModContent.ItemType<ChaosTome>()] = item =>
                {
                    item.useAnimation += 5;
                    item.useTime += 5;
                },
                [arterialSptayType] = item =>
                {
                    item.useAnimation += 3;
                    item.useTime += 3;
                },
                [ModContent.ItemType<Granitbur>()] = item =>
                {
                    item.useTime += 10;
                },
                [StarFusionType] = item =>
                {
                    item.damage += 2;
                },
                [RodStreamType] = item =>
                {
                    item.useAnimation += 6;
                    item.useTime += 6;
                    item.mana += 15;
                    item.damage -= 6;
                },
                [RodShockType] = item =>
                {
                    item.mana += 5;
                },
                [ZapperType] = item =>
                {
                    item.damage -= 5;
                },
                [ModContent.ItemType<Scorcher>()] = item =>
                {
                    item.useAnimation -= 3;
                    item.useTime -= 3;
                    item.damage -= 5;
                    item.mana += 5;
                },

                [ModContent.ItemType<HandicraftedBlunderbuss>()] = item =>
                {
                    item.useAnimation += 8;
                    item.useTime += 8;
                },
                [ModContent.ItemType<PaintRoller>()] = item =>
                {
                    item.useAnimation -= 3;
                    item.useTime -= 3;
                    item.damage -= 5;
                },
                [ModContent.ItemType<ValhalliteSword>()] = item =>
                {
                    item.damage -= 8;
                },
                [ItemID.TentacleSpike] = item =>
                {
                    item.useAnimation -= 3;
                    item.useTime -= 3;
                    item.damage -= 12;
                },
                [ItemID.ThunderSpear] = item =>
                {
                    item.useAnimation -= 2;
                    item.useTime -= 2;
                    item.damage -= 5;
                },
                [ModContent.ItemType<Bulbasword>()] = item =>
                {
                    item.useAnimation -= 3;
                    item.useTime -= 3;
                    item.damage -= 8;
                },
                [ModContent.ItemType<SanguineKatana>()] = item =>
                {
                    item.useAnimation -= 2;
                    item.useTime -= 2;
                    item.damage -= 6;
                },
                [ModContent.ItemType<Naginata>()] = item =>
                {
                    item.useAnimation -= 2;
                    item.useTime -= 2;
                    item.damage -= 13;
                },
                [ModContent.ItemType<Narsil>()] = item =>
                {
                    item.damage += 18;
                },
                [ModContent.ItemType<NimbusStaff>()] = item =>
                {
                    item.useAnimation -= 3;
                    item.useTime -= 3;
                    item.damage -= 7;
                },
                [ModContent.ItemType<WidowsWhip>()] = item =>
                {
                    item.useAnimation += 3;
                    item.useTime += 3;
                    item.damage -= 28;
                },
                [ModContent.ItemType<GastropodStaff>()] = item =>
                {
                    item.useAnimation += 3;
                    item.useTime += 3;
                    item.damage -= 7;
                },
                [ModContent.ItemType<ParrotStaff>()] = item =>
                {
                     item.damage += 4;
                },

                
                [ModContent.ItemType<GolemSentryStaff>()] = item =>
                {
                    item.damage -= 20;
                },
                [ModContent.ItemType<Prominence>()] = item =>
                {
                    item.damage -= 15;
                },
                [ModContent.ItemType<Graverobber>()] = item =>
                {
                    item.useAnimation += 3;
                    item.useTime += 3;
                    item.damage -= 1;
                },
                [ModContent.ItemType<Graverobber>()] = item =>
                {
                    item.reuseDelay -= 5;
                },
                [ModContent.ItemType<HuntressBow>()] = item =>
                {
                    item.damage -= 9;
                },
                [ModContent.ItemType<Heat>()] = item =>
                {
                    item.reuseDelay += 10;
                },
                [ModContent.ItemType<TheSeaBeast>()] = item =>
                {
                    item.reuseDelay += 5;
                    item.damage -= 1;
                },
                [ModContent.ItemType<Sharanga>()] = item =>
                {
                    item.useAnimation += 10;
                    item.useTime += 10;
                },
                [ModContent.ItemType<BoneDartgun>()] = item =>
                {
                    item.damage += 4;
                },
                [ModContent.ItemType<Boompipe>()] = item =>
                {
                    item.damage += 9;
                },
                [ModContent.ItemType<CorrodeLongbow>()] = item =>
                {
                    item.damage += 8;
                },
                [ModContent.ItemType<Rifle76>()] = item =>
                {
                    item.damage += 8;
                },
                [ModContent.ItemType<Scarabine>()] = item =>
                {
                    item.damage += 6;
                },
                [ModContent.ItemType<PlagueRifleH>()] = item =>
                {
                    item.damage -= 6;
                    item.useAnimation += 2;
                    item.useTime += 2;
                },
                [ModContent.ItemType<JadeBalista>()] = item =>
                {
                    item.damage += 21;
                    item.useAnimation -= 3;
                    item.useTime -= 3;
                    item.reuseDelay -= 10;
                },
                [ModContent.ItemType<HuntersHunch>()] = item =>
                {
                    item.damage += 15;
                },
                [ModContent.ItemType<Beepeater>()] = item =>
                {
                    item.damage -= 12;
                },
                [ModContent.ItemType<HarbingerOfDawn>()] = item =>
                {
                    item.damage += 60;
                    item.useAnimation -= 5;
                    item.useTime -= 5;
                },
                [ModContent.ItemType<VolcanicRepeater>()] = item =>
                {
                    item.damage -= 35;
                },
                [ModContent.ItemType<Splatterink>()] = item =>
                {
                    item.shootSpeed -= 6;
                },
                [ModContent.ItemType<ScissorsKnives>()] = item =>
                {
                    item.damage -= 13;
                },
                [ModContent.ItemType<AeonsEternity>()] = item =>
                {
                    item.damage -= 14;
                },
                [ModContent.ItemType<ConiferousFan>()] = item =>
                {
                    item.useAnimation += 7;
                    item.useTime += 7;
                    item.damage -= 9;
                },
                [ModContent.ItemType<Starstorm>()] = item =>
                {
                    item.useAnimation += 5;
                    item.useTime += 5;
                    item.damage += 19;
                },
                [ModContent.ItemType<DarklightLance>()] = item =>
                {
                    item.useAnimation += 5;
                    item.useTime += 5;
                    item.damage -= 31;
                },
                [ModContent.ItemType<MagicGrenade>()] = item =>
                {
                    item.damage -= 30;
                },
                [ModContent.ItemType<GoldenBomb>()] = item =>
                {
                    item.damage += 30;
                },
                [ModContent.ItemType<JadeSpear>()] = item =>
                {
                    item.damage += 12;
                },
                [ModContent.ItemType<TrueGungnir>()] = item =>
                {
                    item.useAnimation += 8;
                    item.useTime += 8;
                    item.shootSpeed -= 10;
                    item.damage -= 18;
                },
                [ModContent.ItemType<Tonbogiri>()] = item =>
                {
                    item.shootSpeed -= 18;
                },
                [ModContent.ItemType<SolarWind>()] = item =>
                {
                    item.shootSpeed -= 7;
                    item.damage += 25;
                },
                [ModContent.ItemType<CarrotDagger>()] = item =>
                {
                    item.damage += 3;
                },
                [ModContent.ItemType<Typhoon>()] = item =>
                {
                    item.damage += 4;
                },
                [ModContent.ItemType<DemoniteGlaive>()] = item =>
                {
                    item.damage += 8;
                },
                [ModContent.ItemType<BloodSpiller>()] = item =>
                {
                    item.damage += 2;
                },
                [ModContent.ItemType<CrimiteGlaive>()] = item =>
                {
                    item.damage += 7;
                },
                [ModContent.ItemType<NewHorizons.Content.Items.Weapons.Throwing.NightGlaive>()] = item =>
                {
                    item.damage += 6;
                    item.shootSpeed -= 5;
                },
                [ModContent.ItemType<DungeonGlaive>()] = item =>
                {
                    item.damage += 7;
                },
                [kamotype] = item =>
                {
                    item.damage += 5;
                },
                [ModContent.ItemType<SpicySauce>()] = item =>
                {
                    item.damage -= 13;
                    item.useAnimation += 12;
                    item.useTime += 12;
                    item.shootSpeed -= 3;
                },
                [ModContent.ItemType<CrystalTomahawk>()] = item =>
                {
                    item.damage -= 10;
                    item.useAnimation += 13;
                    item.useTime += 13;
                },
                [ModContent.ItemType<OrcishJavelin>()] = item =>
                {
                    item.damage += 5;
                },
                [ModContent.ItemType<CorrodeShuriken>()] = item =>
                {
                    item.damage -= 13;
                },
                [ModContent.ItemType<ValhallaMod.Items.Weapons.Melee.Glaives.NightGlaive>()] = item =>
                {
                    item.damage -= 8;
                },
                [ModContent.ItemType<JaguarsChakram>()] = item =>
                {
                    item.damage += 15;
                },
                [ModContent.ItemType<SharkKnife>()] = item =>
                {
                    item.damage -= 17;
                },
                [ModContent.ItemType<TrueNightGlaive>()] = item =>
                {
                    item.damage += 16;
                },
                [ModContent.ItemType<Carnwennan>()] = item =>
                {
                    item.shootSpeed += 3;
                },
                [ModContent.ItemType<SolarDisk>()] = item =>
                {
                    item.damage += 15;
                },
                [ModContent.ItemType<NewHorizons.Content.Items.Weapons.Throwing.ChlorophyteJavelin>()] = item =>
                {
                    item.shootSpeed += 10;
                },
                [ModContent.ItemType<JaguarsDagger>()] = item =>
                {
                    item.useAnimation += 6;
                    item.useTime += 6;
                },
                [ModContent.ItemType<NebulaRod>()] = item =>
                {
                    item.useAnimation += 18;
                    item.useTime += 18;
                    item.mana += 13;
                },
                [ModContent.ItemType<JadeArcanum>()] = item =>
                {
                    item.reuseDelay -= 30;
                    item.useAnimation -= 24;
                },
                [ModContent.ItemType<SpaceCowboy>()] = item =>
                {
                    item.damage += 30;
                },
                [ModContent.ItemType<DarkVolley>()] = item =>
                {
                    item.useAnimation += 10;
                    item.useTime -= 15;
                },
                [ModContent.ItemType<MagicWand>()] = item =>
                {
                    item.useAnimation += 4;
                    item.useTime += 4;
                    item.damage -= 6;
                },
                [ModContent.ItemType<AquaImpact>()] = item =>
                {
                    item.useAnimation -= 10;
                    item.useTime -= 10;
                },
                [ModContent.ItemType<LeafShield>()] = item =>
                {
                    item.damage += 15;
                },
                [ModContent.ItemType<ShellStaff>()] = item =>
                {
                    item.damage += 15;
                },
                [ModContent.ItemType<Avalon.Items.Weapons.Magic.Hardmode.DevilsScythe.DevilsScythe>()] = item =>
                {
                    item.damage -= 70;
                    item.useAnimation += 14;
                    item.useTime += 14;
                },
                [ModContent.ItemType<GhostVenomStaff>()] = item =>
                {
                    item.damage -= 18;
                    item.useAnimation += 4;
                    item.useTime += 4;
                },
                [ModContent.ItemType<Boomlash>()] = item =>
                {
                    item.damage -= 26;
                    item.useAnimation += 10;
                    item.useTime += 10;
                },
                [ModContent.ItemType<Sunstorm>()] = item =>
                {
                    item.damage += 25;
                },
                [ModContent.ItemType<Unlighter>()] = item =>
                {
                    item.damage += 11;
                    item.useAnimation -= 36;
                    item.useTime -= 36;
                },

            };
        }

        public override void SetDefaults(Item item)
        {
            if (Disabled || Changes == null) return;
            if (Changes.TryGetValue(item.type, out var edit))
                edit(item);
        }
    }
}