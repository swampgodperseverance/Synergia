using System;
using Microsoft.Xna.Framework;
using Synergia.Common.GlobalPlayer;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class ShadowBeamRework : ModProjectile
    {

        private ref float Timer => ref Projectile.ai[0];

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 240;

            Projectile.width = 18;
            Projectile.height = 18;

            Projectile.alpha = 255;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Timer++;

            Projectile.velocity *= 0.985f;
            Projectile.rotation = Projectile.velocity.ToRotation();

            SpawnShadowflameTrail();
            SpawnOrbitingFlames();

            if (Timer % 24 == 0)
                ShadowPulse();
        }

        private void SpawnShadowflameTrail()
        {
            int dust = Dust.NewDust(
                Projectile.position,
                Projectile.width,
                Projectile.height,
                DustID.Shadowflame,
                Projectile.velocity.X * 0.3f,
                Projectile.velocity.Y * 0.3f,
                150,
                default,
                1.2f
            );

            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0.4f;
        }

        private void SpawnOrbitingFlames()
        {
            for (int i = 0; i < 2; i++)
            {
                float rot = Timer * 0.15f + MathHelper.TwoPi * i / 2f;
                Vector2 offset = new Vector2(0f, 10f).RotatedBy(rot);

                int dust = Dust.NewDust(
                    Projectile.Center + offset,
                    4,
                    4,
                    DustID.Shadowflame,
                    0f,
                    0f,
                    150,
                    default,
                    0.9f
                );

                Main.dust[dust].velocity = Vector2.Zero;
                Main.dust[dust].noGravity = true;
            }
        }

        private void ShadowPulse()
        {
            for (int i = 0; i < 6; i++)
            {
                Vector2 vel = Main.rand.NextVector2Circular(2f, 2f);

                int dust = Dust.NewDust(
                    Projectile.Center,
                    0,
                    0,
                    DustID.Shadowflame,
                    vel.X,
                    vel.Y,
                    100,
                    default,
                    1.4f
                );

                Main.dust[dust].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 180);

            HitCircleEffect();

            Projectile.velocity *= 0.75f;
        }
        private void HitCircleEffect()
        {
            int count = 20;
            float radius = 24f;

            for (int i = 0; i < count; i++)
            {
                float angle = MathHelper.TwoPi * i / count;
                Vector2 dir = angle.ToRotationVector2();

                int dust = Dust.NewDust(
                    Projectile.Center + dir * radius,
                    0,
                    0,
                    DustID.Shadowflame,
                    dir.X * 2.5f,
                    dir.Y * 2.5f,
                    120,
                    default,
                    1.4f
                );

                Main.dust[dust].noGravity = true;
            }
        }


        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item100, Projectile.Center);

            for (int i = 0; i < 12; i++)
            {
                Vector2 vel = Main.rand.NextVector2Circular(3f, 3f);

                int dust = Dust.NewDust(
                    Projectile.Center,
                    0,
                    0,
                    DustID.Shadowflame,
                    vel.X,
                    vel.Y,
                    100,
                    default,
                    1.5f
                );

                Main.dust[dust].noGravity = true;
            }
        }


    }
}
