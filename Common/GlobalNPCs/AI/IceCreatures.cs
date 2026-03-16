using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace Synergia.Common.GlobalNPCs.AI
{
	public class IceGolem : GlobalNPC 
	{
		public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == NPCID.IceGolem;
		public override void AI(NPC npc) {
			bool shockwave = false;
			if(npc.localAI[3] != npc.frame.Y) {
				shockwave = npc.localAI[3] == 1664;
				npc.localAI[3] = npc.frame.Y;
				shockwave &= npc.localAI[3] == 1408;
			}
			if(shockwave) {
				if(npc.localAI[2] > 0f) {
					npc.localAI[1] = 0f;
					npc.localAI[2] = 0f;
					npc.rotation = 0f;
				}
				npc.velocity.X *= 0.1f;
				for(int j = (int)npc.position.X - 20; j <= (int)npc.position.X + npc.width + 20; j += 20) {
					for(int k = 0; k < 4; k++) Main.dust[Dust.NewDust(new Vector2(npc.position.X - 20, npc.position.Y + (float)npc.height), npc.width + 20, 4, DustID.Smoke, 0f, 0f, 100, default(Color), 1.5f)].velocity *= 0.2f;
					Main.gore[Gore.NewGore(npc.GetSource_FromAI(), new Vector2((float)(j - 20), npc.position.Y + (float)npc.height - 8f), default(Vector2), Main.rand.Next(61, 64), 1f)].velocity *= 0.4f;
				}
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, npc.position);
			}
			if(npc.localAI[3] == 1536 || npc.localAI[2] == 1f) npc.ai[2] = 0f;
			if(npc.localAI[2] == 1f) {
				npc.localAI[1]++;
				if(npc.velocity.Y < 0) npc.rotation = npc.velocity.X / 45f * MathHelper.Min(20f, npc.localAI[1]) * 0.05f;
				else {
					npc.rotation += npc.velocity.X / 120f;
					if(Math.Abs(npc.rotation) > MathHelper.TwoPi) npc.rotation = Math.Sign(npc.velocity.X) * MathHelper.TwoPi;
				}
				Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, DustID.IceTorch, 0f, 0f, 100, default(Color), 1.5f)].noGravity = true;
			}
			if(npc.target < -1) return;
			if(Math.Sign(npc.velocity.X) == Math.Sign(Main.player[npc.target].Center.X - npc.Center.X) && Math.Abs(Main.player[npc.target].Center.X - npc.Center.X) > 640 && npc.velocity.Y == 0f) {
				npc.velocity += new Vector2(Math.Sign(npc.velocity.X) * 16f, -12f);
				npc.localAI[2] = 1f;
				npc.netUpdate = true;
			}
		}
	}
	public class IceElemental : GlobalNPC 
	{
		public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == NPCID.IceElemental;
		public override void AI(NPC npc) {
			if(npc.ai[3] > 0f) {
				if(npc.ai[3] < 60f) {
					npc.ai[3] += 0.0001f;
					int d = Dust.NewDust(npc.position, npc.width, npc.height, DustID.IceTorch, 0f, 0f, 100, default(Color), 1.5f);
					Main.dust[d].noGravity = true;
					Main.dust[d].position += Main.dust[d].position - npc.Center;
					Main.dust[d].velocity -= (Main.dust[d].position - npc.Center) * 0.05f;
				}
				if(Main.netMode != 1 && (int)npc.ai[3] == 60) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 8f, ProjectileID.FrostBlastHostile, npc.damage / (Main.masterMode ? 3 : Main.expertMode ? 2 : 1) / 2, 0f, Main.myPlayer);
				npc.velocity *= 1f - npc.ai[3] / 60f;
			}
		}
	}
}
