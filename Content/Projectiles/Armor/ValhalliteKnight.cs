using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Armor
{
    public class ValhalliteKnight : ModProjectile
    {
        private float alpha = 0f;
        private float shootTimer = 0f;
        private Vector2 recoilVelocity = Vector2.Zero;
        private bool fadingOut = false;
        private bool hasSpawnedIn = false;

        private const float FadeInTime = 24f;
        private const float FadeOutTime = 38f;
        private const float RecoilPower = 0.38f;
        private const float RecoilDamping = 0.86f;

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.netImportant = true;
            Projectile.Opacity = 0f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            Vector2 targetPos = player.Center + new Vector2(0, -60f);
            Projectile.Center = Vector2.Lerp(Projectile.Center, targetPos, 0.15f);

            Projectile.Center += recoilVelocity;
            recoilVelocity *= RecoilDamping;

            Projectile.spriteDirection = player.direction;

            if (!fadingOut)
            {
                alpha += 1f / FadeInTime;
                if (alpha >= 1f)
                {
                    alpha = 1f;
                    if (!hasSpawnedIn)
                    {
                        SpawnAppearParticles();
                        hasSpawnedIn = true;
                    }
                }
            }

            if (Projectile.timeLeft < FadeOutTime)
            {
                fadingOut = true;
                alpha -= 1f / FadeOutTime;
                if (alpha <= 0f) alpha = 0f;
            }

            Projectile.Opacity = alpha * 0.8f; 

            Projectile.position.Y += (float)Math.Sin(Main.GameUpdateCount * 0.1f) * 0.3f;

            shootTimer++;
            if (shootTimer >= 60f)
            {
                shootTimer = 0f;
                NPC target = FindClosestEnemy(player.Center, 600f);
                if (target != null && Main.myPlayer == Projectile.owner)
                {
                    Vector2 shootDir = Vector2.Normalize(target.Center - Projectile.Center);
                    Vector2 shootVel = shootDir * 12f;
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromAI(),
                        Projectile.Center,
                        shootVel,
                        ModContent.ProjectileType<ValhalliteKnightIceProj>(),
                        Projectile.damage,
                        0f,
                        player.whoAmI
                    );

                    SoundEngine.PlaySound(SoundID.Item27 with { Volume = 0.75f, PitchVariance = 0.3f }, Projectile.Center);

                    recoilVelocity = -shootDir * RecoilPower;

                    for (int i = 0; i < 7; i++)
                    {
                        Dust d = Dust.NewDustPerfect(
                            Projectile.Center + shootDir * 14f,
                            DustID.IceTorch,
                            shootDir.RotatedByRandom(0.45f) * Main.rand.NextFloat(1.0f, 2.8f),
                            100,
                            default,
                            Main.rand.NextFloat(1.2f, 1.7f)
                        );
                        d.noGravity = true;
                        d.fadeIn = 0.4f;
                    }

                    Lighting.AddLight(Projectile.Center, 0.5f, 0.7f, 1.2f);
                }
            }

            Lighting.AddLight(Projectile.Center, 0.4f, 0.6f, 1.2f);

            if (Main.rand.NextBool(40) && alpha > 0.75f && !fadingOut)
            {
                SpawnSmallEdgeSparkle();
            }
        }

        private void SpawnAppearParticles()
        {
            for (int i = 0; i < 5; i++)
            {
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.IceTorch,
                    Main.rand.NextVector2Circular(2f, 2f),
                    100,
                    default,
                    Main.rand.NextFloat(1.2f, 1.8f)
                );
                d.noGravity = true;
                d.fadeIn = 0.5f;

                Dust d2 = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(10f, 10f),
                    DustID.IceTorch,
                    Main.rand.NextVector2Circular(1f, 1f),
                    100,
                    default,
                    Main.rand.NextFloat(0.8f, 1.2f)
                );
                d2.noGravity = true;
            }

            for (int i = 0; i < 20; i++)
            {
                Vector2 dir = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.IceTorch,
                    dir * Main.rand.NextFloat(2f, 5f),
                    0,
                    default,
                    Main.rand.NextFloat(1.0f, 1.5f)
                ).noGravity = true;
            }
        }

        private void SpawnSmallEdgeSparkle()
        {
            Vector2 offset = Main.rand.NextVector2CircularEdge(Projectile.width * 0.45f, Projectile.height * 0.45f);

            for (int i = 0; i < 3; i++)
            {
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center + offset,
                    DustID.IceTorch,
                    offset * 0.1f + Main.rand.NextVector2Circular(0.3f, 0.3f),
                    100,
                    default,
                    Main.rand.NextFloat(0.6f, 1.0f)
                );
                d.noGravity = true;
                d.fadeIn = 0.3f;
            }
        }

        private void SpawnDisappearParticles()
        {
            for (int i = 0; i < 6; i++)
            {
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.IceTorch,
                    Main.rand.NextVector2Circular(3f, 3f),
                    100,
                    default,
                    Main.rand.NextFloat(1.0f, 1.5f)
                );
                d.noGravity = true;
                d.fadeIn = 0.4f;
            }

            for (int i = 0; i < 18; i++)
            {
                Vector2 dir = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.IceTorch,
                    dir * Main.rand.NextFloat(2.5f, 6f),
                    0,
                    default,
                    Main.rand.NextFloat(1.0f, 1.7f)
                ).noGravity = true;
            }
        }

        private NPC FindClosestEnemy(Vector2 center, float maxDistance)
        {
            NPC closest = null;
            float sqrMax = maxDistance * maxDistance;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy() && !npc.friendly && npc.active)
                {
                    float sqrDist = Vector2.DistanceSquared(center, npc.Center);
                    if (sqrDist < sqrMax)
                    {
                        sqrMax = sqrDist;
                        closest = npc;
                    }
                }
            }
            return closest;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            float lerpTime = alpha;
            float lighting = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            lighting = MathHelper.Clamp(lighting * 1.5f, 0.3f, 1f); 
            Color baseColor = Color.White * Projectile.Opacity * lighting;
            Main.EntitySpriteDraw(texture, drawPos, null, baseColor, Projectile.rotation, texture.Size() / 2f, Projectile.scale, effects, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Vector2 centerSparkle = drawPos;
            DrawPrettyStarSparkle(
                Projectile.Opacity * 2f, 
                SpriteEffects.None,
                centerSparkle,
                new Color(255, 255, 255, 0) * lerpTime * 0.6f,
                new Color(180, 240, 255),
                Main.GameUpdateCount % 100f / 100f,
                0f, 0.3f, 0.7f, 1f,
                Projectile.rotation,
                new Vector2(1.6f, Utils.Remap(lerpTime, 0f, 1f, 3f, 0.8f, true)) * Projectile.scale,
                Vector2.One * Projectile.scale * 1f
            );

            for (float i = 0f; i < 8f; i += 1f)
            {
                float rotOffset = Projectile.rotation + (float)Math.Sin(Main.GameUpdateCount * 0.08f + i) * 0.18f;
                Vector2 sparklePos = drawPos + rotOffset.ToRotationVector2() * (texture.Width * 0.48f) * Projectile.scale;

                DrawPrettyStarSparkle(
                    Projectile.Opacity * 1.5f,
                    SpriteEffects.None,
                    sparklePos,
                    new Color(255, 255, 255, 0) * lerpTime * (i / 10f),
                    new Color(180, 240, 255),
                    Main.GameUpdateCount % 120f / 120f,
                    0f, 0.4f, 0.5f, 0.9f,
                    rotOffset,
                    new Vector2(0f, Utils.Remap(lerpTime, 0f, 1f, 2.8f, 0.5f, true)) * Projectile.scale,
                    Vector2.One * Projectile.scale * 0.9f
                );
            }

  
            Color glowColor = new Color(180, 240, 255) * Projectile.Opacity * 0.4f;
            Main.EntitySpriteDraw(texture, drawPos, null, glowColor, Projectile.rotation, texture.Size() / 2f, Projectile.scale * 1.05f, effects, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        private static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawPos, Color drawColor, Color shineColor,
            float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd,
            float rotation, Vector2 scale, Vector2 fatness)
        {
            Texture2D tex = TextureAssets.Extra[98].Value;
            Color bigColor = shineColor * opacity * 0.6f;
            bigColor.A = 0;

            Vector2 origin = tex.Size() / 2f;

            float lerpValue = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, true) *
                              Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, true);

            Vector2 scaleLR = new Vector2(fatness.X * 0.5f, scale.X) * lerpValue;
            Vector2 scaleUD = new Vector2(fatness.Y * 0.5f, scale.Y) * lerpValue;

            bigColor *= lerpValue;
            drawColor *= lerpValue;

            Main.EntitySpriteDraw(tex, drawPos, null, bigColor, MathHelper.PiOver2 + rotation, origin, scaleLR, dir, 0);
            Main.EntitySpriteDraw(tex, drawPos, null, bigColor, 0f + rotation, origin, scaleUD, dir, 0);
            Main.EntitySpriteDraw(tex, drawPos, null, drawColor * 0.6f, MathHelper.PiOver2 + rotation, origin, scaleLR * 0.6f, dir, 0);
            Main.EntitySpriteDraw(tex, drawPos, null, drawColor * 0.6f, 0f + rotation, origin, scaleUD * 0.6f, dir, 0);
        }

        public override void OnKill(int timeLeft)
        {
            SpawnDisappearParticles();
            SoundEngine.PlaySound(SoundID.Item30 with { Volume = 0.7f, Pitch = -0.3f }, Projectile.Center);
            Lighting.AddLight(Projectile.Center, 0.6f, 0.9f, 1.4f);
        }
    }
}