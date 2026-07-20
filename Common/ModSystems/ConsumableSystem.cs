using System.Collections.Generic;
using Avalon.Items.Consumables;
using Consolaria.Content.Items.Summons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Common.ModConfigs;
using ValhallaMod.Items.Consumable;

namespace Synergia.Common.ModSystems
{
    public class Consystem : GlobalItem
    {
        private static MechanicsConfig _config;

        public override void Load()
        {
            _config = ModContent.GetInstance<MechanicsConfig>();
        }
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            if (_config == null || !_config.EnableUnconsumableSummons)
                return false;
            var validItemTypes = new[]
            {
            ItemID.SlimeCrown,
            ItemID.SuspiciousLookingEye,
            ItemID.WormFood,
            ItemID.BloodySpine,
            ItemID.Abeemination,
            ItemID.DeerThing,
            ItemID.QueenSlimeCrystal,
            ItemID.MechanicalEye,
            ItemID.MechanicalWorm,
            ItemID.MechanicalSkull,
            ItemID.GoblinBattleStandard,
            ItemID.CelestialSigil,
            ItemID.MechdusaSummon,
            ItemID.SnowGlobe,
            ItemID.PirateMap,
            ItemID.PumpkinMoonMedallion,
            ItemID.NaughtyPresent,
            ItemID.SolarTablet,
            ItemID.BloodMoonStarter,
            ModContent.ItemType<HeavensSeal>(),
            ModContent.ItemType<SuspiciousLookingSkull>(),
            ModContent.ItemType<ShinyBait>(),
            ModContent.ItemType<SuspiciousLookingEgg>(),
            ModContent.ItemType<InfestedCarcass>(),
            ModContent.ItemType<CursedStuffing>(),
            ModContent.ItemType<DesertHorn>(),
        };

            var sus = item.ToolTip;

            foreach (var validItemType in validItemTypes)
                if (item.type == validItemType)
                    return true;

            return false;
        }


        public override void SetDefaults(Item item)
        {
            if (_config == null || !_config.EnableUnconsumableSummons)
                return;
            item.consumable = false;
        }

    }

}