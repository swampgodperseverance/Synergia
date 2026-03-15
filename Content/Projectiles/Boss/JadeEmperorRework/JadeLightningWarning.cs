using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.JadeEmperorRework
{
	public class JadeLightningWarning : ModProjectile
	{
		public override string Texture => "Terraria/Images/Extra_98";
		public override void SetStaticDefaults() => ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
		public override void SetDefaults() {
			Projectile.width = 1;
			Projectile.height = 1;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 30;
			Projectile.alpha = 255;
		}
		public override void AI() {
			if(Projectile.ai[1] > 0) Projectile.timeLeft = (int)(Projectile.ai[1]--) + 30;
			if(Projectile.timeLeft > 30) return;
			Projectile.rotation = Projectile.velocity.ToRotation();
			if(Projectile.timeLeft < 15) Projectile.alpha += 17;
			else if(Projectile.alpha > 0) Projectile.alpha -= 17;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(texture.Width / 2, texture.Height / 2, texture.Width / 2, 1), Color.Turquoise with {A = 0} * Projectile.Opacity, Projectile.rotation - MathHelper.PiOver2, new Vector2(texture.Width / 2, 0f), new Vector2(0.4f, 2400f), SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / 2, texture.Width / 2, 1), Color.Turquoise with {A = 0} * Projectile.Opacity, Projectile.rotation - MathHelper.PiOver2, Vector2.Zero, new Vector2(0.4f, 2400f), SpriteEffects.None, 0);
			return false;
		}
		public override void OnKill(int timeLeft) {
			if(Main.netMode != 1) Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + Vector2.Normalize(Projectile.velocity) * Projectile.ai[0], Projectile.velocity, ModContent.ProjectileType<JadeLightning>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
		}
		public override bool ShouldUpdatePosition() => false;
	}
}