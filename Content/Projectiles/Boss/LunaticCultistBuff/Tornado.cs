using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.LunaticCultistBuff
{
	public class Tornado : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_657";
		public override void SetDefaults() {
			Projectile.width = 128;
			Projectile.height = 2000;
			Projectile.hostile = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 180;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
		}
		public override void AI() {
			//for(int i = 0; i < 2; i++) Main.dust[Dust.NewDust(Projectile.Center + Projectile.velocity * 100f - new Vector2(600, 1000), 600, 2000, 278, Projectile.velocity.X * -8f, 0f, 0, Color.White, 1.2f)].noGravity = true;
			if(Projectile.timeLeft < 51) Projectile.alpha += 5;
			else if(Projectile.alpha > 0) Projectile.alpha -= 15;
			Projectile.rotation += 0.15f * Projectile.direction;
			if(Projectile.rotation > MathHelper.Pi) Projectile.rotation -= MathHelper.TwoPi;
			if(Projectile.rotation < -MathHelper.Pi) Projectile.rotation += MathHelper.TwoPi;
			if(System.Math.Sign(Projectile.velocity.X) > 0 ? Main.LocalPlayer.Left.X > Projectile.Right.X :  Main.LocalPlayer.Right.X < Projectile.Left.X) return;
			Main.LocalPlayer.velocity.X += Projectile.velocity.X;
			Main.LocalPlayer.runSlowdown *= 0f;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			lightColor = new Color(200, 200, 255, 0) * Projectile.Opacity;
			for(int k = 0; k < 120; k++) Main.EntitySpriteDraw(texture, Projectile.Center + new Vector2(Vector2.UnitX.RotatedBy(Projectile.rotation + k * 0.3f).X * MathHelper.Lerp(3f, 12f, k / 120f) * Projectile.direction, MathHelper.Lerp(-1000f, 1000f, (float)k / 120f)) - Main.screenPosition, null, lightColor * MathHelper.Lerp(0.4f, 0.9f, Vector2.UnitX.RotatedBy(MathHelper.Pi * (float)k / 120f).Y), Projectile.rotation + k * 0.05f * Projectile.direction, texture.Size() * 0.5f, Projectile.scale * MathHelper.Lerp(12f, 3f, (float)k / 120f), Projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}
	}
}