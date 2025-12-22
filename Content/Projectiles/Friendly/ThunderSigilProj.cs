using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.Audio;

namespace Synergia.Content.Projectiles.Friendly
{
    public sealed class ThunderSigilProj : ModProjectile
    {
        private const int ShootCooldown = 120; 
        private const float SearchRange = 500f;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1; 
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 2;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Projectile.timeLeft = 2;
            Vector2 targetPos = Main.MouseWorld;
            float time = Main.GlobalTimeWrappedHourly * 0.5f;
            float swayX = (float)Math.Sin(time * 1.2f) * 5f;
            float swayY = (float)Math.Cos(time * 0.9f) * 3.5f;
            Projectile.Center = targetPos + new Vector2(swayX, swayY);
            Lighting.AddLight(Projectile.Center, 1.2f, 0.95f, 0.3f);
            if (Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(
                    Projectile.Center - new Vector2(24f, 24f),
                    48, 48,
                    DustID.GoldFlame,
                    0f, 0f,
                    100,
                    default,
                    Main.rand.NextFloat(0.8f, 1.3f)
                );
                Main.dust[dust].velocity *= 0.4f;
                Main.dust[dust].noGravity = true;
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= ShootCooldown)
            {
                NPC target = FindClosestNPC(SearchRange);
                if (target != null)
                {
                    ShootAt(target);
                    Projectile.ai[0] = 0;
                }
            }
        }
        private NPC FindClosestNPC(float maxDistance)
        {
            NPC closest = null;
            float bestDistSqr = maxDistance * maxDistance;

            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (!npc.CanBeChasedBy()) continue;

                float distSqr = Vector2.DistanceSquared(npc.Center, Projectile.Center);
                if (distSqr < bestDistSqr)
                {
                    bestDistSqr = distSqr;
                    closest = npc;
                }
            }
            return closest;
        }

        private void ShootAt(NPC target)
        {
            if (Main.myPlayer != Projectile.owner) return;

            Vector2 velocity = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 12f;

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                velocity,
                ModContent.ProjectileType<ThunderSigilProj1>(), 
                Projectile.damage,
                Projectile.knockBack,
                Projectile.owner
            );

            SoundEngine.PlaySound(SoundID.Item93 with { Volume = 0.7f, PitchVariance = 0.3f }, Projectile.Center);
        }

        public override bool? CanDamage() => false;
    }
}