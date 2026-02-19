using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.AI;

namespace Synergia.Content.Projectiles.Thrower
{
    public class AzraelsHeartstopper2 : ModProjectile
    {
        private ref float Timer => ref Projectile.ai[0];
        private const float InitialSpeed = 6f;
        private bool boosted;
        private float pulseTimer;
        private int hitEffectTimer = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
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
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;

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

            if (Timer <= 25f)
            {
                Projectile.velocity *= 0.94f;
            }
            else
            {
                Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.UnitY);

                float progress = MathHelper.Clamp((Timer - 25f) / 18f, 0f, 1f);
                float ease = 1f - (float)Math.Pow(1f - progress, 4);

                float targetSpeed = InitialSpeed * 4.8f;
                float startSpeed = InitialSpeed * 0.25f;
                float currentSpeed = MathHelper.Lerp(startSpeed, targetSpeed, ease);

                Projectile.velocity = direction * currentSpeed;

                if (!boosted && progress > 0.15f)
                {
                    boosted = true;
                    SpawnBoostBurst();
                    SoundEngine.PlaySound(SoundID.Item91 with { Volume = 0.8f, Pitch = -0.2f }, Projectile.Center);
                    SoundEngine.PlaySound(SoundID.Item29 with { Volume = 0.5f, Pitch = 0.3f }, Projectile.Center);
                }

                if (Main.rand.NextBool(2))
                {
                    Dust d = Dust.NewDustPerfect(
                        Projectile.Center,
                        DustID.BlueTorch,
                        Projectile.velocity.RotatedByRandom(0.3f) * -0.25f,
                        100,
                        new Color(120, 200, 255),
                        Main.rand.NextFloat(1.1f, 1.7f)
                    );
                    d.noGravity = true;
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        private void SpawnBoostBurst()
        {
            for (int i = 0; i < 40; i++)
            {
                Vector2 velocity = Main.rand.NextVector2CircularEdge(1f, 1f) * Main.rand.NextFloat(5f, 12f);
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.BlueCrystalShard,
                    velocity,
                    80,
                    new Color(100, 200, 255),
                    Main.rand.NextFloat(1.8f, 2.8f)
                );
                d.noGravity = true;
            }

            for (int i = 0; i < 15; i++)
            {
                Vector2 velocity = Main.rand.NextVector2CircularEdge(1f, 1f) * Main.rand.NextFloat(3f, 8f);
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.BlueTorch,
                    velocity,
                    70,
                    new Color(150, 230, 255),
                    Main.rand.NextFloat(2.2f, 3.2f)
                );
                d.noGravity = true;
            }

            for (int i = 0; i < 12; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                float distance = Main.rand.NextFloat(15f, 30f);
                Vector2 offset = angle.ToRotationVector2() * distance;

                Dust d = Dust.NewDustPerfect(
                    Projectile.Center + offset,
                    DustID.Electric,
                    -offset * 0.2f,
                    50,
                    new Color(80, 180, 255),
                    Main.rand.NextFloat(1.2f, 2.0f)
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
                    if (distance < 250f && Main.rand.NextBool(2))
                    {
                        Vector2 offset = Main.rand.NextVector2Circular(npc.width / 2f, npc.height / 2f);
                        float pulse = 0.7f + (float)Math.Sin(pulseTimer * 2 + npc.whoAmI) * 0.3f;
                        float dustScale = Main.rand.NextFloat(1.0f, 2.2f) * pulse;

                        Dust d = Dust.NewDustPerfect(
                            npc.Center + offset,
                            Main.rand.NextBool(3) ? DustID.BlueCrystalShard : DustID.BlueTorch,
                            Main.rand.NextVector2Circular(1.5f, 1.5f),
                            50,
                            new Color(100, 190, 255) * pulse,
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

            for (int i = 0; i < 20; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                float distance = Main.rand.NextFloat(15f, 40f);
                Vector2 offset = angle.ToRotationVector2() * distance;

                Dust d = Dust.NewDustPerfect(
                    target.Center + offset,
                    DustID.BlueTorch,
                    -offset * 0.1f,
                    60,
                    new Color(120, 210, 255),
                    Main.rand.NextFloat(1.5f, 2.8f)
                );
                d.noGravity = true;
            }

            SoundEngine.PlaySound(SoundID.Item14 with { Volume = 0.5f, Pitch = 0.3f }, target.Center);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D trailTex = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Trails/AzraelsHeartstopper_Trail").Value;
            Texture2D ring = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ring").Value;
            Texture2D glow = TextureAssets.Extra[98].Value;

            Vector2 origin = texture.Size() / 2f;
            Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.Zero);
            float pulse = 0.8f + (float)Math.Sin(pulseTimer) * 0.25f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float progress = (float)i / Projectile.oldPos.Length;
                Color trailColor = new Color(80, 180, 255, 100) * (1f - progress);
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;

                spriteBatch.Draw(
                    trailTex,
                    drawPos,
                    null,
                    trailColor,
                    Projectile.rotation,
                    trailTex.Size() / 2f,
                    Projectile.scale * (1f - progress * 0.3f),
                    SpriteEffects.None,
                    0f
                );
            }

            if (boosted)
            {
                for (int i = 1; i <= 4; i++)
                {
                    float progress = (float)i / 4f;
                    float opacity = (1f - progress) * 0.7f;
                    Vector2 forwardOffset = direction * (15f * i);
                    Vector2 drawPos = Projectile.Center + forwardOffset - Main.screenPosition;

                    Color maskColor = new Color(100, 200, 255, 0) * opacity;

                    spriteBatch.Draw(
                        glow,
                        drawPos,
                        null,
                        maskColor,
                        Projectile.rotation,
                        glow.Size() / 2f,
                        Projectile.scale * (1.2f + i * 0.1f),
                        SpriteEffects.None,
                        0f
                    );
                }

                Vector2 frontPos = Projectile.Center + direction * 20f - Main.screenPosition;

                spriteBatch.Draw(
                    glow,
                    frontPos,
                    null,
                    new Color(120, 220, 255, 100) * (0.8f + pulse * 0.2f),
                    0f,
                    glow.Size() / 2f,
                    0.7f * (1f + pulse * 0.3f),
                    SpriteEffects.None,
                    0f
                );

                spriteBatch.Draw(
                    glow,
                    frontPos,
                    null,
                    new Color(80, 160, 255, 50) * pulse,
                    0f,
                    glow.Size() / 2f,
                    0.5f * pulse,
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
                            float ringScale = (npc.width / (float)ring.Width) * 2.2f * (0.8f + npcPulse * 0.4f);
                            Color ringColor = new Color(100, 200, 255, 0) * (0.4f * npcPulse);

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

                            for (int j = 0; j < 5; j++)
                            {
                                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                                float offsetDist = (npc.width / 2f + 15f) * (0.7f + npcPulse * 0.3f);
                                Vector2 sparkPos = npc.Center + angle.ToRotationVector2() * offsetDist;
                                float sparkScale = Main.rand.NextFloat(0.15f, 0.35f) * (1f + npcPulse);
                                Color sparkColor = new Color(120, 210, 255, 50) * (0.6f * npcPulse);

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
            for (int i = 0; i < 25; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(10f, 10f);
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.BlueCrystalShard,
                    velocity,
                    80,
                    new Color(100, 210, 255),
                    Main.rand.NextFloat(1.8f, 3.0f)
                );
                d.noGravity = true;
            }

            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
        }
    }
}
