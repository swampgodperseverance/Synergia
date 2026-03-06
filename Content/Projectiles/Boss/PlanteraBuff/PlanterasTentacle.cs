using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.PlanteraBuff
{
	public class PlanterasTentacle : ModProjectile
	{
		public override string Texture => "Terraria/Images/NPC_" + NPCID.PlanterasTentacle;
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = Main.npcFrameCount[NPCID.PlanterasTentacle];
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageProjectile", Projectile, true);
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.penetrate = -1;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}
		public override void AI() {
			if(++Projectile.frameCounter > 3) {
				if(++Projectile.frame >= Main.projFrames[Type]) Projectile.frame = 0;
				Projectile.frameCounter = 0;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.spriteDirection = Projectile.direction;
			Projectile.ai[0]++;
			if(Projectile.ai[0] < 30f) Projectile.velocity *= 0.94f;
			else if(Projectile.ai[0] < 60f) Projectile.velocity /= 0.94f;
			if(Projectile.ai[0] == 30f) Terraria.Audio.SoundEngine.PlaySound(SoundID.Item46, Projectile.Center);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture2D.Width, texture2D.Height / Main.projFrames[Type]) * 0.5f;
			SpriteEffects spriteDirection = SpriteEffects.FlipHorizontally;
			if(Projectile.spriteDirection < 0) spriteDirection |= SpriteEffects.FlipVertically;
			for(int i = 0; i < Projectile.oldPos.Length; i++) {
				Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
				Color color = Projectile.GetAlpha(lightColor * 0.5f) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture2D, drawPos, new Rectangle(0, texture2D.Height / Main.projFrames[Type] * Projectile.frame, texture2D.Width, texture2D.Height / Main.projFrames[Type]), color * 0.35f, Projectile.rotation, drawOrigin, Projectile.scale, spriteDirection, 0f);
			}
			Main.EntitySpriteDraw(texture2D, Projectile.Center - Main.screenPosition, new Rectangle(0, texture2D.Height / Main.projFrames[Type] * Projectile.frame, texture2D.Width, texture2D.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, drawOrigin, Projectile.scale, spriteDirection, 0f);
			return false;
		}
	}
}
