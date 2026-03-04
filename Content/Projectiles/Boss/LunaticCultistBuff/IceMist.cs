using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.LunaticCultistBuff
{
	public class IceMist : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_464";
		public override string GlowTexture => "Terraria/Images/Extra_35";
		public override void SetDefaults() {
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.hostile = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.light = 0.05f;
		}
		public override void AI() {
			if(Projectile.ai[0] == 0f) {
				if(Projectile.ai[1] > 0f) {
					Projectile.rotation += 0.3f * Projectile.direction;
					Projectile.velocity *= 0.97f;
					if(--Projectile.ai[1] == 0f) Projectile.velocity = Projectile.ai[2].ToRotationVector2() * 16f;
				}
				else Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			}
			if(Projectile.alpha > 0) Projectile.alpha -= 17;
			else {
				int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 92, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 50, default(Color), Projectile.scale * 1.2f);
				Main.dust[d].noGravity = true;
				Main.dust[d].velocity *= 0.1f;
			}
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture;
			if(Projectile.ai[0] == 0f) {
				texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
				Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / 3 * (Projectile.whoAmI % 3), texture.Width, texture.Height / 3), lightColor * Projectile.Opacity, Projectile.rotation, new Vector2(texture.Width, texture.Height / 3) / 2, Projectile.scale, SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / 3 * (Projectile.whoAmI % 3), texture.Width, texture.Height / 3), Color.White * 0.1f * Projectile.Opacity, Projectile.rotation, new Vector2(texture.Width, texture.Height / 3) / 2, Projectile.scale, SpriteEffects.None, 0);
			}
			else {
				texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
				Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White * 0.1f, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
			}
			return false;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox) {
			if(Projectile.ai[0] == 0f) return;
			float size = 40;
			hitbox = new Rectangle((int)(Projectile.Center.X - size), (int)(Projectile.Center.Y - size), (int)(size * 2f), (int)(size * 2f));
		}
	}
}