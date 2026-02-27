using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace Synergia.Content.Projectiles.Boss.SinlordWyrm
{
	public class Hellheart : ModProjectile
	{
		private List<Vector3> dusts = new();
		public override string GlowTexture => "Synergia/Assets/Textures/Glow";
		public override void SetDefaults() {
			Projectile.width = 0;
			Projectile.height = 0;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 16;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
		}
		public override void AI() {
			if(Projectile.alpha > 0) Projectile.alpha -= 5;
			if(Main.rand.NextBool(4 + dusts.Count)) {
				Vector2 t = Main.rand.NextVector2Circular(640f, 640f);
				dusts.Add(new(t.X, t.Y, Main.rand.Next(30, 120)));
			}
			for(int i = 0; i < dusts.Count; i++) if(dusts[i].Z > 0) for(int j = 0; j < 3; j++) {
				Vector2 pos = new(dusts[i].X, dusts[i].Y);
				int dust = Dust.NewDust(Projectile.position + pos, 0, 0, 6);
				Main.dust[dust].scale = Main.rand.NextFloat(1f, 1.4f) * (dusts[i].Z < 30 ? dusts[i].Z / 30f : 1f) * Projectile.Opacity;
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].fadeIn = 0.9f;
				pos = pos.RotatedBy(1f / pos.Length() * 2f);
				dusts[i] = new(pos.X, pos.Y, dusts[i].Z - 1);
			}
			else dusts.Remove(dusts[i]);
			if(Projectile.ai[0] < 0f) return;
			NPC npc = Main.npc[(int)Projectile.ai[0]];
			if(npc.active) {
				Projectile.timeLeft = 16;
				if(npc.ModNPC is Content.NPCs.Boss.SinlordWyrm.Sinlord s && npc.ai[0] > 12f) {
					switch(npc.ai[0]) {
						default:
							if(Projectile.scale > 1f) Projectile.scale *= 0.99f;
							else Projectile.scale = 1f;
						break;
						case 14:
							Projectile.scale = MathHelper.Lerp(1f, 3f, npc.ai[1] / 480f);
						break;
						case 15:
						case 16:
							Projectile.scale = 3f;
						break;
					}
					s.storedPos = Projectile.Center;
				}
			}
			else Projectile.ai[0] = -1f;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = (float)MathHelper.Min(15, Projectile.timeLeft) / 15f * Projectile.Opacity;
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(171, 39, 0, 0) * fade, Main.GlobalTimeWrappedHourly * MathHelper.TwoPi, texture.Size() * 0.5f, Projectile.scale * fade, SpriteEffects.None, 0);
			for(int k = 0; k < 20; k++) Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * fade * 0.5f, (Main.GlobalTimeWrappedHourly * MathHelper.TwoPi / (k / 20f + 1) * 7.5f - MathHelper.ToRadians(k * 36f)), texture.Size() * 0.5f, Projectile.scale * 0.05f * k * fade, SpriteEffects.None, 0);
			return false;
		}
	}
}