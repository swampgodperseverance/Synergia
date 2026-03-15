using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.Graphics.CameraModifiers;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace Synergia.Content.Projectiles.Boss.JadeEmperorRework
{
	public class JadeLightning : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 1200;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 7, base.Projectile.type);
		}
		public override void SetDefaults() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageProjectile", Projectile, true);
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = -1;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = true;
			Projectile.MaxUpdates = Projectile.timeLeft = 300;
		}
		public override void AI() {
			if(Projectile.localAI[0] == 0f && Projectile.localAI[1] == 0f) {
				Projectile.localAI[0] = Projectile.Center.X;
				Projectile.localAI[1] = Projectile.Center.Y;
				if(Projectile.velocity == Vector2.Zero) Projectile.velocity = Vector2.UnitY;
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 8f;
				Main.instance.CameraModifiers.Add(new PunchCameraModifier(Projectile.Center, Vector2.Normalize(Projectile.velocity), 6f, 8, 20, 1200f, "Superbolt"));
				SoundEngine.PlaySound(SoundID.Item94, Projectile.Center);
			}
			if(Projectile.ai[0] > 0f || (Projectile.timeLeft <= 2 && Projectile.ai[0] == 0f)) {
				Projectile.extraUpdates = 0;
				Projectile.timeLeft = 2;
				Projectile.velocity = Vector2.Zero;
				if(Projectile.ai[0] == 0f) for(int i = 0; i < 10; i++) {
					Vector2 dustVelocity = Main.rand.NextVector2Circular(Projectile.width, Projectile.height);
					dustVelocity.Y -= Projectile.height;
					dustVelocity.Y *= 0.5f;
					Dust.NewDustPerfect(Projectile.Center + dustVelocity, 278, dustVelocity, 128, new(132, 250, 168), Projectile.scale * 1.1f);
				}
				if(Projectile.ai[0] < 20f * Projectile.MaxUpdates) Projectile.ai[0]++;
				else Projectile.active = false;
				if(Projectile.ai[0] == 1f) Main.instance.CameraModifiers.Add(new PunchCameraModifier(Projectile.Center, Vector2.Normalize(new Vector2(Projectile.localAI[0], Projectile.localAI[1]) - Projectile.Center), 6f, 8, 20, 1200f, "Jade Lightning Strike"));
			}
		}
		private List<Vector2> arcPoints;
		public override bool PreDraw(ref Color lightColor) {
			if(Projectile.ai[0] <= 0f) return false;
			Vector2 startPos = new Vector2(Projectile.localAI[0], Projectile.localAI[1]);
			Vector2 endPos = Projectile.Center;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float scale = Vector2.UnitX.RotatedBy(Projectile.ai[0] / (20f * Projectile.MaxUpdates) * MathHelper.Pi).Y * Projectile.scale * 0.65f;
			int arcCount = (int)(Vector2.Distance(startPos, endPos) / 32f);
			if(arcPoints == null) {
				arcPoints = new();
				for(int i = 1; i < arcCount; i++) arcPoints.Add(Vector2.SmoothStep(startPos, endPos, (float)i / (float)arcCount) + (i > 1 ? 1f : 0f) * Main.rand.NextVector2Circular(texture.Width, texture.Height));
			}
			lightColor = Color.White;
			for(int i = 0; i < arcPoints.Count; i++) {
				if(i > 0) startPos = arcPoints[i - 1];
				Main.EntitySpriteDraw(texture, startPos - Main.screenPosition, null, lightColor, Main.rand.NextFloat(MathHelper.TwoPi), texture.Size() / 2f, scale, SpriteEffects.None, 0);
			}
			Main.EntitySpriteDraw(texture, endPos - Main.screenPosition, null, lightColor, Main.rand.NextFloat(MathHelper.TwoPi), texture.Size() / 2f, scale, SpriteEffects.None, 0);
			startPos = new Vector2(Projectile.localAI[0], Projectile.localAI[1]);
			for(int i = 0; i < arcPoints.Count; i++) {
				if(i > 0) startPos = arcPoints[i - 1];
				if(i < arcPoints.Count - 1) endPos = arcPoints[i];
				else endPos = Projectile.Center;
				Main.EntitySpriteDraw(texture, startPos - Main.screenPosition, new Rectangle(0, texture.Height / 2 - 1, texture.Width, 1), lightColor, (startPos - endPos).ToRotation() + MathHelper.PiOver2, new Vector2(texture.Width / 2f, 0f), new Vector2(scale, Vector2.Distance(startPos, endPos)), SpriteEffects.None, 0);
			}
			startPos = new Vector2(Projectile.localAI[0], Projectile.localAI[1]);
			texture = (Texture2D)ModContent.Request<Texture2D>("ValhallaMod/Projectiles/Enemy/JadeJavelin");
			for(int i = 0; i < 3; i++) Main.EntitySpriteDraw(texture, endPos - Main.screenPosition, null, Color.White * (1f - Projectile.ai[0] / (20f * Projectile.MaxUpdates)), (endPos - startPos).ToRotation() + MathHelper.PiOver2, new Vector2(texture.Width / 2f, 4f), Projectile.scale, SpriteEffects.None, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Projectile_238");
			if(++Projectile.frameCounter > Main.projFrames[238] * 3 - 2) Projectile.frameCounter = 0;
			Projectile.frame = Projectile.frameCounter / 3;
			Main.EntitySpriteDraw(texture, startPos - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[238] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[238]), lightColor * MathHelper.Min(scale * 3f, 1f), 0f, new Vector2(texture.Width, texture.Height / Main.projFrames[238]) / 2f, Projectile.scale * 1.25f, SpriteEffects.None, 0);
			return false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {
			Projectile.extraUpdates = 0;
			Projectile.tileCollide = false;
			Projectile.velocity = Vector2.Zero;
			Projectile.position += oldVelocity;
			return false;
		}
	}
}