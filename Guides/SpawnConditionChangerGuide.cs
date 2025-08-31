using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace Synergia.Common.GlobalNPCs.AI
{
    public class CacodemonSpawnCondition : GlobalNPC // we can use it to make progression changes
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (!ModLoader.TryGetMod("ValhallaMod", out Mod valhallaMod)) //mod
                return;

            if (!valhallaMod.TryFind("Cacodemon", out ModNPC cacodemon)) // NPC from this mod
                return; // If it doesn't exist, do nothing

            int cacodemonType = cacodemon.Type; //i think you understand

            // Conditions for spawning Cacodemon:
            //  Player is in the Underworld
            //  Golem has been defeated
            //  No invasion is currently active
            //  Player is active (not dead, not disconnected)
            if (spawnInfo.Player.ZoneUnderworldHeight &&
                NPC.downedGolemBoss &&
                Main.invasionType == 0 &&         
                spawnInfo.Player.active)            
            {
            // Add Cacodemon to the spawn pool with 15% weight
                pool[cacodemonType] = 0.15f;
            }
            else
            {
             // Otherwise, ensure it cannot spawnn
                pool.Remove(cacodemonType);
            }

        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.ZoneUnderworldHeight && NPC.downedGolemBoss)
            {
            // Increase the maximum number of enemies that can exist at on
                maxSpawns = (int)(maxSpawns * 1.5f);
                // cooldown
                spawnRate = (int)(spawnRate * 0.7f); 
            }
        }
    }
}
