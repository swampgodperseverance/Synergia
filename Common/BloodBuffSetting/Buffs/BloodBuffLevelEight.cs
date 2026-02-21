using Synergia.Common.BloodBuffSeting.Core;
using Terraria;

namespace Synergia.Common.BloodBuffSetting.Buffs {
    public class BloodBuffLevelEight : AbstractBloodBuffInfo {
        public override int Leveled => 8;
        public override string Tooltips => Localization("Tir8");
        public override string AdditionalTooltips => AddAttackSpeed(DamageClassName("throwing"), 6);
        public override void Buff(Player player) => player.GetAttackSpeed(DamageClass.Throwing) += 0.06f;
    }
}
