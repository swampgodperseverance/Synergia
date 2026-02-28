
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Synergia.Content.Projectiles.Hostile
{
    public class PrimeSkull : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3; 
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;      
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = false;
            Projectile.hostile = true; 
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.alpha = 0; 
        }

        public override void AI()
        {
            if (++Projectile.frameCounter >= 6) 
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            Player target = Main.player[Player.FindClosest(Projectile.Center, Projectile.width, Projectile.height)];
            if (target.active && !target.dead)
            {
                Vector2 toTarget = target.Center - Projectile.Center;
                toTarget.Normalize();

                float acceleration = 0.1f;
                Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), toTarget, 0.05f * Projectile.timeLeft / 300f)) * (Projectile.velocity.Length() + acceleration);
            }


            Projectile.rotation = Projectile.velocity.ToRotation(); 
            Projectile.spriteDirection = Projectile.direction;

            if (Projectile.timeLeft < 40)
            {
                Projectile.alpha += 6;
                Projectile.velocity *= 0.95f; 
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = Projectile.spriteDirection < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None;
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width / 2, Projectile.height / Main.projFrames[Projectile.type] / 2);
            int textureHeight = texture.Height / Main.projFrames[Projectile.type];
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                if(i > 0) color *= 0.5f;
                Main.EntitySpriteDraw(texture, drawPos, new Rectangle(0, Projectile.frame * textureHeight, texture.Width, textureHeight), color, Projectile.rotation, origin, Projectile.scale, effects, 0);
            }
            texture = ModContent.Request<Texture2D>(GlowTexture).Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * textureHeight, texture.Width, textureHeight), Color.White, Projectile.rotation, origin, Projectile.scale, effects, 0);

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 
                    Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 150, default, 1.2f);
            }
        }
    }
}
