using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Content.Dusts; 
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Thrower
{
    public class CupidJavelin : ModProjectile
    {
        private ref float Timer => ref Projectile.ai[0];
        private const float InitialSpeed = 8f;
        private bool boosted;
        private float pulseTimer;
        private int hitEffectTimer = 0;
        private int homingTimer = 0;
        private NPC homingTarget;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 3;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.extraUpdates = 0;

            DrawOffsetX = -17;
            DrawOriginOffsetY = -10;
            DrawOriginOffsetX = 0f;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override void AI()
        {
            Timer++;
            pulseTimer += 0.05f;
            if (pulseTimer > MathHelper.TwoPi) pulseTimer -= MathHelper.TwoPi;

            if (hitEffectTimer > 0)
            {
                hitEffectTimer--;
                SpawnEnemyDustEffects();
            }

            homingTimer++;
            if (homingTimer > 20)
            {
                FindHomingTarget();
                if (homingTarget != null)
                {
                    Vector2 direction = homingTarget.Center - Projectile.Center;
                    direction.Normalize();
                    float speed = Projectile.velocity.Length();
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * speed, 0.04f);
                }
            }

            if (Timer <= 35f)
            {
                Projectile.velocity *= 0.96f;
            }
            else
            {
                Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.UnitY);

                float progress = MathHelper.Clamp((Timer - 35f) / 25f, 0f, 1f);
                float ease = 1f - (float)Math.Pow(1f - progress, 3);

                float targetSpeed = InitialSpeed * 3.2f;
                float startSpeed = InitialSpeed * 0.4f;
                float currentSpeed = MathHelper.Lerp(startSpeed, targetSpeed, ease);

                Projectile.velocity = direction * currentSpeed;

                if (!boosted && progress > 0.2f)
                {
                    boosted = true;
                    SpawnBoostBurst();
                    SoundEngine.PlaySound(SoundID.Item91 with { Volume = 0.6f, Pitch = -0.1f }, Projectile.Center);
                }

                if (Main.rand.NextBool(4))
                {
                    Dust d = Dust.NewDustPerfect(
                        Projectile.Center,
                        ModContent.DustType<RedLineDust>(),
                        Projectile.velocity.RotatedByRandom(0.2f) * -0.2f,
                        50,
                        default,
                        Main.rand.NextFloat(1.2f, 1.8f)
                    );
                    d.noGravity = true;
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Lighting.AddLight(Projectile.Center, 0.6f, 0.15f, 0.15f);
        }

        private void FindHomingTarget()
        {
            float maxDistance = 300f;
            NPC closest = null;
            float closestDistance = maxDistance;

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.life > 0 && npc.CanBeChasedBy())
                {
                    float distance = Vector2.Distance(Projectile.Center, npc.Center);
                    if (distance < closestDistance)
                    {
                        closest = npc;
                        closestDistance = distance;
                    }
                }
            }

            homingTarget = closest;
        }

        private void SpawnBoostBurst()
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 velocity = Main.rand.NextVector2CircularEdge(1f, 1f) * Main.rand.NextFloat(4f, 8f);
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center,
                    ModContent.DustType<RedLineDust>(),
                    velocity,
                    60,
                    default,
                    Main.rand.NextFloat(1.6f, 2.4f)
                );
                d.noGravity = true;
            }

            for (int i = 0; i < 8; i++)
            {
                Vector2 velocity = Main.rand.NextVector2CircularEdge(1f, 1f) * Main.rand.NextFloat(2f, 5f);
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center,
                    ModContent.DustType<RedLineDust>(),
                    velocity,
                    50,
                    default,
                    Main.rand.NextFloat(2.0f, 3.0f)
                );
                d.noGravity = true;
            }
        }

        private void SpawnEnemyDustEffects()
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.life > 0 && npc.CanBeChasedBy())
                {
                    float distance = Vector2.Distance(Projectile.Center, npc.Center);
                    if (distance < 200f && Main.rand.NextBool(3))
                    {
                        Vector2 offset = Main.rand.NextVector2Circular(npc.width / 2f, npc.height / 2f);
                        float pulse = 0.7f + (float)Math.Sin(pulseTimer * 2 + npc.whoAmI) * 0.3f;
                        float dustScale = Main.rand.NextFloat(1.0f, 2.0f) * pulse;

                        Dust d = Dust.NewDustPerfect(
                            npc.Center + offset,
                            ModContent.DustType<RedLineDust>(),
                            Main.rand.NextVector2Circular(1.2f, 1.2f),
                            40,
                            default,
                            dustScale
                        );
                        d.noGravity = true;
                    }
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hitEffectTimer = 90;

            for (int i = 0; i < 15; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                float distance = Main.rand.NextFloat(12f, 30f);
                Vector2 offset = angle.ToRotationVector2() * distance;

                Dust d = Dust.NewDustPerfect(
                    target.Center + offset,
                    ModContent.DustType<RedLineDust>(),
                    -offset * 0.1f,
                    50,
                    default,
                    Main.rand.NextFloat(1.5f, 2.5f)
                );
                d.noGravity = true;
            }

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player p = Main.player[i];
                if (p.active && !p.dead)
                {
                    p.statLife += 5;
                    p.HealEffect(5, true);
                    if (p.statLife > p.statLifeMax2) p.statLife = p.statLifeMax2;
                }
            }

            SoundEngine.PlaySound(SoundID.Item14 with { Volume = 0.4f, Pitch = 0.3f }, target.Center);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D ring = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ring").Value;
            Texture2D glow = TextureAssets.Extra[98].Value;

            Vector2 origin = texture.Size() / 2f;
            Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.Zero);
            float pulse = 0.8f + (float)Math.Sin(pulseTimer) * 0.25f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) continue;

                float progress = (float)i / Projectile.oldPos.Length;
                Color trailColor = new Color(255, 80, 100, 80) * (1f - progress);
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;

                spriteBatch.Draw(
                    texture,
                    drawPos,
                    null,
                    trailColor,
                    Projectile.rotation,
                    origin,
                    Projectile.scale * (1f - progress * 0.6f),
                    SpriteEffects.None,
                    0f
                );
            }

            if (boosted)
            {
                for (int i = 1; i <= 3; i++)
                {
                    float progress = (float)i / 3f;
                    float opacity = (1f - progress) * 0.6f;
                    Vector2 forwardOffset = direction * (12f * i);
                    Vector2 drawPos = Projectile.Center + forwardOffset - Main.screenPosition;

                    Color maskColor = new Color(255, 100, 120, 0) * opacity;

                    spriteBatch.Draw(
                        glow,
                        drawPos,
                        null,
                        maskColor,
                        Projectile.rotation,
                        glow.Size() / 2f,
                        Projectile.scale * (1.1f + i * 0.1f),
                        SpriteEffects.None,
                        0f
                    );
                }

                Vector2 frontPos = Projectile.Center + direction * 18f - Main.screenPosition;

                spriteBatch.Draw(
                    glow,
                    frontPos,
                    null,
                    new Color(255, 100, 120, 80) * (0.7f + pulse * 0.2f),
                    0f,
                    glow.Size() / 2f,
                    0.6f * (1f + pulse * 0.2f),
                    SpriteEffects.None,
                    0f
                );
            }

            if (hitEffectTimer > 0)
            {
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && npc.life > 0 && npc.CanBeChasedBy())
                    {
                        float distance = Vector2.Distance(Projectile.Center, npc.Center);
                        if (distance < 300f)
                        {
                            float npcPulse = 0.5f + (float)Math.Sin(pulseTimer * 4 + npc.whoAmI) * 0.5f;
                            float ringScale = (npc.width / (float)ring.Width) * 2.0f * (0.8f + npcPulse * 0.3f);
                            Color ringColor = new Color(255, 80, 100, 0) * (0.3f * npcPulse);

                            spriteBatch.Draw(
                                ring,
                                npc.Center - Main.screenPosition,
                                null,
                                ringColor,
                                0f,
                                ring.Size() / 2f,
                                ringScale,
                                SpriteEffects.None,
                                0f
                            );

                            for (int j = 0; j < 4; j++)
                            {
                                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                                float offsetDist = (npc.width / 2f + 12f) * (0.7f + npcPulse * 0.2f);
                                Vector2 sparkPos = npc.Center + angle.ToRotationVector2() * offsetDist;
                                float sparkScale = Main.rand.NextFloat(0.12f, 0.3f) * (1f + npcPulse * 0.5f);
                                Color sparkColor = new Color(255, 100, 120, 40) * (0.5f * npcPulse);

                                spriteBatch.Draw(
                                    glow,
                                    sparkPos - Main.screenPosition,
                                    null,
                                    sparkColor,
                                    0f,
                                    glow.Size() / 2f,
                                    sparkScale,
                                    SpriteEffects.None,
                                    0f
                                );
                            }
                        }
                    }
                }
            }

            spriteBatch.Draw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center,
                    ModContent.DustType<RedLineDust>(),
                    velocity,
                    60,
                    default,
                    Main.rand.NextFloat(1.6f, 2.6f)
                );
                d.noGravity = true;
            }

            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
        }
    }
}