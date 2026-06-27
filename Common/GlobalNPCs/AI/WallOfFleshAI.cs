using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using Synergia.Content.Projectiles.Hostile;
using System;
using System.IO;

namespace Synergia.Common.GlobalNPCs.AI
{
	public class WallOfFleshAI : GlobalNPC
	{
		internal static bool Disabled = false;
		public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == NPCID.WallofFlesh || npc.type == NPCID.WallofFleshEye;
		public override void AI(NPC npc) {
			if(Disabled) return;
			if(npc.type == NPCID.WallofFlesh) {
				if(++npc.ai[3] > 840f) npc.ai[3] = 0f;
				if(npc.ai[3] > 600f) {
					npc.ai[1]--;
					npc.ai[2] = 0f;
					foreach(NPC eyes in Main.ActiveNPCs) if(eyes.type == NPCID.WallofFleshEye) if(eyes.localAI[2] > 0f) {
						npc.ai[3] = 600f;
						return;
					}
					if(npc.ai[3] > 660f && npc.ai[3] < 810f) {
                        if (npc.ai[3] % 20 == 0)
                        {
                            if (Main.netMode != 1)
                            {
                                Vector2 baseDirection = Vector2.Normalize(new Vector2(npc.direction, -0.4f));

                                // 7 degrees
                                float randomSpread = Main.rand.NextFloat(-0.12f, 0.12f);
                                Vector2 shotVelocity = (baseDirection * 8f).RotatedBy(randomSpread) + npc.velocity;

                                Projectile.NewProjectile(
                                    npc.GetSource_FromAI(),
                                    npc.Center + baseDirection * npc.width * 0.55f,
                                    shotVelocity, // randomed too
                                    ModContent.ProjectileType<StonedBlood>(),
                                    23,
                                    2f,
                                    Main.myPlayer
                                );
                            }
                            SoundEngine.PlaySound(SoundID.NPCHit8, npc.Center);
                        }
                        if (npc.frame.Y == 0) npc.frameCounter = 0.0;
					}
					else if(npc.ai[3] < 655f && npc.frame.Y == npc.frame.Height) npc.frameCounter = 0.0;
					npc.rotation = Vector2.Normalize(Vector2.Lerp(npc.rotation.ToRotationVector2(), Vector2.Normalize(new Vector2(Math.Sign(npc.velocity.X), -0.4f)) * npc.spriteDirection, npc.ai[3] > 810f ? 1f - (npc.ai[3] - 810f) / 30f : npc.ai[3] < 630f ? (npc.ai[3] - 600f) / 30f : 1f)).ToRotation();
				}
				else if(npc.ai[3] % 150f == 0) if(Main.netMode != 1) {
					bool top = Main.rand.NextBool(2);
					foreach(NPC eyes in Main.ActiveNPCs) if(eyes.type == NPCID.WallofFleshEye && (top ? eyes.rotation.ToRotationVector2().Y > 0f : eyes.rotation.ToRotationVector2().Y < 0f)) {
						eyes.ai[3] = 1f;
						eyes.netUpdate = true;
						break;
					}
				}
				if((npc.ai[2] > 0f || npc.ai[3] > 500f) && npc.localAI[3] > 500f) npc.localAI[3] = 500f;
				if(npc.localAI[3] < 0f || (npc.localAI[2] > 0f && npc.localAI[2] < 120f)) {
					if(npc.localAI[2] == 0f && Main.netMode < 2) Main.LocalPlayer.GetModPlayer<Common.GlobalPlayer.ScreenShakePlayer>().TriggerShake(120, 0.8f);
					if(npc.localAI[2] < 120f && npc.frame.Y == 0) {
						if(npc.localAI[2] % 20 == 0 && Main.netMode == 0) foreach(NPC hungry in Main.ActiveNPCs) if(Main.rand.NextBool(3) && hungry.type == NPCID.TheHungry) {
							hungry.StrikeInstantKill();
							break;
						}
						int l = Dust.NewDust(npc.Center + new Vector2(npc.spriteDirection * npc.width * 0.55f, -6f).RotatedBy(npc.rotation) - Vector2.UnitY * 4f - Vector2.One * 4f, 8, 8, DustID.Blood);
						Main.dust[l].noGravity = true;
						Main.dust[l].velocity += (Main.dust[l].position - npc.Center) * 0.1f;
						npc.frameCounter = 0.0;
					}
					npc.localAI[2]++;
				}
				else npc.localAI[2] = 0f;
			}
			else {
				if(npc.ai[3] == 1f) SoundEngine.PlaySound(SoundID.Item4, npc.Center);
				if(npc.ai[3] > 0f) if(++npc.ai[3] > (int)MathHelper.Lerp(12f, 8f, (float)npc.life / (float)npc.lifeMax) * 10) npc.ai[3] = 0f;
				if(Main.netMode != 1 && npc.ai[3] > 40f && npc.ai[3] % 10f == 5f) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + new Vector2(npc.spriteDirection * npc.width * 0.55f, -6f).RotatedBy(npc.rotation) - Vector2.UnitY * 4f, npc.rotation.ToRotationVector2() * npc.spriteDirection * 12f, ProjectileID.EyeLaser, 21, 0f, Main.myPlayer);
				npc.localAI[1] = 0f;
			}
		}
		public override void PostDraw(NPC npc, SpriteBatch sprite, Vector2 screenPos, Color lightColor) {
			if(Disabled) return;
			int frameHeight = Terraria.GameContent.TextureAssets.Npc[npc.type].Height() / Main.npcFrameCount[npc.type];
			Vector2 center = npc.Bottom - screenPos;
			center.Y += -frameHeight * npc.scale + 4f + frameHeight / 2 * npc.scale;
			if(npc.type == NPCID.WallofFleshEye) {
				center += new Vector2(npc.spriteDirection * npc.width * 0.65f, -6f).RotatedBy(npc.rotation);
				if(npc.ai[3] < 20f) {
					float glowTime = (float)System.Math.Sin(npc.ai[3] * 0.05f * MathHelper.Pi);
					float flash = npc.ai[3] / 20f * MathHelper.Pi;
					for(int i = 1; i < 3; i++) {
						Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_" + i);
						sprite.Draw(texture, center, null, Color.BlueViolet with {A = 25} * glowTime, flash, texture.Size() * 0.5f, new Vector2(glowTime * (i == 1 ? 1.1f : 0.8f), (i != 1 ? 1.1f : 0.8f) * glowTime) * 2.4f, SpriteEffects.None, 0);
					}
					for(int i = 1; i < 3; i++) {
						Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_" + i);
						sprite.Draw(texture, center, null, new Color(200, 200, 200, 0) * glowTime, flash, texture.Size() * 0.5f, new Vector2(glowTime * (i == 1 ? 1.1f : 0.8f), (i != 1 ? 1.1f : 0.8f) * glowTime) * 1.6f, SpriteEffects.None, 0);
					}
				}
				else if(npc.ai[3] > 40f) {
					float glowTime = (float)System.Math.Sin(npc.ai[3] % 10f * 0.1f * MathHelper.Pi);
					for(int i = 1; i < 3; i++) {
						Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_" + i);
						sprite.Draw(texture, center, null, Color.BlueViolet with {A = 25} * glowTime, 0f, texture.Size() * 0.5f, new Vector2(glowTime * (i == 1 ? 1.1f : 0.8f), (i != 1 ? 1.1f : 0.8f) * glowTime) * 1.2f, SpriteEffects.None, 0);
					}
					for(int i = 1; i < 3; i++) {
						Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_" + i);
						sprite.Draw(texture, center, null, new Color(200, 200, 200, 0) * glowTime, 0f, texture.Size() * 0.5f, new Vector2(glowTime * (i == 1 ? 1.1f : 0.8f), (i != 1 ? 1.1f : 0.8f) * glowTime) * 0.8f, SpriteEffects.None, 0);
					}
				}
			}
		}
		public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
			if(Main.netMode == 0) return;
			bitWriter.WriteBit(npc.spriteDirection > 0);
			binaryWriter.Write(npc.rotation);
		}
		public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
			if(Main.netMode == 0) return;
			npc.spriteDirection = bitReader.ReadBit() ? 1 : -1;
			npc.rotation = binaryReader.ReadSingle();
		}
	}
}
