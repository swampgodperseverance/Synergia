using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using ValhallaMod;
using ValhallaMod.Items.Weapons.Melee.Swords;
using NewHorizons.Content.Items.Weapons.Throwing;
using Synergia.Content.Items.Weapons.Throwing;
using ValhallaMod.Items.Weapons.Melee.Glaives;
using ValhallaMod.Items.Weapons.Melee.Spears;
using Avalon.Items.Weapons.Melee.Hardmode.CraniumCrusher;

namespace Synergia.Common.GlobalItems.Changes
{
    public class BalanceEditor : GlobalItem
    {
        private static Dictionary<int, Action<Item>> Changes;

        public override void Load()
        {
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

            };
        }

        public override void SetDefaults(Item item)
        {
            if (Changes.TryGetValue(item.type, out var edit))
                edit(item);
        }
    }
}