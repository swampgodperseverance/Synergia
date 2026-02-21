// Code by 𝒜𝑒𝓇𝒾𝓈
using Synergia.Common.BloodBuffSeting.Core;
using Terraria;

namespace Synergia.Common.BloodBuffSetting.Buffs {
    public class BloodBuffLevelFour : AbstractBloodBuffInfo {
        public override int Leveled => 4;
        public override string Tooltips => Localization("Tir4");
        public override string AdditionalTooltips => AddAttackSpeed(DamageClassName("throwing"), 7);
        public override void Buff(Player player) => player.GetAttackSpeed(DamageClass.Throwing) += 0.07f;
    }
}