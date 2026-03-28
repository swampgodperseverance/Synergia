using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System;
using System.IO;

namespace Synergia.Common.GlobalNPCs.AI
{
	public class BloodEel : GlobalNPC 
	{
		internal static bool Disabled = false;
		public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type >= NPCID.BloodEelHead && npc.type <= NPCID.BloodEelTail;
		public override void AI(NPC npc) {
			if(Disabled) return;
			if(npc.type != NPCID.BloodEelHead) {
				NPC body = Main.npc[(int)npc.ai[1]];
				npc.target = body.target;
				float distancing = 1f / npc.scale * 0.45f;
				Vector2 attachToBody = body.Center - Vector2.UnitY.RotatedBy(body.localAI[3]) * body.height * distancing - npc.Center;
				if(body.localAI[3] != npc.rotation) attachToBody = Utils.MoveTowards(Utils.RotatedBy(attachToBody, MathHelper.WrapAngle(body.localAI[3] - npc.rotation) * 0.2f, Vector2.Zero), (body.localAI[3] - npc.rotation).ToRotationVector2(), 1f);
				npc.Center = body.Center + Vector2.UnitY.RotatedBy(body.rotation) * body.height * distancing - Utils.SafeNormalize(attachToBody, Vector2.Zero) * npc.height * distancing;
				npc.rotation = attachToBody.ToRotation() + MathHelper.PiOver2;
				body.localAI[3] = body.localAI[2];
				body.localAI[2] = body.type != NPCID.BloodEelHead ? body.localAI[1] : body.rotation;
				if(body.type != NPCID.BloodEelHead) body.localAI[1] = body.rotation;
				return;
			}
			if(npc.target < -1) return;
			if((npc.ai[2] < 300f && npc.velocity.Y < 0f) || (npc.ai[2] == 300f && Math.Sign(Main.player[npc.target].Center.X - npc.Center.X) == Math.Sign(npc.velocity.X) && npc.velocity.Y > 0f && npc.Bottom.Y < Main.player[npc.target].Top.Y)) npc.ai[2]++;
			if(npc.ai[2] > 300f) {
				npc.ai[2]++;
				npc.netUpdate = true;
				npc.velocity = Vector2.Lerp(npc.velocity, Vector2.UnitX * npc.spriteDirection * -16f, MathHelper.Min(npc.ai[2] - 300f, 10f) * 0.1f);
				if(npc.ai[2] % 6f == 0 && Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + Vector2.Normalize(npc.velocity) * npc.width * 0.45f, Vector2.Normalize(npc.velocity) * 4f, 811, npc.damage / (Main.masterMode ? 3 : Main.expertMode ? 2 : 1) / 2, 0f, Main.myPlayer, 1000f);
				if(npc.ai[2] > 330f) npc.ai[2] = Math.Sign(Main.player[npc.target].Center.X - npc.Center.X) == Math.Sign(npc.velocity.X) ? 300f : 0f;
			}
			else if(npc.localAI[1] == 0f && npc.Distance(Main.player[npc.target].Center) < 240f && (Vector2.Normalize(Main.player[npc.target].Center - npc.Center) - Vector2.Normalize(npc.velocity)).Length() < 0.05f) npc.localAI[1] = 30f;
			if(npc.localAI[1] > 0f) {
				npc.velocity = npc.oldVelocity;
				npc.localAI[1]--;
			}
			npc.oldVelocity = npc.velocity;
		}
	}
	public class WanderingEye : GlobalNPC
	{
		public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
			if(Main.netMode > 0) binaryWriter.Write(npc.aiStyle);
		}
		public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
			if(Main.netMode > 0) npc.aiStyle = binaryReader.ReadInt32();
		}
		public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == NPCID.WanderingEye;
		public override void AI(NPC npc) {
			if(npc.target < 0) {
				if(npc.aiStyle == 5) npc.aiStyle = 2;
				return;
			}
			Player target = Main.player[npc.target];
			Vector2 shootDir = target.Center - npc.Center;
			bool phase2 = npc.life < npc.lifeMax * 0.5 || npc.localAI[0] == 1f;
			if(npc.aiStyle == 5) {
				if(target.dead) {
					npc.aiStyle = 2;
					npc.netUpdate = true;
				}
				if(++npc.ai[3] > (phase2 ? 30f : 180)) {
					npc.ai[3] = 0;
					npc.localAI[3] = 0;
					npc.aiStyle = 2;
					npc.netUpdate = true;
					npc.velocity = Vector2.Normalize(shootDir) * (phase2 ? 8f : 6f);
				}
				else if(npc.ai[3] > 0f && npc.ai[3] < 180f && npc.ai[3] % 60f == 0 && Main.expertMode && Main.netMode != 1 && NPC.CountNPCS(NPCID.ServantofCthulhu) < NPC.CountNPCS(npc.type) * 4) {
					int d = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.ServantofCthulhu);
					Main.npc[d].scale *= 0.75f;
					Main.npc[d].velocity += npc.velocity;
					if(Main.netMode == 2) NetMessage.SendData(23, -1, -1, null, d);
				}
				else if(npc.ai[3] < 5f) npc.velocity *= (phase2 ? 0.94f : 0.96f);
			}
			else if(!target.dead) {
				if(++npc.ai[3] > 30) {
					npc.ai[3] = 0;
					npc.aiStyle = 5;
					npc.netUpdate = true;
				}
				if(phase2) npc.velocity = npc.oldVelocity;
			}
			else npc.ai[3] = 0;
		}
		public override void FindFrame(NPC npc, int frameHeight) {
			if(npc.target < 0) return;
			Vector2 shootDir = Main.player[npc.target].Center - npc.Center;
			if(npc.aiStyle == 5) {
				if(npc.localAI[3] < 20) {
					npc.rotation = lerpRotation(npc.velocity, shootDir, npc.localAI[3] * 0.05f);
					npc.localAI[3]++;
				}
				else npc.rotation = shootDir.ToRotation();
				npc.spriteDirection = Math.Abs(npc.rotation) < MathHelper.PiOver2 ? 1 : -1;
				if(npc.spriteDirection < 0) npc.rotation += MathHelper.Pi;
			}
			else if(npc.localAI[3] > 0) {
				npc.rotation = lerpRotation(npc.velocity, shootDir, npc.localAI[3] * 0.05f);
				npc.localAI[3]--;
			}
		}
		public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers) {
			if(npc.ai[3] > 0f && npc.aiStyle == 3) modifiers.Knockback *= 0f;
		}
		private static float lerpRotation(Vector2 old, Vector2 direction, float lerp) => Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(old), Vector2.Normalize(direction), lerp)).ToRotation();
	}
}
