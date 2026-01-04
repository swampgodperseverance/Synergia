using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ValhallaMod.Projectiles.AI;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Content.Projectiles.Aura
{
    [ExtendsFromMod("ValhallaMod")]
    public class VeldorousStaffProj : AuraAI
    {
        private int[] spiritIDs = new int[2] { -1, -1 };
        private int respawnTimer = 0;
        private const int RespawnDelay = 300;
        private float[] orbitAngles = new float[2] { 0f, MathHelper.Pi };
        private Vector2[] spiritVelocities = new Vector2[2];

        public int PlayerLinked2 
        {
            get => (int)Projectile.ai[1];
            set { Projectile.ai[1] = value; }
        }

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
            distanceMax = 240f;
            orbCount = 6;
            orbSpeed = 1.2f;
            orbTrailCount = 10;
            orbTrailGap = 0.02f;
            shootSpawnStyle = AuraShootStyles.None;
            spectreCut = false;

            buffTypes[AuraEffectTarget.Team].Add(BuffID.Regeneration);
            buffTypes[AuraEffectTarget.Team].Add(BuffID.ManaRegeneration);
        }

        public override void ApplyEffectPlayer(Player target)
        {
            target.statDefense += 7;

            Mod RoA = ModLoader.GetMod("RoA");
            if (RoA != null)
            {
                int resilienceBuffType = RoA.Find<ModBuff>("Resilience").Type;
                target.AddBuff(resilienceBuffType, 10);
            }
        }

        public override void AI()
        {
            base.AI();
            
            if (PlayerLinked2 != 0)
            {
                HandleSpiritsLogic();
            }
            else
            {
                CleanupSpirits();
            }
        }

        private void CleanupSpirits()
        {
            for (int i = 0; i < 2; i++)
            {
                if (spiritIDs[i] != -1 && spiritIDs[i].IsWithinBounds())
                {
                    Main.projectile[spiritIDs[i]].Kill();
                    spiritIDs[i] = -1;
                }
            }
        }

        private void HandleSpiritsLogic()
        {
            for (int i = 0; i < 2; i++)
            {
                if (spiritIDs[i] == -1 && respawnTimer <= 0)
                {
                    TrySpawnSpirit(i);
                }
            }

            for (int i = 0; i < 2; i++)
            {
                if (spiritIDs[i] != -1)
                {
                    UpdateSpiritBehavior(i);
                }
            }

            if (respawnTimer > 0) respawnTimer--;
        }

        private void TrySpawnSpirit(int index)
        {
            Mod roaMod = ModLoader.GetMod("RoA");
            if (roaMod == null) return;

            int id = Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                Vector2.Zero,
                roaMod.Find<ModProjectile>("VengefulSpirit").Type,
                Projectile.damage,
                0f,
                Projectile.owner
            );

            if (id.IsWithinBounds())
            {
                spiritIDs[index] = id;
                var spirit = Main.projectile[id];
                spirit.friendly = true;
                spirit.hostile = false;
                spirit.tileCollide = false;
                spirit.penetrate = 1;
                spirit.timeLeft = 600;
                spirit.alpha = 60;
                spirit.netUpdate = true;
                spiritVelocities[index] = Vector2.Zero;
            }
        }

        private void UpdateSpiritBehavior(int index)
        {
            if (!spiritIDs[index].IsWithinBounds())
            {
                spiritIDs[index] = -1;
                return;
            }

            Projectile spirit = Main.projectile[spiritIDs[index]];
            if (!IsValidSpirit(spirit))
            {
                spiritIDs[index] = -1;
                respawnTimer = RespawnDelay;
                return;
            }

            const float orbitRadius = 120f;
            const float baseOrbitSpeed = 0.05f;
            float orbitSpeed = baseOrbitSpeed;

            orbitAngles[index] = MathHelper.WrapAngle(orbitAngles[index] + orbitSpeed);
    
            Vector2 idealPosition = Projectile.Center + new Vector2(
                (float)Math.Cos(orbitAngles[index]) * orbitRadius,
                (float)Math.Sin(orbitAngles[index]) * orbitRadius * 0.7f
            );

            Vector2 springForce = (idealPosition - spirit.Center) * 0.01f;
            spiritVelocities[index] = (spiritVelocities[index] * 0.9f) + springForce;
    
            float maxSpeed = 6f;
            if (spiritVelocities[index].Length() > maxSpeed)
            {
                spiritVelocities[index] = Vector2.Normalize(spiritVelocities[index]) * maxSpeed;
            }

            spirit.velocity = spiritVelocities[index];
            spirit.position += spirit.velocity;

            if (spirit.velocity.LengthSquared() > 0.1f)
            {
                float targetRotation = spirit.velocity.ToRotation() + MathHelper.PiOver2;
                spirit.rotation = MathHelper.Lerp(spirit.rotation, targetRotation, 0.05f);
                spirit.spriteDirection = spirit.velocity.X > 0 ? 1 : -1;
            }

            spirit.timeLeft = 10;
        }

        private bool IsValidSpirit(Projectile spirit)
        {
            return spirit.active && 
                   spirit.type == ModLoader.GetMod("RoA")?.Find<ModProjectile>("VengefulSpirit").Type;
        }
    }

    public static class ProjectileExtensions
    {
        public static bool IsWithinBounds(this int projectileIndex)
        {
            return projectileIndex >= 0 && projectileIndex < Main.maxProjectiles;
        }
    }
}