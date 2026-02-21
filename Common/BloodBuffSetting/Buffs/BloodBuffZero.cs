// Code by 𝒜𝑒𝓇𝒾𝓈
using Synergia.Common.BloodBuffSeting.Core;
using Terraria;
using Terraria.ID;

namespace Synergia.Common.BloodBuffSeting.Buffs {
    public class BloodBuffZero : AbstractBloodBuffInfo {
        public override int Leveled => 0;
        public override string Tooltips => Localization("Tir0");
        public override void Buff(Player player) {
            player.moveSpeed += 0.08f;
            if (Main.rand.NextBool(6)) {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Blood, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(0.5f, 2f), 100, default, Main.rand.NextFloat(0.8f, 1.2f));
                dust.noGravity = false;
                dust.velocity *= 0.5f;
            }
        }
    }
}