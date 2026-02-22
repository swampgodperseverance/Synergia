// Code by 𝒜𝑒𝓇𝒾𝓈
using Synergia.Content.Buffs;
using Synergia.Content.Projectiles;
using Terraria;
using Terraria.ID;

namespace Synergia.Common.GlobalPlayer {
    public class BloodPlayer : ModPlayer {
        //Blood Buff Effect
        public bool Tir2Buffs = false; // Dodge
        public bool Tir5Buffs = false; // Aura
        public bool Tir7Buffs = false; // Shot
        public bool Tir9Buffs = false; // Dash
        public bool Tir10Buffs = false; // Vampiric

        public const int hitForActiveBloodBuff = 15;
        public const int timeForResetHit = 120;

        public bool activeBloodUI = false;
        public bool activeBloodBuff = false;

        public int currentHit = 0;
        public int timeLastHit = 0;

        public override void Initialize() {
            Tir2Buffs = false;
            Tir5Buffs = false;
            Tir7Buffs = false;
            Tir9Buffs = false;
            Tir10Buffs = false;
            activeBloodUI = false;
            activeBloodBuff = false;
            currentHit = 0;
            timeLastHit = 0;
        }
        public override bool FreeDodge(Player.HurtInfo info) {
            if (Tir2Buffs && Main.rand.NextBool(5)) { // 5 || 6 
                Player.immune = true;
                Player.immuneTime = 20;
                ArmorPlayers.SpawnBurst(Player.position, DustID.Blood);
                return true;
            }
            else { return base.FreeDodge(info); }
        }
        public override void PostUpdate() {
            if (Lists.Items.WeaponActiveBlood.Contains(Player.HeldItem.type)) { activeBloodUI = true; }
            else { activeBloodUI = false; }
            if (currentHit >= hitForActiveBloodBuff) { activeBloodBuff = true; }
            if (currentHit >= 1) {
                if (!activeBloodBuff) {
                    timeLastHit++;
                    if (timeLastHit >= timeForResetHit) { currentHit = 0; }
                }
            }
            if (!activeBloodUI) {
                activeBloodBuff = false;
                currentHit = 0;
            }
            if (Tir5Buffs && Player.ownedProjectileCounts[ProjectileType<LifesAura>()] <= 0) {
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ProjectileType<LifesAura>(), 0, 0, Player.whoAmI);
            }
            if (!Player.HasBuff<BloodBuff>()) { Clear(); }
        }
        public override void PostUpdateBuffs() {
            if (activeBloodBuff) { Player.AddBuff(BuffType<BloodBuff>(), 1); }
            else { Player.ClearBuff(BuffType<BloodBuff>()); }
        }
        void Clear() {
            Tir2Buffs = false;
            Tir5Buffs = false;
            Tir7Buffs = false;
            Tir9Buffs = false;
            Tir10Buffs = false;
        }
    }
}