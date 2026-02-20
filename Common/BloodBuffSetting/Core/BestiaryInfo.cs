using Terraria;

namespace Synergia.Common.BloodBuffSeting.Core {
    public class BestiaryInfo {
        public virtual int Leveled => -1;
        public virtual string Tooltips => "";
        public virtual void Buff(Player player) { }
    }
}