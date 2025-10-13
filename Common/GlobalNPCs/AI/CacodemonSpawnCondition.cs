using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace Synergia.Common.GlobalNPCs.AI
{
    public class CacodemonSpawnCondition : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (!ModLoader.TryGetMod("ValhallaMod", out Mod valhallaMod))
                return;

            if (!valhallaMod.TryFind("Cacodemon", out ModNPC cacodemon))
                return;

            int cacodemonType = cacodemon.Type;

            if (spawnInfo.Player.ZoneUnderworldHeight &&
                NPC.downedGolemBoss &&
                Main.invasionType == 0 &&         
                spawnInfo.Player.active)            
            {
                pool[cacodemonType] = 0.15f;
            }
            else
            {
                pool.Remove(cacodemonType);
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
