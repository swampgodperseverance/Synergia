using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.GolemBuff
{
	public class GolemFireCharge : ModProjectile
	{
		public override string Texture => "Terraria/Images/Extra_98";
		public override string GlowTexture => "Terraria/Images/Extra_174";
		public override void SetDefaults() {
			Projectile.width = 1;
			Projectile.height = 1;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 20;
			Projectile.aiStyle = -1;
		}
		public override void AI() {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.velocity *= 0.9f;
			Projectile.Center = Main.npc[(int)Projectile.ai[0]].Center;
			if(ShouldUpdatePosition()) Projectile.position += Projectile.velocity * 2f;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			lightColor = Color.OrangeRed * (float)System.Math.Sin(Projectile.timeLeft * 0.05f * MathHelper.Pi);
			lightColor.A = 0;
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale * new Vector2(0.065f, 0.135f) * 0.25f * Projectile.velocity.Length(), SpriteEffects.None, 0);
			return false;
		}
		public override bool ShouldUpdatePosition() => Projectile.ai[1] == 0f;
	}
}