using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Synergia.Content.Projectiles.Boss.JadeEmperorRework
{
	public class JadeGreaterJavelin : ModProjectile
	{
		public override string Texture => "ValhallaMod/Projectiles/Enemy/JadeGreatJavelin";
		public override void SetDefaults() {
			DrawOffsetX = -25;
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.aiStyle = 0;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 30;
			Projectile.tileCollide = false;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) {
			Projectile.hide = true;
			behindNPCs.Add(index);
			base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
		}
		public override void OnKill(int timeLeft) {
			if(Main.netMode != 1) for(int i = -10; i < 10; i++) Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + Vector2.UnitX * (i + 0.5f) * 128f, -Projectile.velocity, ModContent.ProjectileType<JadeLightningWarning>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
		} 
	}
}
