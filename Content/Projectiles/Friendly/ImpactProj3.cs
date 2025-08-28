using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Friendly
{
    public class CogwormProj3 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.damage = 60;
            Projectile.aiStyle = ProjAIStyleID.Boomerang;
            AIType = ProjectileID.EnchantedBoomerang;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, Projectile.height / 2);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float progress = (float)(Projectile.oldPos.Length - i) / Projectile.oldPos.Length;

                Color trailColor = Color.Lerp(Color.Red, Color.Orange, progress);
                trailColor = Color.Lerp(trailColor, Color.Yellow, progress * 0.5f);

                trailColor *= progress; 

                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);

                Main.spriteBatch.Draw(texture, drawPos, null, trailColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);

                Main.spriteBatch.Draw(texture, drawPos, null, Color.White * (progress * 0.3f), Projectile.rotation, drawOrigin, Projectile.scale * 1.2f, SpriteEffects.None, 0f);
            }

            return true; 
        }

    }
}
