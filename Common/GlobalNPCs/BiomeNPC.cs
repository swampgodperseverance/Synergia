using Synergia.Common.GlobalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalNPCs
{
    public class BiomeNPC : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.GetModPlayer<BiomePlayer>().InSnowVillage) { spawnRate = 0; maxSpawns = 0; }
        }
    }
}