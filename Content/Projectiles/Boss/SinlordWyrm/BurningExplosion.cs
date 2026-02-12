using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.SinlordWyrm
{
	public class BurningExplosion : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_85";
		public override string GlowTexture => "Synergia/Content/Projectiles/Boss/SinlordWyrm/BurningScream";
		public override void SetStaticDefaults() => Main.projFrames[Type] = 7;
		public override void SetDefaults() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageProjectile", Projectile, true);
			Projectile.width = 90;
			Projectile.height = 90;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 9;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.scale = 1.5f;
		}
		public override void AI() {
			if(Projectile.ai[0] > 0f) {
				if(Projectile.alpha > 0) Projectile.alpha -= 15;
				Projectile.ai[0]--;
				Projectile.timeLeft = 9;
				return;
			}
			if(Projectile.timeLeft == 8) Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
			Projectile.alpha = 0;
			if(Projectile.localAI[0] == 0) {
				Projectile.localAI[0]++;
				Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			}
			else {
				for(int i = 0; i < 3; i++) {
					int l = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(15, 15), 0, 0, 6);
					Main.dust[l].noGravity = true;
					Main.dust[l].scale *= i + 1;
					Main.dust[l].velocity = Main.dust[l].position - Projectile.Center;
					Main.dust[l].position += Main.dust[l].velocity * 0.1f;
				}
				Color color = Color.White;
				if(Projectile.timeLeft < 3) color = Color.Lerp(Color.Black, Color.Orange, (float)Projectile.timeLeft / 3f);
				else  color = Color.Lerp(Color.Orange, Color.OrangeRed, (float)System.Math.Sqrt((float)(Projectile.timeLeft - 3) / 3f));
				Lighting.AddLight(Projectile.Center, color.ToVector3());
			}
			if(Projectile.ai[1] < Main.projFrames[Projectile.type]) if(++Projectile.frameCounter >= (Projectile.ai[1] > 2 ? 1 : 2)) {
				Projectile.ai[1]++;
				Projectile.frameCounter = 0;
			}
			Projectile.frame = (int)Projectile.ai[1];
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
			fallThrough = true;
			float hitboxSize = 15;
			switch(Projectile.ai[1]) {
				case 1:
					hitboxSize += 9;
				break;
				case 2:
					hitboxSize += 14;
				break;
				case 3:
					hitboxSize += 23;
				break;
				case 4:
					hitboxSize += 26;
				break;
				case 5:
					hitboxSize += 28;
				break;
				case 6:
					hitboxSize += 30;
				break;
			}
			hitboxSize *= Projectile.scale;
			return Collision.SolidCollision(Projectile.Center - new Vector2(hitboxSize), (int)(hitboxSize * 2f), (int)(hitboxSize * 2f));
		}
		public override bool CanHitPlayer(Player target) {
			float hitboxSize = 15;
			switch(Projectile.ai[1]) {
				case 1:
					hitboxSize += 9;
				break;
				case 2:
					hitboxSize += 14;
				break;
				case 3:
					hitboxSize += 23;
				break;
				case 4:
					hitboxSize += 26;
				break;
				case 5:
					hitboxSize += 28;
				break;
				case 6:
					hitboxSize += 30;
				break;
			}
			hitboxSize *= Projectile.scale;
			return Projectile.Distance(target.Center) < hitboxSize;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture;
			if(Projectile.ai[0] > 0f) {
				texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
				Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.DarkOrange with {A = 0} * 0.1f * Projectile.Opacity, Main.rand.NextFloat(MathHelper.TwoPi), texture.Size() / 2, 0.6f * Projectile.scale, SpriteEffects.None, 0);
				return false;
			}
			if(Projectile.timeLeft < 3) lightColor = Color.Lerp(Color.Black, Color.Orange, (float)Projectile.timeLeft / 3f);
			else lightColor = Color.Lerp(Color.Orange, Color.OrangeRed, (float)System.Math.Sqrt((float)(Projectile.timeLeft - 3) / 6f));
			texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]), lightColor, 0f, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, SpriteEffects.None, 0);
			lightColor *= 0.5f;
			lightColor.A = 0;
			texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Main.rand.NextFloat(MathHelper.TwoPi), texture.Size() / 2, MathHelper.Lerp(0.9f * Projectile.scale, 0f, (float)Projectile.timeLeft / 9f), SpriteEffects.None, 0);
			return false;
		}
		public override bool? CanDamage() => Projectile.ai[0] > 0f ? false : null;
	}
}