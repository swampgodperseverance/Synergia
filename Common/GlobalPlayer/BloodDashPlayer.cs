using Terraria;
using Terraria.ID;

namespace Synergia.Common.GlobalPlayer {
    public class BloodDashPlayer : ModPlayer {
        int DashDir = -1;
        int DashDelay;
        int DashTimer;

        public override void ResetEffects() {
            if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[0] < 15) {
                DashDir = 0;
                return;
            }
            if (Player.controlUp && Player.releaseUp && Player.doubleTapCardinalTimer[1] < 15) {
                DashDir = 1;
                return;
            }
            if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[2] < 15) {
                DashDir = 2;
                return;
            }
            if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[3] < 15) {
                DashDir = 3;
                return;
            }
            DashDir = -1;
        }
        public override void PreUpdateMovement() {
            if (CanUseDash() && DashDir != -1 && DashDelay == 0) {
                Vector2 newVelocity = Player.velocity;
                switch (DashDir) {
                    case 0: if (Player.velocity.Y >= 15f) { return; } break;
                    case 1: if (Player.velocity.Y <= -15f) { return; } break;
                    case 2: if (Player.velocity.X < 15f) { goto IL_CF; } return;
                    case 3: if (Player.velocity.X <= -15f) { return; } goto IL_CF;
                    default: return;
                }
                float dashDirection = (DashDir == 0) ? 1f : -1.3f;
                newVelocity.Y = dashDirection * 15f;
                goto IL_EF;
            IL_CF:
                float dashDirection2 = (float)((DashDir == 2) ? 1 : -1);
                newVelocity.X = dashDirection2 * 13f;
            IL_EF:
                DashDelay = 35;
                DashTimer = 40;
                Player.velocity = newVelocity;
            }
            if (DashDelay > 0) {
                DashDelay--;
            }
            if (DashTimer > 0) {
                Player.eocDash = DashTimer;
                Player.armorEffectDrawShadowEOCShield = true;
                DashTimer--;
                for (int num24 = 0; num24 < 20; num24++) {
                    int num25 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y), Player.width, Player.height, DustID.Blood, 0f, 0f, 100, default, 1f);
                    Main.dust[num25].position.X += Main.rand.Next(-5, 6);
                    Main.dust[num25].position.Y += Main.rand.Next(-5, 6);
                    Main.dust[num25].velocity *= 0.2f;
                    Main.dust[num25].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                    Main.dust[num25].noGravity = true;
                    Main.dust[num25].fadeIn = 0.5f;
                }
            }
        }
        private bool CanUseDash() => Player.GetModPlayer<BloodPlayer>().Tir9Buffs && Player.dashType == 0 && !Player.setSolar && !Player.mount.Active;
    }
}