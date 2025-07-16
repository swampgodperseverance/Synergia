using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Vanilla.Content.Projectiles
{
    public class InfamousFlameProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = 3; // стиль как у Paladin's Hammer
            AIType = ProjectileID.PaladinsHammerFriendly;

            // Для трейла
            Projectile.extraUpdates = 1; // сглаживание движения
            Projectile.alpha = 50;
            Projectile.light = 0.6f;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 0.4f, 0.1f); // свет
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                dust.noGravity = true;
                dust.velocity *= 0.5f;
                dust.scale = Main.rand.NextFloat(1.2f, 1.5f);
            }

            // трейл с помощью TrailCacheLength
            ProjectileTrailEffect();
        }

        private void ProjectileTrailEffect()
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 trailPosition = Projectile.position - Projectile.velocity * i * 0.2f;
                int dustIndex = Dust.NewDust(trailPosition, Projectile.width, Projectile.height, DustID.FlameBurst);
                Main.dust[dustIndex].scale = 1.1f;
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].fadeIn = 1.2f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 240); // адский огонь
        }
    }
}