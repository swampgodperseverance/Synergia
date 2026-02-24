using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using ValhallaMod;
using ValhallaMod.Items.Weapons.Melee.Swords;
using NewHorizons.Content.Items.Weapons.Throwing;
using Synergia.Content.Items.Weapons.Throwing;

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
                [ModContent.ItemType<Flarion>()] = item =>
                {
                    item.damage -= 90;
                    item.useAnimation += 15;
                    item.useTime += 15;
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