using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Dusts;
using Terraria.Audio;
using System;
using Synergia.Common.GlobalPlayer;

namespace Synergia.Content.Projectiles.Friendly
{
    public class JadeGlaiveProj : ModProjectile
    {
        private bool hasDashed = false;
        private NPC target = null;
        private bool isDying = false;
        private float deathTimer = 0f;
        private float outlineIntensity = 0f;

        public static readonly SoundStyle Impulse = new("Synergia/Assets/Sounds/Impulse");

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 260;
            Projectile.height = 40;
            Projectile.width = 40;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (owner.whoAmI == Main.myPlayer && Main.mouseRight && Projectile.timeLeft > 10 && !isDying)
            {
                CreateExplosionDust();
                SoundEngine.PlaySound(Impulse, Projectile.Center);

                var s = Projectile.GetSource_FromThis();
                Projectile.NewProjectile(
                    s,
                    Projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<JadeShardFriendly>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    Projectile.owner
                );

                Projectile.Kill();
                return;
            }

            if (isDying)
            {
                HandleDeathAnimation();
                return;
            }

            if (Projectile.ai[0] == 0)
            {
                float baseRotSpeed = 0.15f;
                float dynamicRot = MathHelper.Lerp(0.05f, 0.4f, 1f - Projectile.velocity.Length() / 10f);
                Projectile.rotation += baseRotSpeed + dynamicRot;
                Projectile.velocity *= 0.97f;

                if (Projectile.timeLeft == 200)
                {
                    target = FindNearestEnemy(Projectile.Center, 700f);
                    if (target != null)
                    {
                        hasDashed = true;
                        Vector2 dashDir = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitY);
                        Projectile.velocity = dashDir * 25f;

                        for (int i = 0; i < 20; i++)
                        {
                            Dust d = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<JadeDust>());
                            d.noGravity = true;
                            d.velocity = dashDir.RotatedByRandom(0.5f) * Main.rand.NextFloat(2f, 6f);
                            d.scale = Main.rand.NextFloat(1.2f, 1.8f);
                        }

                        SoundEngine.PlaySound(SoundID.Item74 with { Pitch = 0.5f, Volume = 1f }, Projectile.Center);
                    }
                }

                if (hasDashed)
                {
                    Projectile.rotation += 1.2f;
                    Projectile.velocity *= 0.985f;
                }

                if (Projectile.timeLeft <= 60 && !isDying)
                {
                    StartDeathSequence();
                }
            }

            if (!isDying)
            {
                outlineIntensity = MathHelper.Lerp(outlineIntensity, Projectile.timeLeft <= 30 ? 1f : 0f, 0.2f);
            }

            Lighting.AddLight(Projectile.Center, 0.1f, 0.8f, 0.3f);
        }

        private void StartDeathSequence()
        {
            isDying = true;
            deathTimer = 0f;
            Projectile.velocity *= 0.5f;

            if (Main.myPlayer == Projectile.owner)
            {
                Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>().TriggerShake(4, 0.3f);
            }

            SoundEngine.PlaySound(SoundID.Item29 with { Pitch = -0.3f, Volume = 0.6f }, Projectile.Center);
        }

        private void HandleDeathAnimation()
        {
            deathTimer++;

            float deathProgress = deathTimer / 30f;

            if (deathTimer <= 15)
            {
                float scale = 1f + deathProgress * 2f;
                Projectile.scale = scale;
                outlineIntensity = deathProgress * 2f;

                if (deathTimer % 3 == 0 && Main.myPlayer == Projectile.owner)
                {
                    Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>().TriggerShake(2, 0.1f);
                }
            }
            else if (deathTimer <= 30)
            {
                Projectile.scale = 2.5f - (deathProgress - 0.5f) * 3f;
                outlineIntensity = 1f - (deathProgress - 0.5f) * 2f;

                Dust deathDust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<JadeDust>());
                deathDust.scale = Main.rand.NextFloat(1.5f, 2.5f);
                deathDust.velocity = Main.rand.NextVector2Circular(4f, 4f);
                deathDust.noGravity = true;
            }

            if (deathTimer >= 30)
            {
                CreateSpectacularDeath();
                Projectile.Kill();
            }
        }

        private void CreateSpectacularDeath()
        {
            if (Main.myPlayer == Projectile.owner)
            {
                Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>().TriggerShake(12, 0.6f);
            }

            for (int i = 0; i < 60; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<JadeDust>());
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(1.8f, 3.5f);
                d.velocity = Main.rand.NextVector2Circular(8f, 8f) * Main.rand.NextFloat(1f, 2.5f);
                d.fadeIn = 0.5f;
            }

            for (int i = 0; i < 15; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.GemEmerald);
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(1.2f, 2f);
                d.velocity = Main.rand.NextVector2Circular(6f, 6f);
            }

            SoundEngine.PlaySound(SoundID.Item14 with { Volume = 1.5f, Pitch = 0.2f }, Projectile.Center);
            SoundEngine.PlaySound(SoundID.Item29 with { Volume = 1.3f, Pitch = 0.6f }, Projectile.Center);

            Lighting.AddLight(Projectile.Center, 0.3f, 2f, 0.5f);
        }

        private void CreateExplosionDust()
        {
            for (int i = 0; i < 30; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<JadeDust>());
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(1.4f, 2.2f);
                d.velocity = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(3f, 8f);
            }

            SoundEngine.PlaySound(SoundID.Item14 with { Volume = 0.9f, Pitch = 0.3f }, Projectile.Center);
        }

        private NPC FindNearestEnemy(Vector2 center, float maxDistance)
        {
            NPC nearest = null;
            float minDist = maxDistance;
            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy(this))
                {
                    float dist = Vector2.Distance(center, npc.Center);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearest = npc;
                    }
                }
            }
            return nearest;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            StartDeathSequence();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            StartDeathSequence();
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(tex.Width / 2, tex.Height / 2);

            if (outlineIntensity > 0.05f)
            {
                float alpha = outlineIntensity * 0.8f;
                float scale = Projectile.scale * (1f + outlineIntensity * 0.3f);

                for (int i = 0; i < 8; i++)
                {
                    float angle = i * MathHelper.TwoPi / 8f;
                    Vector2 offset = angle.ToRotationVector2() * (4f * outlineIntensity);
                    Vector2 outlinePos = Projectile.Center - Main.screenPosition + offset;

                    Color outlineColor = new Color(50, 200, 100, 0) * alpha;
                    Main.spriteBatch.Draw(tex, outlinePos, null, outlineColor,
                        Projectile.rotation + angle * 0.5f, drawOrigin, scale, SpriteEffects.None, 0f);
                }

                Color glowColor = new Color(100, 255, 150, 0) * (outlineIntensity * 0.5f);
                Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, glowColor,
                    Projectile.rotation, drawOrigin, scale, SpriteEffects.None, 0f);
            }

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
                float progress = 1f - (float)k / Projectile.oldPos.Length;
                float alpha = 0.4f * progress * (1f - outlineIntensity * 0.7f);
                Color trailColor = new Color(100, 255, 150, 100) * alpha;
                Main.spriteBatch.Draw(tex, drawPos, null, trailColor,
                    Projectile.rotation, drawOrigin, Projectile.scale * progress, SpriteEffects.None, 0f);
            }

            Color mainColor = isDying ? new Color(255, 255, 200, 255) : Color.White;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, mainColor,
                Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);

            return false;
        }
    }
}