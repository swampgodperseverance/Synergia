using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.PlanteraBuff
{
	public class FlowerPow : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}
		public override void SetDefaults() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageProjectile", Projectile, true);
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.penetrate = -1;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.hide = true;
		}
		public override void AI() {
			Projectile.rotation += Projectile.direction * 0.3f;
			if(Projectile.ai[1] == 0f) SoundEngine.PlaySound(SoundID.Item42, Projectile.Center);
			Projectile.ai[1]++;
			if(Projectile.ai[1] < 60f) {
				Projectile.velocity += Vector2.Normalize(Main.npc[NPC.plantBoss].Center + Projectile.ai[0].ToRotationVector2() * 160f - Projectile.Center) * 0.84f;
				Projectile.velocity *= 0.96f;
			}
			if(Projectile.ai[1] == 60f) {
				SoundEngine.PlaySound(SoundID.Grass, Projectile.Center);
				Projectile.velocity = Vector2.Normalize(Projectile.Center - Main.npc[NPC.plantBoss].Center) * 6f;
				Vector2 origin = Main.npc[NPC.plantBoss].Center;
				Vector2 distToProj = origin - Projectile.Center;
				float distance = distToProj.Length();
				for(int i = 0; i < distance; i += 8) Dust.NewDust(Projectile.Center + distToProj * (i / distance), 0, 0, 40);
			}
			else if(Projectile.ai[1] < 90f) {
				Player player = Main.player[Main.npc[NPC.plantBoss].target];
				float speed = Projectile.velocity.Length();
				if(speed < 8f) speed += 0.1f;
				else speed = 8f;
				Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Projectile.velocity.SafeNormalize(Projectile.oldVelocity), Vector2.Normalize(player.Center - Projectile.Center), 0.1f)) * speed;
			}
			Lighting.AddLight(Projectile.Center, (Color.HotPink * Projectile.Opacity).ToVector3());
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = texture2D.Size() * 0.5f;
			if(Projectile.ai[1] < 60f) try {
				Texture2D texture = TextureAssets.Chain27.Value;
				Vector2 origin = Main.npc[NPC.plantBoss].Center;
				Vector2 center = Projectile.Center;
				Vector2 distToProj = origin - center;
				float distance = distToProj.Length();
				while(distance > 30f && !float.IsNaN(distance)) {
					distToProj.Normalize();
					distToProj *= texture.Height;
					center += distToProj;
					distToProj = origin - center;
					distance = distToProj.Length();
					Main.EntitySpriteDraw(texture, center - Main.screenPosition, null, Lighting.GetColor((int)(center.X / 16f), (int)(center.Y / 16f)), distToProj.ToRotation() + MathHelper.PiOver2, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
				}
			}
			catch(Exception e) {
			}
			else for(int i = 0; i < Projectile.oldPos.Length; i++) {
				Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
				Color color = Projectile.GetAlpha(lightColor * 0.5f) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture2D, drawPos, null, color * 0.35f, Projectile.rotation, drawOrigin, Projectile.scale, 0, 0f);
			}
			lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f));
			Main.EntitySpriteDraw(texture2D, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, drawOrigin, Projectile.scale, 0, 0f);
			return true;
		}
		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) => behindNPCs.Add(index);
	}
}
