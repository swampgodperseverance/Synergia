using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.SinlordWyrm
{
	public class HellMeteor3 : ModProjectile
	{
		public override void SetDefaults() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageProjectile", Projectile, true);
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 300;
			Projectile.aiStyle = -1;
		}
		public override void AI() {
			int l = Dust.NewDust(Projectile.Center, 0, 0, 6);
			Main.dust[l].noGravity = true;
			Main.dust[l].velocity = Projectile.velocity.RotatedBy(MathHelper.PiOver2) * 0.1f;
			Main.dust[l].position += Main.dust[l].velocity.SafeNormalize(Vector2.One) * Projectile.width / 3;
			l = Dust.NewDust(Projectile.Center, 0, 0, 6);
			Main.dust[l].noGravity = true;
			Main.dust[l].velocity = Projectile.velocity.RotatedBy(-MathHelper.PiOver2) * 0.1f;
			Main.dust[l].position += Main.dust[l].velocity.SafeNormalize(Vector2.One) * Projectile.width / 3;
			if(Projectile.ai[0] > 0f) if(Projectile.timeLeft < 270 && Projectile.timeLeft > 270 - Projectile.ai[1]) {
				Projectile.velocity += Vector2.Normalize(Main.player[(int)Projectile.ai[0] - 1].Center- Projectile.Center) * 0.34f;
				Projectile.velocity *= 0.96f;
			}
			else if(Projectile.velocity.Length() < 16f) Projectile.velocity *= 1.015f;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Texture2D glow = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() * 0.5f + Vector2.One * 2f, Projectile.scale, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, glow.Size() * 0.5f + Vector2.One * 2f, Projectile.scale, SpriteEffects.None, 0);
			glow = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Extra_91");
			Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, null, Color.Red with {A = 0} * 0.1f, Projectile.rotation, new Vector2(glow.Width) * 0.5f, Projectile.scale * 1.1f, SpriteEffects.None, 0);
			for(int i = 0; i < 3; i++) Main.EntitySpriteDraw(glow, Projectile.Center + Vector2.UnitY.RotatedBy(MathHelper.TwoPi * 3f / i - Projectile.rotation) * 4f - Main.screenPosition, null, Color.Orange with {A = 0} * 0.1f, Projectile.rotation, new Vector2(glow.Width) * 0.5f, Projectile.scale * 1.1f, SpriteEffects.None, 0);
			return false;
		}
	}
}