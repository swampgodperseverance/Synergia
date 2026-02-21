// Code by 𝒜𝑒𝓇𝒾𝓈
using Synergia.Common.BloodBuffSeting.Core;
using Terraria;

namespace Synergia.Common.BloodBuffSetting.Buffs {
    public class BloodBuffLevelOne : AbstractBloodBuffInfo {
        public override int Leveled => 1;
        public override string Tooltips => Localization("Tir1");
        public override string AdditionalTooltips => AddAttackSpeed(DamageClassName("throwing"), 8);
        public override void Buff(Player player) => player.GetAttackSpeed(DamageClass.Throwing) += 0.08f;
    }
}