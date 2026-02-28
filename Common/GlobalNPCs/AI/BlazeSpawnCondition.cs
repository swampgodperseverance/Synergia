using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace Synergia.Common.GlobalNPCs.AI
{
    public class BlazeSpawnCondition : GlobalNPC
    {
        //Written this way to crash proof it
        public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.ModNPC?.Mod.Name == "Avalon" && npc.ModNPC?.Name == "Blaze"; 
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (!ModLoader.TryGetMod("Avalon", out Mod avalon))
                return;

            if (!avalon.TryFind("Blaze", out ModNPC blaze))
                return;

            int blazeType = blaze.Type;

            if (spawnInfo.Player.ZoneUnderworldHeight &&
                NPC.downedGolemBoss &&
                Main.invasionType == 0 &&         
                spawnInfo.Player.active)            
            {
                pool[blazeType] = 0.15f;
            }
            else
            {
                pool.Remove(blazeType);
            }

        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.ZoneUnderworldHeight && NPC.downedGolemBoss)
            {
                maxSpawns = (int)(maxSpawns * 1.5f);
                spawnRate = (int)(spawnRate * 0.7f); 
            }
        }
    }
}
