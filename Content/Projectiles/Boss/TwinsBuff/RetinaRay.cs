using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.TwinsBuff
{
	public class RetinaRay : ModProjectile
	{
		public override string Texture => "Synergia/Assets/Textures/Ray";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.DismountsPlayersOnHit[Type] = true;
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
		}
		public override void SetDefaults() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageProjectile", Projectile, true);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 360;
			Projectile.netImportant = true;
		}
		public override void AI() {
			Projectile.rotation = Projectile.velocity.ToRotation();
			if(Projectile.ai[0] > 0f) {
				if(Projectile.timeLeft > 30 && Projectile.timeLeft < 300) Main.player[Projectile.owner].GetModPlayer<Common.GlobalPlayer.ScreenShakePlayer>().TriggerShake(6, 0.8f);
				NPC npc = Main.npc[(int)Projectile.ai[0]];
				if(Projectile.timeLeft == 300) SoundEngine.PlaySound(SoundID.NPCDeath56 with { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, Pitch = -0.9f, Volume = 3f }, npc.Center);
				else if(Projectile.timeLeft < 300 && Projectile.timeLeft > 30) SoundEngine.PlaySound(SoundID.Item15 with { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, MaxInstances = 100, Pitch = -0.4f }, npc.Center);
				Projectile.Center = npc.Center - Vector2.UnitY * 4f + Projectile.velocity * npc.width * 0.8f;
				npc.rotation = Projectile.rotation - MathHelper.PiOver2;
				if(Projectile.timeLeft > 30) npc.localAI[3] = Projectile.localAI[0];
				else if(npc.localAI[3] > 0f) npc.localAI[3]--;
				float rotationSpeed = 1.5f;
				if(Main.masterMode) rotationSpeed = 2f;
				if(Main.getGoodWorld) rotationSpeed = 3f;
				if(npc.ai[3] < 80f) rotationSpeed *= npc.ai[3] / 80f;
				if(npc.ai[3] > 280f) rotationSpeed -= (npc.ai[3] - 280f) / 80f * rotationSpeed;
				Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Projectile.ai[1]) * rotationSpeed);
				if(Projectile.timeLeft < 15) Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Main.player[npc.target == -1 ? Player.FindClosest(npc.Center, 0, 0) : npc.target].Center - npc.Center), Vector2.Normalize(Projectile.velocity), Projectile.timeLeft / 15f));
				if(!npc.active) Projectile.Kill();
			}
			if(Projectile.timeLeft <= 300 && Projectile.localAI[0] < 5f) Projectile.localAI[0]++;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			if(Projectile.timeLeft < 30 || Projectile.timeLeft > 300) return false;
			float point = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Vector2.UnitX.RotatedBy(Projectile.rotation) * 2400f, 16 * Projectile.scale, ref point);
		}
		public override bool PreDraw(ref Color lightColor) {
			if(Projectile.timeLeft < 30) return false;
			float fade = Projectile.localAI[0] / 5f * MathHelper.Min(Projectile.timeLeft - 30, 5) / 5f;
			if(Projectile.timeLeft > 300) fade = (float)System.Math.Sin(MathHelper.Pi * (Projectile.timeLeft - 300) / 30f);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			for(int i = 0; i < (Projectile.timeLeft < 300 ? 2 : 1); i++) {
				lightColor = i > 0 ? Color.White : Color.Red;
				if(Projectile.timeLeft >= 300) lightColor *= 0.2f;
				lightColor.A = 0;
				if(i > 0) fade /= 2f;
				texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
				Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height - 1), lightColor * fade * (i + 1), Projectile.rotation - MathHelper.PiOver2, new Vector2(texture.Width * 0.5f, texture.Height - 1), new Vector2(Projectile.scale * 2f, 0.25f) * fade * (2 - i), SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height - 1, texture.Width, 1), lightColor * fade * (i + 1), Projectile.rotation - MathHelper.PiOver2, new Vector2(texture.Width * 0.5f, 0f), new Vector2(Projectile.scale * 2f * fade * (2 - i), 2400f), SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, Projectile.Center + Projectile.rotation.ToRotationVector2() * 2400f - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height - 1), lightColor * fade * (i + 1), Projectile.rotation + MathHelper.PiOver2, new Vector2(texture.Width * 0.5f, texture.Height - 1), new Vector2(Projectile.scale * 2f, 0.75f) * fade * (2 - i), SpriteEffects.None, 0);
			}
			return false;
		}
		public override bool ShouldUpdatePosition() => false;
	}
}