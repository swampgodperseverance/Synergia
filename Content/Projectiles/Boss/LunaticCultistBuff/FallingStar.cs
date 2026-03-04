using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.LunaticCultistBuff
{
	public class FallingStar : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_538";
		public override string GlowTexture => "Terraria/Images/Extra_91";
		public override void SetDefaults() {
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.hostile = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 180;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
		}
		public override void AI() {
			Projectile.rotation += Projectile.direction * 0.1f;
			Projectile.spriteDirection = Projectile.direction;
			Projectile.localAI[2] = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Texture2D glow = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor with {A = 0}, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, (Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally), 0);
			Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, null, Color.Cyan with {A = 0} * 0.5f, Projectile.localAI[2], new Vector2(glow.Width) * 0.5f, Projectile.scale, SpriteEffects.None, 0);
			for(int i = 0; i < 3; i++) Main.EntitySpriteDraw(glow, Projectile.Center + Vector2.UnitY.RotatedBy(MathHelper.TwoPi * i / 3f - Projectile.rotation) * 4f - Main.screenPosition, null, Color.Magenta with {A = 0} * 0.5f, Projectile.localAI[2], new Vector2(glow.Width) * 0.5f, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}