using Terraria;

namespace Synergia.Common {
    public class BismuthArtifactsLevels : ModSystem {
        public int GetLevel() {
            int level = 1;
            if (Main.hardMode) { level++; }
            if (ValhallaMod.Systems.DownedBossSystem.downedEmperorBoss) { level++; }
            if (NPC.downedPlantBoss) { level++; }
            if (Consolaria.Common.ModSystems.DownedBossSystem.downedOcram) { level++; }
            return level;
        }
    }
}