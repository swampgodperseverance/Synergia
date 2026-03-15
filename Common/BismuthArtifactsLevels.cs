using Terraria;

namespace Synergia.Common {
    public static class BismuthArtifactsLevels {
        public static int GetLevel() {
            int level = 0;
            if (Main.hardMode) { level++; }
            if (ValhallaMod.Systems.DownedBossSystem.downedEmperorBoss) { level++; }
            if (NPC.downedPlantBoss) { level++; }
            if (Consolaria.Common.ModSystems.DownedBossSystem.downedOcram) { level++; }
            return level;
        }
    }
}