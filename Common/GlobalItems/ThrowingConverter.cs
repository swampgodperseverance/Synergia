using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;

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

		public override void SetDefaults(Item item)
		{
			if (VanillaBoomerangs.Contains(item.type))
			{
				item.DamageType = DamageClass.Throwing;
			}
		}
	}
}