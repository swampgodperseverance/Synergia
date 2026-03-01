using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Bismuth.Content.NPCs;

namespace Synergia.Common.GlobalNPCs.Changes
{
    public class NPCBalanceEditor : GlobalNPC
    {
        private static Dictionary<int, Action<NPC>> Changes;

        public override void Load()
        {
            Changes = new()
            {
                [NPCID.Zombie] = npc =>
                {
                    npc.lifeMax += 20;
                    npc.damage += 10;
                    npc.defense += 4;
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

                [ModContent.NPCType<Orc>()] = npc =>
                {
                    npc.lifeMax += 110;
                    npc.damage += 10;
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
            };
        }

        public override void SetDefaults(NPC npc)
        {
            if (Changes.TryGetValue(npc.type, out var edit))
                edit(npc);
        }
    }
}