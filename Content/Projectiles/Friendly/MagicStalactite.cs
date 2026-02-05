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

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
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
            }

            Projectile.velocity *= 1.02f;

            if (Projectile.velocity.Length() > 0.1f)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

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
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            SpriteEffects effects = (Projectile.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (Projectile.oldPos[k] == Vector2.Zero)
                    continue;

                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                float progress = (float)k / Projectile.oldPos.Length;
                Color color = Color.Orange * (0.4f * (1f - progress));

                float rotation;
                if (k + 1 >= Projectile.oldPos.Length || Projectile.oldPos[k + 1] == Vector2.Zero)
                {
                    rotation = (Projectile.position - Projectile.oldPos[k]).ToRotation() + MathHelper.PiOver2;
                }
                else
                {
                    rotation = (Projectile.oldPos[k + 1] - Projectile.oldPos[k]).ToRotation() + MathHelper.PiOver2;
                }

                float scale = Projectile.scale * (0.7f + 0.3f * progress);
                spriteBatch.Draw(texture, drawPos, null, color, rotation, drawOrigin, scale, effects, 0f);
            }

            Color outlineColor = Color.Lerp(Color.OrangeRed, Color.Yellow, 0.5f);
            outlineColor.A = 150;

            for (int i = 0; i < 3; i++)
            {
                float outlineOffset = (i + 1) * 0.6f;
                Vector2 offset = Vector2.Zero;

                switch (i)
                {
                    case 0: offset = new Vector2(-outlineOffset, 0); break;
                    case 1: offset = new Vector2(outlineOffset, 0); break;
                    case 2: offset = new Vector2(0, -outlineOffset); break;
                }

                spriteBatch.Draw(
                    texture,
                    Projectile.position - Main.screenPosition + drawOrigin + offset,
                    null,
                    outlineColor * 0.3f,
                    Projectile.rotation,
                    drawOrigin,
                    Projectile.scale,
                    effects,
                    0f
                );
            }

            spriteBatch.Draw(
                texture,
                Projectile.position - Main.screenPosition + drawOrigin,
                null,
                Color.White,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale,
                effects,
                0f
            );

            return false;
        }
    }
}