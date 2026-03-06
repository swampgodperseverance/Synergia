using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace Synergia.Common.GlobalProjectiles
{
	public class PlanteraThornBall : GlobalProjectile
	{
		public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation) => projectile.type == 277;
		public override bool PreAI(Projectile projectile) {
			if(Math.Abs(projectile.velocity.X) < Math.Abs(projectile.velocity.Y)) projectile.rotation += Math.Sign(projectile.velocity.X) * Math.Abs(projectile.velocity.Y) / 16f;
			else projectile.rotation += projectile.velocity.X / 16f;
			if(projectile.alpha > 0) projectile.alpha -= 51;
			if(Main.npc[NPC.plantBoss].life < Main.npc[NPC.plantBoss].lifeMax / 2) projectile.velocity *= 0.9f;
			Lighting.AddLight(projectile.Center, (Color.HotPink * projectile.Opacity).ToVector3());
			return false;
		}
		public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity) {
			if(projectile.velocity.X != oldVelocity.X) projectile.velocity.X = -oldVelocity.X;
			if(projectile.velocity.Y != oldVelocity.Y) projectile.velocity.Y = -oldVelocity.Y;
			return false;
		}
	}
}
