using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.IO;
using System.Collections.Generic;
using System;
using System.Reflection;
using Synergia.Content.Projectiles.Boss.JadeEmperorRework;
using ValhallaMod.NPCs.Emperor;
using ValhallaMod.Projectiles.Enemy;

namespace Synergia.Common.GlobalNPCs.AI
{
	public class JadeEmperorRework : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation) => npc.ModNPC is Emperor;
		private Emperor modNPC;
		private float[] ai = new float[4];
		public Vector2 HandPosLeft {
			get {
				try {
					var e = typeof(Emperor);
					Vector2 v = Vector2.Zero;
					if(e.GetField("handLMoveX", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(modNPC) is float x) v.X = x;
					if(e.GetField("handLMoveY", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(modNPC) is float y) v.Y = y;
					return v;
				}
				catch (Exception e) {
					return Vector2.Zero;
				}
			}
			set {
				try {
					var e = typeof(Emperor);
					e.GetField("handLMoveX", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(modNPC, value.X);
					e.GetField("handLMoveY", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(modNPC, value.Y);
				}
				catch (Exception e) {
				}
			}
		}
		public Vector2 HandPosRight {
			get {
				try {
					var e = typeof(Emperor);
					Vector2 v = Vector2.Zero;
					if(e.GetField("handRMoveX", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(modNPC) is float x) v.X = x;
					if(e.GetField("handRMoveY", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(modNPC) is float y) v.Y = y;
					return v;
				}
				catch (Exception e) {
					return Vector2.Zero;
				}
			}
			set {
				try {
					var e = typeof(Emperor);
					e.GetField("handRMoveX", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(modNPC, value.X);
					e.GetField("handRMoveY", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(modNPC, value.Y);
				}
				catch (Exception e) {
				}
			}
		}
		public float HandRotLeft {
			get {
				try {
					var e = typeof(Emperor);
					if(e.GetField("handLRotation", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(modNPC) is float f) return f;
					return MathHelper.PiOver2;
				}
				catch (Exception e) {
					return 0f;
				}
			}
			set {
				try {
					var e = typeof(Emperor);
					e.GetField("handLRotation", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(modNPC, value);
				}
				catch (Exception e) {
				}
			}
		}
		public float HandRotRight {
			get {
				try {
					var e = typeof(Emperor);
					if(e.GetField("handRRotation", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(modNPC) is float f) return f;
					return MathHelper.PiOver2;
				}
				catch (Exception e) {
					return 0f;
				}
			}
			set {
				try {
					var e = typeof(Emperor);
					e.GetField("handRRotation", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(modNPC, value);
				}
				catch (Exception e) {
				}
			}
		}
		public int HandFrameLeft {
			get {
				try {
					var e = typeof(Emperor);
					if(e.GetField("handLFrame", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(modNPC) is int i) return i;
					return 0;
				}
				catch (Exception e) {
					return 0;
				}
			}
			set {
				try {
					var e = typeof(Emperor);
					e.GetField("handLFrame", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(modNPC, value);
				}
				catch (Exception e) {
				}
			}
		}
		public int HandFrameRight {
			get {
				try {
					var e = typeof(Emperor);
					if(e.GetField("handRFrame", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(modNPC) is int i) return i;
					return 0;
				}
				catch (Exception e) {
					return 0;
				}
			}
			set {
				try {
					var e = typeof(Emperor);
					e.GetField("handRFrame", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(modNPC, value);
				}
				catch (Exception e) {
				}
			}
		}
		public override void SetDefaults(NPC npc) => modNPC = npc.ModNPC as Emperor;
		public override void Unload() => modNPC = null;
		public override bool PreAI(NPC npc) {
			modNPC = npc.ModNPC as Emperor;
			if(npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active || Main.player[npc.target].Distance(npc.Center) > 3000f) {
				npc.TargetClosest();
				if(npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active || Main.player[npc.target].Distance(npc.Center) > 3000f) {
					if(npc.timeLeft > 20) npc.timeLeft = 20;
					npc.velocity.Y--;
					return false;
				}
			}
			Vector2 targetPos = Main.player[npc.target].Center;
			Vector2 shootDir = Vector2.Normalize(targetPos - npc.Center);
			switch(ai[0]) {
				case 0:
					npc.ai[0] = 0f;
					HandRotLeft = MathHelper.PiOver2;
					HandPosLeft = new Vector2(35f, 4f);
					HandRotRight = MathHelper.PiOver2;
					HandPosRight = new Vector2(35f, 4f);
					HandFrameLeft = 0;
					HandFrameRight = 0;
					ai[0]++;
					npc.Center = targetPos - Vector2.UnitY * 1000f;
					npc.velocity.Y = 32f;
					npc.dontTakeDamage = true;
					npc.netUpdate = true;
				break;
				case 1:
					npc.velocity *= 0.96f;
					if(ai[1] == 170f) {
						HandFrameLeft = 1;
						HandFrameRight = 1;
						npc.ai[0] = 1f;
					}
					if(ai[1] > 160f) {
						HandRotLeft = MathHelper.SmoothStep(MathHelper.PiOver2, -MathHelper.PiOver4 * 0.2f, (ai[1] - 160f) / 20f);
						HandPosLeft = new Vector2(35f, 4f) + new Vector2(5f * (ai[1] - 160f) / 20f, ai[1] > 170f ? 4f + 16f * (1f - (ai[1] - 170f) / 10f) : 20f * (ai[1] - 160f) / 10f);
						HandRotRight = MathHelper.SmoothStep(MathHelper.PiOver2, -MathHelper.PiOver4 * 0.2f, (ai[1] - 160f) / 20f);
						HandPosRight = new Vector2(35f, 4f) + new Vector2(5f * (ai[1] - 160f) / 20f, ai[1] > 170f ? 4f + 16f * (1f - (ai[1] - 170f) / 10f) : 20f * (ai[1] - 160f) / 10f);
					}
					if(ai[1] == 150f && Main.netMode != 1) for(int i = 0; i < 6; i++) {
						int orb = NPC.NewNPC(npc.GetSource_FromAI(null), (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<JadeOrb>(), 0, npc.whoAmI, i, 0f, 0f, 255);
						if(Main.netMode == 2 && orb < Main.maxNPCs) NetMessage.SendData(23, -1, -1, null, orb);
						NPCLoader.AI(Main.npc[orb]);
						Projectile.NewProjectile(npc.GetSource_FromAI(null), Main.npc[orb].Center, Vector2.Zero, ModContent.ProjectileType<JadePulse>(), 0, 0f, Main.myPlayer, 1.5f);
					}
					if(ai[1] == 50f || ai[1] == 90f || ai[1] == 120f || ai[1] == 140f || ai[1] == 150f) if(Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(null), npc.Center + Vector2.UnitY * modNPC.DrawOffsetY / 2f, Vector2.Zero, ModContent.ProjectileType<JadePulse>(), 0, 0f, Main.myPlayer, 7f);
					if(++ai[1] > 180f) {
						ai[0]++;
						ai[1] = 0f;
						ai[2] = 0f;
						npc.dontTakeDamage = false;
						npc.netUpdate = true;
					}
				break;
				case 2:
					if(ai[2] == 0f) {
						ai[2] = Main.rand.NextBool() ? -1 : 1;
						npc.netUpdate = true;
					}
					if(ai[1] < 60f) {
						ai[1]++;
						npc.velocity += (targetPos + new Vector2(ai[2] * 240f, -320f) - npc.Center) * 0.0028f;
						npc.velocity *= 0.92f;
						npc.velocity += (targetPos + new Vector2(ai[2] * 240f, -320f) - npc.Center) * 0.0028f;
					}
					else {
						Vector2 hpos = new Vector2(40f, 8f);
						float hrot = MathHelper.PiOver4 * -0.2f;
						if(ai[1] < 70f) {
							float lerp = (ai[1] - 60f) / 10f;
							hpos += new Vector2(-16f * lerp * lerp, 8f * (float)Math.Sqrt(lerp));
							hrot += (MathHelper.PiOver4 + 0.6f) * MathHelper.SmoothStep(0f, 1f, lerp);
						}
						else if(ai[1] < 100f) {
							float lerp = (ai[1] - 70f) / 30f;
							hpos += new Vector2(MathHelper.Lerp(-16f, 40f, (float)Math.Sqrt(lerp)), MathHelper.Lerp(8f, -32f, lerp * lerp));
							hrot += MathHelper.SmoothStep(MathHelper.PiOver4 + 0.6f, MathHelper.PiOver4 + -2.3f, lerp);
						}
						else {
							float lerp = MathHelper.SmoothStep(1f, 0f, (ai[1] - 100f) / 20f);
							hpos += new Vector2(40f, -32f) * lerp;
							hrot += (MathHelper.PiOver4 + -2.3f) * lerp;
						}
						if(ai[2] < 0f) {
							HandRotLeft = hrot;
							HandPosLeft = hpos;
						}
						else if(ai[2] > 0f) {
							HandRotRight = hrot;
							HandPosRight = hpos;
						}
						if(ai[1] > 70f && ai[1] % 2 == 0 && ai[1] < 100f) {
							if(ai[2] < 0f) hpos = HandPosLeft + Vector2.UnitY.RotatedBy(hrot = HandRotLeft) * 48f;
							else if(ai[2] > 0f) hpos = HandPosRight + Vector2.UnitY.RotatedBy(hrot = HandRotRight) * 48f;
							hpos.X *= -ai[2];
							if(Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(null), npc.Center + hpos, Vector2.Normalize(hpos).RotatedBy(ai[2] * (ai[1] - 70f) / 30f * MathHelper.PiOver2 - MathHelper.PiOver2 * ai[2]) * 12f, ModContent.ProjectileType<BrightJadeShard>(), 37, 0f, Main.myPlayer, 0f, 1f, 0f);
						}
					}
					npc.velocity *= 0.92f;
					npc.rotation = npc.velocity.X / 45f;
					if(++ai[1] > 120f) {
						if(ai[3] < 2) {
							ai[2] *= -1f;
							ai[3]++;
						}
						else {
							ai[0]++;
							ai[2] = 0f;
							ai[3] = 0f;
						}
						ai[1] = 0f;
						npc.netUpdate = true;
					}
				break;
				case 3:
					if(ai[1] % 60f == 0f) {
						npc.velocity += (targetPos + new Vector2(0f, -240f) - npc.Center) * 0.0048f;
						ai[2] = Main.rand.Next(-30, 31);
						ai[3] = Main.rand.Next(-30, 31);
						npc.netUpdate = true;
					}
					else if(ai[1] % 60f < 45f) npc.velocity += (targetPos + new Vector2(0f, -240f) - npc.Center) * 0.0048f;
					else if(ai[1] % 5f == 0f && Main.netMode != 1) {
						Vector2 right = HandPosRight;
						right.X *= -1f;
						Vector2 right2 = Vector2.UnitY.RotatedBy(-HandRotRight);
						right += right2 * 36f;
						int type = ai[1] < 240f ? ModContent.ProjectileType<UnstableJadeShard>() : ModContent.ProjectileType<JadeShard>();
						Projectile.NewProjectile(npc.GetSource_FromAI(null), npc.Center + right, right2 * 12f, type, 37, 0f, Main.myPlayer, 0f, 1f, 0f);
						Vector2 left = Vector2.UnitY.RotatedBy(HandRotLeft);
						Projectile.NewProjectile(npc.GetSource_FromAI(null), npc.Center + HandPosLeft + left * 36f, left * 12f, type, 37, 0f, Main.myPlayer, 0f, 1f, 0f);
					}
					npc.velocity *= 0.92f;
					npc.rotation = npc.velocity.X / 45f;
					if(ai[1] < 270f) {
						HandPosLeft = Vector2.Lerp(HandPosLeft, new Vector2(-45f, -32f) - Vector2.UnitX.RotatedBy(MathHelper.ToRadians(ai[2])) * 35f, 0.1f);
						HandPosRight = Vector2.Lerp(HandPosRight, new Vector2(-45f, -32f) - Vector2.UnitX.RotatedBy(MathHelper.ToRadians(ai[3])) * 35f, 0.1f);
						HandRotLeft = MathHelper.Lerp(HandRotLeft, MathHelper.ToRadians(ai[2]) + MathHelper.PiOver2, 0.1f);
						HandRotRight = MathHelper.Lerp(HandRotRight, MathHelper.ToRadians(ai[3]) + MathHelper.PiOver2, 0.1f);
					}
					else {
						HandPosLeft = Vector2.Lerp(HandPosLeft, new Vector2(40f, 8f) + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(ai[2])) * 10f, ai[1] == 300f ? 1f : 0.1f);
						HandPosRight = Vector2.Lerp(HandPosRight, new Vector2(40f, 8f) + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(ai[3])) * 10f, ai[1] == 300f ? 1f : 0.1f);
						HandRotLeft = MathHelper.Lerp(HandRotLeft, MathHelper.PiOver4 * 0.2f, ai[1] == 300f ? 1f : 0.1f);
						HandRotRight = MathHelper.Lerp(HandRotRight, MathHelper.PiOver4 * 0.2f, ai[1] == 300f ? 1f : 0.1f);
					}
					if(++ai[1] > 300f) {
						if(!NPC.AnyNPCs(ModContent.NPCType<JadeOrb>())) ai[0] += 3f;
						else ai[0] += Main.rand.Next(2) + 1;
						if(npc.life < npc.lifeMax / 2) ai[0] += 3f;
						ai[1] = 0f;
						ai[2] = 0f;
						ai[3] = 0f;
						npc.netUpdate = true;
					}
				break;
				case 4:
					npc.velocity *= 0.92f;
					npc.rotation = npc.velocity.X / 45f;
					if(ai[1] == 290f) {
						HandFrameLeft = 1;
						HandFrameRight = 1;
						npc.ai[0] = 1f;
					}
					if(ai[1] > 280f) {
						HandRotLeft = MathHelper.SmoothStep(MathHelper.PiOver2, -MathHelper.PiOver4 * 0.2f, (ai[1] - 280f) / 20f);
						HandPosLeft = new Vector2(35f, 4f) + new Vector2(5f * (ai[1] - 280f) / 20f, ai[1] > 290f ? 4f + 16f * (1f - (ai[1] - 290f) / 10f) : 20f * (ai[1] - 280f) / 10f);
						HandRotRight = MathHelper.SmoothStep(MathHelper.PiOver2, -MathHelper.PiOver4 * 0.2f, (ai[1] - 280f) / 20f);
						HandPosRight = new Vector2(35f, 4f) + new Vector2(5f * (ai[1] - 280f) / 20f, ai[1] > 290f ? 4f + 16f * (1f - (ai[1] - 290f) / 10f) : 20f * (ai[1] - 280f) / 10f);
					}
					else {
						if(ai[1] > 0f && ai[1] % 25 == 0 && Main.netMode != 1) {
							Projectile.NewProjectile(npc.GetSource_FromAI(null), npc.Center + Vector2.UnitY * modNPC.DrawOffsetY / 2f, Main.rand.NextVector2CircularEdge(4f, 4f), ModContent.ProjectileType<JadeBlast>(), 37, 0f, Main.myPlayer);
							foreach(NPC orb in Main.ActiveNPCs) if(orb.ModNPC is JadeOrb && orb.ai[0] == npc.whoAmI) Projectile.NewProjectile(npc.GetSource_FromAI(null), orb.Bottom - Vector2.UnitY * orb.scale * orb.height * 0.5f, Vector2.Normalize((orb.Bottom - Vector2.UnitY * orb.scale * orb.height * 0.5f) - (npc.Center + Vector2.UnitY * modNPC.DrawOffsetY * 0.5f)) * 6f, ModContent.ProjectileType<JadeShard>(), 25, 0f, Main.myPlayer, 0f, 1f, 0f);
						}
						if(npc.ai[0] > 0f) npc.ai[0] = 0f;
						modNPC.Hands();
					}
					if(++ai[1] > 300f) {
						npc.ai[0] = 1f;
						ai[0] = 2f;
						ai[1] = 0f;
						ai[2] = 0f;
						ai[3] = 0f;
						npc.netUpdate = true;
					}
				break;
				case 5:
					if(ai[1] < 260f) npc.velocity += (targetPos + new Vector2(0f, -240f) - npc.Center) * 0.0048f;
					npc.velocity *= 0.92f;
					npc.rotation = npc.velocity.X / 45f;
					npc.ai[3] = ai[1] > 100f ? 0f : ai[1] * 2f;
					if(ai[1] == 290f) {
						HandFrameLeft = 1;
						HandFrameRight = 1;
					}
					if(ai[1] > 280f) {
						HandRotLeft = MathHelper.SmoothStep(0f, -MathHelper.PiOver4 * 0.2f, (ai[1] - 280f) / 20f);
						HandPosLeft = new Vector2(60f, 30f) - new Vector2(20f, 22f) * ((ai[1] - 280f) / 20f);
						HandRotRight = MathHelper.SmoothStep(0f, -MathHelper.PiOver4 * 0.2f, (ai[1] - 280f) / 20f);
						HandPosRight = new Vector2(60f, 30f) - new Vector2(20f, 22f) * ((ai[1] - 280f) / 20f);
					}
					else {
						modNPC.Hands();
						if(npc.ai[3] == 100f || npc.ai[3] == 200f) {
							SoundEngine.PlaySound(new SoundStyle("ValhallaMod/Sounds/Throw_2", 0) with {Volume = 1f, PitchVariance = 0.2f}, npc.position);
							if(Main.netMode != 1) {
								int javelins = 20;
								int angle = 60;
								if(Main.expertMode) {
									angle = 90;
									javelins *= 2;
								}
								float posX = -(HandPosRight.X + 10f);
								float posY = HandPosRight.Y;
								if(npc.ai[3] == 200f) {
									posX = HandPosLeft.X + 10f;
									posY = HandPosLeft.Y;
								}
								Vector2 pos = new(npc.position.X + (float)(npc.width / 2) + posX, npc.position.Y + (float)(npc.height / 2) + modNPC.DrawOffsetY / 2f + posY - 120f);
								Projectile.NewProjectile(npc.GetSource_FromAI(null), pos, new Vector2(0f, -20f), ModContent.ProjectileType<JadeGreatJavelin>(), 25, 0f, Main.myPlayer, (float)angle, (float)javelins, 0f);
							}
						}
						else if(ai[1] > 270f && ai[1] % 3 == 0 && ai[1] < 280 && Main.netMode != 1) foreach(NPC orb in Main.ActiveNPCs) if(orb.ModNPC is JadeOrb && orb.ai[0] == npc.whoAmI) Projectile.NewProjectile(npc.GetSource_FromAI(null), orb.Bottom - Vector2.UnitY * orb.scale * orb.height * 0.5f, Vector2.Normalize((orb.Bottom - Vector2.UnitY * orb.scale * orb.height * 0.5f) - (npc.Center + Vector2.UnitY * modNPC.DrawOffsetY * 0.5f)) * 6f, ModContent.ProjectileType<JadeShard>(), 25, 0f, Main.myPlayer, 0f, 1f, 0f);
					}
					if(++ai[1] > 300f) {
						npc.ai[0] = 1f;
						npc.ai[3] = 0f;
						ai[0] = 2f;
						ai[1] = 0f;
						ai[2] = 0f;
						ai[3] = 0f;
						npc.netUpdate = true;
					}
				break;
				case 6:
					npc.velocity.X *= 0.98f;
					npc.velocity.Y -= 0.1f;
					npc.rotation = 0f;
					npc.ai[0] = 0f;
					modNPC.Hands();
					if(ai[1] == 0f && Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(null), npc.Center - Vector2.UnitY * modNPC.DrawOffsetY * 0.5f, Vector2.Zero, ModContent.ProjectileType<JadeNova>(), 45, 0f, Main.myPlayer, npc.whoAmI + 1, 150f, 2f);
					if(++ai[1] > 180f) {
						ai[0] = 0f;
						ai[1] = 0f;
						ai[2] = 0f;
						ai[3] = 0f;
						npc.netUpdate = true;
					}
				break;
				case 7:
					npc.velocity *= 0.92f;
					npc.rotation = npc.velocity.X / 45f;
					if(ai[1] == 290f) {
						HandFrameLeft = 1;
						HandFrameRight = 1;
						npc.ai[0] = 1f;
					}
					if(ai[1] > 280f) {
						HandRotLeft = MathHelper.SmoothStep(MathHelper.PiOver2, -MathHelper.PiOver4 * 0.2f, (ai[1] - 280f) / 20f);
						HandPosLeft = new Vector2(35f, 4f) + new Vector2(5f * (ai[1] - 280f) / 20f, ai[1] > 290f ? 4f + 16f * (1f - (ai[1] - 290f) / 10f) : 20f * (ai[1] - 280f) / 10f);
						HandRotRight = MathHelper.SmoothStep(MathHelper.PiOver2, -MathHelper.PiOver4 * 0.2f, (ai[1] - 280f) / 20f);
						HandPosRight = new Vector2(35f, 4f) + new Vector2(5f * (ai[1] - 280f) / 20f, ai[1] > 290f ? 4f + 16f * (1f - (ai[1] - 290f) / 10f) : 20f * (ai[1] - 280f) / 10f);
					}
					else {
						if(ai[1] > 0f && ai[1] % 5 == 0 && Main.netMode != 1) {
							if(ai[1] < 260) Projectile.NewProjectile(npc.GetSource_FromAI(null), npc.Center + Vector2.UnitY * modNPC.DrawOffsetY / 2f, Main.rand.NextVector2CircularEdge(1f, 1f), ModContent.ProjectileType<JadeBeam>(), 37, 0f, Main.myPlayer, npc.whoAmI + 1);
							if(ai[1] % 25 == 0) foreach(NPC orb in Main.ActiveNPCs) if(orb.ModNPC is JadeOrb && orb.ai[0] == npc.whoAmI) Projectile.NewProjectile(npc.GetSource_FromAI(null), orb.Bottom - Vector2.UnitY * orb.scale * orb.height * 0.5f, Vector2.Normalize((orb.Bottom - Vector2.UnitY * orb.scale * orb.height * 0.5f) - (npc.Center + Vector2.UnitY * modNPC.DrawOffsetY * 0.5f)) * 6f, ModContent.ProjectileType<JadeShard>(), 25, 0f, Main.myPlayer, 0f, 1f, 0f);
						}
						if(npc.ai[0] > 0f) npc.ai[0] = 0f;
						modNPC.Hands();
					}
					if(++ai[1] > 300f) {
						npc.ai[0] = 1f;
						ai[0] = 2f;
						ai[1] = 0f;
						ai[2] = 0f;
						ai[3] = 0f;
						npc.netUpdate = true;
					}
				break;
				case 8:
					if(ai[1] < 260f) npc.velocity += (targetPos + new Vector2(0f, -240f) - npc.Center) * 0.0048f;
					npc.velocity *= 0.92f;
					npc.rotation = npc.velocity.X / 45f;
					npc.ai[3] = ai[1] > 200f ? 0f : ai[1] > 100f ? (ai[1] - 100f) * 2f : ai[1] * 2f;
					if(ai[1] == 290f) {
						HandFrameLeft = 1;
						HandFrameRight = 1;
					}
					if(ai[1] > 280f) {
						HandRotLeft = MathHelper.SmoothStep(0f, -MathHelper.PiOver4 * 0.2f, (ai[1] - 280f) / 20f);
						HandPosLeft = new Vector2(60f, 30f) - new Vector2(20f, 22f) * ((ai[1] - 280f) / 20f);
						HandRotRight = MathHelper.SmoothStep(0f, -MathHelper.PiOver4 * 0.2f, (ai[1] - 280f) / 20f);
						HandPosRight = new Vector2(60f, 30f) - new Vector2(20f, 22f) * ((ai[1] - 280f) / 20f);
					}
					else {
						modNPC.Hands();
						if(npc.ai[3] == 100f || npc.ai[3] == 200f) {
							SoundEngine.PlaySound(new SoundStyle("ValhallaMod/Sounds/Throw_2", 0) with {Volume = 1f, PitchVariance = 0.2f}, npc.position);
							if(Main.netMode != 1) {
								float posX = -(HandPosRight.X + 10f);
								float posY = HandPosRight.Y;
								if(npc.ai[3] == 200f) {
									posX = HandPosLeft.X + 10f;
									posY = HandPosLeft.Y;
								}
								Vector2 pos = new(npc.position.X + (float)(npc.width / 2) + posX, npc.position.Y + (float)(npc.height / 2) + modNPC.DrawOffsetY / 2f + posY - 120f);
								Projectile.NewProjectile(npc.GetSource_FromAI(null), pos, new Vector2(0f, -20f), ModContent.ProjectileType<JadeGreaterJavelin>(), 25, 0f, Main.myPlayer);
							}
						}
					}
					if(++ai[1] > 300f) {
						npc.ai[0] = 1f;
						npc.ai[3] = 0f;
						ai[0] = 2f;
						ai[1] = 0f;
						ai[2] = 0f;
						ai[3] = 0f;
						npc.netUpdate = true;
					}
				break;
				case 9:
					npc.velocity.X *= 0.98f;
					npc.velocity.Y -= 0.1f;
					npc.rotation = 0f;
					npc.ai[0] = 0f;
					modNPC.Hands();
					if(ai[1] == 0f && Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(null), npc.Center - Vector2.UnitY * modNPC.DrawOffsetY * 0.5f, Vector2.Zero, ModContent.ProjectileType<JadeNova>(), 45, 0f, Main.myPlayer, npc.whoAmI + 1, 120f, 3f);
					if(ai[1] == 120f && Main.netMode != 1) Main.projectile[Projectile.NewProjectile(npc.GetSource_FromAI(null), npc.Center - Vector2.UnitY * modNPC.DrawOffsetY * 0.5f, Vector2.Zero, ModContent.ProjectileType<JadeNova>(), 45, 0f, Main.myPlayer, npc.whoAmI + 1, 30f, 3f)].timeLeft += 30;
					if(ai[1] == 150f && Main.netMode != 1) Main.projectile[Projectile.NewProjectile(npc.GetSource_FromAI(null), npc.Center - Vector2.UnitY * modNPC.DrawOffsetY * 0.5f, Vector2.Zero, ModContent.ProjectileType<JadeNova>(), 45, 0f, Main.myPlayer, npc.whoAmI + 1, 30f, 3f)].timeLeft += 60;
					if(++ai[1] > 180f) {
						ai[0] = 0f;
						ai[1] = 0f;
						ai[2] = 0f;
						ai[3] = 0f;
						npc.netUpdate = true;
					}
				break;
			}
			return false;
		}
		public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
			if(Main.netMode == 0) return;
			bitWriter.WriteBit(npc.dontTakeDamage);
			for(int i = 0; i < ai.Length; i++) binaryWriter.Write(ai[i]);
		}
		public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
			if(Main.netMode == 0) return;
			npc.dontTakeDamage = bitReader.ReadBit();
			for(int i = 0; i < ai.Length; i++) ai[i] = binaryReader.ReadSingle();
		}
	}
}