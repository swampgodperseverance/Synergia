using Synergia.Lists;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using ValhallaMod.Items.Weapons.Boomerang;
using ValhallaMod.Items.Weapons.Glaive;
using ValhallaMod.Items.Garden;
using ValhallaMod.Items.Weapons.Thrown;
using Consolaria.Content.Items.Weapons.Melee;
using static Synergia.ModList;

namespace Synergia.Common.GlobalPlayer {
    public class ThrowingPlayer : ModPlayer {
        public byte comboCount;
        public byte comboTimer;
        public byte ModifyMaxComboTime;
        public byte resetTime;
        public byte ModifyMaxTimeForReset;

        const byte baseTimeForReset = 120;
        byte maxTimeForReset;

        const byte baseMaxComboTime = 60;
        byte maxComboTime;

        public bool ActiveUI { get; private set; }
        public bool DoubleMode { get; private set; }
        bool wasHoldingScrew;

        public override void Initialize() {
            comboCount = 0;
            comboTimer = 0;
            resetTime = 0;
            ModifyMaxComboTime = 0;
            ModifyMaxTimeForReset = 0;
            DoubleMode = false;
            wasHoldingScrew = false;
            ActiveUI = false;
        }
        public override void ResetEffects() {
            if (wasHoldingScrew && !ActiveUI) {
                ResetCombo();
            }
            wasHoldingScrew = ActiveUI;
            ModifyMaxComboTime = 0;
            ModifyMaxTimeForReset = 0;
        }
        public override void PostUpdate() {
            if (DoubleMode) {
                maxComboTime = (byte)(baseMaxComboTime + ModifyMaxComboTime);
                comboTimer++;
                if (comboTimer >= maxComboTime) {
                    ResetCombo();
                }
            }
            if (Items.IsComboWeapons.Contains(Player.HeldItem.type)) {
                ActiveUI = true;
                if (comboCount >= 1 && !DoubleMode) {
                    resetTime++;
                }
            }
            else {
                ActiveUI = false;
                resetTime = 0;
            }
            maxTimeForReset = (byte)(baseTimeForReset + ModifyMaxTimeForReset);
            if (resetTime >= maxTimeForReset) {
                ResetCombo();
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
            if (Projectiles.ThrowingProj.Contains(proj.type)) {
                if (comboCount <= 5) {
                    AddCombo();
                }
            }
        }
        public void AddCombo() {
            if (comboCount < 5) {
                comboTimer = 0;
                comboCount++;
                resetTime = 0;
                if (comboCount == 5) {
                    DoubleMode = true;
                    SoundEngine.PlaySound(SoundID.MaxMana, Player.Center);
                }
            }
        }
        public void ResetCombo() {
            comboCount = 0;
            comboTimer = 0;
            resetTime = 0;
            DoubleMode = false;
            SoundEngine.PlaySound(new SoundStyle("Synergia/Assets/Sounds/ThrowingFail"), Player.Center);
        }
    }
}