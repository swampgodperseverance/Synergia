using Bismuth.Content.Projectiles;
using NewHorizons.Content.Items.Weapons.Throwing;
using NewHorizons.Content.Items.Weapons.Ranged;
using Bismuth.Content.Items.Weapons.Throwing;
using NewHorizons.Content.Projectiles.Throwing;
using NewHorizons.Content.Projectiles.Ranged;
using System.Collections.Generic;
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

        public bool ActiveUI;
        public bool doubleMode;
        bool wasHoldingScrew;

        public List<int> IsComboWeapons = [ItemID.Trimarang,ItemID.WoodenBoomerang, ItemID.Flamarang, ItemID.Snowball, ItemType<AlbinoMandible>(), ItemType<NewHorizons.Content.Items.Weapons.Throwing.NightGlaive>(), ItemType<SnowGlaive>(), ItemType<StalloyScrew>(), ItemType<TitaniumWarhammer>(), ItemType<AdamantiteDagger>(), ItemType<TeethBreaker>(), ItemType<Carnwennan>(), ItemType<Aeglos>(), ItemType<Lancea>(), ItemType<Garlic>(), ItemType<ScarletGungnir>(), ItemType<BlazingSaws>(), ItemType<NanoStar>(), ItemType<CrystalGrenade>()];
        static List<int> ProjType = [            
            ProjectileType<ValhallaMod.Projectiles.Boomerang.TeethBreaker>(),
            ProjectileType<ValhallaMod.Projectiles.Glaive.SnowGlaive>(),
            ProjectileType<Consolaria.Content.Projectiles.Friendly.AlbinoMandible>(),
            ProjectileType<CarnwennanProj>(),
            ProjectileType<NightGlaiveProj>(),
            ProjectileType<AdamantiteDaggerProj>(),
            ProjectileType<TitaniumWarhammerProj>(),
            ProjectileType<NanoStarProj>(),
            ProjectileType<BlazingSawsProj>(),
            ProjectileType<CrystalGrenadeProj>(),
            ProjectileType<AeglosP>(),
            ProjectileType<ScarletGungnirProj>(),
            ProjectileType<LanceaP>(),
            ProjectileID.Flamarang,
            Valhalla.Find<ModProjectile>("Garlic").Type    
        ];

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
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
            if (ProjType.Contains(proj.type)) {
                if (comboCount <= 5) {
                    AddCombo();
                }
            }
        }
        public void AddCombo() {
            if (comboCount < 5) {
                comboTimer = 0;
                comboCount++;

                if (comboCount == 5) {
                    doubleMode = true;
                    SoundEngine.PlaySound(SoundID.MaxMana, Player.Center);
                }
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