using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Synergia.Content.NPCs.Dungeon;

namespace Synergia.Common.GlobalNPCs
{
    public class SparkySpawn : GlobalNPC
    {
        static readonly HashSet<int> CantSpawnSparky = [
            NPCID.DungeonSpirit, ModContent.NPCType<Sparky>()
        ];
        public override void OnKill(NPC npc)
        {
            if (!npc.HasValidTarget || Main.netMode == NetmodeID.MultiplayerClient)
                return;

            int range = Main.expertMode ? 9 : 13;
            bool canSpawn = Main.player[npc.target].RollLuck(range) == 0;

            int tileX = (int)(npc.Center.X / 16);
            int tileY = (int)(npc.Center.Y / 16);
            int wallType = Main.tile[tileX, tileY].WallType;
            bool unsafeWall = Main.wallDungeon[wallType];

            if (unsafeWall && canSpawn && Main.hardMode && Main.player[npc.target].ZoneDungeon && NPC.downedPlantBoss &&
                npc.lifeMax > 100 && !npc.SpawnedFromStatue && !CantSpawnSparky.Contains(npc.type)
                )
            {
                NPC.NewNPC(
                    npc.GetSource_FromAI(),
                    (int)npc.Center.X,
                    (int)npc.Center.Y,
                    ModContent.NPCType<Sparky>());
            }
        }
    }
}
