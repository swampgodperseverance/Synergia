using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalPlayer {
    public class BloodPlayer : ModPlayer {
        public const int hitForActiveBloodBuff = 10;
        public const int timeForResetHit = 120;

        public bool activeBloodUI = false;
        public bool activeBloodBuff = false;

        public int currentHit = 0;
        public int timeForBuff = 60; // если какое либо баф или акс должен увеличвать время
        public int timeLastHit = 0;

        int timeForResetBuff = 0;

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
                if (timeForResetBuff <= 0) {
                    activeBloodBuff = true;
                }
            }
            if (activeBloodBuff) {
                timeForResetBuff++;
                if (timeForResetBuff >= timeForBuff) {
                    activeBloodBuff = false;
                    currentHit = 0;
                }
            }
            if (!activeBloodBuff && timeForResetBuff >= 0) {
                timeForResetBuff = 0;
            }
            if (currentHit >= 1) {
                if (!activeBloodBuff) {
                    timeLastHit++;
                    if (timeLastHit >= timeForResetHit) {
                        currentHit = 0;
                    }
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
