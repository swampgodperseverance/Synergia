// Code by 𝒜𝑒𝓇𝒾𝓈
using Synergia.Common.BloodBuffSeting.Core;
using Terraria;

namespace Synergia.Common.BloodBuffSetting.Buffs {
    public class BloodBuffLevelFinal : AbstractBloodBuffInfo {
        public override int Leveled => 10;
        public override string Tooltips => Localization("Tir10");
        public override void Buff(Player player) => GetBloodPlayer(player).Tir10Buffs = true;
    }
}