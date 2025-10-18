using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System;

namespace Synergia.Content.Projectiles.Friendly
{
    public class JadeShardFriendly : ModProjectile
    {
        //private int accelerationDelay = 30;
        private float accelerationAmount = 0.08f;
        private float maxSpeed = 12f;
        private int appearTime = 20;
        private bool startedFalling = false;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 180;
            Projectile.penetrate = 1;
        }

        public override void AI()
        {
            Projectile.ai[0]++;

            if (Projectile.ai[0] <= appearTime)
            {
                Projectile.scale = MathHelper.Lerp(0f, 1f, Projectile.ai[0] / (float)appearTime);
                Projectile.alpha = (int)MathHelper.Lerp(255, 0, Projectile.ai[0] / (float)appearTime);
                Projectile.velocity *= 0.95f;
                return;
            }

            if (!startedFalling && Projectile.ai[0] >= appearTime)
            {
                startedFalling = true;
                Projectile.tileCollide = true;
                Projectile.velocity = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(8f, 12f));
            }

            if (Projectile.velocity.LengthSquared() > 0.01f)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (startedFalling)
            {
                Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, maxSpeed, accelerationAmount);

                if (Main.rand.NextBool(2))
                {
                    Dust.NewDust(
                        Projectile.position,
                        Projectile.width,
                        Projectile.height,
                        ModContent.DustType<JadeDust>(),
                        Projectile.velocity.X * 0.2f,
                        Projectile.velocity.Y * 0.2f,
                        0,
                        default,
                        1.3f
                    );
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explode();
            return true;
        }

        private void Explode()
        {
            for (int i = 0; i < 30; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<JadeDust>());
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(1.4f, 2.2f);
                d.velocity = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(3f, 8f);
            }

            SoundEngine.PlaySound(SoundID.Item14 with { Volume = 0.9f, Pitch = 0.3f }, Projectile.Center);
            Projectile.Kill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    continue;

                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Color trailColor = new Color(0, 255, 210) * fade * 0.8f;

                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    null,
                    trailColor,
                    Projectile.rotation,
                    origin,
                    Projectile.scale,
                    SpriteEffects.None,
                    0
                );
            }

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor * 0.9f,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
}
