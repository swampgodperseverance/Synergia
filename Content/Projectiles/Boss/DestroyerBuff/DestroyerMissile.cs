using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace Synergia.Content.Projectiles.Boss.DestroyerBuff
{
	public class DestroyerMissile : ModProjectile
	{
		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 3;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageProjectile", Projectile, true);
			Projectile.CloneDefaults(448);
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
			Projectile.tileCollide = true;
		}
		public override void AI() {
			if(Projectile.ai[1] > 0) {
				if(Projectile.ai[1] < 30f) {
					double rotOff = (Main.player[(int)Projectile.ai[0]].Center - Projectile.Center).ToRotation() - Projectile.velocity.ToRotation();
					if(rotOff > MathHelper.Pi) rotOff -= MathHelper.Pi * 2.0;
					if(rotOff < -MathHelper.Pi) rotOff += MathHelper.Pi * 2.0;
					Projectile.velocity = Projectile.velocity.RotatedBy(rotOff * MathHelper.ToRadians(10));
				}
				else if(Projectile.ai[1] == 30f) SoundEngine.PlaySound(SoundID.Item46, Projectile.position);
				Projectile.ai[1]--;
			}
			else {
				if(Main.masterMode) {
					double rotOff = (Main.player[(int)Projectile.ai[0]].Center - Projectile.Center).ToRotation() - Projectile.velocity.ToRotation();
					if(rotOff > MathHelper.Pi) rotOff -= MathHelper.Pi * 2.0;
					if(rotOff < -MathHelper.Pi) rotOff += MathHelper.Pi * 2.0;
					Projectile.velocity = Projectile.velocity.RotatedBy(rotOff * MathHelper.ToRadians(Main.getGoodWorld ? 0.6f : 0.2f));
				}
				if(++Projectile.localAI[1] >= Projectile.oldPos.Length) Projectile.localAI[1] = 0f;
			}
			if(Projectile.Distance(Main.player[(int)Projectile.ai[0]].Center) < 32f) Projectile.Kill(); 
			if(++Projectile.frameCounter >= 3) {
				Projectile.frameCounter = 0;
				if(++Projectile.frame >= 3) Projectile.frame = 0;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + (float)MathHelper.PiOver2;
		}
		public override void OnKill(int timeLeft) {
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			Projectile.position = Projectile.Center;
			Projectile.width = (Projectile.height = 112);
			Projectile.position.X -= Projectile.width / 2;
			Projectile.position.Y -= Projectile.height / 2;
			for (int a = 0; a < 3; a++) Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 1f);
			for (int b = 0; b < 30; b++) {
				int c = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 182, 0f, 0f, 0, default(Color), 2f);
				Main.dust[c].noGravity = true;
				Dust dust = Main.dust[c];
				dust.velocity *= 3f;
				c = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 182, 0f, 0f, 100, default(Color), 1f);
				dust = Main.dust[c];
				dust.velocity *= 2f;
				Main.dust[c].noGravity = true;
			}
			int e = Gore.NewGore(Projectile.GetSource_Death(), Projectile.position + new Vector2((float)(Projectile.width * Main.rand.Next(100)) / 100f, (float)(Projectile.height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, default(Vector2), Main.rand.Next(61, 64));
			Gore gore = Main.gore[e];
			gore.velocity *= 0.3f;
			Main.gore[e].velocity.X += (float)Main.rand.Next(-10, 11) * 0.05f;
			Main.gore[e].velocity.Y += (float)Main.rand.Next(-10, 11) * 0.05f;
			Projectile.Damage();
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Extra_98");
			Color glowColor = Color.DarkRed;
			glowColor.A = 0;
			if(Projectile.ai[1] <= 0) {
				glowColor *= MathHelper.Lerp(1f, 0f, Projectile.localAI[1] / Projectile.oldPos.Length);
				texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Extra_174");
				Main.EntitySpriteDraw(texture, Projectile.oldPos[(int)Projectile.localAI[1]] + new Vector2(Projectile.width, Projectile.height) * 0.5f - Main.screenPosition, null, glowColor, Projectile.localAI[1] == 0f ? Projectile.rotation : (Projectile.oldPos[(int)Projectile.localAI[1] - 1] - Projectile.oldPos[(int)Projectile.localAI[1]]).ToRotation(), texture.Size() * 0.5f, Projectile.scale * Vector2.Lerp(new Vector2(0.08f, 0.12f), new Vector2(0.16f, 0.24f), Projectile.localAI[1] / Projectile.oldPos.Length), SpriteEffects.None, 0);
			}
			texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]), lightColor, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, SpriteEffects.None, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]), Color.White, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}