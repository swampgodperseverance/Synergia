using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.SinlordWyrm
{
	public class LavaBone : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Type] = 10;
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}
		public override void SetDefaults() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageProjectile", Projectile, true);
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 300;
			Projectile.aiStyle = -1;
		}
		public override void AI() {
			Projectile.rotation += Projectile.direction * 0.3f;
			Projectile.spriteDirection = Projectile.direction;
			int l = Dust.NewDust(Projectile.Center + Projectile.rotation.ToRotationVector2() * Projectile.width / 3 * 2 * Projectile.direction - Vector2.One * 3.75f, 0, 0, 6);
			Main.dust[l].noGravity = true;
			Main.dust[l].velocity *= 0f;
			l = Dust.NewDust(Projectile.Center - Projectile.rotation.ToRotationVector2() * Projectile.width / 3 * 2 * Projectile.direction - Vector2.One * 3.75f, 0, 0, 6);
			Main.dust[l].noGravity = true;
			Main.dust[l].velocity *= 0f;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Texture2D glow = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			for(int i = 1; i < Projectile.oldPos.Length; i++) Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition, null, lightColor * MathHelper.Lerp(0.5f, 0f, (float)i / (float)Projectile.oldPos.Length), Projectile.oldRot[i], texture.Size() * 0.5f, Projectile.scale, (Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally), 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, (Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally), 0);
			Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, glow.Size() * 0.5f, Projectile.scale, (Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally), 0);
			return false;
		}
	}
}