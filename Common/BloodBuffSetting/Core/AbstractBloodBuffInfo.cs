// Code by SerNik
using Synergia.Common.GlobalPlayer;
using Terraria;

namespace Synergia.Common.BloodBuffSeting.Core {
    public abstract class AbstractBloodBuffInfo : BloodBuffInfo, ILoadable {
        public void Load(Mod mod) {
            BloodBuffManger manager = BloodBuffManger.Instance;
            manager.Info.Add(this);
            manager.CurrentLevel.Add(Leveled, this);
        }
        protected static AbstractBloodBuffInfo GetLevelInstance(int level) => BloodBuffManger.Instance.CurrentLevel.TryGetValue(level, out var info) ? info : null;
        protected static BloodPlayer GetBloodPlayer(Player player) => player.GetModPlayer<BloodPlayer>();
        public void Unload() { }
    }
}