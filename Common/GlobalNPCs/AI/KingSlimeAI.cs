using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using System;
using Synergia.Content.Projectiles.Boss.KingSlimeBuff;

namespace Synergia.Common.GlobalNPCs.AI
{
	public class KingSlimeAI : GlobalNPC
	{
		public static Vector2 GetJewelPos(NPC npc, int which = 0) {
			Vector2 center = new Vector2(npc.Center.X, npc.position.Y + npc.height - (float)npc.height * npc.scale * 0.5f);
			float offset = 0f;
			switch(npc.frame.Y / 120) {
				case 0:
					offset = 2f;
				break;
				case 1:
					offset = -6f;
				break;
				case 2:
					offset = 2f;
				break;
				case 3:
					offset = 10f;
				break;
				case 4:
					offset = 2f;
				break;
			}
			switch(which) {
				case 1:
					center -= new Vector2(21, 12);
				break;
				case 2:
					center -= new Vector2(-21, 12);
				break;
				case 3:
					center.Y -= 15;
				break;
				case 4:
					center -= new Vector2(32, 16);
				break;
				case 5:
					center -= new Vector2(-32, 16);
				break;
			}
			center.Y += npc.gfxOffY - (34 - offset) * npc.scale;
			return center;
		}
		public static int GetTeleportTime(NPC npc) => (int)MathHelper.Lerp(6f, 12f, (float)npc.life / (float)npc.lifeMax) * (Main.getGoodWorld ? 5 : 10);
		public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == 50;
		public override bool PreAI(NPC npc) {
			npc.defense = 5;
			if(npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active) {
				npc.TargetClosest();
				if((npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active) && !npc.despawnEncouraged) npc.EncourageDespawn(30);
			}
			Vector2 targetPos = Main.player[npc.target].Center;
			Vector2 shootDir = Vector2.Normalize(targetPos - npc.Center);
			if(npc.velocity.Length() > 1f) npc.direction = Math.Sign(npc.velocity.X);
			else if(shootDir.X != 0) npc.direction = Math.Sign(shootDir.X);
			npc.scale = MathHelper.Lerp(0.75f, 1.25f, (float)npc.life / (float)npc.lifeMax);
			bool shouldTeleport = Vector2.Distance(npc.Bottom, targetPos) > 640f || npc.ai[2] > 600f;
			if(Math.Abs(npc.velocity.X) > 1f || shouldTeleport) npc.ai[3] = 0f;
			else if(npc.ai[0] == 0f) npc.ai[3]++;
			switch(npc.ai[0]) {
				case -1:
					int attackTime = GetTeleportTime(npc);
					if(npc.ai[1] > attackTime) {
						if(++npc.ai[2] > 5) npc.ai[2] = 1f;
						npc.ai[0] = npc.ai[2];
						npc.ai[1] = 0f;
						npc.TargetClosest();
						npc.netUpdate = true;
						break;
					}
					if(npc.velocity.Y == 0f) {
						npc.velocity.X *= 0.8f;
						if(npc.ai[1] == 0f) npc.ai[1]++;
					}
					if(npc.ai[1] > 0f) npc.ai[1]++;
					if(npc.ai[1] == attackTime / 2) {
						npc.scale = 0.01f;
						Gore.NewGore(npc.GetSource_FromAI(), GetJewelPos(npc) - Vector2.UnitY * 56f, npc.velocity, 734);
						Vector2 chosenTile = npc.Center;
						if(Vector2.Distance(npc.Bottom, targetPos) > 1080f) npc.Bottom = targetPos - Vector2.UnitY * 320f;
						else if(npc.AI_AttemptToFindTeleportSpot(ref chosenTile, (int)targetPos.X / 16, (int)targetPos.Y / 16, rangeFromTargetTile: 24, telefragPreventionDistanceInTiles: 8, teleportInAir: Vector2.Distance(npc.Bottom, targetPos) > 800f)) npc.Bottom = chosenTile * 16f;
						npc.netUpdate = true;
					}
					else if(npc.ai[1] < attackTime / 2) npc.scale *= MathHelper.SmoothStep(1f, 0.01f, npc.ai[1] / attackTime * 2);
					else if(npc.ai[1] > attackTime / 2) npc.scale *= MathHelper.SmoothStep(0.01f, 1f, (npc.ai[1] - attackTime / 2) / attackTime * 2);
					if(npc.ai[1] > 0f && npc.scale > 0f) for(int f = 0; f < 3; f++) {
						int d = Dust.NewDust(new Vector2(npc.Center.X - npc.width * npc.scale * 0.6f, npc.position.Y + npc.height - npc.height * npc.scale), (int)(npc.width * npc.scale * 1.2f), (int)(npc.height * npc.scale), 4, npc.velocity.X, npc.velocity.Y, 150, new Color(78, 136, 255, 80), 1f);
						Main.dust[d].noGravity = true;
						Dust dust = Main.dust[d];
						dust.velocity *= 0.5f;
					}
				break;
				case 0:
					attackTime = (int)MathHelper.Lerp(2f, 18f, (float)npc.life / (float)npc.lifeMax) * 10;
					if(npc.ai[1] > attackTime) {
						if(shouldTeleport) npc.ai[0] = -1f;
						else {
							if(++npc.ai[2] > 5) npc.ai[2] = 1f;
							npc.ai[0] = npc.ai[2];
						}
						npc.ai[1] = 0f;
						npc.TargetClosest();
						npc.netUpdate = true;
						break;
					}
					if(npc.velocity.Y == 0f) {
						if(shouldTeleport) {
							npc.ai[0] = -1f;
							npc.ai[1] = 0f;
							npc.TargetClosest();
							npc.netUpdate = true;
							break;
						}
						npc.direction = Math.Sign(shootDir.X);
						npc.velocity.X *= 0.8f;
						npc.ai[1]++;
					}
					if(npc.ai[1] % (int)(attackTime / 3) == 0f) {
						if(targetPos.Y < npc.position.Y + npc.velocity.Y - npc.height) npc.velocity = new Vector2(npc.direction * 8f, -16f);
						else npc.velocity = new Vector2(npc.direction, -1.25f) * MathHelper.Lerp(8f, 6f, (float)npc.life / (float)npc.lifeMax);
						npc.ai[1]++;
					}
					npc.noGravity = false;
					npc.noTileCollide = targetPos.Y > npc.position.Y + npc.velocity.Y + npc.height;
				break;
				case 1: 
					if(npc.ai[1] > 120f) {
						npc.ai[2] = npc.ai[0];
						npc.ai[0] = shouldTeleport ? -1f : 0f;
						npc.ai[1] = 0f;
						npc.TargetClosest();
						npc.netUpdate = true;
						break;
					}
					if(npc.velocity.Y == 0f) {
						npc.velocity.X *= 0.8f;
						if(npc.ai[1] == 0f) npc.ai[1]++;
					}
					if(npc.ai[1] > 0f) {
						npc.ai[1]++;
						if(npc.ai[1] == 5f) SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_4") { Pitch = 0.5f, Volume = 2f }, npc.Center);
					}
					shootDir = Vector2.Normalize(targetPos - GetJewelPos(npc));
					if(npc.ai[1] >= 60f && npc.ai[1] % (Main.getGoodWorld ? 10 : Main.masterMode ? 15 : 20) == 0) {
						if(Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), GetJewelPos(npc, 3), shootDir * (Main.expertMode ? 10f : 6f), ModContent.ProjectileType<RoyalBlast>(), 12, 0f, Main.myPlayer, 0f, 4f);
						SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_8") { MaxInstances = 3, Pitch = 0.5f, Volume = 2f }, GetJewelPos(npc));
					}
					npc.noGravity = false;
					npc.noTileCollide = false;
				break;
				case 2:
					if(npc.ai[1] > 120f) {
						npc.ai[2] = npc.ai[0];
						npc.ai[0] = shouldTeleport ? -1f : 0f;
						npc.ai[1] = 0f;
						npc.TargetClosest();
						npc.netUpdate = true;
						break;
					}
					if(npc.velocity.Y == 0f) {
						npc.velocity.X *= 0.8f;
						if(npc.ai[1] == 0f) npc.ai[1]++;
					}
					if(npc.ai[1] > 0f) {
						npc.ai[1]++;
						if(npc.ai[1] == 5f) SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_4") { Pitch = 0.2f, Volume = 2f }, npc.Center);
					}
					if(npc.ai[1] >= 60f && npc.ai[1] % 20f == 0) {
						if(Main.netMode != 1) if(npc.ai[1] % 40f == 0) Projectile.NewProjectile(npc.GetSource_FromAI(), GetJewelPos(npc, 1), Vector2.One * -(Main.expertMode ? MathHelper.Lerp(6f, 4f, (float)npc.life / (float)npc.lifeMax) : 4f), ModContent.ProjectileType<RoyalBlast>(), 11, 0f, Main.myPlayer, npc.target + 1, 3f);
					else Projectile.NewProjectile(npc.GetSource_FromAI(), GetJewelPos(npc, 2), (Vector2.UnitX - Vector2.UnitY) * (Main.expertMode ? MathHelper.Lerp(6f, 4f, (float)npc.life / (float)npc.lifeMax) : 4f), ModContent.ProjectileType<RoyalBlast>(), 11, 0f, Main.myPlayer, npc.target + 1, 3f);
						SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_8") { MaxInstances = 2, Pitch = 0.2f, Volume = 2f }, GetJewelPos(npc, npc.ai[1] % 40 == 0 ? 1 : 2));
					}
					npc.noGravity = false;
					npc.noTileCollide = false;
				break;
				case 3: 
					if(npc.ai[1] > 60f) {
						npc.ai[2] = npc.ai[0];
						npc.ai[0] = shouldTeleport ? -1f : 0f;
						npc.ai[1] = 0f;
						npc.TargetClosest();
						npc.netUpdate = true;
						break;
					}
					if(npc.velocity.Y == 0f) {
						npc.velocity.X *= 0.8f;
						if(npc.ai[1] == 0f) npc.ai[1]++;
					}
					if(npc.ai[1] > 0f) {
						npc.ai[1]++;
						if(npc.ai[1] == 5f) SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_4") { Pitch = -0.3f, Volume = 4f }, npc.Center);
					}
					if(npc.ai[1] == 60f) {
						shootDir = Vector2.Normalize(targetPos - GetJewelPos(npc));
						if(Main.netMode != 1) for(int i = 0; i < (Main.masterMode ? 18 : 12); i++) Projectile.NewProjectile(npc.GetSource_FromAI(), GetJewelPos(npc, 3), shootDir.RotatedBy(MathHelper.ToRadians(Main.masterMode ? 20f : 30f) * i) * (Main.expertMode ? MathHelper.Lerp(8f, 6f, (float)npc.life / (float)npc.lifeMax) : 6f), ModContent.ProjectileType<RoyalBlast>(), 13, 0f, Main.myPlayer, -1f);
						SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_8") { Pitch = -0.3f, Volume = 4f }, GetJewelPos(npc));
					}
					npc.noGravity = false;
					npc.noTileCollide = false;
				break;
				case 4:
					if(npc.ai[1] > 90f) {
						npc.ai[2] = npc.ai[0];
						npc.ai[0] = shouldTeleport ? -1f : 0f;
						npc.ai[1] = 0f;
						npc.TargetClosest();
						npc.netUpdate = true;
						break;
					}
					if(npc.velocity.Y == 0f) {
						npc.velocity.X *= 0.8f;
						if(npc.ai[1] == 5f) SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_4") { Pitch = -0.1f, Volume = 2f }, npc.Center);
						if(npc.ai[1] != 45f) npc.ai[1]++;
					}
					if(npc.ai[1] == 45f) {
						npc.velocity.X = npc.direction * 8f;
						npc.velocity.Y = -16f;
						npc.ai[1]++;
					}
					if(npc.ai[1] > 0f && npc.velocity.Y > 0f && npc.ai[1] < 90f) {
						if(npc.ai[1] % 10f == 0f) {
							if(Main.netMode != 1) if(npc.ai[1] % 20f == 0f) for(int i = -2; i < 2; i++) {
								Projectile.NewProjectile(npc.GetSource_FromAI(), GetJewelPos(npc, 4), Vector2.UnitX.RotatedBy(MathHelper.PiOver4 * (i + 0.5f)) * -(Main.expertMode ? MathHelper.Lerp(9f, 6f, (float)npc.life / (float)npc.lifeMax) : 6f), ModContent.ProjectileType<RoyalBlast>(), 11, 0f, Main.myPlayer, 0f, 2f);
								Projectile.NewProjectile(npc.GetSource_FromAI(), GetJewelPos(npc, 5), Vector2.UnitX.RotatedBy(MathHelper.PiOver4 * (i + 0.5f)) * (Main.expertMode ? MathHelper.Lerp(9f, 6f, (float)npc.life / (float)npc.lifeMax) : 6f), ModContent.ProjectileType<RoyalBlast>(), 11, 0f, Main.myPlayer, 0f, 2f);
							}
							else for(int i = -2; i <= 2; i++) {
								Projectile.NewProjectile(npc.GetSource_FromAI(), GetJewelPos(npc, 4), Vector2.UnitX.RotatedBy(MathHelper.PiOver4 * i) * -(Main.expertMode ? MathHelper.Lerp(9f, 6f, (float)npc.life / (float)npc.lifeMax) : 6f), ModContent.ProjectileType<RoyalBlast>(), 11, 0f, Main.myPlayer, 0f, 2f);
								Projectile.NewProjectile(npc.GetSource_FromAI(), GetJewelPos(npc, 5), Vector2.UnitX.RotatedBy(MathHelper.PiOver4 * i) * (Main.expertMode ? MathHelper.Lerp(9f, 6f, (float)npc.life / (float)npc.lifeMax) : 6f), ModContent.ProjectileType<RoyalBlast>(), 11, 0f, Main.myPlayer, 0f, 2f);
							}
							SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_8") { Pitch = -0.1f, Volume = 2f }, GetJewelPos(npc));
						}
						npc.ai[1]++;
					}
					npc.noGravity = false;
					npc.noTileCollide = false;
				break;
				case 5:
					if(npc.ai[1] > 90f) {
						npc.ai[2] = npc.ai[0];
						npc.ai[0] = shouldTeleport ? -1f : 0f;
						npc.ai[1] = 0f;
						npc.TargetClosest();
						npc.netUpdate = true;
						break;
					}
					if(npc.velocity.Y == 0f) {
						npc.velocity.X *= 0.8f;
						if(npc.ai[1] != 45f) npc.ai[1]++;
					}
					if(npc.ai[1] == 44f) {
						npc.velocity.X = npc.direction * 2f;
						npc.velocity.Y = -MathHelper.Lerp(24f, 12f, (float)npc.life / (float)npc.lifeMax);
						for(int a = 0; a < 36; a++) Dust.NewDustPerfect(npc.Bottom, 4, MathHelper.ToRadians(a * 10f).ToRotationVector2() * new Vector2(16f * npc.scale, 8f), 150, new Color(78, 136, 255, 80), npc.scale * 2f).noGravity = true;
						SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_154") { Pitch = -0.2f, Volume = 4f }, npc.Center);
						npc.ai[1]++;
					}
					if(npc.ai[1] == 45f && npc.velocity.Y == 0f) {
						if(Main.netMode != 1) for(int i = -6; i <= 6; i++) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Bottom - Vector2.UnitY.RotatedBy(MathHelper.ToRadians(15f) * i) * 100f * npc.scale - Vector2.UnitY * 16f, Vector2.UnitY.RotatedBy(MathHelper.ToRadians(15f) * i) * -(Main.expertMode ? MathHelper.Lerp(12f, 8f, (float)npc.life / (float)npc.lifeMax) : 8f), 605, 14, 0f, Main.myPlayer);
						for(int a = 0; a < 36; a++) Dust.NewDustPerfect(npc.Bottom, 4, MathHelper.ToRadians(a * 10f).ToRotationVector2() * new Vector2(16f * npc.scale, 8f), 150, new Color(78, 136, 255, 80), npc.scale * 2f).noGravity = true;
						SoundEngine.PlaySound(Terraria.ID.SoundID.Item154, npc.Bottom - Vector2.UnitY * 16f);
						SoundEngine.PlaySound(Terraria.ID.SoundID.DeerclopsRubbleAttack, npc.Bottom - Vector2.UnitY * 16f);
						Main.instance.CameraModifiers.Add(new Terraria.Graphics.CameraModifiers.PunchCameraModifier(npc.Bottom, Vector2.UnitY, 8f, 8, 60, 1000f, "Stomp"));
						npc.ai[1]++;
					}
					if(npc.ai[1] > 0f && npc.velocity.Y > 0f) {
						if(!npc.noGravity) SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_46") { Pitch = -0.8f, Volume = 4f }, npc.Center);
						npc.noGravity = true;
						npc.velocity.X = npc.direction * 2f;
						if(npc.velocity.Y < 24f) npc.velocity.Y++;
					}
					else npc.noGravity = false;
					npc.noTileCollide = false;
				break;
			}
			int slimeCount = 1;
			if(npc.life > npc.lifeMax / 2 && npc.justHit && Main.rand.NextBool(slimeCount) && slimeCount < 8 && Main.netMode != 1) {
				int s = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.position.X + Main.rand.Next(npc.width), (int)npc.position.Y + Main.rand.Next(npc.height), Main.getGoodWorld || (Main.rand.NextBool(slimeCount / 2 + 2) && Main.expertMode) ? 535 : 1);
				Main.npc[s].velocity = (Main.rand.NextFloat(MathHelper.Pi) - MathHelper.PiOver2).ToRotationVector2();
				if(Main.netMode == 2 && s < 200) NetMessage.SendData(23, -1, -1, null, s);
			}
			return false;
		}
		public override bool PreDraw(NPC npc, SpriteBatch sprite, Vector2 screenPosition, Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/NPC_50");
			Vector2 center = new Vector2(npc.Center.X, npc.position.Y + npc.height + 2) - screenPosition;
			sprite.Draw(texture, center, npc.frame, lightColor, 0f, new Vector2(texture.Width * 0.5f, texture.Height / Main.npcFrameCount[npc.type]), npc.scale, SpriteEffects.None, 0);
			center = GetJewelPos(npc) - screenPosition;
			texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Extra_39");
			sprite.Draw(texture, center, null, lightColor, 0f, new Vector2(texture.Width * 0.5f, texture.Height), 1f, SpriteEffects.None, 0);
			if(npc.ai[0] == 1f && npc.ai[1] < 60f) {
				center = GetJewelPos(npc, 3) - screenPosition;
				texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Extra_98");
				float glowTime = MathHelper.SmoothStep(0f, 2f, npc.ai[1] / 60f);
				float glowColor = MathHelper.Lerp(1f, 0f, npc.ai[1] / 60f);
				float glowRotation = MathHelper.SmoothStep(0f, MathHelper.Pi * npc.direction, npc.ai[1] / 60f);
				sprite.Draw(texture, center, null, new Color(175, 0, 0, 25) * glowColor, glowRotation, texture.Size() * 0.5f, new Vector2(1f, 2f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(175, 0, 0, 25) * glowColor, glowRotation - MathHelper.PiOver2 * npc.direction, texture.Size() * 0.5f, new Vector2(1f, 2f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(225, 225, 225, 55) * glowColor, glowRotation, texture.Size() * 0.5f, new Vector2(0.5f, 1.5f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(225, 225, 225, 55) * glowColor, glowRotation - MathHelper.PiOver2 * npc.direction, texture.Size() * 0.5f, new Vector2(0.5f, 1.5f * glowTime), SpriteEffects.None, 0);
			}
			else if(npc.ai[0] == 2f && npc.ai[1] < 60f) {
				center = GetJewelPos(npc, 1) - screenPosition;
				texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Extra_98");
				float glowTime = MathHelper.SmoothStep(0f, 1.5f, npc.ai[1] / 60f);
				float glowColor = MathHelper.Lerp(1f, 0f, npc.ai[1] / 60f);
				float glowRotation = MathHelper.SmoothStep(0f, MathHelper.Pi, npc.ai[1] / 60f);
				sprite.Draw(texture, center, null, new Color(0, 175, 0, 25) * glowColor, glowRotation, texture.Size() * 0.5f, new Vector2(1f, 2f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(0, 175, 0, 25) * glowColor, glowRotation - MathHelper.PiOver2, texture.Size() * 0.5f, new Vector2(1f, 2f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(225, 225, 225, 55) * glowColor, glowRotation, texture.Size() * 0.5f, new Vector2(0.5f, 1.5f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(225, 225, 225, 55) * glowColor, glowRotation - MathHelper.PiOver2, texture.Size() * 0.5f, new Vector2(0.5f, 1.5f * glowTime), SpriteEffects.None, 0);
				center = GetJewelPos(npc, 2) - screenPosition;
				sprite.Draw(texture, center, null, new Color(0, 175, 0, 25) * glowColor, -glowRotation, texture.Size() * 0.5f, new Vector2(1f, 2f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(0, 175, 0, 25) * glowColor, -glowRotation + MathHelper.PiOver2, texture.Size() * 0.5f, new Vector2(1f, 2f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(225, 225, 225, 55) * glowColor, -glowRotation, texture.Size() * 0.5f, new Vector2(0.5f, 1.5f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(225, 225, 225, 55) * glowColor, -glowRotation + MathHelper.PiOver2, texture.Size() * 0.5f, new Vector2(0.5f, 1.5f * glowTime), SpriteEffects.None, 0);
			}
			else if(npc.ai[0] == 3f && npc.ai[1] < 60f) {
				center = GetJewelPos(npc, 3) - screenPosition;
				texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Extra_98");
				float glowTime = MathHelper.SmoothStep(0f, 2.5f, npc.ai[1] / 60f);
				float glowColor = MathHelper.Lerp(1f, 0f, npc.ai[1] / 60f);
				float glowRotation = MathHelper.SmoothStep(0f, MathHelper.TwoPi, npc.ai[1] / 60f);
				sprite.Draw(texture, center, null, new Color(175, 0, 155, 25) * glowColor, glowRotation, texture.Size() * 0.5f, new Vector2(1f, 2f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(175, 0, 155, 25) * glowColor, glowRotation - MathHelper.PiOver2, texture.Size() * 0.5f, new Vector2(1f, 2f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(175, 0, 155, 25) * glowColor, -glowRotation, texture.Size() * 0.5f, new Vector2(1f, 2f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(175, 0, 155, 25) * glowColor, -glowRotation + MathHelper.PiOver2, texture.Size() * 0.5f, new Vector2(1f, 2f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(225, 225, 225, 55) * glowColor, glowRotation, texture.Size() * 0.5f, new Vector2(0.5f, 1.5f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(225, 225, 225, 55) * glowColor, glowRotation - MathHelper.PiOver2, texture.Size() * 0.5f, new Vector2(0.5f, 1.5f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(225, 225, 225, 55) * glowColor, -glowRotation, texture.Size() * 0.5f, new Vector2(0.5f, 1.5f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(225, 225, 225, 55) * glowColor, -glowRotation + MathHelper.PiOver2, texture.Size() * 0.5f, new Vector2(0.5f, 1.5f * glowTime), SpriteEffects.None, 0);
			}
			else if(npc.ai[0] == 4f && npc.ai[1] < 45f) {
				center = GetJewelPos(npc, 4) - screenPosition;
				texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Extra_98");
				float glowTime = MathHelper.SmoothStep(0f, 1f, npc.ai[1] / 45f);
				float glowColor = MathHelper.Lerp(1f, 0f, npc.ai[1] / 45f);
				float glowRotation = MathHelper.SmoothStep(0f, MathHelper.TwoPi, npc.ai[1] / 45f);
				sprite.Draw(texture, center, null, new Color(0, 0, 175, 25) * glowColor, -glowRotation, texture.Size() * 0.5f, new Vector2(1f, 2f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(0, 0, 175, 25) * glowColor, -glowRotation + MathHelper.PiOver2, texture.Size() * 0.5f, new Vector2(1f, 2f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(225, 225, 225, 55) * glowColor, -glowRotation, texture.Size() * 0.5f, new Vector2(0.5f, 1.5f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(225, 225, 225, 55) * glowColor, -glowRotation + MathHelper.PiOver2, texture.Size() * 0.5f, new Vector2(0.5f, 1.5f * glowTime), SpriteEffects.None, 0);
				center = GetJewelPos(npc, 5) - screenPosition;
				sprite.Draw(texture, center, null, new Color(0, 0, 175, 25) * glowColor, glowRotation, texture.Size() * 0.5f, new Vector2(1f, 2f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(0, 0, 175, 25) * glowColor, glowRotation - MathHelper.PiOver2, texture.Size() * 0.5f, new Vector2(1f, 2f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(225, 225, 225, 55) * glowColor, glowRotation, texture.Size() * 0.5f, new Vector2(0.5f, 1.5f * glowTime), SpriteEffects.None, 0);
				sprite.Draw(texture, center, null, new Color(225, 225, 225, 55) * glowColor, glowRotation - MathHelper.PiOver2, texture.Size() * 0.5f, new Vector2(0.5f, 1.5f * glowTime), SpriteEffects.None, 0);
			}
			return false;
		}
	}
}