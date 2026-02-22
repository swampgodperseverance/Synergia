// Code by 𝒜𝑒𝓇𝒾𝓈
using Synergia.Common.BloodBuffSeting.Core;
using Terraria;

namespace Synergia.Common.BloodBuffSetting.Buffs {
    public class BloodBuffLevelNine : AbstractBloodBuffInfo {
        public override int Leveled => 9;
        public override string Tooltips => Localization("Tir9");
        public override void Buff(Player player) => GetBloodPlayer(player).Tir9Buffs = true;
    }
}