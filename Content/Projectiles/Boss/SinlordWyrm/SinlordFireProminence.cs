using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.SinlordWyrm
{
	public class SinlordFireProminence : ModProjectile
	{
		public override string Texture => "Synergia/Assets/Textures/Ray";
		public override void SetDefaults() {
			Projectile.width = 0;
			Projectile.height = 0;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 120;
			Projectile.alpha = 255;
		}
		public override void AI() {
			if(Projectile.alpha > 0) Projectile.alpha -= 17;
			if(Projectile.timeLeft > 80) Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[0]);
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if(Projectile.timeLeft > 20) return;
			Vector2 shootDir = Projectile.velocity.SafeNormalize(Vector2.UnitX);
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + shootDir * Projectile.ai[1], shootDir * 9f, ModContent.ProjectileType<SinlordFireBreath>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f, 1f);
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + shootDir * Projectile.ai[1], shootDir * 11f, ModContent.ProjectileType<SinlordFireBreath>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f, 1f);
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + shootDir * Projectile.ai[1], shootDir * 13f, ModContent.ProjectileType<SinlordFireBreath>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f, 1f);
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + shootDir * Projectile.ai[1], shootDir * 15f, ModContent.ProjectileType<SinlordFireBreath>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f, 1f);
			if(Projectile.timeLeft % 4 == 0) SoundEngine.PlaySound(Terraria.ID.SoundID.DD2_FlameburstTowerShot with { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, Pitch = -0.8f, Volume = 4f }, Projectile.Center);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = (float)MathHelper.Min(15, Projectile.timeLeft) / 15f;
			for(int k = 0; k < 20; k++) Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.OrangeRed with {A = 0} * fade * MathHelper.Lerp(0.1f, 0.8f, k / 40f), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height), Projectile.scale * new Vector2(0.05f, 0.3f) * k * fade * Projectile.Opacity, SpriteEffects.None, 0);
			return false;
		}
		public override bool ShouldUpdatePosition() => false;
	}
}