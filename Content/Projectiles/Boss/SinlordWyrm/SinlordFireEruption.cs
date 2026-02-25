using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.GameContent.Drawing;

namespace Synergia.Content.Projectiles.Boss.SinlordWyrm
{
	public class SinlordFireEruption : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_85";
		public override void SetDefaults() {
			Projectile.width = 0;
			Projectile.height = 0;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 60;
		}
		public override void AI() {
			if(Projectile.timeLeft > 20) ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ChlorophyteLeafCrystalShot, new ParticleOrchestraSettings { PositionInWorld = Projectile.Center, MovementVector = (Main.rand.NextVector2Circular(1f, 1f) - Vector2.UnitY * 2f) * 2f, UniqueInfoPiece = (byte)(Main.rgbToHsl(Color.OrangeRed).X * 255f)});
			else {
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitY * -9f, ModContent.ProjectileType<SinlordFireBreath>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f, 1f);
				if(Projectile.timeLeft % 4 == 0) SoundEngine.PlaySound(Terraria.ID.SoundID.DD2_FlameburstTowerShot with { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, Pitch = -0.8f, Volume = 4f }, Projectile.Center);
			}
		}
		public override bool PreDraw(ref Color lightColor) => false;
		public override bool ShouldUpdatePosition() => false;
	}
}