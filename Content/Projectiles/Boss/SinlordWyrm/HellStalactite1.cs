using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Drawing;

namespace Synergia.Content.Projectiles.Boss.SinlordWyrm
{
	public class HellStalactite1 : ModProjectile
	{
		public override void SetDefaults() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageProjectile", Projectile, true);
			Projectile.width = 20;
			Projectile.height = 32;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 480;
			Projectile.aiStyle = -1;
		}
		public override void AI() {
			Projectile.velocity.Y += 0.1f;
			Projectile.velocity *= 0.99f;
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ChlorophyteLeafCrystalShot, new ParticleOrchestraSettings { PositionInWorld = Projectile.BottomRight, MovementVector = new Vector2(1f, -5f), UniqueInfoPiece = (byte)(Main.rgbToHsl(Color.OrangeRed).X * 255f)});
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ChlorophyteLeafCrystalShot, new ParticleOrchestraSettings { PositionInWorld = Projectile.BottomLeft, MovementVector = new Vector2(-1f, -5f), UniqueInfoPiece = (byte)(Main.rgbToHsl(Color.OrangeRed).X * 255f)});
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Texture2D glow = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, 0f, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, null, Color.White, 0f, glow.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}