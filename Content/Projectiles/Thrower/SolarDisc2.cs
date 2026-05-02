using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Common.GlobalPlayer;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;

namespace Synergia.Content.Projectiles.Thrower
{
    public class SolarDisc2 : ModProjectile
    {
        private NPC target;
        private bool hasTarget = false;
        private bool isHovering = false;
        private bool isDiving = false;
        private float hoverTimer = 0f;
        private float diveCharge = 0f;
        private float outlineIntensity = 0f;
        private float originalHeight = 0f;
        private Vector2 hoverPosition;
        private float targetLockTimer = 0f;
        private bool isLocked = false;

        private const float MAX_HOVER_TIME = 60f;
        private const float DIVE_DAMAGE_MULTIPLIER = 0.15f;
        private const float LOCK_TIME = 20f; 

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.height = 28;
            Projectile.width = 28;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
        }

        public override void AI()
        {
            if (!hasTarget)
            {
                FindTarget();
            }

            if (!hasTarget)
            {
                FreeFall();
                return;
            }

            if (!isLocked && !isHovering && !isDiving)
            {
                ApproachAndLockTarget();
            }
            else if (isLocked && !isHovering && !isDiving)
            {
                HandleLock();
            }
            else if (isHovering && !isDiving)
            {
                HandleHover();
            }
            else if (isDiving)
            {
                HandleDive();
            }

            UpdateOutlineIntensity();
            AddRingTrail();
            Lighting.AddLight(Projectile.Center, 1f, 0.5f, 0.2f);

            // Увеличенная скорость вращения
            float rotationSpeed = isDiving ? 0.8f : 0.6f;
            Projectile.rotation += rotationSpeed;
        }

