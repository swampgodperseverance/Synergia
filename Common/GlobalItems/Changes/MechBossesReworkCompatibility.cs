using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalItems.Changes
{
    public class MechBossesReworkCompatEditor : GlobalItem
    {
        private static Dictionary<int, Action<Item>> Changes;
        private static Mod primeRework;

        public override void Load()
        {
            Changes = new Dictionary<int, Action<Item>>();

            if (ModLoader.TryGetMod("PrimeRework", out primeRework))
            {
                SetupPrimeReworkChanges();
            }

        }

        private void SetupPrimeReworkChanges()
        {
            if (primeRework.TryFind<ModItem>("LaserStar", out ModItem laserStar))
            {
                Changes[laserStar.Type] = item =>
                {
                    item.useAnimation += 6;
                    item.useTime += 6;
                    item.damage -= 16;
                };
            }

            if (primeRework.TryFind<ModItem>("Finis", out ModItem finis))
            {
                Changes[finis.Type] = item =>
                {
                    item.useAnimation += 6;
                    item.useTime += 6;
                    item.damage -= 31;
                };
            }

            if (primeRework.TryFind<ModItem>("Exitium", out ModItem exitium))
            {
                Changes[exitium.Type] = item =>
                {
                    item.useAnimation += 8;
                    item.useTime += 8;
                    item.damage -= 18;
                };
            }

            if (primeRework.TryFind<ModItem>("RepurposedBrainRemote", out ModItem repurposedBrainRemote))
            {
                Changes[repurposedBrainRemote.Type] = item =>
                {
                    item.damage -= 23;
                };
            }

            if (primeRework.TryFind<ModItem>("PlasmaPistol", out ModItem plasmaPistol))
            {
                Changes[plasmaPistol.Type] = item =>
                {
                    item.damage -= 14;
                    item.useAnimation += 8;
                    item.useTime += 8;
                };
            }
        }

        public override void SetDefaults(Item item)
        {
            if (Changes != null && Changes.TryGetValue(item.type, out var edit))
                edit(item);
        }

        public override void Unload()
        {
            Changes?.Clear();
            Changes = null;
            primeRework = null;
        }
    }
}