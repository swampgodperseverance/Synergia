using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalNPCs.AI
{
	public class SkeletronPrimeAI : GlobalNPC
	{
		internal static bool Disabled = false;
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation) => npc.type == NPCID.SkeletronPrime || npc.type == NPCID.PrimeCannon || npc.type == NPCID.PrimeLaser;
		public override void AI(NPC npc) {
			if(Disabled) return;
			if(npc.type == NPCID.PrimeLaser) {
				if(npc.localAI[0] >= (npc.ai[2] == 1f ? 30f : 180f)) {
					Vector2 shootDir = Vector2.UnitY.RotatedBy(npc.rotation);
					Vector2 spawnPos = npc.Center + shootDir * npc.height;
					if(Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), spawnPos, shootDir * (10f + npc.ai[2] * 4f), ProjectileID.DeathLaser, 35, 1f, Main.myPlayer);
					npc.localAI[0] = 0;
					npc.localAI[1] = 10f;
					npc.velocity -= shootDir * 4f;
					SoundEngine.PlaySound(SoundID.Item61, npc.position);
				}
				if(npc.localAI[1] > 0f) npc.localAI[1]--;
				return;
			}
			if(npc.type == NPCID.PrimeCannon) {
				if(npc.localAI[0] >= (npc.ai[2] == 1f ? 30f : 120f)) {
					Vector2 shootDir = Vector2.UnitY.RotatedBy(npc.rotation);
					Vector2 spawnPos = npc.Center + shootDir * npc.height;
					if(Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), spawnPos, shootDir * (npc.ai[2] == 1f ? 1f : 12f), npc.ai[2] == 1f ? ProjectileID.RocketSkeleton : ProjectileID.BombSkeletronPrime, 35, 1f, Main.myPlayer);
					npc.localAI[0] = 0f;
					npc.localAI[1] = 6f;
					npc.velocity -= shootDir * 4f;
					SoundEngine.PlaySound(SoundID.Item61, npc.position);
				}
				if(npc.localAI[1] > 0f) npc.localAI[1]--;
				return;
			}
			if(npc.type != NPCID.SkeletronPrime || npc.ai[1] > 0f) {
				if(npc.localAI[1] > 0f) npc.localAI[1]--;
				return;
			}
			bool cannon = false;
			bool laser = false;
			foreach(NPC arm in Main.ActiveNPCs) if(arm.type == NPCID.PrimeCannon || arm.ModNPC?.Name == "PrimeLauncher") cannon = true;
			else if(arm.type == NPCID.PrimeLaser || arm.ModNPC?.Name == "PrimeRail") laser = true;
			else if(cannon && laser) return;
			if(!cannon) if(npc.frame.Y > 0 ? npc.localAI[2] % 2 != 0 : npc.localAI[2] % 2 == 0) if(++npc.localAI[2] > (Main.expertMode && npc.life < npc.lifeMax / 2 ? 24f : 9f)) {
				if(npc.localAI[2] > 24f) FireSkulls(npc);
				else if(Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center - new Vector2(0f, 16f * npc.scale) + new Vector2(0f, 28f * npc.scale).RotatedBy(npc.rotation), npc.velocity, ProjectileID.BombSkeletronPrime, 35, 1f, Main.myPlayer);
				npc.localAI[2] = 0f;
			}
			if(!laser && ++npc.localAI[3] > 60f) {
				FireLasers(npc);
				npc.localAI[1] = 10f;
				npc.localAI[3] = 0f;
			}
			if(npc.localAI[1] > 0f) npc.localAI[1]--;
		}
		public override void PostDraw(NPC npc, SpriteBatch sprite, Vector2 screenPos, Color lightColor) {
			if(Disabled) return;
			int frameHeight = Terraria.GameContent.TextureAssets.Npc[npc.type].Height() / Main.npcFrameCount[npc.type];
			if(npc.type == NPCID.SkeletronPrime && npc.localAI[1] > 0f) for(int k = -1; k <= 1; k += 2) {
				Vector2 center = npc.Bottom - screenPos;
				center.Y += -frameHeight * npc.scale + 4f + frameHeight / 2 * npc.scale;
				center += -new Vector2(k * 16f, 8f).RotatedBy(npc.rotation) * npc.scale;
				float glowTime = (float)System.Math.Sin(npc.localAI[1] * 0.1f * MathHelper.Pi);
				for(int i = 1; i < 3; i++) {
					Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_" + i);
					sprite.Draw(texture, center, null, new Color(175, 0, 0, 25) * glowTime, npc.rotation, texture.Size() * 0.5f, new Vector2(glowTime * (i == 1 ? 1.1f : 0.8f), (i != 1 ? 1.1f : 0.8f) * glowTime) * 1.2f, SpriteEffects.None, 0);
				}
				for(int i = 1; i < 3; i++) {
					Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_" + i);
					sprite.Draw(texture, center, null, new Color(200, 200, 200, 0) * glowTime, npc.rotation, texture.Size() * 0.5f, new Vector2(glowTime * (i == 1 ? 1.1f : 0.8f), (i != 1 ? 1.1f : 0.8f) * glowTime) * 0.8f, SpriteEffects.None, 0);
				}
			}
			else if(npc.type == NPCID.PrimeLaser && npc.localAI[1] > 0f) {
				Vector2 center = npc.Bottom - screenPos;
				center.Y += -frameHeight * npc.scale + 4f + frameHeight / 2 * npc.scale;
				center += Vector2.UnitY.RotatedBy(npc.rotation) * npc.height * 0.415f * npc.scale;
				float glowTime = (float)System.Math.Sin(npc.localAI[1] * 0.1f * MathHelper.Pi);
				for(int i = 1; i < 3; i++) {
					Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_" + i);
					sprite.Draw(texture, center, null, new Color(175, 0, 0, 25) * glowTime, npc.rotation, texture.Size() * 0.5f, new Vector2(glowTime * (i == 1 ? 1.1f : 0.8f), (i != 1 ? 1.1f : 0.8f) * glowTime) * 1.2f, SpriteEffects.None, 0);
				}
				for(int i = 1; i < 3; i++) {
					Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_" + i);
					sprite.Draw(texture, center, null, new Color(200, 200, 200, 0) * glowTime, npc.rotation, texture.Size() * 0.5f, new Vector2(glowTime * (i == 1 ? 1.1f : 0.8f), (i != 1 ? 1.1f : 0.8f) * glowTime) * 0.8f, SpriteEffects.None, 0);
				}
			}
			else if(npc.type == NPCID.PrimeCannon && npc.localAI[1] > 0f && npc.localAI[1] < 6f) {
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/FieryMuzzleFlash");
				Vector2 center = npc.Bottom - screenPos;
				center.Y += -frameHeight * npc.scale + 4f + frameHeight / 2 * npc.scale;
				center += new Vector2(4, npc.height - 16).RotatedBy(npc.rotation) * npc.scale;
				Main.EntitySpriteDraw(texture, center, new Rectangle(0, (texture.Height / 6) * (int)(6 - npc.localAI[1]), texture.Width, texture.Height / 6), Color.White, npc.rotation + MathHelper.PiOver2, new Vector2(0, texture.Height / 6) * 0.5f, npc.scale * 1.25f, SpriteEffects.None, 0);
			}
		}
		private static void FireSkulls(NPC npc) {
			if(Main.netMode == NetmodeID.MultiplayerClient) return;
			Player target = Main.player[npc.target];
			if(!target.active || target.dead) target = Main.player[Player.FindClosest(npc.Center, 1, 1)];
			Vector2 baseDir = npc.DirectionTo(target.Center);
			if(baseDir == Vector2.Zero) baseDir = new Vector2(0f, -1f);
			float speed = 10f;
			int damage = 30;
			int knockback = 2;
			int type = ModContent.ProjectileType<Content.Projectiles.Hostile.PrimeSkull>();
			for(int i = -1; i <= 1; i++) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center - new Vector2(0f, 16f * npc.scale) + new Vector2(0f, 28f * npc.scale).RotatedBy(npc.rotation), baseDir.RotatedBy(i * MathHelper.PiOver2) * speed, type, damage, knockback);
			if(Main.netMode != NetmodeID.Server) SoundEngine.PlaySound(SoundID.NPCHit8, npc.Center);
		}
		private static void FireLasers(NPC npc) {
			if(Main.netMode == NetmodeID.MultiplayerClient) return;
			Player target = Main.player[npc.target];
			if(!target.active || target.dead) target = Main.player[Player.FindClosest(npc.Center, 1, 1)];
			float speed = 8f;
			int damage = 25;
			int knockback = 2;
			int type = ProjectileID.DeathLaser;
			for(int i = -1; i <= 1; i += 2) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center - new Vector2(0f, 16f * npc.scale) - new Vector2(i * 16f, 20f).RotatedBy(npc.rotation) * npc.scale + Vector2.Normalize(npc.Center - new Vector2(0f, 16f * npc.scale) - new Vector2(i * -16f, 20f).RotatedBy(npc.rotation) * npc.scale - target.Center) * -speed * 2f, Vector2.Normalize(npc.Center - new Vector2(0f, 16f * npc.scale) - new Vector2(i * -16f, 20f).RotatedBy(npc.rotation) * npc.scale - target.Center) * -speed, type, damage, knockback);
		}
	}
}
