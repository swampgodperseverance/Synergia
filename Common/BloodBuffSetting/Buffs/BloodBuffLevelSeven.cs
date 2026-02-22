// Code by SerNik
using Synergia.Common.BloodBuffSeting.Core;
using Terraria;

namespace Synergia.Common.BloodBuffSetting.Buffs {
    public class BloodBuffLevelSeven : AbstractBloodBuffInfo {
        public override int Leveled => 7;
        public override string Tooltips => Localization("Tir7");
        public override void Buff(Player player) => GetBloodPlayer(player).Tir7Buffs = true;
    }
}