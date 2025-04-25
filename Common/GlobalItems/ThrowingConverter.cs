using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.Localization;
using static Terraria.ModLoader.ModContent;
using static Terraria.Localization.Language;

using Consolaria.Content.Items.Weapons.Melee;
using Consolaria.Content.Items.Weapons.Ranged;
using Starforgedclassic.Content.Weapons.Solarang;
using ValhallaMod.Items.Weapons.Thrown;
using ValhallaMod.Items.Weapons.Glaive;
using ValhallaMod.Items.Weapons.Javelin;
using ValhallaMod.Items.Garden;

namespace Vanilla.Common.GlobalItems
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

			//Consolaria
			ItemType<AlbinoMandible>(),
			ItemType<SpicySauce>(),

			//Starforged Classic
			ItemType<Solarang>(),

			//ValhallaMod
			ItemType<OmegaDisc>(),
			ItemType<TerraGlaive>(),
			ItemType<DungeonGlaive>(),
			ItemType<SunJavelin>(),
			ItemType<IchorCoctail>(),
			ItemType<ExplosiveCharge>(),
			ItemType<BigBeeNade>(),
			ItemType<CursedCoctail>(),
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
			ItemType<CactusStar>()
		};

		private static Mod avalon = ModLoader.GetMod("Avalon");

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
		}
	}
}