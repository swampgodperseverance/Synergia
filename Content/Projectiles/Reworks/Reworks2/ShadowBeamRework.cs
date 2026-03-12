using System;
using Microsoft.Xna.Framework;
using Synergia.Common.GlobalPlayer;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
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
        private const float FADE_IN_TIME = 24f;
        private const float TOTAL_LIFETIME = 120f;
        private const float LAUNCH_DELAY = FADE_IN_TIME;

        private ref float Timer => ref Projectile.ai[0];
        private ref float State => ref Projectile.ai[1]; 

        private static readonly Color BeamColor = new(180, 80, 255);
        private static readonly Color BeamColorDim = BeamColor * 0.65f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 16;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 6;
            Projectile.timeLeft = (int)TOTAL_LIFETIME;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 60;
            Projectile.scale = 0.01f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            Lighting.AddLight(Projectile.Center, BeamColor.ToVector3() * 0.7f);

            Timer++;

            if (Timer <= LAUNCH_DELAY)
            {
                UpdateAimingPhase(owner);
            }
            else
            {
                if (Timer == LAUNCH_DELAY + 1)
                {
                    LaunchProjectile(owner);
                }

                UpdateFlyingPhase();
            }

            SpawnVisualEffects();
        }

        private void UpdateAimingPhase(Player owner)
        {
            Vector2 direction = owner.DirectionTo(Main.MouseWorld).SafeNormalize(Vector2.Zero);
            Projectile.Center = owner.Center + direction * 54f;
            Projectile.velocity = Vector2.Zero;
            Projectile.rotation = direction.ToRotation() + MathHelper.PiOver2;

            float progress = Timer / LAUNCH_DELAY;
            Projectile.scale = MathHelper.Lerp(0.2f, 1.1f, progress);
            Projectile.alpha = (int)(180 * (1f - progress));
        }

        private void LaunchProjectile(Player owner)
        {
            Projectile.tileCollide = true;

            Vector2 direction = owner.DirectionTo(Main.MouseWorld).SafeNormalize(Vector2.Zero);
            float shootSpeed = owner.HeldItem.shootSpeed * 1.15f; 
            Projectile.velocity = direction * shootSpeed;

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                Vector2.Zero,
                ModContent.ProjectileType<ShadowBeamFlash>(),
                0, 0, Projectile.owner
            );

            SoundEngine.PlaySound(SoundID.Item125 with { Volume = 0.65f, PitchVariance = 0.12f }, Projectile.Center);
        }

        private void UpdateFlyingPhase()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Timer > TOTAL_LIFETIME * 0.7f)
            {
                Projectile.velocity *= 0.985f;
            }
        }

        private void SpawnVisualEffects()
        {
            // Orbiting particles
            if (Main.rand.NextBool(2 + (int)(Timer / 40f)))
            {
                float angle = Timer * 0.22f + Main.rand.NextFloat(-0.4f, 0.4f);
                Vector2 offset = new Vector2(0f, Main.rand.NextFloat(8f, 14f)).RotatedBy(angle);
                var dust = Dust.NewDustPerfect(
                    Projectile.Center + offset,
                    DustID.Shadowflame,
                    Vector2.Zero,
                    140,
                    BeamColor,
                    Main.rand.NextFloat(0.9f, 1.25f)
                );
                dust.noGravity = true;
            }

            if (Main.rand.NextBool(3))
            {
                var trailDust = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(6f, 6f),
                    DustID.ShadowbeamStaff,
                    Projectile.velocity * Main.rand.NextFloat(-0.15f, -0.05f),
                    100,
                    BeamColor * 0.9f,
                    Main.rand.NextFloat(1f, 1.4f)
                );
                trailDust.noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.NewProjectile(
                Projectile.GetSource_Death(),
                Projectile.Center,
                Vector2.Zero,
                ModContent.ProjectileType<ShadowBeamFlash>(),
                0, 0, Projectile.owner
            );

            for (int i = 0; i < 14; i++)
            {
                var dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.ShadowbeamStaff,
                    Main.rand.NextVector2Circular(3.2f, 3.2f),
                    80,
                    BeamColor,
                    1.3f + Main.rand.NextFloat(0.4f)
                );
                dust.noGravity = true;
            }

            SoundEngine.PlaySound(SoundID.Item74 with { Volume = 0.5f, Pitch = -0.3f }, Projectile.Center);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var texture = TextureAssets.Projectile[Type].Value;
            var origin = texture.Size() / 2f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) continue;

                float progress = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Vector2 pos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;

                Color trailColor = BeamColorDim * progress * 0.85f;
                float trailScale = Projectile.scale * (0.7f + progress * 0.6f);

                Main.EntitySpriteDraw(
                    texture, pos, null,
                    trailColor, Projectile.rotation,
                    origin, trailScale, SpriteEffects.None, 0
                );
            }

            Color mainColor = BeamColor * (Projectile.alpha / 255f * -1f + 1f);
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                mainColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
    public class ShadowBeamFlash : ModProjectile
    {
        public override string Texture => "Synergia/Assets/Textures/Glow";

        private const float DESIRED_MAX_SCALE = 0.30f;   

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.timeLeft = 20;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 0;
            Projectile.scale = 0.01f;         
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.scale = 0.01f;           
            Projectile.alpha = 0;
        }

        public override void AI()
        {
            float progress = 1f - (float)Projectile.timeLeft / 20f;
            progress = MathHelper.Clamp(progress, 0f, 1f);

            if (progress < 0.3f)
            {
                Projectile.scale = MathHelper.Lerp(0.01f, DESIRED_MAX_SCALE, progress / 0.3f);
            }
            else
            {
                float remain = (progress - 0.3f) / 0.7f;
                float eased = MathF.Pow(1f - remain, 1.4f);
                Projectile.scale = DESIRED_MAX_SCALE * eased;
            }

            Projectile.alpha = (int)(255 * MathF.Pow(progress, 0.7f));

            Projectile.scale = MathHelper.Clamp(Projectile.scale, 0.01f, DESIRED_MAX_SCALE * 1.15f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var tex = TextureAssets.Projectile[Type].Value;
            var color = new Color(210, 140, 255, 255) * (1f - Projectile.alpha / 255f) * 0.92f; 
            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                color,
                0f,
                tex.Size() / 2f,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
}