using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.SinlordWyrm
{
	public class LavaStalactite : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageProjectile", Projectile, true);
			Projectile.width = 14;
			Projectile.height = 30;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 480;
			Projectile.aiStyle = -1;
			Projectile.extraUpdates = 1;
		}
		public override void AI() {
			Projectile.velocity.Y += 0.1f;
			Projectile.velocity *= 0.99f;
		}
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D glow = ModContent.Request<Texture2D>(GlowTexture).Value;
            Vector2 drawOrigin = texture.Size() * 0.5f;
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] + Projectile.Size / 2f - Main.screenPosition;
                float progress = (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;

                Color color = Color.BlueViolet * 0.2f * progress;

                float rotation;
                if (k + 1 >= Projectile.oldPos.Length)
                    rotation = (Projectile.position - Projectile.oldPos[k]).ToRotation() + MathHelper.PiOver2;
                else
                    rotation = (Projectile.oldPos[k + 1] - Projectile.oldPos[k]).ToRotation() + MathHelper.PiOver2;

                spriteBatch.Draw(texture, drawPos, null, color, rotation, drawOrigin, Projectile.scale * progress, effects, 0f);
            }

            spriteBatch.Draw(texture,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale,
                effects,
                0f);

            spriteBatch.Draw(glow,
                Projectile.Center - Main.screenPosition,
                null,
                Color.White,
                Projectile.rotation,
                glow.Size() * 0.5f,
                Projectile.scale,
                effects,
                0f);

            return false;
        }

    }
}