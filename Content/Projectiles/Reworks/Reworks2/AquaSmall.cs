using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Helpers; // предполагаю, что EaseFunctions оттуда

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class AquaSmall : ModProjectile
    {
        private float startSpeed;
        private NPC target;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.alpha = 255;
            Projectile.scale = 1.15f;
            Projectile.extraUpdates = 1;
        }

        private void FindTarget()
        {
            float maxDistance = 400f;
            target = null;
            float closestDistance = maxDistance;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage && !npc.immortal)
                {
                    float distance = Vector2.Distance(Projectile.Center, npc.Center);
                    if (distance < closestDistance && Collision.CanHitLine(
                        Projectile.position, Projectile.width, Projectile.height,
                        npc.position, npc.width, npc.height))
                    {
                        closestDistance = distance;
                        target = npc;
                    }
                }
            }
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0f)
            {
                if (Projectile.velocity.LengthSquared() < 0.01f)
                    Projectile.velocity = Vector2.UnitY * 6f;

                startSpeed = Projectile.velocity.Length();
                Projectile.ai[1] = Main.rand.NextBool() ? 1f : -1f;
            }

            if (Projectile.ai[0] > 10f && Projectile.ai[0] < 60f)
            {
                if (target == null || !target.active || target.life <= 0)
                {
                    FindTarget();
                }

                if (target != null && target.active)
                {
                    Vector2 directionToTarget = target.Center - Projectile.Center;
                    directionToTarget.Normalize();

                    float turnSpeed = 0.08f;
                    Vector2 desiredVelocity = directionToTarget * Projectile.velocity.Length();
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, turnSpeed);
                    Projectile.velocity = Vector2.Normalize(Projectile.velocity) * startSpeed;
                }
            }

            float lifeProgress = 1f - Projectile.timeLeft / 120f;
            float accel = EaseFunctions.EaseOutCubic(MathHelper.Clamp(lifeProgress * 3f, 0f, 1f));
            Projectile.velocity = Vector2.Normalize(Projectile.velocity) *
                MathHelper.Lerp(startSpeed * 0.7f, startSpeed * 1.4f, accel);

            if (Projectile.ai[0] < 24f)
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(
                    0.04f * -Projectile.ai[1] * (Projectile.ai[0] / 24f));
            }

            if (Projectile.alpha > 0)
                Projectile.alpha = Math.Max(Projectile.alpha - 10, 0);

            if (Projectile.alpha < 180 && Main.rand.NextBool(4))
            {
                Dust d = Dust.NewDustDirect(
                    Projectile.position, Projectile.width, Projectile.height,
                    DustID.Water,
                    Scale: Main.rand.NextFloat(1.1f, 1.5f)
                );
                d.noGravity = true;
                d.velocity = d.velocity * 0.3f - Projectile.velocity * 0.25f;
            }

            if (Projectile.timeLeft <= 40)
            {
                float fade = Projectile.timeLeft / 40f;
                fade = EaseFunctions.EaseInCubic(fade);
                Projectile.alpha = (int)(255 * (1f - fade));
                Projectile.scale = MathHelper.Lerp(0.4f, 1.15f, fade);
                Projectile.velocity *= 0.96f;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            Projectile.ai[0]++;

            Lighting.AddLight(Projectile.Center, 0.06f, 0.28f, 0.4f);
        }

        // ← Вот здесь самое важное — красивая смерть
        public override void OnKill(int timeLeft)
        {
            // Вспышка маленьких океанических дастов при смерти
            int dustCount = Main.rand.Next(8, 15); // 8–14 частиц

            for (int i = 0; i < dustCount; i++)
            {
                // Можно использовать либо ванильный DustID, либо твой GlowDust
                int dustType = DustID.WaterCandle; // очень голубой и красивый
                // или твой кастомный: dustType = ModContent.DustType<GlowDust>();

                Dust d = Dust.NewDustPerfect(
                    Projectile.Center,
                    dustType,
                    Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(1.6f, 3.8f),
                    Scale: Main.rand.NextFloat(0.9f, 1.6f)
                );

                d.noGravity = true;
                d.noLight = false;
                d.color = new Color(80, 180, 255); // яркий океанический голубой
                d.alpha = 80; // чуть прозрачный
                d.fadeIn = 0.8f; // плавное появление (важно для GlowDust)

                // Если используешь GlowDust — можно ещё сильнее кастомизировать цвет
                // d.color = new Color(100, 220, 255) * 1.2f;
            }

            // Дополнительно — лёгкая вспышка света
            Lighting.AddLight(Projectile.Center, 0.4f, 0.7f, 1.1f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle frame = tex.Bounds;
            Vector2 origin = frame.Size() / 2f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 pos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Color c = new Color(90, 170, 230, 140) * fade * Projectile.Opacity;

                Main.EntitySpriteDraw(
                    tex, pos, frame, c,
                    Projectile.rotation, origin,
                    Projectile.scale * (1.05f + fade * 0.25f),
                    SpriteEffects.None, 0
                );
            }

            return true;
        }
    }
}