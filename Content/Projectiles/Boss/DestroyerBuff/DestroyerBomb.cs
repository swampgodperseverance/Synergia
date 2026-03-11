using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace Synergia.Content.Projectiles.Boss.DestroyerBuff
{
	public class DestroyerBomb : ModProjectile
	{
		public override void SetDefaults() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageProjectile", Projectile, true);
			Projectile.CloneDefaults(102);
			Projectile.timeLeft /= 3;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
		}
		public override void AI() {
			Projectile.velocity *= 0.96f;
			if(Projectile.timeLeft > 30 && Projectile.velocity.Length() < 1f) foreach(Player player in Main.ActivePlayers) if(player.Distance(Projectile.Center) < 80f) {
				Projectile.timeLeft = 30;
				return;
			}
		}
		public override void OnKill(int timeLeft) => Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<PlasmaExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
			if(Projectile.timeLeft > 30) return false;
			texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Extra_174");
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 0, 0, 0) * (Projectile.timeLeft / 30f), Projectile.rotation, texture.Size() / 2, Projectile.scale * (1f - Projectile.timeLeft / 30f), SpriteEffects.None, 0);
			return false;
		}
	}
}