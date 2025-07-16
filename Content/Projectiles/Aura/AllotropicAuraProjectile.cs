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
        private static int mode = 0;
        private static int modeSwitchCooldown = 0;
        private int orbShootCooldown = 0;
        private int damageTickCounter = 0;

        public override void SetDefaults()
        {
            Projectile.alpha = 255;
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = false;
            Projectile.damage = 10;

            auraColor = new Color(0, 80, 0, 160);
            auraColor2 = new Color(0, 40, 0, 120);
            distanceMax = 200f;

            orbCount = 6;
            orbSpeed = 1.2f;
            orbTrailCount = 10;
            orbTrailGap = 0.02f;

            shootSpawnStyle = AuraShootStyles.None;

            buffType = 0;
            buffType2 = 0;

            spectreCut = false;
        }

        public override void AI()
        {
            // Handle aura mode switching
            if (modeSwitchCooldown <= 0 && VanillaKeybinds.ToggleAuraModeKeybind.JustPressed)
            {
                mode = (mode + 1) % 3;
                modeSwitchCooldown = 15;
                CombatText.NewText(Projectile.Hitbox, Color.LimeGreen, $"Mode {mode + 1}", true);
            }
            else if (modeSwitchCooldown > 0)
            {
                modeSwitchCooldown--;
            }

            // Change aura color based on mode
            auraColor = mode switch
            {
                0 => new Color(0, 80, 0, 160),     // Green - Damage
                1 => new Color(0, 160, 255, 160),  // Blue - Buff
                2 => new Color(200, 0, 80, 160),   // Red - Debuff
                _ => auraColor
            };

            // Firing orb projectiles in attack mode (every 30 ticks)
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

            base.AI(); // Visuals and aura behavior from AuraAI
        }

        public override void PostAI()
        {
            // Buff allies
            foreach (Player player in Main.player)
            {
                if (player.active && Vector2.Distance(player.Center, Projectile.Center) < distanceMax)
                {
                    if (mode == 1) // Buff mode
                    {
                        player.AddBuff(BuffID.Mining, 10);
                        player.AddBuff(BuffID.Regeneration, 1);
                        player.AddBuff(BuffID.ManaRegeneration, 1);
                        player.statLifeMax2 += 60;
                    }
                }
            }

            // Debuff enemies
            if (mode == 2)
            {
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && Vector2.Distance(npc.Center, Projectile.Center) < distanceMax)
                    {
                        npc.AddBuff(BuffID.Weak, 30);
                        npc.AddBuff(BuffID.BrokenArmor, 30);
                        npc.AddBuff(BuffID.Poisoned, 30);
                    }
                }
            }

            // Damage tick every 20 ticks in attack mode
            if (mode == 0)
            {
                damageTickCounter++;
                if (damageTickCounter >= 20)
                {
                    damageTickCounter = 0;

                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active && !npc.friendly && !npc.dontTakeDamage &&
                            Vector2.Distance(npc.Center, Projectile.Center) < distanceMax)
                        {
                            var hitInfo = new NPC.HitInfo()
                            {
                                Damage = Projectile.damage,
                                Knockback = 0f,
                                HitDirection = npc.Center.X > Projectile.Center.X ? 1 : -1,
                            };

                            npc.StrikeNPC(hitInfo);
                        }
                    }
                }
            }
        }
    }
}