using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.NPCs
{
	public class SkeletronPrimeAI : GlobalNPC
	{
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation) => npc.type == NPCID.SkeletronPrime || npc.type == NPCID.PrimeCannon || npc.type == NPCID.PrimeLaser;
		public override void AI(NPC npc) {
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
			if(npc.type == NPCID.PrimeCannon && npc.ai[2] == 1f) {
				if(npc.localAI[0] >= 30f) {
					Vector2 shootDir = Vector2.UnitY.RotatedBy(npc.rotation);
					Vector2 spawnPos = npc.Center + shootDir * npc.height;
					if(Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), spawnPos, shootDir, ProjectileID.RocketSkeleton, 25, 1f, Main.myPlayer);
					npc.localAI[0] = 0;
					npc.velocity -= shootDir * 4f;
					for(int i = -1; i <= 1; i++) for(int j = 0; j < (10 - System.Math.Abs(i) * 6) * 3; j++) {
						int d = Dust.NewDust(spawnPos - shootDir * npc.height / 2, 0, 0, DustID.Torch, 0, 0, 0, default(Color), npc.scale * 1 + (10 - j) * 0.2f);
						Main.dust[d].noGravity = true;
						Main.dust[d].velocity = shootDir.RotatedBy(MathHelper.PiOver2 * i) * (i == 0 ? j / 2f : j / 3f);
					}
					SoundEngine.PlaySound(SoundID.Item61, npc.position);
				}
				return;
			}
			if(npc.type != NPCID.SkeletronPrime || npc.lifeMax <= 0 || npc.life <= 0 || npc.ai[1] > 0f) return;
			foreach(NPC arm in Main.ActiveNPCs) if(arm.type == NPCID.PrimeCannon || arm.type == NPCID.PrimeLaser || arm.type == NPCID.PrimeSaw || arm.type == NPCID.PrimeVice || arm.ModNPC?.Name == "PrimeLauncher" || arm.ModNPC?.Name == "PrimeMace" || arm.ModNPC?.Name == "PrimeRail") return;
			if(npc.life < npc.lifeMax / 2) {
				if(npc.ai[2] % 90 == 0f) FireLasers(npc);
				if(npc.ai[2] % 240 == 0f) FireSkulls(npc);
			}
			else if(npc.ai[2] % 120 == 0f) FireSkulls(npc);
		}
		public override void PostDraw(NPC npc, SpriteBatch sprite, Vector2 screenPos, Color lightColor) {
			if(npc.type == NPCID.PrimeLaser && npc.localAI[1] > 0f) {
				Vector2 center = npc.Center + Vector2.UnitY.RotatedBy(npc.rotation) * npc.height * 0.415f * npc.scale - Vector2.UnitY * 2 - screenPos;
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
			for(int i = -1; i <= 1; i++) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, baseDir.RotatedBy(i * MathHelper.PiOver2) * speed, type, damage, knockback);
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
			for(int i = -1; i <= 1; i += 2) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center - new Vector2(0f, 16f * npc.scale) - new Vector2(i * 16f, 20f).RotatedBy(npc.rotation) * npc.scale, Vector2.Normalize(npc.Center - new Vector2(0f, 16f * npc.scale) - new Vector2(i * -16f, 20f).RotatedBy(npc.rotation) * npc.scale - target.Center) * -speed, type, damage, knockback);
		}
	}
}
