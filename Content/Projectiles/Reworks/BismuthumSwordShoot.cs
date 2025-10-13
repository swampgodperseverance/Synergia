using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks
{
    public class BismuthumSwordShoot : ModProjectile
    {
        private const int FadeInTime = 10; 
        private const int FadeOutTime = 15;  

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 60;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 25;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 255; 
        }

        public override void AI()
        {

            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                Vector2 mouseWorld = Main.MouseWorld;
                Projectile.position = mouseWorld;
                Projectile.velocity = new Vector2(0f, -22f); // быстрее вверх
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Projectile.timeLeft > FadeOutTime)
            {
                Projectile.alpha -= (int)(255f / FadeInTime);
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;
            }

            else
            {
                Projectile.alpha += (int)(255f / FadeOutTime);
                if (Projectile.alpha > 255)
                    Projectile.alpha = 255;
            }


            Lighting.AddLight(Projectile.Center, 0.4f, 0.4f, 0.4f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] + new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;
                float opacity = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Color color = new Color(200, 200, 200) * opacity * (1f - Projectile.alpha / 255f);
                Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            }

            Color mainColor = new Color(255, 255, 255, 255 - Projectile.alpha);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, mainColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);

            return false;
        }
    }
}
