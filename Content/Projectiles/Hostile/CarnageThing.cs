using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;//consolaria

namespace Synergia.Content.Projectiles.Hostile
{
    public class CarnageThing : ModProjectile
      {
        private int fadeInTime = 20; 

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            int size = 20;
            Projectile.Size = new Vector2(size, size);

            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.aiStyle = 2;
            Projectile.damage = 10;
            Projectile.tileCollide = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            SpriteEffects effects = (Projectile.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            float alpha = MathHelper.Clamp(Projectile.timeLeft < fadeInTime ? 
                            (fadeInTime - Projectile.timeLeft) / (float)fadeInTime : 1f, 0f, 1f);

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 trailPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                float intensity = (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
                Color trailColor = Color.White * (0.5f * intensity * alpha);

                spriteBatch.Draw(texture, trailPos, null, trailColor, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);
            }

            Vector2 drawPos = Projectile.position - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
            spriteBatch.Draw(texture, drawPos, null, lightColor * alpha, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);

            return false;
        }


    }
}
