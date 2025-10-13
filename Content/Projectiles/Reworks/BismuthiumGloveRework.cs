using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks
{
    public class BismuthiumGloveRework : ModProjectile
    {
        private float speedTimer = 0f;

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 52;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 5;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.timeLeft = 255;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;   
        }

        public override void AI()
        {
            Projectile.rotation += 0.2f;

            Projectile.alpha = (int)MathHelper.Clamp(Projectile.alpha + 1.5f, 0, 255);

            speedTimer += 0.03f; 
            float speedFactor = MathHelper.Clamp(speedTimer, 0f, 2.5f);

            Projectile.velocity *= (1f + 0.01f * speedFactor);

            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.MagicMirror);
                Main.dust[dust].velocity *= 0.3f;
                Main.dust[dust].scale = 0.8f;
                Main.dust[dust].noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 trailPos = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                float trailProgress = (float)(Projectile.oldPos.Length - i) / Projectile.oldPos.Length;
                Color trailColor = new Color(180, 200, 255, 100) * trailProgress * 0.8f;

                Main.EntitySpriteDraw(texture, trailPos, null, trailColor, Projectile.rotation, origin, Projectile.scale * (0.9f + 0.1f * trailProgress), SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }
}
