using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.IO;
using Synergia.Content.Projectiles.Boss.TwinsBuff;

namespace Synergia.Common.GlobalNPCs.AI
{
	public class TwinsAI : GlobalNPC
	{
		internal static bool Disabled = false;
		private static int ret = -1;
		private static int spaz = -1;
		private static int specialAttackCounter = 0;
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation) => npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism;
		public override void OnSpawn(NPC npc, Terraria.DataStructures.IEntitySource source) => ret = spaz = -1;
		public override void AI(NPC npc) {
			if(Disabled) return;
			if(npc.type == NPCID.Retinazer) {
				ret = npc.whoAmI;
				bool spazPhase2 = true;
				if(spaz > -1) {
					NPC twin = Main.npc[spaz];
					if(specialAttackCounter == 2 && twin.aiStyle == -1 && npc.ai[0] == 3f && twin.ai[0] == 3f) if(twin.ai[1] == 4f) {
						npc.localAI[1] = 0f;
						npc.ai[1] = 0f;
						npc.ai[2] = 0f;
						npc.ai[3] = 0f;
					}
					else {
						npc.localAI[1] = 0f;
						npc.ai[1] = 0f;
						npc.ai[2] = 0f;
						npc.ai[3] = 0f;
						if(npc.Distance(twin.Center) > 240f) npc.velocity += (twin.Center - npc.Center) * 0.0016f;
						npc.velocity *= 0.96f;
						return;
					}
					if(npc.ai[0] == 0f && npc.ai[1] == 0f && twin.ai[0] == 0f && twin.ai[1] > 0f) {
						npc.ai[2] = 0f;
						npc.ai[3] = 0f;
					}
					else if(npc.ai[0] == 3f) {
						npc.ai[2]--;
						spazPhase2 = twin.ai[1] > 0f;
						if(twin.ai[0] == 0f) spazPhase2 = !spazPhase2;
						if(spazPhase2) npc.ai[1] = 0f;
						else npc.ai[1] = 1f;
					}
					if(npc.ai[0] == 0f && twin.ai[0] == 3f) if(npc.ai[1] == 0f && twin.ai[1] == 0f) npc.ai[2] = float.PositiveInfinity;
					else if(npc.ai[1] > 0f && twin.ai[1] > 0f) npc.ai[3] = float.PositiveInfinity;
					if(!twin.active || twin.type != NPCID.Spazmatism) spaz = -1;
					else spazPhase2 = twin.ai[0] == 3f;
				}
				else if(npc.ai[0] == 3f) if(specialAttackCounter < 2) specialAttackCounter = 2;
				else if(npc.ai[1] == 1f) specialAttackCounter = 3;
				if(npc.ai[0] == 0f) {
					if(npc.localAI[3] > 0f) npc.localAI[3]--;
					if(npc.ai[1] == 0f && npc.ai[3] > 50f) {
						if(Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center - Vector2.UnitY * 4f + Vector2.UnitY.RotatedBy(npc.rotation) * npc.width * 0.6f, Vector2.UnitY.RotatedBy(npc.rotation) * 10f, ProjectileID.EyeLaser, 35, 0f, Main.myPlayer);
						npc.ai[3] = 0f;
						npc.localAI[3] = 10f;
					}
					else if(npc.ai[1] == 2f && npc.ai[2] == 0f) {
						npc.velocity *= 1.3f;
						SoundEngine.PlaySound(SoundID.Item46 with { Pitch = -0.3f }, npc.Center);
					}
				}
				else if(npc.ai[0] == 3f) {
					npc.localAI[1] = 0f;
					if(npc.aiStyle == -1) {
						if(specialAttackCounter == 3) {
							if(++npc.ai[3] > 360f) {
								specialAttackCounter = 0;
								npc.aiStyle = 30;
								npc.ai[1] = 0f;
								npc.ai[2] = 0f;
								npc.ai[3] = 0f;
								npc.netUpdate = true;
							}
							if(npc.ai[3] == 330f) {
								for(int i = 0; i < 30; i++) {
									int d = Dust.NewDust(npc.position, npc.width, npc.height, 6);
									Main.dust[d].noGravity = true;
									Main.dust[d].scale *= 2.1f;
									Main.dust[d].velocity = (Main.dust[d].position - npc.Center) * 0.15f;
									int e = Gore.NewGore(npc.GetSource_FromAI(), npc.position + new Vector2((float)(npc.width * Main.rand.Next(100)) / 100f, (float)(npc.height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, default(Vector2), Main.rand.Next(61, 64));
									Gore gore = Main.gore[e];
									gore.velocity *= 0.3f;
									gore.scale *= 0.8f;
									Main.gore[e].velocity = (Main.gore[e].position - npc.Center) * 0.15f;
								}
								SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot with {Pitch = -0.1f, Volume = 2f}, npc.Center);
							}
							else if(npc.ai[3] > 240f && npc.ai[3] < 345f) {
								int d = Dust.NewDust(npc.position, npc.width, npc.height, 6);
								Main.dust[d].noGravity = true;
								Main.dust[d].scale *= 2.1f;
								Main.dust[d].velocity = (Main.dust[d].position - npc.Center) * 0.05f;
								int e = Gore.NewGore(npc.GetSource_FromAI(), npc.position + new Vector2((float)(npc.width * Main.rand.Next(100)) / 100f, (float)(npc.height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, default(Vector2), Main.rand.Next(61, 64));
								Gore gore = Main.gore[e];
								gore.velocity *= 0.3f;
								gore.scale *= 0.8f;
								Main.gore[e].velocity.X += (float)Main.rand.Next(-10, 11) * 0.05f;
								Main.gore[e].velocity.Y += (float)Main.rand.Next(-10, 11) * 0.05f;
							}
							if(npc.ai[3] < 330f) {
								npc.color = Color.Lerp(default(Color), Color.Orange, npc.ai[3] / 330f);
								Lighting.AddLight(npc.Center, (Color.Orange * (npc.ai[3] / 330f)).ToVector3());
							}
							else {
								npc.color = Color.Lerp(Color.Orange, default(Color), (npc.ai[3] - 330f) / 30f);
								Lighting.AddLight(npc.Center, (Color.Orange * ((npc.ai[3] - 330f) / 30f)).ToVector3());
							}
							npc.velocity *= 0.98f;
						}
						return;
					}
					else if(npc.aiStyle == 30 && npc.ai[1] == 0f && specialAttackCounter == 3) {
						npc.aiStyle = -1;
						npc.ai[3] = 0f;
						npc.netUpdate = true;
						SoundEngine.PlaySound(SoundID.Zombie66, npc.Center);
						if(Main.netMode != 1) {
							Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center - Vector2.UnitY * 4f + Vector2.UnitY.RotatedBy(npc.rotation) * npc.width, Vector2.Zero, ModContent.ProjectileType<RetinazerPulse>(), 0, 0f, Main.myPlayer, 20f);
							Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center - Vector2.UnitY * 4f + Vector2.UnitY.RotatedBy(npc.rotation) * npc.width, Vector2.UnitY.RotatedBy(npc.rotation), ModContent.ProjectileType<RetinaRay>(), 45, 0f, Main.myPlayer, npc.whoAmI, Main.rand.NextBool() ? 1 : -1);
						}
						return;
					}
					if(npc.localAI[3] > 0f) npc.localAI[3]--;
					if(++npc.ai[3] > (6f - npc.ai[1] * (spazPhase2 ? 5f : 4f)) * 15f) {
						if(Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center - Vector2.UnitY * 4f + Vector2.UnitY.RotatedBy(npc.rotation) * npc.width, Vector2.UnitY.RotatedBy(npc.rotation) * 10f, ProjectileID.DeathLaser, 37, 0f, Main.myPlayer);
						npc.ai[3] = 0f;
						npc.localAI[3] = 10f;
					}
				}
			}
			if(npc.type == NPCID.Spazmatism) {
				spaz = npc.whoAmI;
				bool retPhase2 = true;
				if(ret > -1) {
					NPC twin = Main.npc[ret];
					if(npc.ai[0] == 3f && twin.ai[0] == 3f) if(specialAttackCounter == 0 && npc.ai[1] == 4f && twin.aiStyle == 30) {
						npc.ai[1] = 1f;
						npc.ai[2] = 0f;
						npc.netUpdate = true;
					}
					else if(specialAttackCounter != 1 && twin.aiStyle == -1) if(npc.ai[1] == 4f) {
						if(npc.Distance(twin.Center) > 240f) npc.velocity += (twin.Center - npc.Center) * 0.0016f;
						npc.velocity *= 0.96f;
					}
					else {
						npc.ai[1] = 4f;
						npc.netUpdate = true;
					}
					if(!twin.active || twin.type != NPCID.Retinazer) ret = -1;
					else retPhase2 = twin.ai[0] == 3f;
				}
				else {
					if(npc.aiStyle == 31 && npc.ai[0] == 3f && npc.ai[1] == 4f) {
						npc.ai[1] = 1f;
						npc.ai[2] = 0f;
						npc.netUpdate = true;
					}
					if(specialAttackCounter > 1) specialAttackCounter = 0;
				}
				if(npc.ai[0] == 0f) {
					if(npc.localAI[3] > 0f) npc.localAI[3]--;
					if(npc.ai[1] == 0f && npc.ai[3] > 40f) {
						if(Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center - Vector2.UnitY * 4f + Vector2.UnitY.RotatedBy(npc.rotation) * npc.width * 0.6f, Vector2.UnitY.RotatedBy(npc.rotation) * 12f, ProjectileID.CursedFlameHostile, 25, 1f, Main.myPlayer);
						npc.ai[3] = 0f;
						npc.localAI[3] = 6f;
					}
					else if(npc.ai[1] == 2f && npc.ai[2] == 0f) {
						npc.velocity *= 2f;
						SoundEngine.PlaySound(SoundID.Item46 with { Pitch = 0.1f }, npc.Center);
					}
					if(npc.ai[1] > 0f && npc.ai[3] < 3) npc.ai[3] = 3f;
				}
				else if(npc.ai[0] == 3f) {
					if(npc.aiStyle == -1) {
						if(++npc.ai[1] > 240f) {
							npc.aiStyle = 31;
							npc.ai[1] = 3f;
							npc.ai[2] = 0f;
							npc.netUpdate = true;
						}
						npc.velocity *= 0.98f;
						npc.rotation += npc.direction * (npc.ai[1] < 120f ? npc.ai[1] / 120f : (1f - (npc.ai[1] - 120f) / 120f) * 0.5f);
						bool shoot = false;
						if(npc.rotation > MathHelper.Pi) {
							shoot = true;
							npc.rotation -= MathHelper.TwoPi;
						}
						if(npc.rotation < -MathHelper.Pi) {
							shoot = true;
							npc.rotation += MathHelper.TwoPi;
						}
						if(shoot && Main.netMode != 1) {
							Vector2 random = Main.rand.NextVector2CircularEdge(1f, 1f);
							Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center - Vector2.UnitY * 4f, Vector2.Zero, ModContent.ProjectileType<SpazmatismPulse>(), 0, 0f, Main.myPlayer, 3f);
							int target = npc.target == -1 || npc.target == 255 ? Player.FindClosest(npc.position, npc.width, npc.height) : npc.target;
							Vector2 v = Main.player[target].Center + Main.rand.NextVector2Circular(320f, 320f);
							Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center - Vector2.UnitY * 4f + random * npc.width * 0.6f, random * 12f, ModContent.ProjectileType<Sclerocket>(), 32, 0f, Main.myPlayer, v.X, v.Y, Main.masterMode ? target : -1f);
						}
					}
					else if(npc.ai[1] == 0f) {
						if(retPhase2 && npc.ai[2] == 0f && npc.aiStyle == 31) {
							if(specialAttackCounter == 1) {
								npc.aiStyle = -1;
								if(Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center - Vector2.UnitY * 4f, Vector2.Zero, ModContent.ProjectileType<SpazmatismPulse>(), 0, 0f, Main.myPlayer, 20f);
								npc.netUpdate = true;
							}
							specialAttackCounter++;
						}
						if(npc.aiStyle == 31) {
							npc.velocity = (Main.expertMode ? (Main.player[npc.target].Center - npc.Center) * 0.016f : Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 6f) * MathHelper.Min(1f, npc.ai[2] * 0.1f);
							npc.ai[2] += retPhase2 ? 2f : 1f;
							if(npc.ai[2] > 5f && Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + npc.velocity - Vector2.UnitY * 4f + Vector2.UnitY.RotatedBy(npc.rotation) * npc.width * 0.3f, Vector2.UnitY.RotatedBy(npc.rotation) * 2.2f + npc.velocity / 7f, ModContent.ProjectileType<EyeFire>(), 35, 1f, Main.myPlayer);
						}
						npc.localAI[1] = 0f;
					}
					else if(npc.ai[1] == 1f) SoundEngine.PlaySound(SoundID.Item46 with { Pitch = -0.2f }, npc.Center);
					else if(npc.ai[1] == 2f && npc.ai[2] == 0f && Main.expertMode) {
						float lerp = (1f - (float)(npc.life / (float)npc.lifeMax) - 0.6f);
						npc.velocity *= 1f + lerp;
						npc.ai[2] += (int)(lerp * 30);
					}
					else if(npc.ai[1] == 3f) {
						if(++npc.ai[2] > 60f) {
							npc.ai[1] = 0f;
							npc.ai[2] = float.PositiveInfinity;
							npc.netUpdate = true;
						}
						npc.velocity += Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 0.54f;
						npc.velocity *= 0.96f;
					}
					else if(specialAttackCounter == 0 && npc.ai[1] == 4f) {
						npc.ai[1] = 1f;
						npc.ai[2] = 0f;
						npc.netUpdate = true;
					}
				}
			}
			npc.dontTakeDamage = npc.ai[0] == 1f || npc.ai[0] == 2f;
		}
		public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
			if(Main.netMode == 0) return;
			binaryWriter.Write(npc.aiStyle);
			binaryWriter.Write(npc.rotation);
		}
		public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
			if(Main.netMode == 0) return;
			npc.aiStyle = binaryReader.ReadInt32();
			npc.rotation = binaryReader.ReadSingle();
		}
		public override void OnKill(NPC npc) {
			if(npc.type == NPCID.Retinazer) ret = -1;
			else if(npc.type == NPCID.Spazmatism) spaz = -1;
			if(ret == -1 && spaz == -1) specialAttackCounter = 0;
		}
		public override void PostDraw(NPC npc, SpriteBatch sprite, Vector2 screenPos, Color lightColor) {
			if(Disabled) return;
			int frameHeight = Terraria.GameContent.TextureAssets.Npc[npc.type].Height() / Main.npcFrameCount[npc.type];
			Vector2 center = npc.Bottom - screenPos;
			center.Y += -frameHeight * npc.scale + 4f + 107f * npc.scale + Main.NPCAddHeight(npc);
			if(npc.type == NPCID.Retinazer && (npc.ai[0] == 0f || npc.ai[0] == 3f) && npc.localAI[3] > 0f && npc.localAI[3] < 10f) {
				center += Vector2.UnitY.RotatedBy(npc.rotation) * npc.width * (npc.ai[0] == 3f ? 0.8f : 0.55f);
				float glowTime = (float)System.Math.Sin(npc.localAI[3] * 0.1f * MathHelper.Pi);
				for(int i = 1; i < 3; i++) {
					Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_" + i);
					sprite.Draw(texture, center, null, (npc.ai[0] == 3f ? Color.DarkRed : Color.BlueViolet) with {A = 25} * glowTime, 0f, texture.Size() * 0.5f, new Vector2(glowTime * (i == 1 ? 1.1f : 0.8f), (i != 1 ? 1.1f : 0.8f) * glowTime) * 2.4f, SpriteEffects.None, 0);
				}
				for(int i = 1; i < 3; i++) {
					Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_" + i);
					sprite.Draw(texture, center, null, new Color(200, 200, 200, 0) * glowTime, 0f, texture.Size() * 0.5f, new Vector2(glowTime * (i == 1 ? 1.1f : 0.8f), (i != 1 ? 1.1f : 0.8f) * glowTime) * 1.6f, SpriteEffects.None, 0);
				}
			}
			else if(npc.type == NPCID.Spazmatism && npc.aiStyle == 31 && npc.ai[0] == 0f && npc.localAI[3] > 0f && npc.localAI[3] < 6f) {
				center += new Vector2(2f * npc.scale, npc.width * 0.55f).RotatedBy(npc.rotation);
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/CursedMuzzleFlash");
				Main.EntitySpriteDraw(texture, center, new Rectangle(0, (texture.Height / 6) * (int)(6 - npc.localAI[3]), texture.Width, texture.Height / 6), Color.White, npc.rotation + MathHelper.PiOver2, new Vector2(0, texture.Height / 6) * 0.5f, npc.scale * 1.25f, SpriteEffects.None, 0);
			}
		}
	}
}
