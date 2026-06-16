using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Bismuth.Content.NPCs;
using Avalon.NPCs.Bosses.PreHardmode;
using ValhallaMod.NPCs.Snowman;
using ValhallaMod.NPCs.Emperor;
using ValhallaMod.NPCs.Pirate;
using ValhallaMod.NPCs.Jungle;
using Synergia.Common.GlobalNPCs.AI;
using Consolaria.Content.NPCs.Bosses.Ocram;

namespace Synergia.Common.GlobalNPCs.Changes
{
    public class NPCBalanceEditor : GlobalNPC
    {
        private static Dictionary<int, Action<NPC>> Changes;

        public override void Load()
        {
            int lothorType =
                 ModLoader.GetMod("RoA")?.Find<ModNPC>("Lothor")?.Type ?? 0;

            int GrimType =
                 ModLoader.GetMod("RoA")?.Find<ModNPC>("GrimDruid")?.Type ?? 0;

            int ravencallerType =
                 ModLoader.GetMod("RoA")?.Find<ModNPC>("Ravencaller")?.Type ?? 0;

            int lumberjackType =
                 ModLoader.GetMod("RoA")?.Find<ModNPC>("Lumberjack")?.Type ?? 0;

            Changes = new()
            {
                [NPCID.Zombie] = npc =>
                {
                    npc.lifeMax += 20;
                    npc.damage += 10;
                    npc.defense += 4;
                },
                [NPCID.KingSlime] = npc =>
                {
                    npc.lifeMax += 200;
                },
                [NPCID.CultistBoss] = npc =>
                {
                    npc.lifeMax += 20000;
                },
                [NPCID.CultistDragonHead] = npc =>
                {
                    npc.lifeMax += 8000;
                },
                [NPCID.VoodooDemon] = npc =>
                {
                    npc.lifeMax += 70;
                    npc.damage += 10;
                    npc.defense += 4;
                },
                [NPCID.Demon] = npc =>
                {
                    npc.lifeMax += 80;
                    npc.damage += 10;
                    npc.defense += 4;
                },
                [NPCID.LavaSlime] = npc =>
                {
                    npc.lifeMax += 50;
                    npc.damage += 10;
                    npc.defense += 4;
                },
                [NPCID.SkeletonArcher] = npc =>
                {
                    npc.lifeMax += 60;
                    npc.defense += 4;
                },
                [NPCID.ArmoredSkeleton] = npc =>
                {
                    npc.lifeMax += 60;
                    npc.defense += 4;
                },
                [NPCID.MossHornet] = npc =>
                {
                    npc.lifeMax += 60;
                    npc.defense += 4;
                },
                [NPCID.AngryTrapper] = npc =>
                {
                    npc.lifeMax += 60;
                    npc.defense += 4;
                },
                [NPCID.Plantera] = npc =>
                {
                    npc.lifeMax += 20000;
                    npc.defense += 8;
                    npc.damage += 5;
                },
                [NPCID.PlanterasHook] = npc =>
                {
                    npc.lifeMax += 150;
                    npc.defense += 5;
                    npc.damage += 5;
                },
                [NPCID.PlanterasTentacle] = npc =>
                {
                    npc.lifeMax += 150;
                    npc.defense += 5;
                    npc.damage += 5;
                },
                [NPCID.TurtleJungle] = npc =>
                {
                    npc.lifeMax += 60;
                    npc.defense += 4;
                },
                [NPCID.PossessedArmor] = npc =>
                {
                    npc.lifeMax += 200;
                    npc.damage += 4;
                    npc.defense += 8;
                },
                [NPCID.Lavabat] = npc =>
                {
                    npc.lifeMax += 65;
                    npc.damage += 10;
                    npc.defense += 4;
                },
                [NPCID.FireImp] = npc =>
                {
                    npc.lifeMax += 105;
                    npc.damage += 15;
                    npc.defense += 6;
                },
                [NPCID.BoneSerpentHead] = npc =>
                {
                    npc.lifeMax += 180;
                    npc.damage += 12;
                    npc.defense += 7;
                },
                [NPCID.WallofFlesh] = npc =>
                {
                    npc.lifeMax += 600;
                    npc.damage += 10;
                    npc.defense += 10;
                },
                [NPCID.SkeletronHand] = npc =>
                {
                    npc.lifeMax += 200;
                    npc.damage += 8;
                    npc.defense += 5;
                },
                [NPCID.GoblinSummoner] = npc =>
                {
                    npc.lifeMax += 1100;
                    npc.damage += 8;
                    npc.defense += 5;
                },
                [NPCID.PirateShip] = npc =>
                {
                    npc.lifeMax += 1000;
                    npc.defense += 5;
                },
                [NPCID.PirateShipCannon] = npc =>
                {
                    npc.lifeMax += 100;
                },
                [NPCID.SnowBalla] = npc =>
                {
                    npc.lifeMax += 120;
                },
                [NPCID.SnowmanGangsta] = npc =>
                {
                    npc.lifeMax += 150;
                },
                [ModContent.NPCType<PapuanWizard>()] = npc =>
                {
                    npc.lifeMax += 1200;
                },
                [ModContent.NPCType<RunicElemental>()] = npc =>
                {
                    npc.lifeMax += 250;
                },
                [ModContent.NPCType<Orc>()] = npc =>
                {
                    npc.lifeMax += 110;
                    npc.damage += 10;
                },
                [ModContent.NPCType<Banshee>()] = npc =>
                {
                    npc.lifeMax += 150;
                    npc.damage += 2;
                },

                [ModContent.NPCType<OrcDefender>()] = npc =>
                {
                    npc.lifeMax += 140;
                    npc.damage += 15;
                    npc.defense += 12;
                },

                [ModContent.NPCType<OrcWizard>()] = npc =>
                {
                    npc.lifeMax += 160;
                    npc.damage += 10;
                    npc.defense += 5;
                },
                [ModContent.NPCType<RhinoOrc>()] = npc =>
                {
                    npc.lifeMax += 500;
                    npc.damage += 25;
                    npc.defense += 10;
                },
                [ModContent.NPCType<OrcishPortal>()] = npc =>
                {
                    npc.lifeMax += 1100;
                    npc.defense += 15;
                },
                [ModContent.NPCType<Necromant>()] = npc =>
                {
                    npc.defense += 6;
                },
                [ModContent.NPCType<EvilBabaYaga>()] = npc =>
                {
                    npc.lifeMax -= 500; 
                },
                [ModContent.NPCType<DesertBeak>()] = npc =>
                {
                    npc.lifeMax += 600;
                    npc.damage += 6;
                    npc.defense += 2;
                },
                [ModContent.NPCType<ColdFather>()] = npc =>
                {
                    npc.lifeMax += 3000;
                    npc.damage += 10;
                    npc.defense += 5;
                },
                [ModContent.NPCType<ValhallaMod.NPCs.Goblin.VoodooCultist>()] = npc =>
                {
                    npc.lifeMax += 200;
                    npc.damage += 5;
                    npc.defense += 5;
                },
               [ModContent.NPCType<Emperor>()] = npc =>
                {
                    npc.lifeMax += 4000;
                    npc.damage += 10;
                    npc.defense -= 15;
                },
               [ModContent.NPCType<JadeOrb>()] = npc =>
                {
                    npc.lifeMax += 500;
                    //npc.damage += 10;
                    //npc.defense += 4;
                },
                [ModContent.NPCType<InkSlime>()] = npc =>
                {
                    npc.lifeMax += 100;
                    //npc.damage += 10;
                    //npc.defense += 4;
                },
                [ModContent.NPCType<PirateSquid>()] = npc =>
                {
                    npc.lifeMax += 5000;
                    //npc.damage += 10;
                    npc.defense += 6;
                },
                [ModContent.NPCType<SnowmanTrasher>()] = npc =>
                {
                    npc.lifeMax += 200;
                    //npc.damage += 10;
                    npc.defense += 6;
                },
                [ModContent.NPCType<BeePrincess>()] = npc =>
                {
                    npc.lifeMax += 2000;
                    //npc.damage += 10;
                    npc.defense += 6;
                },
                [ModContent.NPCType<Ocram>()] = npc =>
                {
                    npc.lifeMax += 35000;
                    npc.damage += 4;
                    npc.defense += 3;
                },
                [lothorType] = npc =>
                {
                    npc.lifeMax += 500;
                },
                [GrimType] = npc =>
                {
                    npc.lifeMax += 50;
                },
                [ravencallerType] = npc =>
                {
                    npc.lifeMax += 50;
                },
                [lumberjackType] = npc =>
                {
                    npc.lifeMax += 50;
                },
                [ModContent.NPCType<SandWorm>()] = npc =>
                {
                    npc.lifeMax += 200;
                    npc.damage += 2;
                    npc.defense += 1;
                },

            };
        }


        public override void SetDefaults(NPC npc)
        {
            if (Changes.TryGetValue(npc.type, out var edit))
                edit(npc);
        }
    }
}