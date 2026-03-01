using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.BrainOfCthulhuBuff
{
	public class BrainofCthulhu : ModProjectile
	{
		public override string Texture => "Terraria/Images/NPC_266";
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = Main.npcFrameCount[266];
			ProjectileID.Sets.TrailCacheLength[Type] = 5;
			ProjectileID.Sets.TrailingMode[Type] = 0;
		}
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 80;
			Projectile.aiStyle = -1;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 60;
		}
		public override void AI() {
			if(NPC.crimsonBoss < 0) {
				Projectile.Kill();
				return;
			}
			NPC brain = Main.npc[NPC.crimsonBoss];
			if(!brain.active) {
				Projectile.Kill();
				return;
			}
			if(Projectile.timeLeft == 45) Projectile.velocity = Vector2.Normalize(Main.player[brain.target].Center - brain.Center) * 16f;
			brain.velocity *= 0;
			brain.Center = Projectile.Center - brain.velocity + Projectile.velocity;
			brain.localAI[1] = Projectile.timeLeft < 2 ? 0f : 60f;
			brain.ai[0] = Projectile.timeLeft < 2 ? -2f : -3f;
			brain.ai[3] = brain.alpha = 255;
			if(Projectile.timeLeft < 15) Projectile.alpha += 17;
			else if(Projectile.alpha > 0) Projectile.alpha -= 17;
		}
		public override bool PreDraw(ref Color lightColor) {
			if(NPC.crimsonBoss < 0) return false;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			NPC brain = Main.npc[NPC.crimsonBoss];
			if(Projectile.timeLeft < 45) for(int i = 0; i < MathHelper.Min(-(Projectile.timeLeft - 45), Projectile.oldPos.Length); i++) Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + Projectile.Size * 0.5f + Vector2.UnitY * 16f - Main.screenPosition, brain.frame, lightColor * MathHelper.Lerp(Projectile.Opacity * 0.5f, 0f, (float)i / (float)Projectile.oldPos.Length), brain.rotation, new Vector2(brain.frame.Width, brain.frame.Height) * 0.5f, brain.scale, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center + Vector2.UnitY * 16f - Main.screenPosition, brain.frame, lightColor * Projectile.Opacity, brain.rotation, new Vector2(brain.frame.Width, brain.frame.Height) * 0.5f, brain.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}