using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace Synergia.Content.Projectiles.Boss.TwinsBuff
{
	public class Sclerocket : ModProjectile
	{
		public override void SetStaticDefaults() {
			if(ModLoader.TryGetMod("Redemption", out Mod mor)) {
				mor.Call("addElementProj", 7, Projectile.type);
				mor.Call("addElementProj", 15, Projectile.type);
			}
			Main.projFrames[Projectile.type] = 3;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageProjectile", Projectile, true);
			Projectile.CloneDefaults(448);
			Projectile.scale = 1.2f;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
			Projectile.tileCollide = false;
		}
		public override void AI() {
			if(++Projectile.localAI[1] >= Projectile.oldPos.Length) Projectile.localAI[1] = 0f;
			if(++Projectile.frameCounter >= 3) {
				Projectile.frameCounter = 0;
				if(++Projectile.frame >= 3) Projectile.frame = 0;
			}
			double rotOff = (new Vector2(Projectile.ai[0], Projectile.ai[1]) - Projectile.Center).ToRotation() - Projectile.velocity.ToRotation();
			if(rotOff > MathHelper.Pi) rotOff -= MathHelper.Pi * 2.0;
			if(rotOff < -MathHelper.Pi) rotOff += MathHelper.Pi * 2.0;
			Projectile.velocity += Vector2.Normalize(new Vector2(Projectile.ai[0], Projectile.ai[1]) - Projectile.Center) * 0.2f;
			Projectile.velocity = Projectile.velocity.RotatedBy(rotOff * MathHelper.ToRadians(10)) * 0.98f;
			Vector2 targetPos = new Vector2(Projectile.ai[0], Projectile.ai[1]);
			if(Projectile.Distance(targetPos) < 16f) Projectile.Kill();
			else if(Projectile.ai[2] >= 0f) {
				Player player = Main.player[(int)Projectile.ai[2]];
				Vector2 moveTo = Vector2.Normalize(player.Center - targetPos);
				Projectile.ai[0] += moveTo.X;
				Projectile.ai[1] += moveTo.Y;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_2");
			Color glowColor = Color.DarkGreen;
			glowColor.A = 0;
			for(int k = 0; k < Projectile.oldPos.Length - 1; k++) if(!Projectile.oldPos[k].HasNaNs() && !Projectile.oldPos[k + 1].HasNaNs() && Projectile.oldPos[k] != Vector2.Zero && Projectile.oldPos[k + 1] != Vector2.Zero) if(k == 0) Main.EntitySpriteDraw(texture, Vector2.Lerp(Projectile.oldPos[k], Projectile.oldPos[k + 1], 0.5f) + new Vector2(Projectile.width, Projectile.height) * 0.5f - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height / 2), glowColor * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (Projectile.oldPos[k] - Projectile.oldPos[k + 1]).ToRotation() + MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height / 4), new Vector2(Projectile.scale * 0.4f, Vector2.Distance(Projectile.oldPos[k], Projectile.oldPos[k + 1]) / texture.Height * 4), SpriteEffects.None, 0);
			else if(k < Projectile.oldPos.Length - 2) Main.EntitySpriteDraw(texture, Vector2.Lerp(Projectile.oldPos[k], Projectile.oldPos[k + 1], 0.5f) + new Vector2(Projectile.width, Projectile.height) * 0.5f - Main.screenPosition, new Rectangle(0, texture.Height / 2 - 1, texture.Width, 1), glowColor * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (Projectile.oldPos[k] - Projectile.oldPos[k + 1]).ToRotation() + MathHelper.PiOver2, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale * 0.4f, Vector2.Distance(Projectile.oldPos[k], Projectile.oldPos[k + 1])), SpriteEffects.None, 0);
			else Main.EntitySpriteDraw(texture, Vector2.Lerp(Projectile.oldPos[k], Projectile.oldPos[k + 1], 0.5f) + new Vector2(Projectile.width, Projectile.height) * 0.5f - Main.screenPosition, new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2), glowColor * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (Projectile.oldPos[k] - Projectile.oldPos[k + 1]).ToRotation() + MathHelper.PiOver2, new Vector2(texture.Width / 2, 0), Projectile.scale * 0.4f, SpriteEffects.None, 0);
			glowColor = Color.Lime * 0.75f;
			glowColor.A = 0;
			for(int k = 0; k < Projectile.oldPos.Length - 1; k++) if(!Projectile.oldPos[k].HasNaNs() && !Projectile.oldPos[k + 1].HasNaNs() && Projectile.oldPos[k] != Vector2.Zero && Projectile.oldPos[k + 1] != Vector2.Zero) if(k == 0) Main.EntitySpriteDraw(texture, Vector2.Lerp(Projectile.oldPos[k], Projectile.oldPos[k + 1], 0.5f) + new Vector2(Projectile.width, Projectile.height) * 0.5f - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height / 2), glowColor * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (Projectile.oldPos[k] - Projectile.oldPos[k + 1]).ToRotation() + MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height / 4), new Vector2(Projectile.scale * 0.1f, Vector2.Distance(Projectile.oldPos[k], Projectile.oldPos[k + 1]) / texture.Height * 4), SpriteEffects.None, 0);
			else if(k < Projectile.oldPos.Length - 2) Main.EntitySpriteDraw(texture, Vector2.Lerp(Projectile.oldPos[k], Projectile.oldPos[k + 1], 0.5f) + new Vector2(Projectile.width, Projectile.height) * 0.5f - Main.screenPosition, new Rectangle(0, texture.Height / 2 - 1, texture.Width, 1), glowColor * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (Projectile.oldPos[k] - Projectile.oldPos[k + 1]).ToRotation() + MathHelper.PiOver2, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale * 0.1f, Vector2.Distance(Projectile.oldPos[k], Projectile.oldPos[k + 1])), SpriteEffects.None, 0);
			else Main.EntitySpriteDraw(texture, Vector2.Lerp(Projectile.oldPos[k], Projectile.oldPos[k + 1], 0.5f) + new Vector2(Projectile.width, Projectile.height) * 0.5f - Main.screenPosition, new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2), glowColor * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (Projectile.oldPos[k] - Projectile.oldPos[k + 1]).ToRotation() + MathHelper.PiOver2, new Vector2(texture.Width / 2, 0), Projectile.scale * 0.1f, SpriteEffects.None, 0);
			glowColor = Color.DarkGreen;
			glowColor.A = 0;
			glowColor *= MathHelper.Lerp(1f, 0f, Projectile.localAI[1] / Projectile.oldPos.Length);
			texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Extra_174");
			Main.EntitySpriteDraw(texture, Projectile.oldPos[(int)Projectile.localAI[1]] + new Vector2(Projectile.width, Projectile.height) * 0.5f - Main.screenPosition, null, glowColor, Projectile.localAI[1] == 0f ? Projectile.rotation : (Projectile.oldPos[(int)Projectile.localAI[1] - 1] - Projectile.oldPos[(int)Projectile.localAI[1]]).ToRotation(), texture.Size() * 0.5f, Projectile.scale * Vector2.Lerp(new Vector2(0.08f, 0.12f), new Vector2(0.16f, 0.24f), Projectile.localAI[1] / Projectile.oldPos.Length), SpriteEffects.None, 0);
			glowColor = Color.Lime * 0.75f;
			glowColor.A = 0;
			glowColor *= MathHelper.Lerp(1f, 0f, Projectile.localAI[1] / Projectile.oldPos.Length);
			Main.EntitySpriteDraw(texture, Projectile.oldPos[(int)Projectile.localAI[1]] + new Vector2(Projectile.width, Projectile.height) * 0.5f - Main.screenPosition, null, glowColor, Projectile.localAI[1] == 0f ? Projectile.rotation : (Projectile.oldPos[(int)Projectile.localAI[1] - 1] - Projectile.oldPos[(int)Projectile.localAI[1]]).ToRotation(), texture.Size() * 0.5f, Projectile.scale * Vector2.Lerp(new Vector2(0.08f, 0.12f), new Vector2(0.16f, 0.24f), Projectile.localAI[1] / Projectile.oldPos.Length), SpriteEffects.None, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]), lightColor, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, SpriteEffects.None, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]), Color.White, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, SpriteEffects.None, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>(Texture + "_Telegraph");
			Main.EntitySpriteDraw(texture, new Vector2(Projectile.ai[0], Projectile.ai[1]) - Main.screenPosition, null, Color.Lerp(Color.DarkGreen, Color.Lime, Vector2.UnitX.RotatedBy(Projectile.localAI[1] / Projectile.oldPos.Length * MathHelper.Pi).Y), 0f, texture.Size() * 0.5f, 0.8f, SpriteEffects.None, 0);
			return false;
		}
		public override void OnKill(int timeLeft) {
			SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
			Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<EyeFire>(), Projectile.damage, 0f, Projectile.owner);
			Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SpazmatismPulse>(), 0, 0f, Projectile.owner, 1.2f);
		}
	}
}