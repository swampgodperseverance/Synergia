using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace Synergia.Content.Projectiles.Boss.TwinsBuff
{
	public class EyeFire : ModProjectile
	{
		public override void SetStaticDefaults() {
			if(ModLoader.TryGetMod("Redemption", out Mod mor)) {
				mor.Call("addElementProj", 2, Projectile.type);
				mor.Call("addElementProj", 9, Projectile.type);
			}
			Main.projFrames[Projectile.type] = 7;
			ProjectileID.Sets.DismountsPlayersOnHit[Type] = true;
		}
		public override void SetDefaults() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageProjectile", Projectile, true);
			Projectile.width = 90;
			Projectile.height = 90;
			Projectile.aiStyle = -1;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 6;
			Projectile.timeLeft = 120;
			Projectile.tileCollide = true;
		}
		public override void AI() {
			if(Projectile.localAI[0] == 0) {
				Projectile.localAI[0]++;
				Projectile.rotation = Main.rand.NextFloat() * MathHelper.Pi * 2;
			}
			else Lighting.AddLight(Projectile.Center, 0.9f, 1.1f, 0.4f);
			if(Main.rand.NextBool(90)) {
				Vector2 vector = -Vector2.UnitX.RotatedByRandom(0.78539818525314331).RotatedBy(Projectile.velocity.ToRotation());
				int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 75, 0f, 0f, 0, default(Color), 1.2f);
				Main.dust[d].velocity *= 1.3f;
				Main.dust[d].noGravity = true;
				Main.dust[d].position = Projectile.Center + vector * Projectile.width / 2f;
				if(Main.rand.NextBool(2)) Main.dust[d].fadeIn = 1.4f;
			}
			if(Projectile.ai[0] < Main.projFrames[Projectile.type]) if(++Projectile.frameCounter >= (Projectile.ai[0] > 3 ? 10 : 30)) {
				Projectile.ai[0]++;
				Projectile.frameCounter = 0;
			}
			Projectile.frame = (int)Projectile.ai[0];
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox) {
			int hitboxSize = 30;
			switch(Projectile.ai[0]) {
				case 1:
					hitboxSize += 18;
				break;
				case 2:
					hitboxSize += 28;
				break;
				case 3:
					hitboxSize += 46;
				break;
				case 4:
					hitboxSize += 52;
				break;
				case 5:
					hitboxSize += 56;
				break;
				case 6:
					hitboxSize += 60;
				break;
			}
			hitbox = new Rectangle((int)Projectile.Center.X - (hitboxSize / 2), (int)Projectile.Center.Y - (hitboxSize / 2), hitboxSize, hitboxSize);
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
			fallThrough = true;
			float hitboxSize = 15;
			switch(Projectile.ai[0]) {
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
			switch(Projectile.ai[0]) {
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
		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers) => target.AddBuff(BuffID.CursedInferno, 180);
		public override bool OnTileCollide(Vector2 oldVelocity) {
			Projectile.timeLeft -= 12;
			return false;
		}
		public override Color? GetAlpha(Color newColor) => new Color(200, 200, 200, 25);
	}
}