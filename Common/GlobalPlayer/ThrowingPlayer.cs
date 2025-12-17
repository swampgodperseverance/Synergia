using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Weapons.Thrown;

namespace Synergia.Common.GlobalPlayer {
    public class ThrowingPlayer : ModPlayer {
        public byte comboCount;
        public byte comboTimer;

        public bool ActiveUI;
        public bool doubleMode;
        bool wasHoldingScrew;

        public List<int> IsComboWeapons = [ItemID.Trimarang, ItemID.Snowball, ModContent.ItemType<StalloyScrew>()];

        public override void Initialize() {
            comboCount = 0;
            comboTimer = 0;
            doubleMode = false;
            wasHoldingScrew = false;
            ActiveUI = false;
        }
        public override void ResetEffects() {
            if (wasHoldingScrew && !ActiveUI) {
                ResetCombo();
            }
            wasHoldingScrew = ActiveUI;
        }
        public override void PostUpdate() {
            if (doubleMode) {
                comboTimer++;
                if (comboTimer >= 120)
                    ResetCombo();
            }
            if (IsComboWeapons.Contains(Player.HeldItem.type)) {
                ActiveUI = true;
            }
            else {
                ActiveUI = false;
            }
        }
        public void AddCombo() {
            comboTimer = 0;
            comboCount++;
            if (comboCount >= 5) {
                comboCount = 5;
                doubleMode = true;
                comboTimer = 0;
                SoundEngine.PlaySound(SoundID.MaxMana, Player.Center);
            }
        }
        public void ResetCombo() {
            comboCount = 0;
            doubleMode = false;
            comboTimer = 0;

            SoundEngine.PlaySound(new SoundStyle("Synergia/Assets/Sounds/ThrowingFail"), Player.Center);
        }
    }
}