// Code by SerNik
using Synergia.Common.BloodBuffSeting.Core;
using Terraria;

namespace Synergia.Common.BloodBuffSetting.Buffs {
    public class BloodBuffLevelSix : AbstractBloodBuffInfo {
        public override int Leveled => 6;
        public override string Tooltips => Localization("Tir6");
        public override void Buff(Player player) => player.endurance += 0.08f;
    }
}