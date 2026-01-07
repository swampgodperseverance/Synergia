using System.Collections.Generic;

namespace Synergia.Common.GlobalNPCs {
    public class SynergiaGlobalNPC : GlobalNPC {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo) {
            ModEvent modEvent = ModEvent.Instance;
            if (modEvent.IsActive) {
                if (modEvent.CurrentTimeToSpawnNPC >= modEvent.TimeToSpawnNPC) {
                    modEvent.SpawnNPC(ref pool, modEvent.CurrentWave);
                }
            }
        }
    }
}
