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
namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class ShadowBeamRework2 : ModProjectile
    {
        private ref float Timer => ref Projectile.ai[0];
        private ref float ChargeLevel => ref Projectile.ai[1]; 

        public override string GlowTexture => "Synergia/Assets/Textures/Star";

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 3; 
            Projectile.timeLeft = 300; 
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.alpha = 50; 
            Projectile.extraUpdates = 2; 
            Projectile.tileCollide = true;
            Projectile.scale = 1.2f;
            Lighting.AddLight(Projectile.Center, 0.3f, 0.1f, 0.4f);
        }

        public override void AI()
        {
            Timer++;

            if (ChargeLevel < 3f)
                ChargeLevel += 0.02f;
            Projectile.velocity *= 0.992f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            float pulseScale = 1f + (float)Math.Sin(Timer * 0.2f) * 0.1f * ChargeLevel;
            Projectile.scale = 1.2f * pulseScale;
            SpawnShadowflameTrail();
            SpawnOrbitingFlames();
            SpawnChargedParticles();
            if (Timer % 18 == 0)
                ShadowPulse();
            if (Timer % 12 == 0)
                SpawnDecayField();
            float lightMult = 0.5f + ChargeLevel * 0.3f;
            Lighting.AddLight(Projectile.Center, 0.4f * lightMult, 0.1f * lightMult, 0.6f * lightMult);
        }

        private void SpawnShadowflameTrail()
        {
            for (int i = 0; i < 2; i++)
            {
                int dust = Dust.NewDust(
                    Projectile.position - Projectile.velocity * i,
                    Projectile.width,
                    Projectile.height,
                    DustID.Shadowflame,
                    Projectile.velocity.X * 0.2f,
                    Projectile.velocity.Y * 0.2f,
                    100,
                    default,
                    1.4f + ChargeLevel * 0.3f
                );

                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.3f;
                if (Main.rand.NextBool(3))
                {
                    int spark = Dust.NewDust(
                        Projectile.Center,
                        0, 0,
                        DustID.PurpleTorch,
                        Projectile.velocity.X * 0.1f,
                        Projectile.velocity.Y * 0.1f,
                        80,
                        default,
                        0.8f
                    );
                    Main.dust[spark].noGravity = true;
                }
            }
        }

        private void SpawnOrbitingFlames()
        {
            float speed = Timer * 0.2f;
            float radius = 16f + ChargeLevel * 4f;

            for (int i = 0; i < 3; i++)
            {
                float rot = speed + MathHelper.TwoPi * i / 3f;
                Vector2 offset = new Vector2(0f, radius).RotatedBy(rot);

                int dust = Dust.NewDust(
                    Projectile.Center + offset,
                    2, 2,
                    DustID.Shadowflame,
                    0f, 0f,
                    120,
                    default,
                    1.1f + ChargeLevel * 0.2f
                );

                Main.dust[dust].velocity = Vector2.Zero;
                Main.dust[dust].noGravity = true;

                if (ChargeLevel > 1.5f)
                {
                    offset = new Vector2(0f, radius * 0.6f).RotatedBy(-rot * 1.5f);

                    int dust2 = Dust.NewDust(
                        Projectile.Center + offset,
                        2, 2,
                        DustID.Shadowflame,
                        0f, 0f,
                        100,
                        default,
                        0.9f
                    );
                    Main.dust[dust2].noGravity = true;
                    Main.dust[dust2].alpha = 150;
                }
            }
        }

        private void SpawnChargedParticles()
        {
            if (Main.rand.NextBool(3))
            {
                Vector2 pos = Projectile.Center + Main.rand.NextVector2Circular(8f, 8f);

                int dust = Dust.NewDust(
                    pos, 0, 0,
                    DustID.Shadowflame,
                    0f, 0f,
                    50,
                    default,
                    1.8f
                );
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Projectile.velocity * 0.1f + Main.rand.NextVector2Circular(1f, 1f);
            }
        }

        private void ShadowPulse()
        {
            int count = 8 + (int)(ChargeLevel * 4);
            float strength = 2f + ChargeLevel * 1.5f;

            for (int i = 0; i < count; i++)
            {
                Vector2 vel = Main.rand.NextVector2Circular(strength, strength);

                int dust = Dust.NewDust(
                    Projectile.Center,
                    0, 0,
                    DustID.Shadowflame,
                    vel.X,
                    vel.Y,
                    80,
                    default,
                    1.6f + ChargeLevel * 0.4f
                );

                Main.dust[dust].noGravity = true;
            }

            if (ChargeLevel > 2f && Timer % 36 == 0)
            {
                SoundEngine.PlaySound(SoundID.Item9 with { Volume = 0.3f, Pitch = -0.5f }, Projectile.Center);
            }
        }

        private void SpawnDecayField()
        {
            Vector2 backPos = Projectile.Center - Projectile.velocity.SafeNormalize(Vector2.Zero) * 30f;

            int dust = Dust.NewDust(
                backPos - new Vector2(16, 16),
                32, 32,
                DustID.Shadowflame,
                0f, 0f,
                150,
                default,
                0.8f
            );
            Main.dust[dust].noGravity = true;
            Main.dust[dust].alpha = 200;
            Main.dust[dust].velocity = -Projectile.velocity * 0.1f + Main.rand.NextVector2Circular(1f, 1f);

            if (Main.rand.NextBool(4))
            {
                int rift = Dust.NewDust(
                    backPos - new Vector2(8, 8),
                    16, 16,
                    DustID.Shadowflame,
                    0f, 0f,
                    100,
                    default,
                    1.2f
                );
                Main.dust[rift].noGravity = true;
                Main.dust[rift].alpha = 100;
                Main.dust[rift].scale = 1.5f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 240 + (int)(60 * ChargeLevel)); 

            HitCircleEffect(ChargeLevel);

            if (ChargeLevel > 2f)
            {
                target.velocity += Projectile.velocity.SafeNormalize(Vector2.Zero) * 3f;
            }

            Projectile.velocity *= 0.6f;

            SoundEngine.PlaySound(SoundID.DD2_SkeletonHurt with { Volume = 0.8f, Pitch = -0.3f }, Projectile.Center);

            if (ChargeLevel > 2f)
            {
                SpawnShadowChains(target);
            }
        }

        private void HitCircleEffect(float charge)
        {
            int count = 20 + (int)(charge * 10);
            float radius = 24f + charge * 8f;
            float strength = 3f + charge * 2f;

            for (int i = 0; i < count; i++)
            {
                float angle = MathHelper.TwoPi * i / count;
                Vector2 dir = angle.ToRotationVector2();

                int dust = Dust.NewDust(
                    Projectile.Center + dir * radius,
                    0, 0,
                    DustID.Shadowflame,
                    dir.X * strength,
                    dir.Y * strength,
                    120,
                    default,
                    1.6f + charge * 0.3f
                );

                Main.dust[dust].noGravity = true;
            }

            for (int i = 0; i < count / 2; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                Vector2 dir = angle.ToRotationVector2();
                float dist = Main.rand.NextFloat(radius * 0.3f);

                int dust = Dust.NewDust(
                    Projectile.Center + dir * dist,
                    0, 0,
                    DustID.Shadowflame,
                    dir.X * strength * 0.7f,
                    dir.Y * strength * 0.7f,
                    100,
                    default,
                    1.2f + charge * 0.2f
                );

                Main.dust[dust].noGravity = true;
                Main.dust[dust].alpha = 100;
            }
        }

        private void SpawnShadowChains(NPC target)
        {
            if (Main.myPlayer != Projectile.owner) return;

            float range = 200f;
            int chainDamage = (int)(Projectile.damage * 0.6f);

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.whoAmI != target.whoAmI &&
                    npc.Distance(target.Center) < range && Collision.CanHitLine(target.Center, 0, 0, npc.Center, 0, 0))
                {
                    for (int d = 0; d < 15; d++)
                    {
                        float progress = d / 15f;
                        Vector2 pos = Vector2.Lerp(target.Center, npc.Center, progress) +
                                     Main.rand.NextVector2Circular(8f, 8f);

                        int dust = Dust.NewDust(
                            pos, 0, 0,
                            DustID.Shadowflame,
                            0f, 0f,
                            80,
                            default,
                            1.1f
                        );
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].velocity = (npc.Center - target.Center) * 0.05f + Main.rand.NextVector2Circular(1f, 1f);
                    }

                    if (Main.rand.NextBool(3)) 
                    {
                        Projectile.NewProjectile(
                            Projectile.GetSource_OnHit(target),
                            target.Center,
                            (npc.Center - target.Center).SafeNormalize(Vector2.Zero) * 10f,
                            ModContent.ProjectileType<ShadowChain>(), 
                            chainDamage,
                            0f,
                            Projectile.owner
                        );
                    }
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item100 with { Volume = 1.2f, Pitch = -0.3f }, Projectile.Center);
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);

            for (int i = 0; i < 30 + (int)(ChargeLevel * 10); i++)
            {
                Vector2 vel = Main.rand.NextVector2Circular(6f + ChargeLevel * 2f, 6f + ChargeLevel * 2f);

                int dust = Dust.NewDust(
                    Projectile.Center,
                    0, 0,
                    DustID.Shadowflame,
                    vel.X,
                    vel.Y,
                    100,
                    default,
                    1.8f + ChargeLevel * 0.5f
                );

                Main.dust[dust].noGravity = true;

                if (Main.rand.NextBool(3))
                {
                    int spark = Dust.NewDust(
                        Projectile.Center,
                        0, 0,
                        DustID.PurpleTorch,
                        vel.X * 0.5f,
                        vel.Y * 0.5f,
                        80,
                        default,
                        1.2f
                    );
                    Main.dust[spark].noGravity = true;
                }
            }

            for (int i = 0; i < 2; i++)
            {
                float radius = 40f;
                for (int a = 0; a < 40; a++)
                {
                    float angle = MathHelper.TwoPi * a / 40;
                    Vector2 pos = Projectile.Center + angle.ToRotationVector2() * radius;

                    int dust = Dust.NewDust(
                        pos, 0, 0,
                        DustID.Shadowflame,
                        0f, 0f,
                        120,
                        default,
                        1.5f
                    );
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity = angle.ToRotationVector2() * 4f;
                }
            }
        }
    }
    public class ShadowChain : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 30;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 3;
        }

        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Shadowflame, 0, 0, 80, default, 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0.1f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 120);
        }
    }
}