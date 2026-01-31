using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace Synergia.Content.Projectiles.Hostile
{
    public class BigBobLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 42;

            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 160;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.alpha = 0;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Blood,
                    -Projectile.velocity.X * 0.2f,
                    -Projectile.velocity.Y * 0.2f,
                    100,
                    default,
                    1.4f
                );
                dust.noGravity = true;
            }

            if (Projectile.timeLeft <= 30)
            {
                Projectile.velocity *= 0.94f;
                Projectile.alpha += 8;
            }
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.position);

            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Blood,
                    Main.rand.NextFloat(-3f, 3f),
                    Main.rand.NextFloat(-3f, 3f),
                    80,
                    default,
                    1.8f
                );
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            Rectangle frame = new Rectangle(
                0,
                frameHeight * Projectile.frame,
                texture.Width,
                frameHeight
            );

            Vector2 origin = frame.Size() / 2f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;

                Color color = new Color(180, 20, 20, 100) * fade;

                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    frame,
                    color,
                    Projectile.rotation,
                    origin,
                    Projectile.scale * (1.1f + fade * 0.2f),
                    SpriteEffects.None,
                    0
                );
            }

            return true;
        }
    }
}
