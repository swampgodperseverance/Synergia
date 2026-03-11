using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Boss.DestroyerBuff;
using System.Collections.Generic;
using System;

namespace Synergia.Content.NPCs.AI
{
	public class DestroyerAI : GlobalNPC
	{
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation) => npc.type == NPCID.Probe || npc.aiStyle == 37;
		public override void Load() => On_NPC.AI_037_Destroyer += (orig, npc) => {
			if(NPC.IsMechQueenUp) {
				orig(npc);
				return;
			}
			if(npc.type == NPCID.TheDestroyer) {
				if(npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead) npc.TargetClosest(true);
				Vector2 targetPos = Main.player[npc.target].Center;
				Vector2 shootDir = (targetPos - npc.Center);
				if(Main.netMode != 1 && npc.ai[0] == 0f) {
					npc.realLife = npc.whoAmI;
					int destroyerSegmentsCount = NPC.GetDestroyerSegmentsCount();
					int oldWhoAmI = npc.whoAmI;
					for(int j = 0; j <= destroyerSegmentsCount; j++) {
						int type = NPCID.TheDestroyerBody;
						if(j == destroyerSegmentsCount) type = NPCID.TheDestroyerTail;
						int d = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), type, npc.whoAmI, oldWhoAmI, 0f, 0f, 0f, 255);
						Main.npc[d].realLife = npc.whoAmI;
						if(Main.netMode == 2) NetMessage.SendData(23, -1, -1, null, d);
						oldWhoAmI = d;
					}
					npc.ai[0] = 1f;
				}
				else switch(npc.ai[0]) {
					case 1:
						if(npc.velocity.Length() < 16f) npc.velocity -= Vector2.UnitY.RotatedBy(npc.rotation);
						else npc.velocity *= 0.97f;
						float distance = shootDir.Length();
						if(distance > 180f + npc.ai[2]) {
							npc.ai[1]++;
							float lerp = npc.ai[2] / 240f;
							if(npc.ai[2] < 240f) npc.ai[2]++;
							npc.velocity = Vector2.Normalize(Vector2.Lerp(npc.velocity.SafeNormalize(npc.oldVelocity), Vector2.Normalize(shootDir), lerp * lerp * lerp * lerp * lerp)) * npc.velocity.Length();
						}
						else {
							if(Main.netMode != 1 && npc.ai[2] > 0f) {
								List<NPC> segments = new();
								bool doNotShootAnyMoreLasers = false;
								foreach(NPC body in Main.ActiveNPCs) if(body.whoAmI != npc.whoAmI && body.realLife == npc.whoAmI && body.aiStyle == npc.aiStyle) if(body.ai[3] > 0f) {
									body.ai[3] += 5f;
									doNotShootAnyMoreLasers = true;
									break;
								}
								else if(body.ai[1] == 0f && Collision.CanHitLine(body.Center, 0, 0, targetPos, 0, 0)) segments.Add(body);
								if(!doNotShootAnyMoreLasers && segments.Count > 0) {
									int i = Main.rand.Next(segments.Count);
									NPC segment = segments[i];
									segment.ai[1] = 10f;
									segment.ai[3] = MathHelper.Min(segments.Count - i, Main.rand.Next(5, 15));
									segment.netUpdate = true;
								}
								segments.Clear();
								foreach(NPC probe in Main.ActiveNPCs) if(probe.ai[1] == 0f && probe.type == NPCID.Probe) segments.Add(probe);
								if(segments.Count > 0) {
									int i = Main.rand.Next(segments.Count);
									NPC probe = segments[i];
									probe.ai[1] = 60f;
									probe.netUpdate = true;
								}
							}
							npc.ai[2] = 0f;
						}
						if(npc.ai[1] > 900f) {
							npc.ai[0]++;
							foreach(Projectile projectile in Main.ActiveProjectiles) if(projectile.ModProjectile is DestroyerBomb) {
								npc.ai[0]++;
								break;
							}
							npc.ai[1] = 0f;
							npc.ai[2] = 0f;
							npc.TargetClosest(true);
							npc.netUpdate = true;
						}
					break;
					case 2:
						if(npc.ai[1] == 120f && Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<DestroyerPulse>(), 0, 0f, Main.myPlayer, 30f);
						else if(npc.ai[1] > 120f && npc.ai[1] < 240f && Main.netMode != 1) {
							List<NPC> segments = new();
							foreach(NPC body in Main.ActiveNPCs) if(body.whoAmI != npc.whoAmI && body.realLife == npc.whoAmI && body.aiStyle == npc.aiStyle) segments.Add(body);
							if(segments.Count > 0 && npc.ai[1] - 120f < segments.Count) {
								NPC segment = segments[(int)npc.ai[1] - 120];
								Projectile.NewProjectile(segment.GetSource_FromAI(), segment.Center, segment.ai[2] == 1f ? Main.rand.NextVector2CircularEdge(12f, 12f) : Main.rand.NextVector2Circular(32f, 32f), segment.ai[2] == 1f ? ModContent.ProjectileType<DestroyerMissile>() : ModContent.ProjectileType<DestroyerBomb>(), 40, 0f, Main.myPlayer, Main.rand.Next(30, 91) * npc.ai[2]);
							}
						}
						if(npc.ai[1] < 120f) {
							if(npc.velocity.Length() > 4f) npc.velocity += Vector2.UnitY.RotatedBy(npc.rotation) * 0.1f;
							float lerp = npc.ai[1] / 120f;
							npc.velocity = Vector2.Normalize(Vector2.Lerp(npc.velocity.SafeNormalize(npc.oldVelocity), Vector2.Normalize(shootDir), lerp * lerp * lerp * lerp * lerp)) * npc.velocity.Length();
						}
						else if(npc.ai[1] > 240f) {
							if(npc.velocity.Length() < 16f) npc.velocity -= Vector2.UnitY.RotatedBy(npc.rotation);
							else npc.velocity *= 0.97f;
							float lerp = (npc.ai[1] - 240f) / 60f;
							npc.velocity = Vector2.Normalize(Vector2.Lerp(npc.velocity.SafeNormalize(npc.oldVelocity), -Vector2.Normalize(shootDir), lerp * lerp * lerp * lerp * lerp)) * npc.velocity.Length();
						}
						if(++npc.ai[1] > 300f) {
							npc.ai[0] = 1f;
							npc.ai[1] = 0f;
							npc.TargetClosest(true);
							npc.netUpdate = true;
						}
					break;
					case 3:
						if(npc.ai[1] == 120f && Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<DestroyerPulse>(), 0, 0f, Main.myPlayer, 30f);
						else if(npc.ai[1] >= 120f && npc.ai[1] <= 240f && Main.netMode != 1) {
							if(npc.ai[1] % 40 == 0) foreach(NPC segment in Main.ActiveNPCs) if(segment.whoAmI != npc.whoAmI && segment.realLife == npc.whoAmI && segment.aiStyle == npc.aiStyle) Projectile.NewProjectile(segment.GetSource_FromAI(), segment.Center, segment.ai[2] == 1f ? Main.rand.NextVector2CircularEdge(12f, 12f) : Vector2.UnitX.RotatedBy(npc.rotation + Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4) * 0.4f) * (Main.rand.NextBool() ? 1f : -1f), segment.ai[2] == 1f ? ModContent.ProjectileType<DestroyerMissile>() : ModContent.ProjectileType<PlasmaBeam>(), 40, 0f, Main.myPlayer, segment.ai[2] == 1f ? 60f : segment.whoAmI + 1);
						}
						if(npc.ai[1] < 120f) {
							if(npc.velocity.Length() > 1f) npc.velocity += Vector2.UnitY.RotatedBy(npc.rotation);
							float lerp = npc.ai[1] / 120f;
							npc.velocity = Vector2.Normalize(Vector2.Lerp(npc.velocity.SafeNormalize(npc.oldVelocity), Vector2.Normalize(shootDir), lerp * lerp * lerp * lerp * lerp)) * npc.velocity.Length();
						}
						if(++npc.ai[1] > 300f) {
							npc.ai[0] = 1f;
							npc.ai[1] = 0f;
							npc.TargetClosest(true);
							npc.netUpdate = true;
						}
					break;
				}
				npc.rotation = npc.velocity.ToRotation() + MathHelper.PiOver2;
				if(npc.alpha > 0) npc.alpha -= 51;
			}
			else if(npc.ai[0] > -1f) {
				NPC body = Main.npc[(int)npc.ai[0]];
				npc.target = body.target;
				npc.alpha = body.alpha;
				float distancing = 1f / npc.scale * 0.8f;
				Vector2 attachToBody = body.Center + Vector2.UnitY.RotatedBy(body.rotation) * body.height * distancing - npc.Center;
				if(body.rotation != npc.rotation) attachToBody = Utils.MoveTowards(Utils.RotatedBy(attachToBody, MathHelper.WrapAngle(body.rotation - npc.rotation) * 0.02f, Vector2.Zero), (body.rotation - npc.rotation).ToRotationVector2(), 1f);
				npc.Center = body.Center + Vector2.UnitY.RotatedBy(body.rotation) * body.height * distancing - Utils.SafeNormalize(attachToBody, Vector2.Zero) * npc.height * distancing;
				npc.rotation = attachToBody.ToRotation() + MathHelper.PiOver2;
				if(npc.ai[1] > 0f) if(--npc.ai[1] == 0f) npc.ai[3] = 0f;
				if(body.ai[1] == 1f && body.ai[3] > 0f) {
					npc.ai[1] = 10f;
					npc.ai[3] = body.ai[3] - 1f;
					body.ai[3] = 0f;
					npc.netUpdate = true;
					body.netUpdate = true;
				}
				if(npc.ai[1] == 5f && npc.target > -1 && Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + Vector2.Normalize(Main.player[npc.target].Center + Main.player[npc.target].velocity * 20f * (1f - npc.ai[2]) - npc.Center) * (24f - npc.ai[2] * 16f), Vector2.Normalize(Main.player[npc.target].Center + Main.player[npc.target].velocity * 20f * (1f - npc.ai[2]) - npc.Center) * (8f + npc.ai[2] * 4f), npc.ai[2] == 1f ? ModContent.ProjectileType<DestroyerMissile>() : ProjectileID.DeathLaser, 27, 0f, Main.myPlayer, 0f, Main.rand.Next(60, 120) * npc.ai[2]);
			}
		};
		public override void AI(NPC npc) {
			if(NPC.IsMechQueenUp || npc.type != NPCID.Probe) return;
			npc.localAI[0] = 0f;
			if(npc.ai[1] > 0f) if(--npc.ai[1] == 5f) {
				if(Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + npc.rotation.ToRotationVector2() * npc.width * 0.5f * npc.spriteDirection, npc.rotation.ToRotationVector2() * 8f * npc.spriteDirection, ProjectileID.DeathLaser, 27, 0f, Main.myPlayer);
				npc.velocity -= npc.rotation.ToRotationVector2() * 4f;
			}
			else if(npc.ai[1] > 5f) npc.velocity *= npc.ai[1] / 60f;
			Vector2 shootDir = Main.player[npc.target].Center - npc.Center;
			if(shootDir.Length() < 320f) {
				if(npc.ai[0] > 0f) npc.ai[0] *= -1f;
				npc.velocity -= Vector2.Normalize(shootDir) * 0.1f;
			}
		}
		public override void PostDraw(NPC npc, SpriteBatch sprite, Vector2 screenPos, Color lightColor) {
			if(NPC.IsMechQueenUp) return;
			int frameHeight = Terraria.GameContent.TextureAssets.Npc[npc.type].Height() / Main.npcFrameCount[npc.type];
			Vector2 center = npc.Bottom - screenPos;
			center.Y += -frameHeight * npc.scale + 4f + frameHeight / 2 * npc.scale;
			if(npc.type == NPCID.Probe) {
				if(npc.ai[1] <= 0f) return;
				center += npc.rotation.ToRotationVector2() * npc.width * 0.5f * npc.spriteDirection;
				if(npc.ai[1] > 10f) {
					Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Extra_174");
					sprite.Draw(texture, center, null, new Color(255, 0, 0, 0) * (float)Math.Sin(MathHelper.Pi * ((npc.ai[1] - 10f) / 50f)) * 0.5f, 0f, texture.Size() * 0.5f, npc.scale * (npc.ai[1] - 10f) / 50f, SpriteEffects.None, 0);
					return;
				}
				float glowTime = (float)Math.Sin(npc.ai[1] * 0.1f * MathHelper.Pi);
				for(int i = 1; i < 3; i++) {
					Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_" + i);
					sprite.Draw(texture, center, null, new Color(175, 0, 0, 25) * glowTime, 0f, texture.Size() * 0.5f, new Vector2(glowTime * (i == 1 ? 1.1f : 0.8f), (i != 1 ? 1.1f : 0.8f) * glowTime) * 1.2f, SpriteEffects.None, 0);
				}
				for(int i = 1; i < 3; i++) {
					Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_" + i);
					sprite.Draw(texture, center, null, new Color(200, 200, 200, 0) * glowTime, 0f, texture.Size() * 0.5f, new Vector2(glowTime * (i == 1 ? 1.1f : 0.8f), (i != 1 ? 1.1f : 0.8f) * glowTime) * 0.8f, SpriteEffects.None, 0);
				}
			}
			else if(npc.type != NPCID.TheDestroyer && npc.ai[1] > 0f && npc.ai[2] == 0f) {
				center.Y += frameHeight / 3.5f * npc.scale;
				float glowTime = (float)Math.Sin(npc.ai[1] * 0.1f * MathHelper.Pi);
				for(int i = 1; i < 3; i++) {
					Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_" + i);
					sprite.Draw(texture, center, null, new Color(175, 0, 0, 25) * glowTime, 0f, texture.Size() * 0.5f, new Vector2(glowTime * (i == 1 ? 1.1f : 0.8f), (i != 1 ? 1.1f : 0.8f) * glowTime) * 1.2f, SpriteEffects.None, 0);
				}
				for(int i = 1; i < 3; i++) {
					Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_" + i);
					sprite.Draw(texture, center, null, new Color(200, 200, 200, 0) * glowTime, 0f, texture.Size() * 0.5f, new Vector2(glowTime * (i == 1 ? 1.1f : 0.8f), (i != 1 ? 1.1f : 0.8f) * glowTime) * 0.8f, SpriteEffects.None, 0);
				}
			}
		}
	}
}
