using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalPlayer {
    public class BloodPlayer : ModPlayer {
        public const int hitForActiveBloodBuff = 10;
        public const int timeForResetHit = 120;

        public bool activeBloodUI = false;
        public bool activeBloodBuff = false;

        public int currentHit = 0;
        public int timeLastHit = 0;


        public override void Initialize() {
            activeBloodUI = false;
            activeBloodBuff = false;
            currentHit = 0;
        }
        public override void PostUpdate() {
            if (Lists.Items.WeaponActiveBlood.Contains(Player.HeldItem.type)) {
                activeBloodUI = true;
            }
            else {
                activeBloodUI = false;
            }
            if (currentHit >= hitForActiveBloodBuff) {
                activeBloodBuff = true;
            }
            if (currentHit >= 1) {
                timeLastHit++;
                if (timeLastHit >= timeForResetHit) {
                    currentHit = 0;
                }
            }
        }
        public override void PostUpdateBuffs() {
            if (activeBloodBuff) {
                // Тут баф

            }
        }
    }
}