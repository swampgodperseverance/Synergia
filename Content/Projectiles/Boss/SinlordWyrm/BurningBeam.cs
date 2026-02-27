using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;

namespace Synergia.Content.Projectiles.Boss.SinlordWyrm
{
	public class BurningBeam : ModProjectile
	{
		public override string Texture => "Synergia/Assets/Textures/Ray";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.DismountsPlayersOnHit[Type] = true;
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
		}
		public override void SetDefaults() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageProjectile", Projectile, true);
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 2;
			Projectile.netImportant = true;
		}
		public override void AI() {
			Projectile.timeLeft = (int)Projectile.ai[0];
			Projectile.ai[0]--;
			if(Projectile.ai[1] > 0f) Projectile.velocity += Vector2.Normalize(Main.player[(int)Projectile.ai[1] - 1].Center - Projectile.Center) * 0.1f;
			Projectile.velocity *= 0.99f;
			float speed = Projectile.velocity.Length();
			if(speed < 1f) Projectile.velocity /= speed;
			Projectile.rotation = Projectile.velocity.ToRotation();
			Main.instance.CameraModifiers.Add(new Terraria.Graphics.CameraModifiers.PunchCameraModifier(Projectile.Center + Vector2.UnitX.RotatedBy(Projectile.rotation), Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi), 4f, 6, 20, 2400f, "Burning Beam"));
			if(Projectile.localAI[0] < 5f) Projectile.localAI[0]++;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			float point = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Vector2.UnitX.RotatedBy(Projectile.rotation) * 2400f, 24 * Projectile.scale, ref point);
		}
		public override bool PreDraw(ref Color lightColor) {
			float fade = Projectile.localAI[0] / 5f * MathHelper.Min(Projectile.timeLeft, 15) / 15f;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			for(int i = 0; i < 2; i++) {
				lightColor = i > 0 ? Color.White : new Color(171, 39, 0, 0);
				lightColor.A = 0;
				if(i > 0) fade /= 2f;
				texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
				Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height - 1), lightColor * fade * (i + 1), Projectile.rotation - MathHelper.PiOver2, new Vector2(texture.Width * 0.5f, texture.Height - 1), new Vector2(Projectile.scale * 5f, 0.25f) * fade * (2 - i), SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height - 1, texture.Width, 1), lightColor * fade * (i + 1), Projectile.rotation - MathHelper.PiOver2, new Vector2(texture.Width * 0.5f, 0f), new Vector2(Projectile.scale * 5f * fade * (2 - i), 2400f), SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, Projectile.Center + Projectile.rotation.ToRotationVector2() * 2400f - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height - 1), lightColor * fade * (i + 1), Projectile.rotation + MathHelper.PiOver2, new Vector2(texture.Width * 0.5f, texture.Height - 1), new Vector2(Projectile.scale * 5f, 0.75f) * fade * (2 - i), SpriteEffects.None, 0);
			}
			if(!Filters.Scene["HeatDistortion"].IsActive() && Main.UseHeatDistortion && Projectile.alpha == 0) Filters.Scene.Activate("HeatDistortion", default(Vector2));
			if(Filters.Scene["HeatDistortion"].IsActive()) {
				Filters.Scene["HeatDistortion"].GetShader().UseIntensity(6f);
				Filters.Scene["HeatDistortion"].IsHidden = false;
			}
			return false;
		}
		public override bool ShouldUpdatePosition() => false;
	}
}