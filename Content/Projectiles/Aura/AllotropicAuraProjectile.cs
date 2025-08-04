using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using ValhallaMod.Projectiles.AI;
using Vanilla.Common.ModSystems;

namespace Vanilla.Content.Projectiles.Aura
{
    [ExtendsFromMod("ValhallaMod")]
    public class AllotropicAuraProjectile : AuraAI
    {
        private static int mode = 0;                        // Current aura mode: 0 - Attack, 1 - Buff, 2 - Debuff
        private static int modeSwitchCooldown = 0;          // Cooldown before switching aura mode
        private int orbShootCooldown = 0;                   // Cooldown for shooting orbs
        private int damageTickCounter = 0;                  // Timer for periodic damage

        public override void SetDefaults()
        {
            Projectile.alpha = 255;
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = false;
            Projectile.damage = 10;

            auraColor = new Color(0, 80, 0, 160);            // Default aura color (green)
            auraColor2 = new Color(0, 40, 0, 120);           // Secondary aura color
            distanceMax = 200f;                              // Aura range

            orbCount = 6;
            orbSpeed = 1.2f;
            orbTrailCount = 10;
            orbTrailGap = 0.02f;

            shootSpawnStyle = AuraShootStyles.None;         // No automatic shooting

            spectreCut = false;                             // Disable spectre effect
        }

        public override void AI()
        {
            // Handle aura mode switching
            if (modeSwitchCooldown <= 0 && VanillaKeybinds.ToggleAuraModeKeybind.JustPressed)
            {
                mode = (mode + 1) % 3;                       // Cycle through 3 modes
                modeSwitchCooldown = 15;
                CombatText.NewText(Projectile.Hitbox, Color.LimeGreen, $"Mode {mode + 1}", true);
            }
            else if (modeSwitchCooldown > 0)
            {
                modeSwitchCooldown--;
            }

            // Change aura color based on current mode
            auraColor = mode switch
            {
                0 => new Color(0, 80, 0, 160),               // Attack mode (green)
                1 => new Color(0, 160, 255, 160),            // Buff mode (blue)
                2 => new Color(200, 0, 80, 160),             // Debuff mode (red)
                _ => auraColor
            };

            // Attack mode: shoot random orb every 30 ticks
            if (mode == 0)
            {
                orbShootCooldown--;
                if (orbShootCooldown <= 0)
                {
                    orbShootCooldown = 30;

                    Vector2 direction = Main.rand.NextVector2Unit();
                    float speed = 6f;
                    Vector2 velocity = direction * speed;

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        velocity,
                        ProjectileType<AllotropicAuraOrb>(),
                        Projectile.damage,
                        0f,
                        Main.myPlayer
                    );
                }
            }

            base.AI(); // Handle visuals and core aura logic
        }

        public override void ApplyEffectPlayer(Player target)
        {
            if (mode == 1) // Buff mode
            {
                target.AddBuff(BuffID.Mining, 10);            // Increase mining speed
                target.AddBuff(BuffID.Regeneration, 10);      // Health regen
                target.AddBuff(BuffID.ManaRegeneration, 10);  // Mana regen
                target.statLifeMax2 += 60;                    // Bonus max health
            }
        }

        public override void ApplyEffectNPC(NPC target)
        {
            if (mode == 2) // Debuff mode
            {
                target.AddBuff(BuffID.Weak, 30);              // Reduce damage
                target.AddBuff(BuffID.BrokenArmor, 30);       // Lower defense
                target.AddBuff(BuffID.Poisoned, 30);          // DoT effect
            }

            if (mode == 0 && damageTickCounter == 0) // Attack mode damage
            {
                if (!target.friendly && !target.dontTakeDamage)
                {
                    var hitInfo = new NPC.HitInfo()
                    {
                        Damage = Projectile.damage,
                        Knockback = 0f,
                        HitDirection = target.Center.X > Projectile.Center.X ? 1 : -1,
                    };

                    target.StrikeNPC(hitInfo);
                }
            }
        }

        public override void PostAI()
        {
            // Damage tick counter reset every 20 ticks
            if (mode == 0)
            {
                damageTickCounter++;
                if (damageTickCounter >= 20)
                {
                    damageTickCounter = 0;
                }
            }
        }
    }
}
