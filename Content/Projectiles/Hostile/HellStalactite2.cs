using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Synergia.Content.Projectiles.Hostile
{
    public class HellStalactite2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 40;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 240;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            float shakeStrength = 0.9f;
            Projectile.position += new Vector2(Main.rand.NextFloat(-shakeStrength, shakeStrength), Main.rand.NextFloat(-shakeStrength, shakeStrength));

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Projectile.ai[0]++;
            float speed = Projectile.ai[0] < 60 ? 2f : 10f;
            Projectile.velocity.Y = speed;

            // Lava Dust
            if (Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Lava, 
                    0f, 2f, 100, default, 1.1f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);

            for (int i = Projectile.oldPos.Length - 1; i >= Projectile.oldPos.Length / 2; i--)
            {
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float alpha = (float)(Projectile.oldPos.Length - i) / Projectile.oldPos.Length;
                Color color = Color.OrangeRed * (1f - alpha);
                float scale = Projectile.scale * (1f - alpha * 0.5f);

                Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            }

            Vector2 center = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, center, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);

            return false;
        }
    }
}
