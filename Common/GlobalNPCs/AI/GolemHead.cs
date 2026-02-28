using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Synergia.Content.Projectiles.Boss.GolemBuff;

namespace Synergia.Common.GlobalNPCs.AI
{
	public class GolemHead : GlobalNPC
	{
		public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == NPCID.GolemHeadFree;
		public override void AI(NPC npc) {
			npc.localAI[1]++;
			if(npc.aiStyle == 48 && npc.localAI[1] > 300f) {
				npc.localAI[0] = 0f;
				npc.localAI[1] = 0f;
				npc.localAI[2] = Math.Abs(Main.player[npc.target].Center.X - npc.Center.X) > 420f ? 1f : Main.rand.Next(2) + 2;
				npc.aiStyle = -1;
				npc.netUpdate = true;
			}
			switch(npc.localAI[2]) {
				default:
					npc.aiStyle = 48;
				break;
				case 1:
					npc.aiStyle = -1;
					if(npc.localAI[3] == 0f) npc.localAI[3] = Math.Sign(Main.player[npc.target].Center.X - npc.Center.X);
					if(npc.localAI[1] < 60f) {
						if(Main.netMode != 1 && npc.localAI[1] < 50f) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + npc.velocity, Main.rand.NextVector2Circular(24f, 24f), ModContent.ProjectileType<GolemFireCharge>(), 0, 0f, Main.myPlayer, npc.whoAmI);
						npc.velocity += Vector2.Normalize(Main.player[npc.target].Center - new Vector2(npc.localAI[3] * 320f, 240f) - npc.Center) * 0.006f;
						npc.velocity *= 0.94f;
					}
					else {
						float speed = MathHelper.Min(1f, (float)Math.Sin((npc.localAI[1] - 60f) / 60f * MathHelper.Pi) * 3f);
						npc.velocity = speed * Vector2.UnitX * npc.localAI[3] * 16f;
						if(Main.netMode != 1 && speed >= 1f) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + npc.velocity, Vector2.UnitY * 6f, ModContent.ProjectileType<GolemFireBreath>(), 40, 0f, Main.myPlayer);
						npc.localAI[0] = speed >= 0.5f ? 1f : 0f;
					}
					if(npc.localAI[1] > 120f) {
						npc.localAI[0] = 0f;
						npc.localAI[1] = 0f;
						npc.localAI[2] = 0f;
						npc.localAI[3] = 0f;
						npc.aiStyle = 48;
						npc.netUpdate = true;
					}
				break;
				case 2:
					npc.aiStyle = -1;
					if(npc.localAI[1] < 60f) {
						npc.velocity += Vector2.Normalize(Main.player[npc.target].Center + new Vector2(Main.player[npc.target].velocity.X, -320f) - npc.Center) * 0.001f;
						npc.velocity *= 0.94f;
					}
					else {
						if(npc.localAI[1] == 90f) {
							npc.localAI[0] = 1f;
							if(Main.netMode != 1) for(int i = -4; i <= 4; i++) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.UnitY.RotatedBy(MathHelper.PiOver4 * i * 0.5f) * 6f, ProjectileID.PoisonDartTrap, 40, 0f, Main.myPlayer);
						}
						else if(npc.localAI[1] == 120f) npc.localAI[0] = 0f;
						npc.velocity *= 0.94f;
					}
					if(npc.localAI[1] > 120f) {
						npc.localAI[0] = 0f;
						npc.localAI[1] = 0f;
						npc.localAI[2] = 0f;
						npc.localAI[3] = 0f;
						npc.aiStyle = 48;
						npc.netUpdate = true;
					}
				break;
				case 3:
					npc.aiStyle = -1;
					if(npc.localAI[1] < 60f) {
						npc.velocity += Vector2.Normalize(Main.player[npc.target].Center + new Vector2(Main.player[npc.target].velocity.X, -320f) - npc.Center) * 0.006f;
						npc.velocity *= 0.94f;
					}
					else {
						if(npc.localAI[1] > 90f && npc.localAI[1] < 120f) {
							npc.localAI[0] = 1f;
							if(npc.localAI[1] % 9 == 0 && Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Main.rand.NextVector2Circular(2f, 2f), ProjectileID.SpikyBallTrap, 40, 0f, Main.myPlayer);
						}
						else npc.localAI[0] = 0f;
						npc.velocity *= 0.94f;
					}
					if(npc.localAI[1] > 120f) {
						npc.localAI[0] = 0f;
						npc.localAI[1] = 0f;
						npc.localAI[2] = 0f;
						npc.localAI[3] = 0f;
						npc.aiStyle = 48;
						npc.netUpdate = true;
					}
				break;
			}
			if(npc.aiStyle == -1 && Main.npc[NPC.golemBoss].ai[0] == 0f) {
				npc.ai[0] = 0f;
				npc.ai[1] = 0f;
				if(npc.localAI[1] % 20 == 0 && Main.netMode != 1) {
					Vector2 shootDir = Vector2.Normalize(Main.player[npc.target].Center + Main.player[npc.target].velocity * 16f - Main.npc[NPC.golemBoss].Top + Vector2.UnitY * 32);
					Projectile.NewProjectile(npc.GetSource_FromAI(), Main.npc[NPC.golemBoss].Top + shootDir * 24f, shootDir * 7f, ProjectileID.EyeBeam, 40, 0f, Main.myPlayer);
				}
				Main.npc[NPC.golemBoss].ai[1] = -40f;
			}
		}
		public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
			if(Main.netMode > 0) for(int i = 1; i < npc.localAI.Length; i++) binaryWriter.Write(npc.localAI[i]);
		}
		public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
			if(Main.netMode > 0) for(int i = 1; i < npc.localAI.Length; i++) npc.localAI[i] = binaryReader.ReadSingle();
		}
	}
}
