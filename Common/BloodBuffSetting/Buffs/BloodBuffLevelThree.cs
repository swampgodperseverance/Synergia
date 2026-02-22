// Code by 𝒜𝑒𝓇𝒾𝓈
using Synergia.Common.BloodBuffSeting.Core;
using Terraria;

namespace Synergia.Common.BloodBuffSetting.Buffs {
    public class BloodBuffLevelThree : AbstractBloodBuffInfo {
        public override int Leveled => 3;
        public override string Tooltips => Localization("Tir3");
        public override void Buff(Player player) => player.moveSpeed += 0.07f;
    }
}