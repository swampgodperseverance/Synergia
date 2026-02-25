using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.SinlordWyrm
{
	public class FireSwirl : ModProjectile
	{
		public override void SetDefaults() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageProjectile", Projectile, true);
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 300;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
		}
		public override void AI() {
			if(Projectile.alpha > 17) Projectile.alpha -= 15;
			if(Projectile.velocity.Length() < 16f) Projectile.velocity *= 1.15f;
			Projectile.spriteDirection = Projectile.direction = (int)Projectile.ai[0];
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = (float)MathHelper.Min(15, Projectile.timeLeft) / 15f;
			float scale = Projectile.scale * Projectile.Opacity;
			for(int i = 0; i < 2; i++) Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * fade, Main.GlobalTimeWrappedHourly * MathHelper.TwoPi * 7.5f * Projectile.spriteDirection, texture.Size() * 0.5f, scale * fade, Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}
	}
}