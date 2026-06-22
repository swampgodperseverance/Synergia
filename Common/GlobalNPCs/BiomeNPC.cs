// Code by SerNik
using Synergia.Common.Biome;
using Synergia.Content.Buffs;
using System.Collections.Generic;
using Terraria;

namespace Synergia.Common.GlobalNPCs {
    public class BiomeNPC : GlobalNPC {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns) {
            if (player.HasBuff<SnowVillageBuff>()) { spawnRate = 0; maxSpawns = 0; }
            GetInstance<EventManger>().Events.TryGetValue(nameof(FrostLegion), out ModEvent mod);
            if (mod.IsActive && Main.invasionX == Main.spawnTileX) {
                spawnRate = 45;
                maxSpawns = 100;
            }
        }
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo) {
            if (spawnInfo.Player.InModBiome<NewHell>()) {
                if (NPC.downedPlantBoss) {
                    for (int i = 0; i < NPCLoader.NPCCount; i++) {
                        if (Lists.NPCs.NewHellNPCs.Contains(i)) { continue; }
                        pool.Remove(i);
                    }
                }
            }
            if (spawnInfo.Player.InModBiome<UndergraundSwamp>()) {
                for (int i = 0; i < NPCLoader.NPCCount; i++) {
                    if (Lists.NPCs.NewSwampNPCs.Contains(i)) { continue; }
                    pool.Remove(i);
                }
            }
        }
    }
}