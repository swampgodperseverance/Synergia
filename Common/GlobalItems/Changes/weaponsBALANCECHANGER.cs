using System;
using System.Collections.Generic;
using Avalon.Items.Weapons.Magic.PreHardmode.ChaosTome;
using Avalon.Items.Weapons.Magic.PreHardmode.Smogscreen;
using Avalon.Items.Weapons.Melee.Hardmode.CraniumCrusher;
using Avalon.Items.Weapons.Melee.PreHardmode.SanguineKatana;
using Avalon.Items.Weapons.Ranged.PreHardmode.EggCannon;
using Bismuth.Content.Items.Weapons.Magical;
using NewHorizons.Content.Items.Weapons.Magic;
using NewHorizons.Content.Items.Weapons.Ranged;
using NewHorizons.Content.Items.Weapons.Throwing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Weapons.Magic.Staffs;
using ValhallaMod.Items.Weapons.Melee.Glaives;
using ValhallaMod.Items.Weapons.Melee.Shortswords;
using ValhallaMod.Items.Weapons.Melee.Spears;
using ValhallaMod.Items.Weapons.Melee.Swords;
using ValhallaMod.Items.Weapons.Ranged.Bows;

namespace Synergia.Common.GlobalItems.Changes
{
    public class BalanceEditor : GlobalItem
    {
        private static Dictionary<int, Action<Item>> Changes;

        public override void Load()
        {
            int chemicalPrisonerType =
                ModLoader.GetMod("RoA")?.Find<ModItem>("ChemicalPrisoner")?.Type ?? 0;
            int arterialSptayType =
                ModLoader.GetMod("RoA")?.Find<ModItem>("ArterialSpray")?.Type ?? 0;
            int ravensEyeType =
                ModLoader.GetMod("RoA")?.Find<ModItem>("RavensEye")?.Type ?? 0;
            int RodStreamType =
                ModLoader.GetMod("RoA")?.Find<ModItem>("RodOfTheStream")?.Type ?? 0;
            int RodShockType =
                ModLoader.GetMod("RoA")?.Find<ModItem>("RodOfTheShock")?.Type ?? 0;


            Changes = new()
            {
                [ItemID.TerraBlade] = item =>
                {
                    item.damage -= 20;
                    item.useAnimation -= 3;
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
                [ModContent.ItemType<EggCannon>()] = item =>
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
                    item.damage += 26;
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
            };
        }

        public override void SetDefaults(Item item)
        {
            if (Changes.TryGetValue(item.type, out var edit))
                edit(item);
        }
    }
}