using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Synergia.Trails;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Friendly
{
    public sealed class MagicStalactite : ModProjectile
    {
        private const int DustSpawnRate = 10;
        private bool initialized;
        private float randomScale;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.alpha = 0;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!initialized)
            {
                initialized = true;

                if (player.whoAmI == Main.myPlayer)
                {
                    Vector2 direction = Main.MouseWorld - player.Center;
                    direction.Normalize();
                    direction *= 14f;
                    Projectile.velocity = direction;
                    Projectile.netUpdate = true;
                }

                randomScale = Main.rand.NextFloat(0.85f, 1.25f);

                if (Main.rand.NextBool(6))
                    randomScale *= 1.4f;
            }

            Projectile.velocity *= 1.02f;

            if (Projectile.velocity.Length() > 0.1f)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Projectile.scale = randomScale;
            float pulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 8f) * 0.05f;
            Projectile.scale += pulse;

            if (Main.rand.NextBool(DustSpawnRate))
                SpawnTravelDust();

            Lighting.AddLight(Projectile.Center, 0.9f, 0.55f, 0.15f);
        }

        private void SpawnTravelDust()
        {
            int dustType = DustID.Torch;
            float scale = Main.rand.NextFloat(1.1f, 1.6f);
            Dust dust = Dust.NewDustPerfect(
                Projectile.Center + Main.rand.NextVector2Circular(4f, 8f),
                dustType,
                Projectile.velocity * 0.3f + Main.rand.NextVector2Circular(0.8f, 0.8f),
                60,
                default,
                scale
            );
            dust.noGravity = true;
            dust.fadeIn = 0.9f;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27 with { Volume = 0.7f, PitchVariance = 0.2f }, Projectile.Center);

            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Torch,
                    Main.rand.NextVector2Circular(4.5f, 4.5f),
                    80,
                    default,
                    Main.rand.NextFloat(1.3f, 2.0f)
                );
                dust.noGravity = true;
                dust.fadeIn = 1.3f;
            }

            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Torch,
                    Main.rand.NextVector2Circular(3f, 3f),
                    0,
                    default,
                    Main.rand.NextFloat(0.9f, 1.4f)
                );
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Projectiles/LightTrail_1");
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            SpriteEffects effects = (Projectile.spriteDirection == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            for (int k = 0; k < Projectile.oldPos.Length - 1; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;

                float progress = k / (float)Projectile.oldPos.Length;

                Color color = Color.Lerp(
                    new Color(255, 90, 0, 0),
                    new Color(40, 0, 0, 0),
                    progress
                );

                spriteBatch.Draw(
                    texture,
                    drawPos,
                    null,
                    color * 0.6f,
                    Projectile.oldRot[k] + MathF.PI / 2,
                    drawOrigin,
                    Projectile.scale - progress,
                    effects,
                    0f
                );

                spriteBatch.Draw(
                    texture,
                    drawPos - Projectile.oldPos[k] * 0.5f + Projectile.oldPos[k + 1] * 0.5f,
                    null,
                    color * 0.45f,
                    Projectile.oldRot[k] * 0.5f + Projectile.oldRot[k + 1] * 0.5f + MathF.PI / 2,
                    drawOrigin,
                    Projectile.scale - progress,
                    effects,
                    0f
                );
            }
            return true;
        }
    }
}