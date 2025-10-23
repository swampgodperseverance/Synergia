using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Synergia.Helpers;
using System;

namespace Synergia.Content.Items.Weapons.Melee
{
    public class SeabornAttack2 : ModProjectile
    {
        private const float RiseHeight = 200f;        
        private const float MaxRiseSpeed = -10f;       // 🔹 чуть быстрее подъем
        private const float AccelUp = -0.45f;          // 🔹 ускорили старт подъема
        private const float DecelUp = 0.3f;
        private const float FallSpeed = 22f;           // 🔹 падение быстрее
        private const float HoverDelay = 25f;
        private const float MiniRiseDuration = 40f;    // 🔹 мини-подъем быстрее
        private const float MaxRotationSpeed = 0.7f;   // 🔹 быстрее вращение

        private enum AxeState { Rising, SlowingSpin, MiniRise, Diving }

        private AxeState _state = AxeState.Rising;
        private float _rotationSpeed = MaxRotationSpeed;
        private float _startY;
        private int _spawnedSwordCount = 0; 
        private int _nextSwordSpawnTick = 0;
        private int _timer;
        private bool _riseDecelerating = false;
        private Vector2 _miniRiseStart;
        private bool _spawnedSwords = false;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.damage = 65;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 0;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            _timer++;

            switch (_state)
            {
                case AxeState.Rising:
                    HandleRising();
                    break;
                case AxeState.SlowingSpin:
                    HandleSlowingSpin();
                    break;
                case AxeState.MiniRise:
                    HandleMiniRise();
                    break;
                case AxeState.Diving:
                    HandleDiving();
                    break;
            }

            // вращение пока не падает
            if (_state != AxeState.Diving)
                Projectile.rotation += _rotationSpeed * Projectile.direction;

            // 💧 вспышка воды
            if (Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.DungeonWater, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f),
                    150, new Color(100, 180, 255), 1.4f);
                Main.dust[dust].velocity *= 0.6f;
                Main.dust[dust].noGravity = true;
            }

            Lighting.AddLight(Projectile.Center, new Vector3(0.25f, 0.45f, 1.0f));
        }

        // 🔹 Подъём
        private void HandleRising()
        {
            if (_timer == 1)
            {
                _startY = Projectile.Center.Y;
                Projectile.velocity.Y = 0f;
            }

            // лёгкое "дыхание"
            float waveOffset = (float)Math.Sin(Main.GameUpdateCount * 0.15f) * 1.8f;
            Projectile.position.Y += waveOffset * 0.1f;

            if (!_riseDecelerating)
            {
                Projectile.velocity.Y += AccelUp;
                if (Projectile.velocity.Y < MaxRiseSpeed)
                    Projectile.velocity.Y = MaxRiseSpeed;
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

            _rotationSpeed = MathHelper.Lerp(_rotationSpeed, MaxRotationSpeed, 0.08f);
        }

        // 🔹 Постепенное замедление
        private void HandleSlowingSpin()
        {
            _rotationSpeed = MathHelper.Lerp(_rotationSpeed, 0f, 0.08f);

            if (_timer >= 25) // быстрее переход к мини-подъему
            {
                _rotationSpeed = 0f;
                _miniRiseStart = Projectile.Center;
                _state = AxeState.MiniRise;
                _timer = 0;
            }
        }

        private void HandleMiniRise()
        {
            float t = _timer / MiniRiseDuration;
            if (t > 1f) t = 1f;

            float eased = EaseFunctions.EaseOutCubic(t);
            float riseAmount = MathHelper.Lerp(0f, -35f, eased);
            Projectile.Center = _miniRiseStart + new Vector2(0, riseAmount);

            // 🌊 Спавн мечей SeabornSword2 4 раза с интервалом 10 тиков
            if (_spawnedSwordCount < 4 && _timer >= _nextSwordSpawnTick && Main.myPlayer == Projectile.owner)
            {
                int swordCount = Main.rand.Next(3, 6);
                for (int i = 0; i < swordCount; i++)
                {
                    Vector2 offset = new Vector2(Main.rand.NextFloat(-60f, 60f), Main.rand.NextFloat(-70f, -30f));
                    Vector2 spawnPos = Projectile.Center + offset;
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        spawnPos,
                        new Vector2(0f, Main.rand.NextFloat(-1.5f, -2.5f)), // мечи всплывают
                        ModContent.ProjectileType<SeabornSword2>(),
                        Projectile.damage / 2,
                        0f,
                        Projectile.owner
                    );
                }
                SoundEngine.PlaySound(SoundID.Item29, Projectile.Center);

                _spawnedSwordCount++;
                _nextSwordSpawnTick = _timer + 10; // следующий спавн через 10 тиков
            }

            if (_timer >= MiniRiseDuration)
            {
                Projectile.velocity.Y = FallSpeed;
                _state = AxeState.Diving;
                _timer = 0;
                SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            }
        }


        // 💫 Падение
        private void HandleDiving()
        {
            Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, FallSpeed, 0.25f);

            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.DungeonWater,
                    0f, 2f, 150, new Color(80, 160, 255), 1.2f);
                Main.dust[dust].velocity *= 0.7f;
                Main.dust[dust].noGravity = true;
            }

            if (Projectile.velocity.Y > 0 && Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                Projectile.Kill();
            }
        }

        // 🎨 Отрисовка
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() * 0.5f;
            Color drawColor = Projectile.GetAlpha(lightColor);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float trailAlpha = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Main.EntitySpriteDraw(
                    texture,
                    Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition,
                    null,
                    drawColor * trailAlpha * 0.5f,
                    Projectile.oldRot[i],
                    origin,
                    Projectile.scale * (1f - i * 0.1f),
                    SpriteEffects.None,
                    0);
            }

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                drawColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0);

            return false;
        }

        // 💥 Разрушение
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 14; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.DungeonWater, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 150,
                    new Color(80, 180, 255), 1.6f);
                Main.dust[dust].velocity *= 2.8f;
            }
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
        }
    }
}
