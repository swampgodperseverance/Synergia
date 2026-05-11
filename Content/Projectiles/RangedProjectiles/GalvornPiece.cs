using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.RangedProjectiles
{
    public class GalvornPiece : ModProjectile
    {
        private int fadeOutDuration = 20;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 6;
            ProjectileID.Sets.TrailingMode[Type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 75;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.alpha = 0;
            Projectile.light = 0.3f;
        }

        public override void AI()
        {
            Projectile.rotation += 0.3f;
            Lighting.AddLight(Projectile.Center, new Color(80, 80, 100).ToVector3() * 0.5f);
            Projectile.velocity.Y += 0.02f;

            if (Projectile.timeLeft <= fadeOutDuration)
            {
                float progress = 1f - (float)Projectile.timeLeft / fadeOutDuration;
                Projectile.alpha = (int)(255 * progress);
                Projectile.light = MathHelper.Lerp(0.3f, 0f, progress);
            }
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Stone,
                    Projectile.velocity.X * 0.5f,
                    Projectile.velocity.Y * 0.5f,
                    100,
                    default,
                    1.2f
                );
                dust.noGravity = true;
                dust.velocity *= 0.8f;
                dust.velocity += new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            }

            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Smoke,
                    0, 0,
                    100,
                    default,
                    0.8f
                );
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            SpriteEffects effects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            for (int i = 1; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) break;

                float trailProgress = 1f - (float)i / Projectile.oldPos.Length;
                Color trailColor = new Color(60, 60, 75, 100) * trailProgress * (1f - (float)Projectile.alpha / 255f);
                Vector2 trailPosition = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;

                Main.EntitySpriteDraw(
                    texture,
                    trailPosition,
                    null,
                    trailColor,
                    Projectile.oldRot[i],
                    origin,
                    Projectile.scale * (0.7f + trailProgress * 0.3f),
                    effects
                );
            }

            for (int i = 0; i < 8; i++)
            {
                Vector2 offset = new Vector2(1.5f, 1.5f).RotatedBy(MathHelper.TwoPi / 8f * i);
                Main.EntitySpriteDraw(
                    texture,
                    Projectile.Center + offset - Main.screenPosition,
                    null,
                    new Color(40, 40, 50, Projectile.alpha / 2),
                    Projectile.rotation,
                    origin,
                    Projectile.scale,
                    effects
                );
            }

            Color drawColor = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                drawColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                effects
            );

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    target.position,
                    target.width,
                    target.height,
                    DustID.Stone,
                    0, 0,
                    50,
                    default,
                    0.8f
                );
                dust.velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
                dust.noGravity = true;
            }
        }
    }
}