using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Other
{
    public abstract class TransformProj : ModProjectile
    {
        public abstract int FirstTypeOfDust { get; }
        public abstract int SecondTypeOfDust { get; }
        public abstract int TransformsInItemType { get; }

        const float RiseHeight = 200f;
        const float MaxRiseSpeed = -10f;       // 🔹 чуть быстрее подъем
        const float AccelUp = -0.45f;          // 🔹 ускорили старт подъема
        const float DecelUp = 0.3f;
        const float MaxRotationSpeed = 0.7f;   // 🔹 быстрее вращение

        enum AxeState { Rising, SlowingSpin, MiniRise, Diving }

        AxeState _state = AxeState.Rising;
        float _rotationSpeed = MaxRotationSpeed;
        float _startY;
        int _timer;
        bool _riseDecelerating = false;

        // Dust particles
        const int ParticleCount = 30;
        readonly Vector2[] particlePositions = new Vector2[ParticleCount];
        readonly float[] particleRadii = new float[ParticleCount];
        readonly int[] particleDustTypes = new int[ParticleCount];
        float particleAngleOffset;
        float glowPulse;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.damage = 0;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.alpha = 0;
            Projectile.timeLeft = 300;
        }
        public override void AI()
        {
            _timer++;
            glowPulse = (float)Math.Sin(Main.GameUpdateCount * 0.1f) * 0.3f + 0.7f;

            if (_timer > 200)
            {
                float progress = (float)(_timer - 200) / Projectile.timeLeft;
                Projectile.alpha = (int)MathHelper.Lerp(0f, 255f, progress);
            }

            switch (_state) { case AxeState.Rising: HandleRising(); break; }

            if (_state != AxeState.Diving) Projectile.rotation += _rotationSpeed * Projectile.direction;

            if (Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.MagicMirror, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f), 150, new Color(100, 180, 255), 1.4f);
                Main.dust[dust].velocity *= 0.6f;
                Main.dust[dust].noGravity = true;
            }
            if (Main.rand.NextBool(3))
            {
                int dustType = Main.rand.NextBool() ? FirstTypeOfDust : SecondTypeOfDust;
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 100, new Color(100, 180, 255) * glowPulse, 1.6f);
                Main.dust[dust].velocity *= 0.8f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].fadeIn = 1.2f;
            }
            if (_state == AxeState.SlowingSpin && _timer == 1)
            {
                for (int i = 0; i < 15; i++)
                {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.MagicMirror, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f), 100, Color.White, 1.8f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.5f;
                }
                SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
            }

            particleAngleOffset += 0.05f;

            for (int i = 0; i < ParticleCount; i++)
            {
                if (_timer > 45)
                {
                    particleRadii[i] *= 0.995f;
                    if (particleRadii[i] < 10f) particleRadii[i] = 10f;
                }

                float angle = MathHelper.TwoPi * i / ParticleCount + particleAngleOffset;
                particlePositions[i] = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * particleRadii[i];

                Vector2 dustPos = Projectile.Center + particlePositions[i];
                int dustType = particleDustTypes[i];

                Dust dust = Dust.NewDustPerfect(dustPos, dustType, Vector2.Zero, 150, Color.White, 1.1f);
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
            }

            Lighting.AddLight(Projectile.Center, new Vector3(0.3f, 0.5f, 1.2f) * glowPulse);
            HandleRising();
        }
        void HandleRising()
        {
            if (_timer == 1)
            {
                for (int i = 0; i < ParticleCount; i++)
                {
                    float angle = MathHelper.TwoPi * i / ParticleCount + Main.rand.NextFloat(-0.2f, 0.2f);
                    float radius = Main.rand.NextFloat(30f, 55f);
                    particlePositions[i] = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
                    particleRadii[i] = radius;
                    particleDustTypes[i] = (i % 2 == 0) ? FirstTypeOfDust : SecondTypeOfDust;
                }
                _startY = Projectile.Center.Y;
                Projectile.velocity.Y = 0f;
            }

            float waveOffset = (float)Math.Sin(Main.GameUpdateCount * 0.15f) * 1.8f;
            Projectile.position.Y += waveOffset * 0.1f;

            if (!_riseDecelerating)
            {
                Projectile.velocity.Y += AccelUp;
                if (Projectile.velocity.Y < MaxRiseSpeed) Projectile.velocity.Y = MaxRiseSpeed;
            }
            if (_startY - Projectile.Center.Y >= RiseHeight * 0.7f)
            {
                _riseDecelerating = true;
                Projectile.velocity.Y += DecelUp;
                if (Projectile.velocity.Y >= 0f)
                {
                    Projectile.velocity.Y = 0f;
                    _state = AxeState.SlowingSpin;
                    _timer = 0;
                }
            }
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, -2f, 100, new Color(255, 200, 150), 1.4f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity.Y -= 1f;
            }
            _rotationSpeed = MathHelper.Lerp(_rotationSpeed, MaxRotationSpeed, 0.08f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() * 0.5f;
            Color drawColor = Projectile.GetAlpha(lightColor);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float trailAlpha = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition, null, drawColor * trailAlpha * 0.6f, Projectile.oldRot[i], origin, Projectile.scale * (1f - i * 0.08f), SpriteEffects.None, 0);
            }
            for (int i = 0; i < 4; i++)
            {
                Vector2 offset = new Vector2(2f).RotatedBy(MathHelper.TwoPi * i / 4f);
                Main.EntitySpriteDraw(texture, Projectile.Center + offset - Main.screenPosition, null, Color.White * 0.3f * glowPulse, Projectile.rotation, origin, Projectile.scale * 1.05f, SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, drawColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.MagicMirror, Projectile.velocity.X * 0.3f, Projectile.velocity.Y * 0.3f, 100, new Color(80, 160, 255), 1.8f);
                Main.dust[dust].velocity *= 3f;
                Main.dust[dust].noGravity = true;
            }
            for (int i = 0; i < ParticleCount; i++)
            {
                Vector2 offset = particlePositions[i].RotatedBy(particleAngleOffset);
                Vector2 startPos = Projectile.Center + offset;
                Vector2 velocityToCenter = (Projectile.Center - startPos).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(8f, 12f);

                int dust = Dust.NewDust(startPos, 2, 2, particleDustTypes[i], velocityToCenter.X, velocityToCenter.Y, 100, Color.White * glowPulse, 1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity += velocityToCenter;
            }
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f), 100, Color.Orange, 1.6f);
                Main.dust[dust].noGravity = true;
            }
            if (TransformsInItemType != 0 || TransformsInItemType != -1)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PostTransformProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                Item.NewItem(Projectile.GetSource_FromThis(), Projectile.position, TransformsInItemType);
            }
            SoundEngine.PlaySound(SoundID.Item27 with { PitchVariance = 0.2f }, Projectile.Center);
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
        }
    }
}