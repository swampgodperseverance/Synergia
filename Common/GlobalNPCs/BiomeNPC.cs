// Code by 𝒜𝑒𝓇𝒾𝓈
using Synergia.Common.Biome;
using Synergia.Common.GlobalPlayer;
using System.Collections.Generic;
using Terraria;

namespace Synergia.Common.GlobalNPCs
{
    public class BiomeNPC : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.GetModPlayer<BiomePlayer>().InSnowVillage) { spawnRate = 0; maxSpawns = 0; }
        }
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo) {
            if (Main.LocalPlayer.InModBiome<NewHell>()) {
                if (NPC.downedPlantBoss) {
                    for (int i = 0; i < NPCLoader.NPCCount; i++) {
                        if (Lists.NPCs.NewHellNPCs.Contains(i)) {
                            continue;
                        }
                        pool.Remove(i);
                    }
                }
            }
        }
    }
}