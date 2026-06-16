using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Thrower
{
    public class SunJavelin2 : ModProjectile
    {
        private const int MaxStickingJavelins = 5;
        private const int ExplosionDamage = 40;
        private int skyJavelinsLeft = 0;
        private int skyJavelinTimer = 0;
        private int skyTarget = -1;
        private bool hasHit = false;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

            if (Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin,
                    Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1.2f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.3f;
            }

            if (skyJavelinsLeft > 0 && skyTarget >= 0)
            {
                NPC target = Main.npc[skyTarget];

                if (!target.active || target.life <= 0)
                {
                    skyJavelinsLeft = 0;
                    skyTarget = -1;
                    return;
                }

                skyJavelinTimer++;

                if (skyJavelinTimer >= 4)
                {
                    skyJavelinTimer = 0;
                    skyJavelinsLeft--;

                    float randomX = Main.rand.NextFloat(-250f, 250f);
                    Vector2 spawnPos = new Vector2(
                        target.Center.X + randomX,
                        target.Center.Y - Main.rand.NextFloat(400f, 600f)
                    );

                    Vector2 velocity = target.Center - spawnPos;
                    velocity.Normalize();
                    velocity *= Main.rand.NextFloat(20f, 28f);

                    int damage = (int)(Projectile.damage * 0.65f);

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        spawnPos,
                        velocity,
                        ModContent.ProjectileType<SunJavelinProj>(),
                        damage,
                        2f,
                        Projectile.owner
                    );

                    for (int i = 0; i < 5; i++)
                    {
                        Dust dust = Dust.NewDustDirect(spawnPos, 10, 10, DustID.GoldFlame,
                            Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 100, default, 1.3f);
                        dust.noGravity = true;
                    }
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasHit) return;
            hasHit = true;


            if (Main.myPlayer != Projectile.owner)
                return;

            skyJavelinsLeft = Main.rand.Next(3, 6);
            skyJavelinTimer = 0;
            skyTarget = target.whoAmI;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

            for (int i = 0; i < 20; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.GoldFlame, 0f, 0f, 0, default, 1.2f);
                Dust dust = Main.dust[dustIndex];
                dust.velocity *= 0.5f;
                dust.noGravity = true;
            }
        }
    }
}