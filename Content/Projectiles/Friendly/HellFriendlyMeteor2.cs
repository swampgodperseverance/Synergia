using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Synergia.Content.Projectiles.Aura;

namespace Synergia.Content.Projectiles.Friendly
{
    public class HellFriendlyMeteor2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.damage = 90;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.rotation += 0.2f * Projectile.direction;
            Projectile.velocity.Y += 0.3f;

            int parentAuraID = (int)Projectile.localAI[0];
            if (parentAuraID >= 0 && parentAuraID < Main.maxProjectiles)
            {
                Projectile aura = Main.projectile[parentAuraID];

                if (!aura.active || aura.type != ModContent.ProjectileType<HellgateAura>())
                {
                    Projectile.Kill();
                    return;
                }

                float maxDistance = 200f;
                if (Vector2.Distance(Projectile.Center, aura.Center) > maxDistance)
                {
                    Projectile.Kill();
                    return;
                }
            }

            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Lava,
                    Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1.1f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 3; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Lava);
                Main.dust[dust].velocity *= 0.5f;
                Main.dust[dust].scale = 1.0f;
            }
        }

        public override void OnKill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.Item74, Projectile.position); // Meteor burst sound

            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Lava,
                    Main.rand.NextFloat(-2f, 2f),
                    Main.rand.NextFloat(-2f, 2f),
                    100,
                    default,
                    Main.rand.NextFloat(1f, 1.5f)
                );
                Main.dust[dust].noGravity = true;
            }

            for (int i = 0; i < 5; i++)
            {
                int dust = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Smoke,
                    Main.rand.NextFloat(-1f, 1f),
                    Main.rand.NextFloat(-1f, 1f),
                    150,
                    default,
                    Main.rand.NextFloat(0.8f, 1.2f)
                );
                Main.dust[dust].noGravity = false;
            }
        }
    }
}
