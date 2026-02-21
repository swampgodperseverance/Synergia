// Code by 𝒜𝑒𝓇𝒾𝓈
using Synergia.Common.GlobalPlayer;
using Terraria;

namespace Synergia.Common.BloodBuffSeting.Core {
    public abstract class AbstractBestiaryInfo : BestiaryInfo, ILoadable {
        public void Load(Mod mod) {
            BestiaryManger manager = BestiaryManger.Instance;
            manager.Info.Add(this);
            manager.CurrentLevel.Add(Leveled, this);
        }
        protected static AbstractBestiaryInfo GetLevelInstance(int level) => BestiaryManger.Instance.CurrentLevel.TryGetValue(level, out var info) ? info : null;
        protected static BloodPlayer GetBloodPlayer(Player player) => player.GetModPlayer<BloodPlayer>();
        public void Unload() { }
    }
}