// Code by SerNik
using Synergia.Common.BloodBuffSeting.Core;
using Terraria;

namespace Synergia.Common.BloodBuffSetting.Buffs {
    public class BloodBuffLevelTwo : AbstractBloodBuffInfo {
        public override int Leveled => 2;
        public override string Tooltips => Localization("Tir2");
        public override void Buff(Player player) {
            GetBloodPlayer(player).Tir2Buffs = true;
        }
    }
}