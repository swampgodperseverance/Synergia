// Code by 𝒜𝑒𝓇𝒾𝓈
using Synergia.Common.BloodBuffSeting.Core;
using Terraria;

namespace Synergia.Common.BloodBuffSetting.Buffs {
    public class BloodBuffLevelFive : AbstractBestiaryInfo {
        public override int Leveled => 5;
        public override string Tooltips => Localization("Tir5");
        public override void Buff(Player player) => GetBloodPlayer(player).Tir5Buffs = true;
    }
}