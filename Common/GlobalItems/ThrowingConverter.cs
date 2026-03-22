using Consolaria.Content.Items.Weapons.Melee;
using Consolaria.Content.Items.Weapons.Ranged;
using Consolaria.Content.Items.Weapons.Throwing;
using NewHorizons.Content.Items.Weapons.Ranged;
using Starforgedclassic.Content.Weapons.Solarang;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Garden;
using ValhallaMod.Items.Weapons.Blood;
using ValhallaMod.Items.Weapons.Melee.Boomerangs;
using ValhallaMod.Items.Weapons.Melee.Glaives;
using ValhallaMod.Items.Weapons.Ranged.Javelins;
using ValhallaMod.Items.Weapons.Ranged.Thrown;

namespace Synergia.Common.GlobalItems
{
	public class VanillaThrowingConverter : GlobalItem
	{

		private static readonly HashSet<int> VanillaBoomerangs = new()
		{
			//Terraria
			ItemID.EnchantedBoomerang,
			ItemID.WoodenBoomerang,
			ItemID.IceBoomerang,
			ItemID.FruitcakeChakram,
			ItemID.ThornChakram,
			ItemID.BloodyMachete,
			ItemID.Flamarang,
			ItemID.LightDisc,
			ItemID.Bananarang,
			ItemID.PossessedHatchet,
			ItemID.ScourgeoftheCorruptor,
			ItemID.Anchor,
			ItemID.Trimarang,
			ItemID.Shroomerang,
			ItemID.FlyingKnife,
			ItemID.ShadowFlameKnife,
			ItemID.CombatWrench,
			ItemID.PaperAirplaneA,
			ItemID.PaperAirplaneB,
			ItemID.BouncingShield,
			ItemID.Grenade,
            ItemID.BouncyGrenade,
			ItemID.PartyGirlGrenade,
			ItemID.StickyGrenade,
			ItemID.Beenade,	


			//Consolaria
			ItemType<AlbinoMandible>(),
			ItemType<SpicySauce>(),
			ItemType<Squib>(),

			//Starforged Classic
			ItemType<Solarang>(),

			//ValhallaMod
			ItemType<OmegaDisc>(),
			ItemType<SnowGlaive>(),
			ItemType<TerraGlaive>(),
			ItemType<DungeonGlaive>(),
			ItemType<SunJavelin>(),
			ItemType<IchorCoctail>(),
			ItemType<ExplosiveCharge>(),
			ItemType<BigBeeNade>(),
			ItemType<CursedCoctail>(),
			ItemType<StalloyScrew>(),
			ItemType<ChlorophyleGlaive>(),
			ItemType<NightGlaive>(),
			ItemType<CrimiteGlaive>(),
			ItemType<DemoniteGlaive>(),
			ItemType<TrueLightDisc>(),
			ItemType<TrueNightGlaive>(),
			ItemType<Garlic>(),
			ItemType<SpiderKnife>(),
			ItemType<CarrotDagger>(),
			ItemType<CorrodeShuriken>(),
			ItemType<CactusKnife>(),
			ItemType<CactusStar>(),
            ItemType<ChitinShuriken>(),
            ItemType<ValhallaMod.Items.Weapons.Ranged.Thrown.IncendiaryGrenade>(),
            ItemType<TeethBreaker>(),
            ItemType<HuskarJavelin>(),
            ItemType<AzraelsHeartstopper>(),
            ItemType<Sufferang>(),
            ItemType<Pumpkinade>(),

			//Horizon
			ItemType<NewHorizons.Content.Items.Weapons.Ranged.IncendiaryGrenade>(),
            ItemType<CrystalGrenade>(),
        };

		private static Mod avalon = ModLoader.GetMod("Avalon");
		private static Mod bismuth = ModLoader.GetMod("Bismuth");
        private static Mod roa = ModLoader.GetMod("RoA");

        public override void SetDefaults(Item item)
		{
			if (VanillaBoomerangs.Contains(item.type))
			{
				item.DamageType = DamageClass.Throwing;
				return;
			}

			if (avalon != null && item.type == avalon.Find<ModItem>("TetanusChakram").Type)
			{
				item.DamageType = DamageClass.Throwing;
			}
            if (roa != null && item.type == roa.Find<ModItem>("SlipperyGrenade").Type)
            {
                item.DamageType = DamageClass.Throwing;
            }
            if (avalon != null && item.type == avalon.Find<ModItem>("Shurikerang").Type)
			{
				item.DamageType = DamageClass.Throwing;
			}
			if (avalon != null && item.type == avalon.Find<ModItem>("EnchantedShuriken").Type)
			{
				item.DamageType = DamageClass.Throwing;
			}
			if (avalon != null && item.type == avalon.Find<ModItem>("RottenApple").Type)
			{
				item.DamageType = DamageClass.Throwing;
			}
			if (avalon != null && item.type == avalon.Find<ModItem>("CrystalTomahawk").Type)
			{
				item.DamageType = DamageClass.Throwing;
			}
			if (avalon != null && item.type == avalon.Find<ModItem>("Icicle").Type)
			{
				item.DamageType = DamageClass.Throwing;
			}
			if (avalon != null && item.type == avalon.Find<ModItem>("VirulentScythe").Type)
			{
				item.DamageType = DamageClass.Throwing;
			}
			if (bismuth != null && item.type == bismuth.Find<ModItem>("Prominence").Type)
			{
				item.DamageType = DamageClass.Summon;
			}

		}
	}

}