        private void FindTarget()
        {
            float closestDist = 800f;
            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy(this) && npc.active)
                {
                    float dist = Vector2.Distance(Projectile.Center, npc.Center);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        target = npc;
                        hasTarget = true;
                        originalHeight = Projectile.Center.Y;

                        SoundEngine.PlaySound(SoundID.Item9 with { Pitch = 0.2f, Volume = 0.8f }, Projectile.Center);

                        for (int i = 0; i < 15; i++)
                        {
                            Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.SolarFlare);
                            d.velocity = Main.rand.NextVector2Circular(3f, 3f);
                            d.scale = Main.rand.NextFloat(1f, 1.8f);
                            d.noGravity = true;
                        }
                    }
                }
            }
        }

        private void FreeFall()
        {
            Projectile.velocity.Y += 0.2f;
            Projectile.velocity.X *= 0.99f;

            if (Projectile.velocity.Y > 10f)
                Projectile.velocity.Y = 10f;
        }

        private void ApproachAndLockTarget()
        {
            if (target == null || !target.active)
            {
                hasTarget = false;
                return;
            }

            Vector2 targetPos = target.Center - new Vector2(0, target.height * 0.6f);
            Vector2 toTarget = targetPos - Projectile.Center;
            float distance = toTarget.Length();

            if (distance < 100f)
            {
                isLocked = true;
                targetLockTimer = 0f;
                Projectile.velocity *= 0.3f;

                SoundEngine.PlaySound(SoundID.Item15 with { Pitch = 0.3f, Volume = 0.5f }, Projectile.Center);

                for (int i = 0; i < 20; i++)
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.SolarFlare);
                    d.velocity = Main.rand.NextVector2Circular(2f, 2f);
                    d.scale = Main.rand.NextFloat(1f, 1.5f);
                    d.noGravity = true;
                }
                return;
            }

            Vector2 direction = toTarget.SafeNormalize(Vector2.UnitY);
            Projectile.velocity = direction * 16f;
        }

        private void HandleLock()
        {
            if (target == null || !target.active)
            {
                isHovering = true;
                isLocked = false;
                return;
            }

            targetLockTimer++;

            Vector2 targetPos = target.Center - new Vector2(0, target.height * 0.6f);

            float lockRadius = 25f;
            float angle = targetLockTimer * 0.15f;
            Vector2 offset = new Vector2(
                (float)Math.Cos(angle) * lockRadius,
                (float)Math.Sin(angle * 1.5f) * lockRadius * 0.6f
            );

            Projectile.Center = targetPos + offset;
            Projectile.velocity *= 0.95f;
            outlineIntensity = MathHelper.Lerp(outlineIntensity, 0.8f, 0.1f);
            if (Main.rand.NextBool(5))
            {
                for (int i = 0; i < 3; i++)
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.SolarFlare);
                    d.velocity = Main.rand.NextVector2Circular(1.5f, 1.5f);
                    d.scale = Main.rand.NextFloat(0.8f, 1.3f);
                    d.noGravity = true;
                    d.fadeIn = 0.5f;
                }
            }

            if (targetLockTimer >= LOCK_TIME)
            {
                isLocked = false;
                isHovering = true;
                hoverTimer = 0f;
                hoverPosition = targetPos;
            }
        }

        private void HandleHover()
        {
            if (target == null || !target.active)
            {
                isDiving = true;
                return;
            }

            hoverTimer++;
            hoverPosition = target.Center - new Vector2(0, target.height * 0.6f);

            float hoverRadius = 8f;
            Vector2 offset = new Vector2(
                (float)Math.Sin(hoverTimer * 0.1f) * hoverRadius,
                (float)Math.Cos(hoverTimer * 0.15f) * hoverRadius * 0.5f
            );

            Projectile.Center = hoverPosition + offset;
            Projectile.velocity *= 0.95f;

            if (hoverTimer >= MAX_HOVER_TIME)
            {
                StartDive();
            }

            if (Main.rand.NextBool(8))
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.SolarFlare);
                d.velocity = Main.rand.NextVector2Circular(1f, 1f);
                d.scale = Main.rand.NextFloat(0.8f, 1.2f);
                d.noGravity = true;
                d.fadeIn = 0.5f;
            }
        }

        private void StartDive()
        {
            isHovering = false;
            isDiving = true;
            diveCharge = 0f;

            float fallHeight = Math.Abs(originalHeight - Projectile.Center.Y);
            float damageBonus = 1f + (fallHeight / 500f) * DIVE_DAMAGE_MULTIPLIER;
            Projectile.damage = (int)(Projectile.damage * damageBonus);

            SoundEngine.PlaySound(SoundID.Item60 with { Pitch = -0.3f, Volume = 1f }, Projectile.Center);

            for (int i = 0; i < 30; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.SolarFlare);
                d.velocity = Main.rand.NextVector2Circular(5f, 5f);
                d.scale = Main.rand.NextFloat(1.5f, 2.5f);
                d.noGravity = true;
            }

            if (Main.myPlayer == Projectile.owner)
            {
                Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>().TriggerShake(15, 0.4f); // Уменьшена тряска
            }
        }

        private void HandleDive()
        {
            diveCharge++;

            if (diveCharge <= 15)
            {
                outlineIntensity = diveCharge / 15f;
                Projectile.velocity *= 0.95f;

                if (diveCharge % 3 == 0)
                {
                    CreateAfterimageRing();

                    if (Main.myPlayer == Projectile.owner)
                    {
                        Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>().TriggerShake(4, 0.15f); // Уменьшена тряска
                    }
                }
            }
            else
            {
                Vector2 diveDirection = Vector2.UnitY * 18f;
                Projectile.velocity = diveDirection;

                Dust trailDust = Dust.NewDustPerfect(Projectile.Center, DustID.SolarFlare);
                trailDust.velocity = -Projectile.velocity * 0.3f;
                trailDust.scale = Main.rand.NextFloat(1.2f, 2f);
                trailDust.noGravity = true;
            }

            if (Projectile.Center.Y > target.Center.Y + target.height / 2 + 30f || diveCharge > 45)
            {
                Impact();
            }
        }

        private void CreateAfterimageRing()
        {
            for (int i = 0; i < 12; i++)
            {
                float angle = i * MathHelper.TwoPi / 12f + diveCharge;
                Vector2 offset = angle.ToRotationVector2() * 25f;

                Dust ringDust = Dust.NewDustPerfect(Projectile.Center + offset, DustID.SolarFlare);
                ringDust.velocity = offset * 0.1f;
                ringDust.scale = Main.rand.NextFloat(1f, 1.8f);
                ringDust.noGravity = true;
                ringDust.fadeIn = 0.3f;
            }
        }

        private void Impact()
        {
            if (Main.myPlayer == Projectile.owner)
            {
                Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>().TriggerShake(8, 0.3f); 
            }

            SoundEngine.PlaySound(SoundID.Item14 with { Volume = 1.5f, Pitch = 0.1f }, Projectile.Center);
            SoundEngine.PlaySound(SoundID.Item74 with { Volume = 1.2f, Pitch = -0.2f }, Projectile.Center);

            for (int i = 0; i < 50; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.SolarFlare);
                d.velocity = Main.rand.NextVector2Circular(8f, 8f);
                d.scale = Main.rand.NextFloat(1.5f, 3f);
                d.noGravity = true;
                d.fadeIn = 0.7f;
            }

            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.Torch);
                d.velocity = Main.rand.NextVector2Circular(5f, 5f);
                d.scale = Main.rand.NextFloat(1f, 2f);
                d.noGravity = false;
            }

            Lighting.AddLight(Projectile.Center, 2f, 1f, 0.3f);

            Projectile.Kill();
        }

        private void UpdateOutlineIntensity()
        {
            if (!isDiving && !isLocked)
            {
                outlineIntensity = MathHelper.Lerp(outlineIntensity, 0.5f, 0.05f);
            }
        }

        private void AddRingTrail()
        {
            if (Projectile.oldPos.Length > 1)
            {
                for (int i = 1; i < Projectile.oldPos.Length; i++)
                {
                    if (Projectile.oldPos[i] != Vector2.Zero)
                    {
                        Vector2 pos = Projectile.oldPos[i];
                        float alpha = 1f - (i / (float)Projectile.oldPos.Length);
                        alpha *= isDiving ? 0.9f : 0.5f;
                        for (int j = 0; j < 2; j++)
                        {
                            Dust ring = Dust.NewDustPerfect(pos + Main.rand.NextVector2Circular(5f, 5f), DustID.SolarFlare);
                            ring.scale = 1.2f * alpha;
                            ring.alpha = (int)(200 * (1f - alpha));
                            ring.noGravity = true;
                            ring.velocity = Vector2.Zero;
                        }
                    }
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!isDiving)
            {
                StartDive();
            }

            for (int i = 0; i < 15; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.SolarFlare);
                d.velocity = Main.rand.NextVector2Circular(4f, 4f);
                d.scale = Main.rand.NextFloat(1f, 1.8f);
                d.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D ringTex = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ring").Value;
            Vector2 drawOrigin = new(tex.Width / 2, tex.Height / 2);
            Vector2 ringOrigin = new(ringTex.Width / 2, ringTex.Height / 2);
            if (outlineIntensity > 0.05f)
            {
                float glowAlpha = outlineIntensity * 0.9f;
                float scale = Projectile.scale * (1f + outlineIntensity * 0.8f);
                for (int i = 0; i < 16; i++)
                {
                    float angle = i * MathHelper.TwoPi / 16f;
                    float offsetRadius = 8f * outlineIntensity;
                    Vector2 offset = angle.ToRotationVector2() * offsetRadius;
                    Vector2 outlinePos = Projectile.Center - Main.screenPosition + offset;

                    Color outlineColor = new Color(255, 120, 50, 0) * glowAlpha;
                    float outlineRot = Projectile.rotation + angle * 0.3f;
                    Main.spriteBatch.Draw(tex, outlinePos, null, outlineColor,
                        outlineRot, drawOrigin, scale * 0.9f, SpriteEffects.None, 0f);
                }

                if (isDiving && diveCharge <= 15)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        float angle = i * MathHelper.TwoPi / 12f + diveCharge * 0.5f;
                        float radius = 35f * (1f - diveCharge / 15f);
                        Vector2 ringPos = Projectile.Center - Main.screenPosition + angle.ToRotationVector2() * radius;

                        Color ringColor = new Color(255, 200, 100, 100) * glowAlpha;
                        float ringScale = 1.0f * (1f - diveCharge / 15f);
                        Main.spriteBatch.Draw(ringTex, ringPos, null, ringColor,
                            angle, ringOrigin, ringScale, SpriteEffects.None, 0f);
                    }
                }
            }
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (Projectile.oldPos[k] == Vector2.Zero) continue;

                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
                float progress = 1f - (k / (float)Projectile.oldPos.Length);
                float alpha = 0.4f * progress;

                Color trailColor = new Color(255, 100, 50, 80) * alpha;
                Main.spriteBatch.Draw(ringTex, drawPos, null, trailColor,
                    Projectile.rotation, ringOrigin, 0.08f, SpriteEffects.None, 0f);
            }

            Color mainColor = (isDiving || isLocked) ? new Color(255, 200, 100, 255) : Color.White;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, mainColor,
                Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);

            float ringAlpha = (isHovering || isLocked) ? 0.6f : 0.3f;
            Main.spriteBatch.Draw(ringTex, Projectile.Center - Main.screenPosition, null,
                new Color(255, 100, 50, 80) * ringAlpha,
                Projectile.rotation * 0.5f, ringOrigin, 0.12f, SpriteEffects.None, 0f);

            return false;
        }
    }
}