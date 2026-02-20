using System.Collections.Generic;

namespace Synergia.Common.BloodBuffSeting.Core {
    public class BestiaryManger {
        public static BestiaryManger Instance { get; } = new();
        public List<AbstractBestiaryInfo> Info { get; private set; } = [];
        public Dictionary<int, AbstractBestiaryInfo> CurrentLevel { get; private set; } = [];
        public AbstractBestiaryInfo GetLevelBloodBuff(int index) {
            CurrentLevel.TryGetValue(index, out AbstractBestiaryInfo info);
            return info;
        }
    }
}
