using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Synergia.Content.Projectiles.Hostile
{
    public class JadeShard2 : ModProjectile
    {
        private int accelerationDelay = 30; 
        private float accelerationAmount = 0.08f; 
        private float maxSpeed = 12f; 

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            Projectile.ai[0]++;

            if (Projectile.velocity.LengthSquared() > 0.01f)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Projectile.ai[0] == 1)
            {
                Projectile.velocity *= 0.4f;
            }

            if (Projectile.ai[0] < accelerationDelay)
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-1f, 1f)));
            }
            else
            {
                float targetSpeed = maxSpeed;
                float newSpeed = MathHelper.Lerp(Projectile.velocity.Length(), targetSpeed, accelerationAmount);
                Projectile.velocity = Vector2.Normalize(Projectile.velocity) * newSpeed;

                if (Main.rand.NextBool(2))
                {
                    Dust.NewDust(
                        Projectile.position,
                        Projectile.width,
                        Projectile.height,
                        ModContent.DustType<JadeDust>(),
                        Projectile.velocity.X * 0.3f,
                        Projectile.velocity.Y * 0.3f,
                        0,
                        default,
                        1.2f
                    );
                }
            }
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

                float rotation;
                if (i > 0 && Projectile.oldPos[i - 1] != Vector2.Zero)
                    rotation = (Projectile.oldPos[i - 1] - Projectile.oldPos[i]).ToRotation() + MathHelper.PiOver2;
                else
                    rotation = Projectile.rotation;

                // color
                Color turquoise = new Color(0, 255, 210);

                Color trailColor = Projectile.ai[0] > accelerationDelay
                    ? turquoise * fade * 1.2f 
                    : turquoise * fade * 0.7f;

                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    null,
                    trailColor * 0.8f,
                    rotation,
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
                lightColor,
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
