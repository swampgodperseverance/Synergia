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
            if (primeRework.TryFind<ModItem>("SublimeStellarSling", out ModItem SSS))
            {
                Changes[SSS.Type] = item =>
                {
                    item.damage -= 15;
                    item.useAnimation += 4;
                    item.useTime += 4;
                };
            }
            if (primeRework.TryFind<ModItem>("DoubleTrouble", out ModItem DoubleTrouble))
            {
                Changes[DoubleTrouble.Type] = item =>
                {
                    item.useAnimation += 3;
                    item.useTime += 3;
                    item.damage -= 14;
                };
            }
            if (primeRework.TryFind<ModItem>("FlinxForceWalkieTalkie", out ModItem FlinxForceWalkieTalkie))
            {
                Changes[FlinxForceWalkieTalkie.Type] = item =>
                {
                    item.damage -= 13;
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
    public class MechBossChanges : GlobalNPC
    {
        private static Mod primeRework;

        public override void Load()
        {
            ModLoader.TryGetMod("PrimeRework", out primeRework);
        }

        public override void SetDefaults(NPC npc)
        {
            if (!ValhallaMod.Systems.DownedBossSystem.downedEmperorBoss)
                return;

            bool isMechBoss =
                npc.type == NPCID.TheDestroyer ||
                npc.type == NPCID.TheDestroyerBody ||
                npc.type == NPCID.TheDestroyerTail ||
                npc.type == NPCID.Retinazer ||
                npc.type == NPCID.Spazmatism ||
                npc.type == NPCID.SkeletronPrime;

            if (primeRework != null)
            {
                string[] moddedMechs =
                {
                    "Caretaker",
                    "TheTerminator",
                    "SiegeEngine",
                    "Mechclops"
                };

                foreach (string name in moddedMechs)
                {
                    if (primeRework.TryFind<ModNPC>(name, out var modNpc) &&
                        npc.type == modNpc.Type)
                    {
                        isMechBoss = true;
                        break;
                    }
                }
            }

            if (isMechBoss)
            {
                npc.lifeMax = (int)(npc.lifeMax * 1.15f);
            }
        }
    }
}