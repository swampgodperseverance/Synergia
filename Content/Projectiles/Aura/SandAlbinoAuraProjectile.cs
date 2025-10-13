using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ValhallaMod.Projectiles.AI;
using System;

namespace Synergia.Content.Projectiles.Aura
{
    [ExtendsFromMod("ValhallaMod")]
    public class SandAlbinoAuraProjectile : AuraAI
    {
        public override void SetDefaults()
        {
            Projectile.alpha = 255;
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = false;
            Projectile.damage = 0;

            auraColor = new Color(255, 255, 255, 160);           // Base white aura color
            auraColor2 = new Color(220, 220, 220, 120);          // Secondary tint

            distanceMax = 120f;                                  // Smaller aura radius
            orbCount = 6;
            orbSpeed = 1.2f;
            orbTrailCount = 10;
            orbTrailGap = 0.02f;

            shootSpawnStyle = AuraShootStyles.None;
            spectreCut = false;

            // Buff applied to team players (Wrath = +damage)
            buffTypes[AuraEffectTarget.Team].Add(BuffID.Wrath);
        }

        public override void AI()
        {
            base.AI();

            // Pulsating white aura (alpha shifting effect)
            float pulse = (float)Math.Sin(Main.GameUpdateCount * 0.1f) * 20f;
            auraColor = new Color(255, 255, 255, (int)(140 + pulse));  // Smooth pulsating alpha
        }

        public override void ApplyEffectPlayer(Player target)
        {
            target.statDefense += 6;                                // Bonus defense

            // Slight movement speed increase
            target.moveSpeed += 0.1f;

            // Bonus melee speed (frenzy-like effect)
            target.GetAttackSpeed(DamageClass.Melee) += 0.12f;

            // Optional: glowing white outline (if you have shaders)
        }

        public override void ApplyEffectNPC(NPC target)
        {
            target.AddBuff(BuffID.Slow, 30);                         // Slow debuff

            // 10% chance to apply Frozen if the enemy is not a boss
            if (Main.rand.NextFloat() < 0.10f && !target.boss)
            {
                target.AddBuff(BuffID.Frozen, 30);                   // Temporarily freeze
            }
        }
    }
}
