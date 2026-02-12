using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.SinlordWyrm
{
	public class SinlordFireBreath : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_85";
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 7;
			ProjectileID.Sets.TrailCacheLength[Type] = 30;
			ProjectileID.Sets.TrailingMode[Type] = 0;
		}
		public override void SetDefaults() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageProjectile", Projectile, true);
			Projectile.width = 90;
			Projectile.height = 90;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.aiStyle = -1;
			Projectile.extraUpdates = 7;
			Projectile.timeLeft = 90;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
		}
		public override void AI() {
			if(Projectile.localAI[0] == 0) {
				Projectile.localAI[0]++;
				Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			}
			else {
				Color color = Color.White;
				if(Projectile.timeLeft < 30) color = Color.Lerp(Color.Black, Color.Orange, (float)Projectile.timeLeft / 30f);
				else  color = Color.Lerp(Color.Orange, Color.OrangeRed, (float)System.Math.Sqrt((float)(Projectile.timeLeft - 30) / 60f));
				Lighting.AddLight(Projectile.Center, color.ToVector3());
			}
			if(Projectile.ai[1] < Main.projFrames[Projectile.type]) if(++Projectile.frameCounter >= (Projectile.ai[1] > 3 ? 10 : 30)) {
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
			if(Projectile.timeLeft < 30) lightColor = Color.Lerp(Color.Black, Color.Orange, (float)Projectile.timeLeft / 30f);
			else lightColor = Color.Lerp(Color.Orange, Color.OrangeRed, (float)System.Math.Sqrt((float)(Projectile.timeLeft - 30) / 60f));
			lightColor.A = 25;
			lightColor *= 0.4f;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			for(int k = 0; k < Projectile.oldPos.Length; k++) Main.EntitySpriteDraw(texture, Projectile.oldPos[k] + Projectile.Size * 0.5f - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * (int)MathHelper.Max(Projectile.frame - k / Main.projFrames[Projectile.type] / 2, 0), texture.Width, texture.Height / Main.projFrames[Projectile.type]), lightColor, -MathHelper.TwoPi * Main.GlobalTimeWrappedHourly + Projectile.rotation + k * MathHelper.PiOver4 / 3f, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) => false;
	}
}