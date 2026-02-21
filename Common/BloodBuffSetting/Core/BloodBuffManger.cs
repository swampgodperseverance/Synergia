// Code by 𝒜𝑒𝓇𝒾𝓈
using System.Collections.Generic;

namespace Synergia.Common.BloodBuffSeting.Core {
    public class BloodBuffManger {
        public static BloodBuffManger Instance { get; } = new();
        public List<AbstractBloodBuffInfo> Info { get; private set; } = [];
        public Dictionary<int, AbstractBloodBuffInfo> CurrentLevel { get; private set; } = [];
        public AbstractBloodBuffInfo GetLevelBloodBuff(int index) => CurrentLevel.TryGetValue(index, out AbstractBloodBuffInfo info) ? info : null;
    }
}
