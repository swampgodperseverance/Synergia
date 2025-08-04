using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vanilla.Content.Projectiles.Hostile
{
    public class IfritScythe : ModProjectile
    {
        private int timer = 0;

        public override void SetStaticDefaults()
        {
            // Consolaria
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10; 
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0; 
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 0;
        }

        public override void AI()
        {
            timer++;

            // Simple lava dust trail
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Lava);

            if (timer == 90)
            {
                Projectile.velocity *= 2.5f;
            }

            Projectile.rotation += 0.3f * (Projectile.velocity.X >= 0 ? 1 : -1);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float alpha = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;

                Color color = new Color(255, 100, 0, 100) * alpha;
                float scale = Projectile.scale * (0.9f + 0.1f * alpha);

                Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            }

            Vector2 currentDrawPos = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, currentDrawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);

            return false; 
        }
    }
}
