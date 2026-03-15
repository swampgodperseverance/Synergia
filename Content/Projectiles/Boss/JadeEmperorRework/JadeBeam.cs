using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.JadeEmperorRework
{
	public class JadeBeam : ModProjectile
	{
		public override string Texture => "Synergia/Assets/Textures/Ray";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.DismountsPlayersOnHit[Type] = true;
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
		}
		public override void SetDefaults() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageProjectile", Projectile, true);
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 40;
			Projectile.netImportant = true;
		}
		public override void AI() {
			if(Projectile.ai[0] > 0f) Projectile.Center = Main.npc[(int)Projectile.ai[0] - 1].Center + Vector2.UnitY * (Main.npc[(int)Projectile.ai[0] - 1].ModNPC?.DrawOffsetY ?? 0f) * 0.5f;
			Projectile.rotation = Projectile.velocity.ToRotation();
			if(Projectile.timeLeft <= 10 && Projectile.localAI[0] < 5f) Projectile.localAI[0]++;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			if(Projectile.timeLeft > 10) return false;
			float point = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Vector2.UnitX.RotatedBy(Projectile.rotation) * 2400f, 8 * Projectile.scale, ref point);
		}
		public override bool PreDraw(ref Color lightColor) {
			float fade = Projectile.localAI[0] / 5f * MathHelper.Min(Projectile.timeLeft, 5) / 5f;
			if(Projectile.timeLeft > 10) fade = (float)System.Math.Sin(MathHelper.Pi * (Projectile.timeLeft - 10) / 30f);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			for(int i = 0; i < (Projectile.timeLeft < 10 ? 2 : 1); i++) {
				lightColor = i > 0 ? Color.White : Color.Turquoise;
				if(Projectile.timeLeft >= 10) lightColor *= 0.2f;
				lightColor.A = 0;
				if(i > 0) fade /= 2f;
				texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
				Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height - 1), lightColor * fade * (i + 1), Projectile.rotation - MathHelper.PiOver2, new Vector2(texture.Width * 0.5f, texture.Height - 1), new Vector2(Projectile.scale, 0.25f) * fade * (2 - i), SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height - 1, texture.Width, 1), lightColor * fade * (i + 1), Projectile.rotation - MathHelper.PiOver2, new Vector2(texture.Width * 0.5f, 0f), new Vector2(Projectile.scale * fade * (2 - i), 2400f), SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, Projectile.Center + Projectile.rotation.ToRotationVector2() * 2400f - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height - 1), lightColor * fade * (i + 1), Projectile.rotation + MathHelper.PiOver2, new Vector2(texture.Width * 0.5f, texture.Height - 1), new Vector2(Projectile.scale, 0.75f) * fade * (2 - i), SpriteEffects.None, 0);
			}
			return false;
		}
		public override bool ShouldUpdatePosition() => false;
	}
}